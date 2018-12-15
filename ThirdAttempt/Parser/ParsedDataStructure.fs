module ParsedDataStructure

type ParsedData =
    | String of string
    | Float of float
    | List of ParsedData list
    | Boolean of bool
    | Record of (string*ParsedData) list
    | Null

let rec PasteOver (baseData : ParsedData) (pasteData : ParsedData) =
    match baseData, pasteData with
    | Record a, Record b -> PasteRecord a b |> Record
    | _ , _ -> pasteData

and PasteRecord (baseRecord : (string*ParsedData) list) (pasteRecord : (string*ParsedData) list) =
    //pasteover data where names match