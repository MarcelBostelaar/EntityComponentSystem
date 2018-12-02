module ParserBase
    open FSharp.Data
    open ParserErrors

    let MatchString jsonvalue =
        match jsonvalue with 
        | JsonValue.String x -> Ok x
        | _ -> CreateRootError "string" jsonvalue

    let MatchBool jsonvalue =
        match jsonvalue with 
        | JsonValue.Boolean x -> Ok x
        | _ -> CreateRootError "boolean" jsonvalue

    let MatchFloat jsonvalue =
        match jsonvalue with 
        | JsonValue.Number x -> float x |> Ok
        | JsonValue.Float x -> Ok x
        | _ -> CreateRootError "float" jsonvalue

    let MatchInt jsonvalue =
        match MatchFloat jsonvalue with
        | Error _ -> CreateRootError "Integer" jsonvalue
        | Ok x -> if x % 1.0 = 0.0 then int x |> Ok else CreateRootError "Integer" jsonvalue

    let MatchArray f jsonvalue =
        match jsonvalue with
        | JsonValue.Array x -> Array.map f x |> Ok
        | _ -> CreateRootError "Array" jsonvalue

    let MatchRecord (recordmatch : (string * JsonValue)[] -> Result<'result, ErrorTrace>) jsonvalue=
        match jsonvalue with
        | JsonValue.Record x -> recordmatch x
        | _ -> CreateRootError "MatchRecord" jsonvalue
        
    let MatchStringBound = BindError MatchString "In MatchString"
    let MatchBoolBound = BindError MatchBool "In MatchBool"
    let MatchFloatBound = BindError MatchFloat "In MatchFloat"
    let MatchIntBound = BindError MatchInt "In MatchInt"
    let MatchArrayBound f = BindError (MatchArray f) "In MatchArray"
    let MatchRecordBound recordmatch = BindError (MatchRecord recordmatch) "In MatchRecord"