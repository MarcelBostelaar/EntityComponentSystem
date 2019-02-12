module TopologicalSortFsharp

open System

type Error<'id,'value>=
    | Ids_not_unique of 'id seq 
    | Dependencies_dont_exist of ('value*'id) seq
    | Circular_dependencies_exist of 'value seq

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

let private Sort id_selecter (dependency_selecter: 'value -> 'id seq) equality_function (values: 'value seq)=
    let result = WrappedSort id_selecter dependency_selecter equality_function values
    match System.Linq.Enumerable.Count (snd result) with
    | 0 -> fst result |> Seq.toList |> Ok
    | _ -> snd result |> Circular_dependencies_exist |> Error

let rec private SeqCountBy equality_func values =
    if Seq.length values = 0 then
        []
    else
        let first = Seq.head values
        let count = Seq.where (fun x -> equality_func x first) values |> Seq.length
        [first, count] :: (SeqCountBy equality_func (Seq.where (fun x -> not <| equality_func x first) values))
    
let private CheckIDUnique (id_selecter: 'value -> 'id) equality_function (values : 'value seq)=
    let countbyid = Seq.map id_selecter values |> SeqCountBy equality_function |> Seq.concat
    match Seq.forall (fun x -> snd x = 1) countbyid with
    | true -> Ok values
    | false -> Seq.where (fun x -> snd x <> 1) countbyid |> Seq.map fst |> Ids_not_unique |> Error

let private Contains equalityFunc sequence value=
    Seq.map (equalityFunc value) sequence |> Seq.exists (fun x -> x)

let private CheckDependencyExists (id_selecter: 'value -> 'id) (dependency_selecter: 'value -> 'id seq) (equality_function: 'id -> 'id -> bool) (values : 'value seq) =
    let primaryIds = Seq.map id_selecter values
    let dependencies = Seq.map (fun x -> dependency_selecter x |> Seq.map (fun y -> x,y)) values |> Seq.concat
    let nomatches = Seq.where (fun x -> not <| Contains equality_function primaryIds (snd x)) dependencies
    match Seq.length nomatches with
    | 0 -> Ok values
    | _ -> Dependencies_dont_exist nomatches |> Error

/// <summary>Topologically sorts a list of items and returns a sorted list, or the unsortable items if failure.</summary>
/// <param name="id_selecter">Take a value of type 'a and return an ID value.</param>
/// <param name="dependency_selecter">Takes a value of type 'a and returns the ID values of the 'a values it depends on.</param>
/// <param name="equality_function">Takes two ID values and returns if they are equal.</param>
/// <param name="values">A sequency of 'a values.</param>
/// <returns>A sorted list of 'a if the input list was successfully sorted. An error describing what went wrong if it could not be sorted.</returns>
let SortTopologically id_selecter dependency_selecter equality_function values =
    CheckIDUnique id_selecter equality_function values |>
    Result.bind (CheckDependencyExists id_selecter dependency_selecter equality_function) |> 
    Result.bind (Sort id_selecter dependency_selecter equality_function)
