// Learn more about F# at http://fsharp.org

open ParserBase
open MatchBuild
open ParsedDataStructure

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
    let parsed = JsonParsedDataConverter.ParseJson exampleJson

    let BuildPoint2 x y= {x=x ; y=y}

    let matchx = MatchRecordEntry "x" MatchInt
    let matchy = MatchRecordEntry "y" MatchString

    let MatchPoint2 = MatchBuild2 BuildPoint2 matchx matchy

    0 // return an integer exit code
