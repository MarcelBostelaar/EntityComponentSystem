module ParserBase

open ParserErrors
open ResultHelperFunctions
open ParsedDataStructure

let MatchString data =
    match data with 
    | ParsedData.String x -> Ok x
    | _ -> CreateRootTypeMismatchErrorResult TypeNames.String data

let MatchBool data =
    match data with 
    | ParsedData.Boolean x -> Ok x
    | _ ->  CreateRootTypeMismatchErrorResult TypeNames.Boolean data

let MatchFloat data =
    match data with 
    | ParsedData.Float x -> Ok x
    | _ -> CreateRootTypeMismatchErrorResult TypeNames.Float data

let MatchInt data =
    match MatchFloat data with
    | Error _ -> CreateRootTypeMismatchErrorResult TypeNames.Integer data
    | Ok x -> if x % 1.0 = 0.0 then int x |> Ok else CreateRootTypeMismatchErrorResult TypeNames.Integer data

let MatchList f data = 
    match data with
    | ParsedData.List x -> List.map f |> Ok
    | _ -> CreateRootTypeMismatchErrorResult TypeNames.List data

let MatchRecord data =
    match data with
    | ParsedData.Record x -> Ok x
    | _ -> CreateRootTypeMismatchErrorResult TypeNames.Record data

let MatchRecordEntry name valuematch recordentry =
    if fst recordentry <> name
    then CreateRootNameMismatchErrorResult name (fst recordentry)
    else valuematch <| snd recordentry
    
let maketuple a b = a,b

let MakeTuple2Result a b = ResultMap.Map2 maketuple a b

let MatchEntryInRecord matcher (entries : (string* ParsedData) list) =
    let resultsandvalue = (fun x -> matcher x, x) |> List.map <| entries
    let resultsonly list = List.map fst list
    match (resultsonly resultsandvalue).Length with
    | 0 -> (ErrorDescription.String "No matches were found for an entry" , (resultsonly resultsandvalue |> ExtractErrors |> MultiError)) |> ParentError |> Error
    | 1 ->
        let matchremoved = List.where (fun x -> fst x |> IsError) resultsandvalue |> List.map snd //all unmatched entries
        let matchedvalue = (resultsonly resultsandvalue |> SingleOk).Value //the matched value
        Result.map (fun x -> x, matchremoved) matchedvalue
    | _ ->
        let isfirstok a = (fun x -> fst x) a |> IsOk
        List.filter isfirstok resultsandvalue |> List.map snd |> MultipleMatched |> RootError |> Error


let TupleSecondApply (f: 'a list -> ('b*('a list))) tuple =
    let first = fst tuple
    let second = snd tuple
    let result = f second
    (first, fst result), snd result

let TupleSecondApplyBound (f: Result<'input,ErrorTrace<ErrorDescription>> -> Result<'output1 * 'output2,ErrorTrace<ErrorDescription>>) (tuple : Result<'x*'input, ErrorTrace<ErrorDescription>>) =
    let first = Result.map fst tuple
    let result = f <| Result.map snd tuple
    let output1 = Result.map fst result
    let output2 = Result.map snd result
    MakeTuple2Result first <| output1 |> MakeTuple2Result  <| output2

let MatchStringBound = BindString MatchString "In MatchString"
let MatchBoolBound = BindString MatchBool "In MatchBool"
let MatchFloatBound = BindString MatchFloat "In MatchFloat"
let MatchIntBound = BindString MatchInt "In MatchInt"
let MatchListBound f = BindString (MatchList f) "In MatchList"
let MatchRecordEntryBound name valuematch = BindString (MatchRecordEntry name valuematch) "In MatchRecordEntry"
let MatchRecordBound  = BindString MatchRecord "In MatchRecord"