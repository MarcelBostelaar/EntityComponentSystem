module klad

//Base:

//point: {
//	x: 1,
//	y:2
//}

//point: {
//	y:3
//}

//let modifyinplace 

open ParsedDataStructure

let FindAndTransformSingle (name : string) (f : ParsedData -> ParsedData) (data : string*ParsedData) =
    if fst data <> name
    then data
    else (name, f (snd data))

let FindAndRemoveSingle (name : string) (data : string*ParsedData) =
    match name = fst data with
    | true -> None
    | false -> Some data
    
let FindAndTransform (name : string) (f : ParsedData -> ParsedData) (data : (string*ParsedData) list) =
    List.map (FindAndTransformSingle name f) data

let FindAndRemove (name : string) (data : (string*ParsedData) list) =
    List.choose (FindAndRemoveSingle name) data

let private MapRecord f data=
    match data with
    | ParsedData.Record x -> f x |> ParsedData.Record
    | _ -> data
    
let FindTransformRecordEntry (name : string) (f : ParsedData -> ParsedData) (data : ParsedData) =
    MapRecord (FindAndTransform name f) data

let FindRemoveRecordEntry (name : string) (data : ParsedData) =
    MapRecord (FindAndRemove name) data

let AddData (name : string) (data) (record : (string * ParsedData) list) =
    match List.where (fun x -> fst x = name) record |> List.length with
    | 0 -> (name, data) :: record
    | _ -> raise (new System.Exception("This transformation ought not to be possible, tried to add value to a record where an entry with that name already exists"))

let AddRecordEntry name value_to_add data=
    MapRecord (AddData name value_to_add) data

let test = FindRemoveRecordEntry "removethis" |> FindTransformRecordEntry "nested" |> FindTransformRecordEntry "Point"

//FindTransformRecordEntry "point" (FindTransformRecordEntry "nested" (FindRemoveRecordEntry "removethis"))