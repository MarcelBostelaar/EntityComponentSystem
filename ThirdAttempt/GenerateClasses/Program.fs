// Learn more about F# at http://fsharp.org

open System
open TypeBuilder
open RecordToFieldList
open System.IO

[<EntryPoint>]
let main argv =
    let makefield name typename typematcher serializername = {fieldname = name; typename = typename; serializername = serializername; typematcher = typematcher}
    let examplefields = [ 
        makefield "field1" "int" "MatchInt" "(fun x -> ParsedData.Float ((float)x))" ;
        makefield "field2" "float" "MatchFloat" "SerializeFloat";
        makefield "field3" "bool" "MatchBool" "SerializeBool"]
    
    // make sure a matchbuild with the right size exists by generating it in this library and including it as its own file in the generated library. Make an f# version of the generation code
    let writer = StreamWriter("F:/Projects/EntityComponentSystem/ThirdAttempt/testenvironmentfsharp/generatedexampleclass.fs")
    writer.Write(
"module TestType
open MatchBuild
open ParserBase
open ParsedDataStructure\n\n")
    writer.Write((BuildFullType "TestType" examplefields))
    writer.Close()



    //Console.Write (String.Format("type {0} = {{ {1} }}", "typename", "here be types"))
    Console.ReadKey true
    0 // return an integer exit code
