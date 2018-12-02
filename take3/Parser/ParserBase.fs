module ParserBase

open FSharp.Data
open ParserErrors
open ResultHelperFunctions
open System

type RecordMatcher<'a> = string * JsonValue -> Result<'a, ErrorTrace>

let MatchString jsonvalue =
    match jsonvalue with 
    | JsonValue.String x -> Ok x
    | _ -> CreateRootErrorMessage TypeNames.String jsonvalue

let MatchBool jsonvalue =
    match jsonvalue with 
    | JsonValue.Boolean x -> Ok x
    | _ -> CreateRootErrorMessage TypeNames.Boolean jsonvalue

let MatchFloat jsonvalue =
    match jsonvalue with 
    | JsonValue.Number x -> float x |> Ok
    | JsonValue.Float x -> Ok x
    | _ -> CreateRootErrorMessage TypeNames.Float jsonvalue

let MatchInt jsonvalue =
    match MatchFloat jsonvalue with
    | Error _ -> CreateRootErrorMessage TypeNames.Integer jsonvalue
    | Ok x -> if x % 1.0 = 0.0 then int x |> Ok else CreateRootErrorMessage TypeNames.Integer jsonvalue

let MatchList f jsonvalue = 
    match jsonvalue with
    | JsonValue.Array x -> Array.toList x |>  List.map f |> Ok
    | _ -> CreateRootErrorMessage TypeNames.Array jsonvalue

let MatchRecordEntry name (valuematch : JsonValue -> Result<'output, ErrorTrace>) (recordentry : string * JsonValue) =
    if fst recordentry <> name
    then String.Format("Expected record entry to have name '{0}', has name '{1}' instead", name, fst recordentry) |> RootErrorResult
    else valuematch <| snd recordentry

let MatchAny (matcher : 'a -> Result<'b, ErrorTrace>) (values : 'a list) =
    let result = List.map matcher values
    match result |> FirstOk with
    | Some x -> Ok x
    | None -> ExtractErrors result |> MultiErrorResult

//let private flatten_errorlistlist listlist =
//    let noempties = List.where (fun x -> List.isEmpty x |> not) listlist
//    if noempties.Length = 0 
//        then None 
//        else 
//            if noempties.Length = 1 
//            then MultiError noempties.Head |> Some 
//            else List.map MultiError noempties |> MultiError |> Some

//let private AddAnnotation text error = (text, error) |> ParentError

//let private ProcessResults (matcher_first_results: Result<'a,ErrorTrace> list list) (data_first_results: Result<'a,ErrorTrace> list list) =
//    let Oks = List.map SingleOk matcher_first_results //return the Ok if only a single one exists
//    if AllOk Oks
//    then List.map OptionOfOk Oks |> List.choose id |> Ok
//    else 
//        // Get all errors for each matcher that wasnt matched
//        let nomatcheserror = List.where AllError matcher_first_results |> List.map ExtractErrors 
//        let cleanednomatch = flatten_errorlistlist nomatcheserror
//        let annonatednomatch = Option.map2 AddAnnotation (Some "No matching record entries were found for some matches") cleanednomatch
        
//        // Get all errors for each entry that wasnt matched
//        let leftovererror = List.where AllError data_first_results |> List.map ExtractErrors
//        let cleanedleftover = flatten_errorlistlist leftovererror
//        let annonatedleftover = Option.map2 AddAnnotation (Some "No matches were found for some record entires") cleanedleftover
        
//        // Check if any entries or matchers had multiple matches, if so the matchers contain an error (or something unexpected happened in the json)
//        let multimatch = if List.map IsMultiOk matcher_first_results |> List.exists id then RootError "Some matches had multiple matching entries, this is an error in the parser, name should be unique" |> Some else None
//        let multientry = if List.map IsMultiOk data_first_results |> List.exists id  then RootError "Some entries had multiple matching matchers, this is an error in the parser (or json), name should be unique" |> Some else None

//        let allerrors = [annonatedleftover; annonatednomatch; multientry; multimatch] |> List.choose id
//        if List.isEmpty allerrors then raise (Exception "An error case in this function in the parser was missed, this point ought not to be reached") else MultiError allerrors |> Error

//let MatchRecord entrymatchers jsonvalue=
//    match jsonvalue with
//    | JsonValue.Record x -> 
//        let recordlist = Array.toList x
//        //List of parseresults, ordered by data
//        let results = ListHelperFunctions.ListOfFuncListOfValuesApply entrymatchers recordlist
//        //List of parseresults, ordered by matcher
//        let resultsreversed = (ListHelperFunctions.ReverseNonJaggedNest results).Value //ListOfFuncListOFValuesApply always returns an unjagged list of lists
//        ProcessResults resultsreversed results
//    | _ -> CreateRootErrorMessage TypeNames.Record jsonvalue

let MatchEntryInRecord (matcher : RecordMatcher<'a>) (entries : (string * JsonValue) list) =
    let resultsandvalue = (fun x -> matcher x, x) |> List.map <| entries
    let resultsonly list = List.map fst list
    if resultsonly resultsandvalue |> IsSingleOk
    then 
        let matchremoved = List.where (fun x -> fst x |> IsError) resultsandvalue |> List.map snd
        resultsonly resultsandvalue |> SingleOk |> Result.map (fun x -> x , matchremoved)
    else 
        if resultsonly resultsandvalue |> IsMultiOk
        then RootError "A matcher matched multiple entries, it should only match one" |> Error
        else ("No matches were found for an entry" , (resultsonly resultsandvalue |> ExtractErrors |> MultiError)) |> ParentError |> Error

let MatchRecord jsonvalue =
    match jsonvalue with
    | JsonValue.Record x -> Array.toList x |> Ok
    | _ -> CreateRootErrorMessage TypeNames.Record jsonvalue

let ResultFst value = Result.map fst value
let ResultSnd value = Result.map snd value

let MatchStringBound = Bind MatchString "In MatchString"
let MatchBoolBound = Bind MatchBool "In MatchBool"
let MatchFloatBound = Bind MatchFloat "In MatchFloat"
let MatchIntBound = Bind MatchInt "In MatchInt"
let MatchListBound f = Bind (MatchList f) "In MatchList"
let MatchRecordEntryBound name valuematch = Bind (MatchRecordEntry name valuematch) "In MatchRecordEntry"
let MatchRecordBound  = Bind MatchRecord "In MatchRecord"
let MatchAnyBound entrymatcher = Bind (MatchAny entrymatcher) "In MatchAny"