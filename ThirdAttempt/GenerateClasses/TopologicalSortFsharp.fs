module TopologicalSortFsharp

open TopologicalSort
open System

let private CreateFunc1 f =
    new Func<'a,'b>(f)
let private CreateFunc2 f =
    new Func<'a,'b,'c>(f)

let private WrappedSort id_selecter dependency_selecter equality_function values=
    TopologicalSort.Functions.TopologicalSort(
        values, 
        CreateFunc1 id_selecter, 
        CreateFunc1 dependency_selecter, 
        CreateFunc2 equality_function)

let Sort id_selecter dependency_selecter equality_function values=
    let result = WrappedSort id_selecter dependency_selecter equality_function values
    match System.Linq.Enumerable.Count (snd result) with
    | 0 -> fst result |> Seq.map (fun x -> x.value) |> Seq.toList |> Ok
    | _ -> snd result |> Seq.map (fun x -> x.value) |> Seq.toList |> Error