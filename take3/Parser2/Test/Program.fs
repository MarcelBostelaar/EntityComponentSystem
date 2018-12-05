// Learn more about F# at http://fsharp.org

open System
open FSharp.Data
open ParserBase
open ParserErrors
open ResultHelperFunctions
open RecordChainer

type Point = { x: int; y: string }

[<EntryPoint>]
let main argv =
    let exampleJson = """
    {
  "array": [
    1,
    2,
    3
  ],
  "point":{
    x : 1,
    y: 4
  }
}
"""
    let parsed = JsonValue.TryParse exampleJson |> OptionToResult (ErrorDescription.String "Could not parse json")

    let BuildPoint2 x y= {x=x ; y=y}

    let matchx = MatchRecordEntry "x" MatchInt
    let matchy = MatchRecordEntry "y" MatchString

    let MatchPoint2 = MatchBuild2 BuildPoint2 matchx matchy

    0 // return an integer exit code
