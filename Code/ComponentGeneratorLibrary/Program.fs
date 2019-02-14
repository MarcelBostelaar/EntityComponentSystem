module masterfunctionality

open System
open System.IO
open ParseAndSort
open JsonParsedDataConverter
open ParserErrors
open CSharpClassBuilder
open SettingsHandler
open ErrorStringifier
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
    Directory.GetFiles(settings.TargetSourceLocation) |> Seq.toList |> FilterExtention extention |> List.map (fun x -> OnlyFileName extention x, loadfile x)
let private ReadJson = TupleMaps.MapTupleSnd ParseJson >> TupleMaps.ResultTupleRight |> List.map >> ListResultMap.ResultListMap >> ResultMap.MapError1 MultiError >> ResultMap.MapError1 ErrorUnion.ParserError
let private LoadParseSort settings = 
    LoadFiles settings |> ReadJson |> Result.bind (ParseAndSort settings.Primitives) |> Result.map List.rev
let private CreateClasses (settings : settings)= (fun x -> fst x, FullWriter settings.Primitives "Testnamespace" x) |> List.map |> Result.map

let private MasterBuild settings =
    LoadParseSort settings |> CreateClasses settings
    
let private WriteClassToFile path extention (item: string*string) =
    let out = new StreamWriter(path + "\\" + fst item + extention)
    out.Write(snd item)
    out.Close()

let private HandleResult result (settings : settings)=
    match result with
    | Ok i -> ignore <| List.map (WriteClassToFile settings.TargetBuildLocation ".cs") i; Console.WriteLine("Succesfully generated code")
    | Error i -> Console.WriteLine(String.Format("An error has ocurred:\n{0}", StringifyUnionError i))

let MasterFunction unit = 
    let settings = LoadSettings () 
    let result = Result.mapError ErrorUnion.ParserError settings |> Result.bind MasterBuild
    ignore <| Result.map (HandleResult result) settings
    ()