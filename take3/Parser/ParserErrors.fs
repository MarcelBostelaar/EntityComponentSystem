module ParserErrors
open System
open ParsedDataStructure

type ErrorTrace<'error>=
    | RootError of 'error
    | ParentError of 'error * ErrorTrace<'error>
    | MultiError of ErrorTrace<'error> list
    
type typemismatch = {expectedType: string; actual: ParsedData}
type namemismatch = {expectedName: string; actual: string}

type ErrorDescription=
    | String of string
    | TypeMismatch of typemismatch
    | NameMismatch of namemismatch
    | MultipleMatched of (string * ParsedData) list
    | UnmatchedEntries of (string * ParsedData) list

//let IntOrFloat (number : float) =
//    if number % 1.0 = 0.0 then TypeNames.Integer else TypeNames.Float

//let rec ValueName data = 
//    match data with
//    | ParsedData.String _ -> TypeNames.String
//    | ParsedData.Float x -> IntOrFloat x
//    | ParsedData.List _ -> TypeNames.List
//    | ParsedData.Boolean _ -> TypeNames.Boolean
//    | ParsedData.Null _ -> TypeNames.Null
//    | ParsedData.Record record -> 
//        let NameRecordEntry entry = String.Format("{0} : {1}", fst entry, ValueName <| snd entry)
//        let concatted = List.map NameRecordEntry record |> String.concat ",\n"
//        String.Format("{{0}}", concatted)

let CreateTypeMismatchError expected actual =
    {expectedType = expected; actual = actual} |> TypeMismatch
let CreateNameMismatchError expected actual =
    {expectedName = expected; actual = actual} |> NameMismatch
    
let CreateRootTypeMismatchErrorResult expected actual =
    CreateTypeMismatchError expected actual |> RootError |> Error
let CreateRootNameMismatchErrorResult expected actual =
    CreateNameMismatchError expected actual |> RootError |> Error
    
let RootErrorResult message = message |> RootError |> Error
let MultiErrorResult message = message |> MultiError |> Error