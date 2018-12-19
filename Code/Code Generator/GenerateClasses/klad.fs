﻿module klad

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

let private FindAndTransformSingle (name : string) (f : ParsedData -> ParsedData) (data : string*ParsedData) =
    if fst data <> name
    then data
    else (name, f (snd data))

let private FindAndRemoveSingle (name : string) (data : string*ParsedData) =
    match name = fst data with
    | true -> None
    | false -> Some data

let private FindAndRenameSingle (oldname: string) (newname: string) (data : string*ParsedData)=
    match oldname = fst data with
    | true -> newname, snd data
    | false -> data
    
let private FindAndTransform (name : string) (f : ParsedData -> ParsedData) (data : (string*ParsedData) list) =
    List.map (FindAndTransformSingle name f) data

let private FindAndRemove (name : string) (data : (string*ParsedData) list) =
    List.choose (FindAndRemoveSingle name) data

let private FindAndRename oldname newname data =
    List.map (FindAndRenameSingle oldname newname) data

let private MapRecord f data=
    match data with
    | ParsedData.Record x -> f x |> ParsedData.Record
    | _ -> data
    
let private AddData (name : string) (data) (record : (string * ParsedData) list) =
    match List.where (fun x -> fst x = name) record |> List.length with
    | 0 -> (name, data) :: record
    | _ -> raise (new System.Exception("This transformation ought not to be possible, tried to add value to a record where an entry with that name already exists"))
    
let TransformRecordEntry (name : string) (f : ParsedData -> ParsedData) (data : ParsedData) =
    MapRecord (FindAndTransform name f) data

let RemoveRecordEntry (name : string) (data : ParsedData) =
    MapRecord (FindAndRemove name) data

let RenameRecordEntry oldname newname data =
    MapRecord (FindAndRename oldname newname) data

let AddRecordEntry name value_to_add data=
    MapRecord (AddData name value_to_add) data
    
let (@) entryname func = TransformRecordEntry entryname func

let test = "Point" @ "nested" @ RemoveRecordEntry "removethis"

let test2 = RenameRecordEntry "old" "new" |> TransformRecordEntry "nested" |> TransformRecordEntry "Point"
let testt2 = "Point" @ "nested" @ RenameRecordEntry "old" "new"