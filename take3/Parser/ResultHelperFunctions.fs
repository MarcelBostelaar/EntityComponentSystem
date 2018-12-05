module ResultHelperFunctions

open ParserErrors

let Bind f error value=
    match value with
    | Ok x -> 
        match f x with
        | Ok y -> Ok y
        | Error y -> (error, y) |> ParentError |> Error
    | Error x -> Error x
    
let Apply x f =
    match f with
    | Error i -> i
    | Ok i ->
        match x with
        | Error j -> j
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

let Map1 func a =
    Ok func |> Apply a

let Map2 func a b =
    Ok func |> Apply a |> Apply b

let Map3 func a b c =
    Ok func |> Apply a |> Apply b |> Apply c

let Map4 func a b c d =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d

let Map5 func a b c d e =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e

let Map6 func a b c d e f =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f

let Map7 func a b c d e f g =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g

let Map8 func a b c d e f g h =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h

let Map9 func a b c d e f g h i =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i

let Map10 func a b c d e f g h i j =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j

let Map11 func a b c d e f g h i j k =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k

let Map12 func a b c d e f g h i j k l =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l

let Map13 func a b c d e f g h i j k l m =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m

let Map14 func a b c d e f g h i j k l m n =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n

let Map15 func a b c d e f g h i j k l m n o =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o

let Map16 func a b c d e f g h i j k l m n o p =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o |> Apply p

let Map17 func a b c d e f g h i j k l m n o p q =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o |> Apply p |> Apply q

let Map18 func a b c d e f g h i j k l m n o p q r =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o |> Apply p |> Apply q |> Apply r

let Map19 func a b c d e f g h i j k l m n o p q r s =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o |> Apply p |> Apply q |> Apply r |> Apply s

let Map20 func a b c d e f g h i j k l m n o p q r s t =
    Ok func |> Apply a |> Apply b |> Apply c |> Apply d |> Apply e |> Apply f |> Apply g |> Apply h |> Apply i |> Apply j |> Apply k |> Apply l |> Apply m |> Apply n |> Apply o |> Apply p |> Apply q |> Apply r |> Apply s |> Apply t

//Python generation code
//------------------------------------------------------------------------
//offset = 97
//def create(lenght):
//    letters = " ".join([chr(x+offset) for x in range(lenght)])
//    applies = " ".join(["|> Apply {}".format(chr(x+offset)) for x in range(lenght)])
//    return """let Map{} func {} =
//    Ok func {}""".format(
//        lenght,
//        letters,
//        applies)

//file = open("GeneratedCode.fs","w")
//for i in range(1, 21):
//    file.write(create(i))
//    file.write("\n\n")
//-----------------------------------------------------------------------