module masterfunctionality

open System
open System.IO
open ParseAndSort
open JsonParsedDataConverter
open ParserErrors
open CSharpClassBuilder
open SettingsHandler
open ErrorStringifier
open Types
open ErrorUnion
open ResultHelperFunctions

let extention = ".json"

let private withextention (extention:string) (file : string) =
    if file.EndsWith(extention) then Some file else None
let private FilterExtention extention files =
    List.choose (withextention extention) files
let private OnlyFileName (extention:string) (file : string) =
    file.Substring(0, file.Length - extention.Length) |> fun x -> x.Split(Seq.toArray ["\\"], StringSplitOptions.None) |> fun x -> (List.rev (Array.toList x)).Head

let private loadfile (path:string) =
    let reader = (new StreamReader(path))
    let data = reader.ReadToEnd()
    reader.Close()
    data


let private LoadFiles (settings : settings) = 
    Directory.GetFiles(settings.TargetSourceLocation) |> Seq.toList |> FilterExtention extention |> List.map (fun x -> {filename = OnlyFileName extention x; data= loadfile x})

let private ReadJson (data: Filenamepair<string> list) = FilenamepairMap ParseJson >> FilenamepairResultExtract |> List.map >> ListResultMap.ResultListMap >> ResultMap.MapError1 MultiError >> ResultMap.MapError1 ErrorUnion.ParserError <| data


let private LoadParseSort settings = 
    LoadFiles settings |> ReadJson |> Result.bind (ParseAndSort settings.Primitives) |> Result.map List.rev
let private CreateClasses (settings : settings) = (fun x -> FullWriter settings.Primitives "Testnamespace" x) |> List.map |> Result.map

let private MasterBuild settings =
    LoadParseSort settings |> CreateClasses settings
    
let private WriteClassToFile path extention (item: Filenamepair<string>) =
    let out = new StreamWriter(path + "\\" + item.filename + extention)
    out.Write(item.data)
    out.Close()

let private HandleOkResult (result: Filenamepair<string> list) (settings: settings) =
    ignore <| List.map (WriteClassToFile settings.TargetBuildLocation ".cs") result
    Console.WriteLine("Succesfully generated code")

let private HandleErrorResult (result: ErrorUnion<string, Filenamepair<ParsedField list>, ErrorDescription>) =
    Console.WriteLine(String.Format("An error has ocurred:\n{0}", StringifyUnionError result))

let private HandleResult (result : Result<Filenamepair<string> list,ErrorUnion<string, Filenamepair<ParsedField list>, ErrorDescription>>) (settings : settings option)=
    match (result, settings) with
    | Ok R, Some S -> HandleOkResult R S
    | Error R, _ -> HandleErrorResult R
    | Ok R, None -> raise (Exception("This should never happen, cannot have an OK result while not having settings!"))

let MasterFunction () = 
    let settings = LoadSettings () 
    let result = Result.mapError ErrorUnion.ParserError settings |> Result.bind MasterBuild
    HandleResult result (OptionOfOk settings)