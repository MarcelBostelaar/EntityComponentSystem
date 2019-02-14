module ErrorStringifier

open ParserErrors
open System
open ParsedDataStructure
open JsonParsedDataConverter
open TopologicalSortFsharp
open ErrorUnion
open Types

let StringifyEntries (entries: (string*ParsedData) list) =
    ParsedDataToJson <| Record entries

let StringifyErrorDescription (description : ErrorDescription) = 
    match description with
    | ErrorDescription.String x -> x
    | TypeMismatch x -> String.Format("Expected type '{0}', got '{1}' instead.", x.expectedType, x.actual)
    | NameMismatch x -> String.Format("Expected name '{0}', got '{1}' instead.", x.expectedName, x.actual)
    | MultipleMatched x -> String.Format("Multiple entries were matched:\n{0}", StringifyEntries x)
    | UnmatchedEntries x -> String.Format("One or more entries were not matched:\n{0}", StringifyEntries x)

let rec StringifyErrorTrace (trace : ErrorTrace<ErrorDescription>) = 
    match trace with
    | RootError x -> StringifyErrorDescription x
    | ParentError (parent, child) -> String.Format("{0}\n{{{1}\n}}", StringifyErrorDescription parent, StringifyErrorTrace child)
    | MultiError x -> List.map StringifyErrorTrace x |> String.concat ",\n\n" |> fun x -> "Multiple Errors:\n" + x

let ParsedFieldToData field = 
    fst field, (List.map (fun x -> x.name, ParsedData.String x.typename) (snd field) |> Record)

let missingdependencytodataentry (missing: (string*ParsedField list)*string) =
    let (data, missingid) = missing
    let data = ParsedFieldToData data
    ["Data", Record [data]; "Missing ID", String missingid] |> Record
   
let allmissingdependencies (missinglist : seq<(string*ParsedField list)*string>) = 
    Seq.map missingdependencytodataentry missinglist |> Seq.toList |> List

let StringifyTopologicalSortError (error : TopologicalSortFsharp.Error<string, string*ParsedField list>) =
    match error with
    | Ids_not_unique x -> String.Format("While topologically sorting, some IDs were not unique:\n{0}", String.concat ", " x)
    | Dependencies_dont_exist x -> String.Format("While topologically sorting, some dependencies were missing:\n{0}", ParsedDataToJson <| allmissingdependencies x)
    | Circular_dependencies_exist x -> String.Format("While topologically sorting, some dependencies were circular, which is not allowed.\nThe following data has circular dependencies:\n{0}", Seq.toList x |> List.map ParsedFieldToData |> Record |> ParsedDataToJson)

let StringifyUnionError (error : ErrorUnion<string, string * ParsedField list, ErrorDescription>) =
    match error with
    | ParserError x -> StringifyErrorTrace x
    | TopologicalSortError x -> StringifyTopologicalSortError x
    