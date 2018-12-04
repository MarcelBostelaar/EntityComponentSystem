// Learn more about F# at http://fsharp.org

open System
open FSharp.Data
open ParserBase
open ParserErrors
open ResultHelperFunctions
open RecordChainer

type Point = { x: int; y: int }

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

    let ListOfFuncApply functions value = List.map( fun f -> f value) functions //Applies a value to a list of functions
    let ListOfFuncListOfValuesApply functions values = List.map <| ListOfFuncApply functions <| values //applies a list of values to a list of functions

    let functions = [(fun x -> x);(fun x -> x);(fun x -> x);(fun x -> x);(fun x -> x);(fun x -> x);(fun x -> x)]
    let values = [1;2;3]

    let result = ListOfFuncListOfValuesApply functions values

    let showmefuck = result

    let parsed = JsonValue.TryParse exampleJson |> OptionToResult "Could not parse json"    

    let BuildPoint2 y x = {x=x ; y=y}
        
    let matchx = EntryChainer "x" MatchInt "In MatchX"
    let matchy = EntryChainer "y" MatchInt "In MatchY"

    let point2Matching values=
        values |> EntryChainStarter |> matchx |> matchy |>  EntryChainFinisher

    let point2Building values =
        ApplyChainStart (Ok BuildPoint2) values |> ApplyChain |> ApplyChain |> ApplyChainFinish

    let MatchPoint2 values = point2Matching values |> point2Building

    0 // return an integer exit code
