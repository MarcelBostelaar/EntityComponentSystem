module ParseAndSort

open ParsedDataStructure
open RecordToFieldList
open Types
open ResultHelperFunctions
open TopologicalSortFsharp

let private parsesingledata (data : string * ParsedData) =
    TupleMaps.MapTupleSnd RecordToFieldlist data |> TupleMaps.ResultTupleRight

///<summary>Turns a list of key-data pairs into a list of key-parsedfield pairs, or an error if it isnt a parsedfield</summary>
let private parsedata data =
    List.map parsesingledata data |> OnlyOk

let private IDgetter (data : string * ParsedField list) = fst data
///<summary>Selects the dependencies from a parsed data that is not part of the primitives given to it (IE if you give it 'int' then it will only return dependencies that arent 'int')</summary>
let private DependencySelector primitives (data : string * ParsedField list) = 
    let dependencies = snd data |> List.map (fun x -> x.typename) |> set //the depenencies of some data
    Set.difference dependencies primitives |> Set.toSeq //the dependencies of some data that are not a primitive

let private IDComparer (a:string) (b:string) = a=b

let private Sort primitives data = 
    let dependencygetter = DependencySelector primitives
    SortTopologically IDgetter dependencygetter IDComparer data |> Result.mapError ErrorUnion.TopologicalSortError

let ParseAndSort(primitivetypenames: string Set) data= 
    let parsed = parsedata data |> Result.mapError ErrorUnion.ParserError
    Result.bind (Sort primitivetypenames) parsed