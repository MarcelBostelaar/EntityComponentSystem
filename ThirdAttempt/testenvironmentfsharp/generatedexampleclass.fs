module TestType
open MatchBuild
open ParserBase
open ParsedDataStructure

type TestType = { 
    field1: int;
    field2: float;
    field3: bool 
}

let ConstructTestType arg1 arg2 arg3 = {
    field1 = arg1;
    field2 = arg2;
    field3 = arg3 
}

let private Matchfield1 = MatchRecordEntry "field1" MatchInt
let private Matchfield2 = MatchRecordEntry "field2" MatchFloat
let private Matchfield3 = MatchRecordEntry "field3" MatchBool

let MatchTestType = MatchBuild3 ConstructTestType Matchfield1 Matchfield2 Matchfield3

let SerializeTestType value = [ "field1" , (fun x -> ParsedData.Float ((float)x)) value.field1 ; "field2" , SerializeFloat value.field2 ; "field3" , SerializeBool value.field3 ] |> ParsedData.Record