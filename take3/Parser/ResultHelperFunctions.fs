module ResultHelperFunctions

open ParserErrors

let Bind f error value=
    match value with
    | Ok x -> f x
    | Error x -> (error , x) |> ParentError |> Error

let apply error x f = 
    match f with
    | Error i -> (error , i) |> ParentError |> Error
    | Ok i ->
        match x with
        | Error j -> (error , j) |> ParentError |> Error
        | Ok j -> i j |> Ok

let applynoerror x f =
    match f with
    | Error i -> i
    | Ok i ->
        match x with
        | Error j -> j
        | Ok j -> i j |> Ok
    

let Map1 f error a =
    Ok f |> apply error a
    
let Map2 f error a b =
    Map1 f error a |> apply error b
    
let Map3 f error a b c =
    Map2 f error a b |> apply error c
    
let Map4 f error a b c d =
    Map3 f error a b c |> apply error d
    
let Map5 f error a b c d e =
    Map4 f error a b c d |> apply error e

    
let OptionOfError result = 
    match result with
    | Error x -> Some x
    | _ -> None
    
let OptionOfOk result =
    match result with
    | Ok x -> Some x
    | _ -> None

let ExtractErrors resultlist =
    List.choose OptionOfError resultlist

let ExtractOks resultlist =
    List.choose OptionOfOk resultlist
    
let IsOk result =
    match result with
    | Ok _ -> true
    | Error _ -> false

let IsError result =
    match result with
    | Ok _ -> false
    | Error _ -> true

let IsMultiOk resultlist =
    (ExtractErrors resultlist).Length > 1

let FirstOk resultlist =
    List.choose OptionOfOk resultlist |> List.tryHead

let SingleOk resultlist =
    let oks = List.choose OptionOfOk resultlist
    if oks.Length = 1 then Ok oks.Head else "List contains multiple positive results where one is expected" |> RootErrorResult

let IsSingleOk resultlist =
    match SingleOk resultlist with
    | Ok _ -> true
    | Error _ -> false
    
let AllOk resultlist =
    List.exists IsError resultlist |> not

let AllError resultlist =
    List.exists IsOk resultlist |> not

let OptionToResult error option=
    match option with
    | Some x -> Ok x
    | None -> RootError error |> Error