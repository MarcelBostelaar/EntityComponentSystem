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

let Map1 f error a =
    Ok f |> apply error a
    
let Map2 func error a b =
    Map1 func error a |> apply error b

let Map3 func error a b c =
    Map2 func error a b |> apply error c

let Map4 func error a b c d =
    Map3 func error a b c |> apply error d

let Map5 func error a b c d e =
    Map4 func error a b c d |> apply error e

let Map6 func error a b c d e f =
    Map5 func error a b c d e |> apply error f

let Map7 func error a b c d e f g =
    Map6 func error a b c d e f |> apply error g

let Map8 func error a b c d e f g h =
    Map7 func error a b c d e f g |> apply error h

let Map9 func error a b c d e f g h i =
    Map8 func error a b c d e f g h |> apply error i

let Map10 func error a b c d e f g h i j =
    Map9 func error a b c d e f g h i |> apply error j

let Map11 func error a b c d e f g h i j k =
    Map10 func error a b c d e f g h i j |> apply error k

let Map12 func error a b c d e f g h i j k l =
    Map11 func error a b c d e f g h i j k |> apply error l

let Map13 func error a b c d e f g h i j k l m =
    Map12 func error a b c d e f g h i j k l |> apply error m

let Map14 func error a b c d e f g h i j k l m n =
    Map13 func error a b c d e f g h i j k l m |> apply error n

let Map15 func error a b c d e f g h i j k l m n o =
    Map14 func error a b c d e f g h i j k l m n |> apply error o

let Map16 func error a b c d e f g h i j k l m n o p =
    Map15 func error a b c d e f g h i j k l m n o |> apply error p

let Map17 func error a b c d e f g h i j k l m n o p q =
    Map16 func error a b c d e f g h i j k l m n o p |> apply error q

let Map18 func error a b c d e f g h i j k l m n o p q r =
    Map17 func error a b c d e f g h i j k l m n o p q |> apply error r

let Map19 func error a b c d e f g h i j k l m n o p q r s =
    Map18 func error a b c d e f g h i j k l m n o p q r |> apply error s

let Map20 func error a b c d e f g h i j k l m n o p q r s t =
    Map19 func error a b c d e f g h i j k l m n o p q r s |> apply error t

//Python generation code
//------------------------------------------------------------------------
//offset = 97
//def create(lenght):
//    letters = [chr(x+offset) for x in range(lenght)]
//    return """let Map{} func error {} =
//    Map{} func error {} |> apply error {}""".format(
//        lenght, " ".join(letters),
//        lenght-1,
//        " ".join(letters[:-1]),
//        letters[-1])
//
//file = open("text.fs","w")
//for i in range(2, 21):
//    file.write(create(i))
//    file.write("\n\n")
//-----------------------------------------------------------------------