module JsonParsedDataConverter

open FSharp.Data
open ResultHelperFunctions
open ParserErrors
open ParsedDataStructure

let private parseJson jsonstring =
    JsonValue.TryParse(jsonstring) |> OptionToResult (ErrorDescription.String "Could not parse json")

let rec private jsonToData parsedJson =
    match parsedJson with
    | JsonValue.String x -> ParsedData.String x
    | JsonValue.Boolean x -> ParsedData.Boolean x
    | JsonValue.Number x -> ParsedData.Float (float x)
    | JsonValue.Float x -> ParsedData.Float x
    | JsonValue.Null -> ParsedData.Null
    | JsonValue.Array x -> Array.map jsonToData x |> Array.toList |> ParsedData.List
    | JsonValue.Record x -> TupleMaps.MapTupleSnd jsonToData |> Array.map <| x |> Array.toList |> ParsedData.Record

let ParseJson jsonstring =
    parseJson jsonstring |> Result.map jsonToData


let rec private DataToJson parseddata = 
    match parseddata with
    | ParsedData.String x -> JsonValue.String x
    | ParsedData.Boolean x -> JsonValue.Boolean x
    | ParsedData.Float x -> JsonValue.Float x
    | ParsedData.Null -> JsonValue.Null
    | ParsedData.List x -> List.map DataToJson x |> List.toArray |> JsonValue.Array
    | ParsedData.Record x -> TupleMaps.MapTupleSnd DataToJson |> List.map <| x |> List.toArray |> JsonValue.Record

let ParsedDataToJson parseddata=
    (DataToJson parseddata).ToString JsonSaveOptions.None