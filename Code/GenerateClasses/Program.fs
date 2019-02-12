// Learn more about F# at http://fsharp.org

open System
open System.IO
open ParseAndSort
open JsonParsedDataConverter
open ParsedDataStructure
open ListResultMap
open ParserErrors
open ErrorUnion
open CSharpClassBuilder


[<EntryPoint>]
let main argv =

    let positionexample = "position", """
{
  "xpos": "int",
  "ypos": "int"
}"""
    let complexexample = "test", """
{
  "myposition": "position",
  "favnumber": "int"
}"""
    let primitives = ["int"] |> set
    let allfiles = [positionexample;complexexample]
    let parsealljson = TupleMaps.MapTupleSnd ParseJson >> TupleMaps.ResultTupleRight |> List.map >> ListResultMap.ResultListMap >> ResultMap.MapError1 MultiError >> ResultMap.MapError1 ErrorUnion.ParserError
    let parsedjson =  parsealljson allfiles
    let sorted = ParseAndSort primitives |> Result.bind <| parsedjson
    let classify = Result.map (FullWriter primitives "Testnamespace" |> List.map)
    let files = classify sorted

    let out = new StreamWriter("Testfile.cs")
    let i = 10
    match files with
    | Ok x -> out.WriteLine(String.concat "\n\n" x)
    | Error x -> ()
    out.Close();

    //Console.Write (String.Format("type {0} = {{ {1} }}", "typename", "here be types"))
    //Console.ReadKey true
    0 // return an integer exit code
