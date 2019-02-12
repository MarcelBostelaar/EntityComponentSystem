module JsonParsedDataConverter

open FSharp.Data
open ResultHelperFunctions
open ParserErrors
open ParsedDataStructure
open System

let private parseJson jsonstring =
    JsonValue.TryParse(jsonstring) |> OptionToResult (ErrorDescription.String "Could not parse json")

let naivefloattoexponent (floatvalue:float) =
    let mutable value = floatvalue
    let mutable exponent = 0
    while not (value % 1.0 = 0.0) do
        value <- value*2.0
        exponent <- exponent - 1
    {value = bigint value; exponent = bigint exponent; num_base = 2I}

let naiveexponenttofloat exponentvalue =
    let value = float exponentvalue.value
    let exponentmultiplier = Math.Pow(float exponentvalue.num_base, float exponentvalue.exponent)
    value * exponentmultiplier

let rec private jsonToData parsedJson =
    match parsedJson with
    | JsonValue.String x -> ParsedData.String x
    | JsonValue.Boolean x -> ParsedData.Boolean x
    | JsonValue.Number x -> naivefloattoexponent (float x) |> ParsedData.Number
    | JsonValue.Float x -> naivefloattoexponent x |> ParsedData.Number
    | JsonValue.Null -> ParsedData.Null
    | JsonValue.Array x -> Array.map jsonToData x |> Array.toList |> ParsedData.List
    | JsonValue.Record x -> TupleMaps.MapTupleSnd jsonToData |> Array.map <| x |> Array.toList |> ParsedData.Record

let ParseJson jsonstring =
    parseJson jsonstring |> Result.map jsonToData


let rec private DataToJson parseddata = 
    match parseddata with
    | ParsedData.String x -> JsonValue.String x
    | ParsedData.Boolean x -> JsonValue.Boolean x
    | ParsedData.Number x -> JsonValue.Float (naiveexponenttofloat x)
    | ParsedData.Null -> JsonValue.Null
    | ParsedData.List x -> List.map DataToJson x |> List.toArray |> JsonValue.Array
    | ParsedData.Record x -> TupleMaps.MapTupleSnd DataToJson |> List.map <| x |> List.toArray |> JsonValue.Record

let ParsedDataToJson parseddata=
    (DataToJson parseddata).ToString JsonSaveOptions.None