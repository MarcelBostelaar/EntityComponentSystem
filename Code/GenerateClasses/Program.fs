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
open FSharp.Data
open MatchBuild
open SettingsHandler
let extention = ".json"

let withextention (extention:string) (file : string) =
    if file.EndsWith(extention) then Some file else None
let FilterExtention extention files =
    List.choose (withextention extention) files
let OnlyFileName (extention:string) (file : string) =
    file.Substring(0, file.Length - extention.Length) |> fun x -> x.Split("\\") |> fun x -> (List.rev (Array.toList x)).Head

let loadfile (path:string) =
    let reader = (new StreamReader(path))
    let data = reader.ReadToEnd()
    reader.Close()
    data


let LoadFiles (settings : settings) = 
    Directory.GetFiles(settings.TargetSourceLocation) |> Seq.toList |> FilterExtention extention |> List.map (fun x -> OnlyFileName extention x, loadfile x)
let ReadJson = TupleMaps.MapTupleSnd ParseJson >> TupleMaps.ResultTupleRight |> List.map >> ListResultMap.ResultListMap >> ResultMap.MapError1 MultiError >> ResultMap.MapError1 ErrorUnion.ParserError
let LoadParseSort settings = 
    LoadFiles settings |> ReadJson |> Result.bind (ParseAndSort settings.Primitives) |> Result.map List.rev
let CreateClasses (settings : settings)= (fun x -> fst x, FullWriter settings.Primitives "Testnamespace" x) |> List.map |> Result.map

let MasterBuild settings =
    LoadParseSort settings |> CreateClasses settings
    
let WriteClassToFile path extention (item: string*string) =
    let out = new StreamWriter(path + "\\" + fst item + extention)
    out.Write(snd item)
    out.Close()

let HandleResult result (settings : settings)=
    match result with
    | Ok i -> ignore <| List.map (WriteClassToFile settings.TargetBuildLocation ".cs") i
    | Error i -> Console.WriteLine("An error has occurred")

let MasterFunction unit = 
    let settings = LoadSettings () 
    let result = Result.mapError ErrorUnion.ParserError settings |> Result.bind MasterBuild
    ignore <| Result.map (HandleResult result) settings
    ()


[<EntryPoint>]
let main argv =
    //let settings = LoadSettings

    //read all files in directory
    //settings.json contains: list of primitives, a target build location, a target source
    //read all json files
    //parse them
    //write 


//    let positionexample = "position", """
//{
//  "xpos": "int",
//  "ypos": "int"
//}"""
//    let complexexample = "test", """
//{
//  "myposition": "position",
//  "favnumber": "int"
//}"""
//    let primitives = ["int"] |> set
//    let allfiles = [positionexample;complexexample]
//    let parsedjson =  parsealljson allfiles
//    let sorted = ParseAndSort primitives |> Result.bind <| parsedjson |> Result.map List.rev
//    let classify = (fun x -> fst x, FullWriter primitives "Testnamespace" x) |> List.map |> Result.map
//    let files = classify sorted
    let files = MasterFunction ()
    0 // return an integer exit code
