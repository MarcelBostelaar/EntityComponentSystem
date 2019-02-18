module SettingsHandler

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
open System.Diagnostics

let settingsfile = "settings.json"
let TargetBuildFieldname = "TargetBuildLocation"
let TargetSourceFieldname = "TargetSourceLocation"
let PrimitivesFieldName = "Primitives"

let ExampleJson = [
    TargetBuildFieldname,    JsonValue.String "C:\\Example_Build_Directory";
    TargetSourceFieldname,    JsonValue.String "C:\\Example_Source_Directory";
    PrimitivesFieldName,   JsonValue.Array <| Array.map JsonValue.String (Seq.toArray ["float"; "int"])
                    ] |> Seq.toArray |> JsonValue.Record

type settings = {Primitives: string Set; TargetBuildLocation: string; TargetSourceLocation: string}

let private CreateExampleFile unit= 
    let writer = new StreamWriter(settingsfile)
    writer.Write(ExampleJson.ToString JsonSaveOptions.None)
    writer.Close()
    Debug.WriteLine("Writing settings examplefile")

let private settingscreate prims source target = {Primitives = prims; TargetBuildLocation= target; TargetSourceLocation= source}

let private matchstringfield name (data : string*ParsedData)=
    match fst data = name, snd data with
    | (true, ParsedData.String a) -> Ok a
    | false, _ -> RootErrorResult <| ErrorDescription.String (name + " not found in settings")
    | true, _ -> RootErrorResult <| ErrorDescription.String (name + " has to be a string")

let private isstring data =
    match data with
    | ParsedData.String a -> Ok a
    | _ -> RootErrorResult <| ErrorDescription.String ("Value is not a string")

let private arrayisstring data errordescription= 
    List.map isstring data |> ResultListMap |> Result.mapError ErrorTrace.MultiError |> Result.mapError (fun x -> ErrorTrace.ParentError (ErrorDescription.String errordescription, x))

let private stringarraytoset (value:string list) = 
    set value

let private matchstringset name (data : string*ParsedData)=
    match fst data = name, snd data with
    | (true, ParsedData.List a) -> arrayisstring a (String.Format("While trying to parse stringarray {0}", name)) |> Result.map stringarraytoset
    | false, _ -> RootErrorResult <| ErrorDescription.String (name + " not found in settings")
    | true, _ -> RootErrorResult <| ErrorDescription.String (name + " has to be a list of strings")

let private ReadSettings unit =
    let reader = new StreamReader(settingsfile)
    let data = reader.ReadToEnd()
    let json = JsonParsedDataConverter.ParseJson data
    let matchbuild = MatchBuild3 settingscreate <| matchstringset PrimitivesFieldName <| matchstringfield TargetSourceFieldname <|  matchstringfield TargetBuildFieldname
    Result.bind matchbuild json

let private ValidateSourceDirectory (settings : settings) =
    match Directory.Exists(settings.TargetSourceLocation) with
    | true -> Ok settings
    | false -> RootErrorResult <| ErrorDescription.String (String.Format("No such directory '{0}'", settings.TargetSourceLocation))

let private ValidateOrCreateTarget (settings : settings) =
    ignore <| Directory.CreateDirectory(settings.TargetBuildLocation)
    settings

let AddParentStrError error (someerror : Result<'a, ErrorTrace<ErrorDescription>>) =
    Result.mapError (fun x -> ErrorTrace.ParentError (ErrorDescription.String error, x)) someerror

let LoadSettings unit= 
    let doesexist =  File.Exists(settingsfile)
    match doesexist with
    | false -> 
        CreateExampleFile ()
        RootErrorResult <| ErrorDescription.String "No settings file found, generated a new one. Please edit the settings.json file."
    | true -> ReadSettings () |> AddParentStrError "While reading settings file" |> Result.bind ValidateSourceDirectory |> Result.map ValidateOrCreateTarget