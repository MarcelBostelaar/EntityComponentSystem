module ParsedDataStructure

open System

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

    let RecordEntryPaste possiblereplacements entry=
        match List.where (fun x -> fst x = fst entry) possiblereplacements with
        | [] -> entry
        | [replacement] -> fst entry, PasteOver (snd entry) (snd replacement)
        | _ -> raise (Exception("There ought not to be records with identical names"))

    List.map (RecordEntryPaste pasteRecord) baseRecord