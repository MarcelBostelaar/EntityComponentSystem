module TestFsharpLibrary

open TestLibrary
open MatchBuild
open ParsedDataStructure
open JsonParsedDataConverter


let matchint name i = Ok 1
let matchfloat name i = Ok 1.1f
let matchstring name i = Ok "Hoi"

let serializeint (i : int) = ParsedData.Number {value = bigint i; exponent= 0I; num_base= 10I}
let serializefloat (i : float32) = naivefloattoexponent (float i) |> ParsedData.Number 
let serializestring (i : string) = ParsedData.String i

let constructorwrapper a b c= ExampleClass (a,b,c)
let builder = MatchBuild3 constructorwrapper (matchint "one") (matchfloat "two") (matchstring "three")
 
let deconstructer (i : ExampleClass) = ["one", serializeint i.one; "two", serializefloat i.two; "three", serializestring i.three] |> Record