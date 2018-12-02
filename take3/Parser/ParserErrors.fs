module ParserErrors
open System
open FSharp.Data

type ErrorTrace=
    | RootError of string
    | ParentError of string * ErrorTrace
    | MultiError of ErrorTrace list

let IntOrFloat (number : float) =
    if number % 1.0 = 0.0 then TypeNames.Integer else TypeNames.Float

let rec ValueName jsonvalue = 
    match jsonvalue with
    | JsonValue.String _ -> TypeNames.String
    | JsonValue.Number x -> IntOrFloat (float x)
    | JsonValue.Float x -> IntOrFloat x
    | JsonValue.Array _ -> TypeNames.Array
    | JsonValue.Boolean _ -> TypeNames.Boolean
    | JsonValue.Null _ -> TypeNames.Null
    | JsonValue.Record record -> 
        let NameRecordEntry (entry : string * JsonValue) = String.Format("{0} : {1}", fst entry, ValueName <| snd entry)
        let concatted = Array.map NameRecordEntry record |> String.concat ",\n"
        String.Format("{{0}}", concatted)

let CreateErrorMessage (expectedtype : string) (actualvalue : JsonValue) =
    String.Format("Expected {0}, got {1} instead", expectedtype, ValueName actualvalue)

let CreateRootErrorMessage(expectedtype : string) (actualvalue : JsonValue) =
    CreateErrorMessage expectedtype actualvalue |> RootError |> Error
    
let RootErrorResult message = message |> RootError |> Error
let MultiErrorResult message = message |> MultiError |> Error