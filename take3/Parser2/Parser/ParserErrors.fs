module ParserErrors
    open System
    open FSharp.Data

    type ErrorTrace=
        | RootError of string
        | ParentError of string * ErrorTrace
        | MultiError of ErrorTrace[]

    let IntOrFloat (number : float) =
        if number % 1.0 = 0.0 then "integer" else "float"

    let rec ValueName jsonvalue = 
        match jsonvalue with
        | JsonValue.String _ -> "string"
        | JsonValue.Number x -> IntOrFloat (float x)
        | JsonValue.Float x -> IntOrFloat x
        | JsonValue.Array _ -> "array"
        | JsonValue.Boolean _ -> "boolean"
        | JsonValue.Null _ -> "null"
        | JsonValue.Record record -> 
            let NameRecordEntry (entry : string * JsonValue) = String.Format("{0} : {1}", fst entry, ValueName <| snd entry)
            let concatted = Array.map NameRecordEntry record |> String.concat ",\n"
            String.Format("{{0}}", concatted)

    let CreateErrorMessage (expectedtype : string) (actualvalue : JsonValue) =
        String.Format("Expected {0}, got {1} instead", expectedtype, ValueName actualvalue)

    let CreateRootError (expectedtype : string) (actualvalue : JsonValue) =
        CreateErrorMessage expectedtype actualvalue |> RootError |> Error

    let BindError f error value=
        match value with
        | Ok x -> f x
        | Error x -> (error , x) |> ParentError |> Error

    let Map2 (f: 'a -> 'b -> 'c) error (a:Result<'a, ErrorTrace>) (b:Result<'a, ErrorTrace>) =
        match a with
        | Ok x ->
            match b with
            |Ok y -> f x y |> Ok
            |Error y -> (error , y) |> ParentError |> Error
        | Error x -> (error , x) |> ParentError |> Error