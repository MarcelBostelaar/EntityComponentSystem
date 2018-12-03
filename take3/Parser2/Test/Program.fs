// Learn more about F# at http://fsharp.org

open System
open FSharp.Data
open ParserBase
open ParserErrors
open ResultHelperFunctions

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

    let BuildPoint2 x y = {x=x ; y=y}
    
    let matchx = MatchRecordEntry "x" MatchInt |> MatchEntryInRecord
    let matchy = MatchRecordEntry "y" MatchInt |> MatchEntryInRecord


    let matchpoint2 values=
        matchx values |> TupleSecondApplyBound matchy


    //let MatchPoint2 (values : (string * JsonValue) list ) = 
    //    if values.Length <> 2 
    //    then """Expected record of form {"x":int, "y":int} with two elements, got record with different number of elements instead""" |> RootError |> Error 
    //    else 
    //        let x = matchArray (fun value -> if fst value <> "x" then RootError "Name should be x" |> Error else MatchInt (snd value)) values
    //        let y = matchArray (fun value -> if fst value <> "y" then RootError "Name should be y" |> Error else MatchInt (snd value)) values
    //        Map2 BuildPoint2 "In BuildPoint" x y

    


    //let matchExample json = */

    0 // return an integer exit code
