module ParserErrors
open System
open ParsedDataStructure

type ErrorTrace=
    | RootError of string
    | ParentError of string * ErrorTrace
    | MultiError of ErrorTrace list

let IntOrFloat (number : float) =
    if number % 1.0 = 0.0 then TypeNames.Integer else TypeNames.Float

let rec ValueName data = 
    match data with
    | ParsedData.String _ -> TypeNames.String
    | ParsedData.Float x -> IntOrFloat x
    | ParsedData.List _ -> TypeNames.List
    | ParsedData.Boolean _ -> TypeNames.Boolean
    | ParsedData.Null _ -> TypeNames.Null
    | ParsedData.Record record -> 
        let NameRecordEntry entry = String.Format("{0} : {1}", fst entry, ValueName <| snd entry)
        let concatted = List.map NameRecordEntry record |> String.concat ",\n"
        String.Format("{{0}}", concatted)

let CreateErrorMessage (expectedtype : string) actualvalue =
    String.Format("Expected {0}, got {1} instead", expectedtype, ValueName actualvalue)

let CreateRootErrorMessage(expectedtype : string) actualvalue =
    CreateErrorMessage expectedtype actualvalue |> RootError |> Error
    
let RootErrorResult message = message |> RootError |> Error
let MultiErrorResult message = message |> MultiError |> Error