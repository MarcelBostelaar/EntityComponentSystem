module ParserBase

open FSharp.Data
open ParserErrors
open ResultHelperFunctions
open System

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

let MatchEntryInRecord (matcher : string * JsonValue -> Result<'a, ErrorTrace>) (entries : (string * JsonValue) list) =
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

let maketuple a b = a,b

let MakeTuple2Result a b = Map2 maketuple "Making Tuple" a b

let TupleSecondApply (f: 'a list -> ('b*('a list))) tuple =
    let first = fst tuple
    let second = snd tuple
    let result = f second
    (first, fst result), snd result

let TupleSecondApplyBound (f: Result<'input,ErrorTrace> -> Result<'output1 * 'output2,ErrorTrace>) (tuple : Result<'x*'input, ErrorTrace>) =
    let first = Result.map fst tuple
    let result = f <| Result.map snd tuple
    let output1 = Result.map fst result
    let output2 = Result.map snd result
    MakeTuple2Result first <| output1 |> MakeTuple2Result  <| output2



let MatchStringBound = Bind MatchString "In MatchString"
let MatchBoolBound = Bind MatchBool "In MatchBool"
let MatchFloatBound = Bind MatchFloat "In MatchFloat"
let MatchIntBound = Bind MatchInt "In MatchInt"
let MatchListBound f = Bind (MatchList f) "In MatchList"
let MatchRecordEntryBound name valuematch = Bind (MatchRecordEntry name valuematch) "In MatchRecordEntry"
let MatchRecordBound  = Bind MatchRecord "In MatchRecord"
let MatchAnyBound entrymatcher = Bind (MatchAny entrymatcher) "In MatchAny"