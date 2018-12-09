module ListHelperFunctions


let ListOfFuncApply functions value = List.map( fun f -> f value) functions //Applies a value to a list of functions
let ListOfFuncListOfValuesApply functions values = List.map <| ListOfFuncApply functions <| values //applies a list of values to a list of functions, result is value first


let LongestLenght (listoflist : 'a list list) = List.map List.length listoflist |> List.sortDescending |> List.tryHead
let TakeItems somelist N = (fun i -> List.tryItem i somelist) |> Seq.map <| seq {0 .. (N-1)}
let OptionTakeItems somelist = TakeItems somelist |> Option.map
let TakeItemsListList listoflists = (fun list -> OptionTakeItems list <| LongestLenght listoflists) |> List.map <| listoflists

let OptionSequenceToList x = 
    match x with
    | Some v -> Seq.toList v
    | None -> []

/// <summary>
/// Takes a nested list structure and returns an inversely nested version, optioned to handle jagged nests
/// </summary>
/// <param name="listoflist">Any list of lists</param>
let ReverseNest (listoflist : 'a list list) = List.map OptionSequenceToList <| TakeItemsListList listoflist 

/// <summary>
/// Takes a nested list structure and returns an option of an inversely nested version, Some if its not jagged, none if jagged
/// </summary>
/// <param name="listoflist">Any list of lists</param>
let ReverseNonJaggedNest (listoflist : 'a list list) = 
    if List.exists Option.isNone |> List.map <| ReverseNest listoflist |> List.exists id //if (any item in list is none) map to list, apply to list list option<'a>, if any bool is true
    then None
    else List.choose id |> List.map <| ReverseNest listoflist |> Some
