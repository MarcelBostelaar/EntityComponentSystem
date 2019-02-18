module FSharpBuilder

open System
open Types
open SettingsHandler
open MatchBuild
open ListResultMap
open ParserErrors
open ParsedDataStructure

let constructorwrappername = "constructorwrapper"
let functionspacer = "\n\n"

type DeSerializeInfo = {name: string; serializer: string; deserializer: string; modulename: string}

let private BuildConstructorWrapper (data : Filenamepair<DeSerializeInfo list>) = 
    let fieldnames = List.map (fun x -> x.name) data.data
    String.Format("let {3} {0} = {1} ({2})", String.concat " " fieldnames, data.filename, String.concat ", " fieldnames, constructorwrappername)

let private BuildModuleIncludes (data : Filenamepair<DeSerializeInfo list>) =
    List.map (fun x -> x.modulename) data.data |> set |> seq |> Seq.map (fun x -> "open " + x) |> String.concat "\n"

let private BuildDeserializer (data : Filenamepair<DeSerializeInfo list>)=
    let length = data.data.Length
    let buildername = data.filename + "Deserializer"
    let matchbuildstring = GenerateMatchBuild length
    let builder = String.Format(
        "let {3} = MatchBuild{0} {1} {2}",
        length,
        constructorwrappername,
        List.map (fun (x : DeSerializeInfo) -> "(" + x.deserializer + " \"" + x.name + "\")") data.data |> String.concat " ",
        buildername
    )
    matchbuildstring, builder, buildername

let private BuildSerialiser (data: Filenamepair<DeSerializeInfo list>)=
    let serializername = data.filename + "Serializer"
    let functionstring = String.Format("let {0} (i : {1}) = [{2}] |> Record",
        serializername,
        data.filename,
        List.map (fun (x : DeSerializeInfo) -> String.Format("\"{0}\", {1} i.{2}", x.name, x.serializer, x.name)) data.data |> String.concat ", "
    )
    functionstring, serializername

let private UnboundFullBuilder (data : Filenamepair<DeSerializeInfo list>)=
    let topmodule = "module " + data.filename;
    let matchbuildstring, builderstring, deserializername = BuildDeserializer data
    let buildincludestring = BuildModuleIncludes data
    let constructorwrapperstring = BuildConstructorWrapper data
    let serializerstring, serializername = BuildSerialiser data
    let fsharpfilestring = [topmodule; buildincludestring; matchbuildstring;constructorwrapperstring;builderstring;serializerstring] |> String.concat functionspacer
    fsharpfilestring, {name= data.filename; serializer= serializername; deserializer= deserializername; modulename= data.filename}

let private BuildSingleFile data = FilenamepairBind UnboundFullBuilder data

let private replacefield (existingtypes : DeSerializeInfo list) (data : ParsedField)  =
    match List.tryFind (fun (x : DeSerializeInfo) ->x.name = data.typename) existingtypes with
    | Some x -> Ok x
    | None -> Error data

let private replacefields (existingtypes : DeSerializeInfo list) (data: ParsedField list) =
    List.map (replacefield existingtypes) data |> ResultListMap

let private mappedreplacefield existingtypes data = FilenamepairMap (replacefields existingtypes) data |> FilenamepairFullResultExtract

let private TryBuildItem existingtypes data = mappedreplacefield existingtypes data |> Result.map BuildSingleFile

let private ResultmappedListAThenB a b = Result.map (List.append [a]) b

let rec private BuildFromList (data : Filenamepair<ParsedField list> list) (existingtypes : DeSerializeInfo list)=
    match data with
    | [single] -> TryBuildItem existingtypes single |> (Result.map <| FilenamepairMap fst) |> Result.map (fun x -> [x]) //result<Filenamepair<string*deserializeinfo>, 'b> |> result<Filenamepair<string>, 'b> |> result<Filenamepair<string> list, 'b>
    | [] -> Ok []
    | head :: tail -> 
        let newitem = TryBuildItem existingtypes head
        match newitem with
        | Error x -> Error x
        | Ok x -> 
            let fsharpfile = FilenamepairMap fst <| x
            let newtypeinfo = (fun y -> snd y.data) <| x
            ResultmappedListAThenB fsharpfile <| BuildFromList tail (newtypeinfo :: existingtypes)

let private readdeserialiseinfo (settings: settings) : DeSerializeInfo list = ()

let private errorifyparsedfield (field : ParsedField list) = 
    List.map (fun (x : ParsedField) -> x.name, ParsedData.String x.typename) field |> Record |> ParsedDataError |> RootError

let private ErrorToTraceError (error: Filenamepair<ParsedField list>) =
    let neweerror = (ErrorDescription.String "Could not find a deserialisationinfo object for the following parsed fields:", errorifyparsedfield error.data) |> ParentError
    ParentError (ErrorDescription.String ("In file " + error.filename), neweerror)

let FullBuild (data : Filenamepair<ParsedField list> list) (settings: settings) : Result<Filenamepair<string> list, 'b> =
    let existingtypes = readdeserialiseinfo settings
    BuildFromList data existingtypes |> Result.mapError ErrorToTraceError


//read deserializeinfo list from settings file
//make function that takes data and settings file
