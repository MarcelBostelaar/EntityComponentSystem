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

let Map1 func error a =
    let la x = apply error x
    Ok func |> la a

let Map2 func error a b =
    let la x = apply error x
    Ok func |> la a |> la b

let Map3 func error a b c =
    let la x = apply error x
    Ok func |> la a |> la b |> la c

let Map4 func error a b c d =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d

let Map5 func error a b c d e =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e

let Map6 func error a b c d e f =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f

let Map7 func error a b c d e f g =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g

let Map8 func error a b c d e f g h =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h

let Map9 func error a b c d e f g h i =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i

let Map10 func error a b c d e f g h i j =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j

let Map11 func error a b c d e f g h i j k =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k

let Map12 func error a b c d e f g h i j k l =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l

let Map13 func error a b c d e f g h i j k l m =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m

let Map14 func error a b c d e f g h i j k l m n =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n

let Map15 func error a b c d e f g h i j k l m n o =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o

let Map16 func error a b c d e f g h i j k l m n o p =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o |> la p

let Map17 func error a b c d e f g h i j k l m n o p q =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o |> la p |> la q

let Map18 func error a b c d e f g h i j k l m n o p q r =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o |> la p |> la q |> la r

let Map19 func error a b c d e f g h i j k l m n o p q r s =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o |> la p |> la q |> la r |> la s

let Map20 func error a b c d e f g h i j k l m n o p q r s t =
    let la x = apply error x
    Ok func |> la a |> la b |> la c |> la d |> la e |> la f |> la g |> la h |> la i |> la j |> la k |> la l |> la m |> la n |> la o |> la p |> la q |> la r |> la s |> la t

//Python generation code
//------------------------------------------------------------------------
//offset = 97
//def create(lenght):
//    letters = " ".join([chr(x+offset) for x in range(lenght)])
//    applies = " ".join(["|> la {}".format(chr(x+offset)) for x in range(lenght)])
//    return """let Map{} func error {} =
//    let la x = apply error x
//    Ok func {}""".format(
//        lenght, 
//        letters,
//        applies)

//file = open("text.fs","w")
//for i in range(1, 21):
//    file.write(create(i))
//    file.write("\n\n")
//-----------------------------------------------------------------------