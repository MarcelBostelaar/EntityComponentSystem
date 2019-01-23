module placeholderfilename

open ParsedDataStructure
open RecordToFieldList
open Types
open ResultHelperFunctions
open TopologicalSortFsharp
open ParserErrors

let parsesingledata (data : string * ParsedData) =
    TupleMaps.MapTupleSnd RecordToFieldlist data |> TupleMaps.ResultTupleRight

let parsedata data =
    List.map parsesingledata data |> OnlyOk

let IDgetter (data : string * ParsedField list) = fst data
let DependencySelector (data : string * ParsedField list) = snd data |> List.map (fun x -> x.typename) |> List.toSeq
let IDComparer (a:string) (b:string) = a=b

let Sort data = SortTopologically IDgetter DependencySelector IDComparer data |> Result.mapError ErrorUnion.TopologicalSortError

let ParseAndSort data = parsedata data |> Result.mapError ErrorUnion.ParserError |> Result.bind Sort