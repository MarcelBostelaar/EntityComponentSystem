module ResultHelperFunctions

open ParserErrors

let Bind f error value=
    match value with
    | Ok x -> 
        match f x with
        | Ok y -> Ok y
        | Error y -> (error, y) |> ParentError |> Error
    | Error x -> Error x

let BindString f error value= Bind f (ErrorDescription.String error) value

let ApplyError x f =
    match f with
    | Ok i -> Ok i
    | Error i ->
        match x with
        | Ok j -> Ok j
        | Error j -> i j |> Error

let ApplyOk x f =
    match f with
    | Error i -> Error i
    | Ok i ->
        match x with
        | Error j -> Error j
        | Ok j -> i j |> Ok
    
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
    if oks.Length = 1 then Ok oks.Head |> Some else None

let IsSingleOk resultlist =
    match SingleOk resultlist with
    | Some _ -> true
    | None _ -> false
    
let AllOk resultlist =
    List.exists IsError resultlist |> not

let AllError resultlist =
    List.exists IsOk resultlist |> not

let OptionToResult error option=
    match option with
    | Some x -> Ok x
    | None -> RootError error |> Error
