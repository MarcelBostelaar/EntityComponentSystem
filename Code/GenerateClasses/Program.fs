// Learn more about F# at http://fsharp.org

open System
open TypeBuilder
open RecordToFieldList
open Types
open System.IO
open RecordTransform


[<EntryPoint>]
let main argv =

(*
Roadplan for parsing:
    Programmer:
    Define buildin type parser

    Program:
    Read all fields



*)


    let makefield name typename typematcher serializername = {fieldname = name; typename = typename; serializername = serializername; typematcher = typematcher}
    let examplefields = [ 
        makefield "field1" "int" "MatchInt" "(fun x -> ParsedData.Float ((float)x))" ;
        makefield "field2" "float" "MatchFloat" "SerializeFloat";
        makefield "field3" "bool" "MatchBool" "SerializeBool"]
    
    let writer = new StreamWriter("F:/Projects/EntityComponentSystem/ThirdAttempt/testenvironmentfsharp/generatedexampleclass.fs")
    writer.Write(
"module TestType
open RecordChainer
open ParserBase
open ParsedDataStructure\n\n")
    writer.Write((BuildFullType "TestType" examplefields))
    writer.Close()



    //Console.Write (String.Format("type {0} = {{ {1} }}", "typename", "here be types"))
    //Console.ReadKey true
    0 // return an integer exit code
