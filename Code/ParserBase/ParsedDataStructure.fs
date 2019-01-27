module ParsedDataStructure

open System.Numerics
open System

type BigExponent = {value: bigint; exponent: bigint; num_base: bigint} with
    member this.Simplify = 
        let mutable baseDivisions = 0
        let mutable value = this.value
        while BigInteger.GreatestCommonDivisor(value, this.num_base) = this.num_base do
            baseDivisions <- baseDivisions + 1
            value <- value/this.num_base
        {value = value; exponent= this.exponent + bigint baseDivisions; num_base= this.num_base}
    member this.IsInt = this.Simplify.exponent >= 0I

type ParsedData =
    | String of string
    | Number of BigExponent
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