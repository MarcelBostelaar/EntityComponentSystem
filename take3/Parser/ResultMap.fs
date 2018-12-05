module ResultMap

open ResultHelperFunctions

let Map1 func a =
    Ok func |> ApplyOk a

let MapError1 func a =
    Error func |> ApplyError a

let Map2 func a b =
    Ok func |> ApplyOk a |> ApplyOk b

let MapError2 func a b =
    Error func |> ApplyError a |> ApplyError b

let Map3 func a b c =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c

let MapError3 func a b c =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c

let Map4 func a b c d =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d

let MapError4 func a b c d =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d

let Map5 func a b c d e =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e

let MapError5 func a b c d e =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e

let Map6 func a b c d e f =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f

let MapError6 func a b c d e f =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f

let Map7 func a b c d e f g =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g

let MapError7 func a b c d e f g =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g

let Map8 func a b c d e f g h =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h

let MapError8 func a b c d e f g h =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h

let Map9 func a b c d e f g h i =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i

let MapError9 func a b c d e f g h i =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i

let Map10 func a b c d e f g h i j =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j

let MapError10 func a b c d e f g h i j =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j

let Map11 func a b c d e f g h i j k =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k

let MapError11 func a b c d e f g h i j k =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k

let Map12 func a b c d e f g h i j k l =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l

let MapError12 func a b c d e f g h i j k l =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l

let Map13 func a b c d e f g h i j k l m =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m

let MapError13 func a b c d e f g h i j k l m =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m

let Map14 func a b c d e f g h i j k l m n =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n

let MapError14 func a b c d e f g h i j k l m n =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n

let Map15 func a b c d e f g h i j k l m n o =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o

let MapError15 func a b c d e f g h i j k l m n o =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o

let Map16 func a b c d e f g h i j k l m n o p =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o |> ApplyOk p

let MapError16 func a b c d e f g h i j k l m n o p =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o |> ApplyError p

let Map17 func a b c d e f g h i j k l m n o p q =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o |> ApplyOk p |> ApplyOk q

let MapError17 func a b c d e f g h i j k l m n o p q =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o |> ApplyError p |> ApplyError q

let Map18 func a b c d e f g h i j k l m n o p q r =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o |> ApplyOk p |> ApplyOk q |> ApplyOk r

let MapError18 func a b c d e f g h i j k l m n o p q r =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o |> ApplyError p |> ApplyError q |> ApplyError r

let Map19 func a b c d e f g h i j k l m n o p q r s =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o |> ApplyOk p |> ApplyOk q |> ApplyOk r |> ApplyOk s

let MapError19 func a b c d e f g h i j k l m n o p q r s =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o |> ApplyError p |> ApplyError q |> ApplyError r |> ApplyError s

let Map20 func a b c d e f g h i j k l m n o p q r s t =
    Ok func |> ApplyOk a |> ApplyOk b |> ApplyOk c |> ApplyOk d |> ApplyOk e |> ApplyOk f |> ApplyOk g |> ApplyOk h |> ApplyOk i |> ApplyOk j |> ApplyOk k |> ApplyOk l |> ApplyOk m |> ApplyOk n |> ApplyOk o |> ApplyOk p |> ApplyOk q |> ApplyOk r |> ApplyOk s |> ApplyOk t

let MapError20 func a b c d e f g h i j k l m n o p q r s t =
    Error func |> ApplyError a |> ApplyError b |> ApplyError c |> ApplyError d |> ApplyError e |> ApplyError f |> ApplyError g |> ApplyError h |> ApplyError i |> ApplyError j |> ApplyError k |> ApplyError l |> ApplyError m |> ApplyError n |> ApplyError o |> ApplyError p |> ApplyError q |> ApplyError r |> ApplyError s |> ApplyError t


//Python generation code
//------------------------------------------------------------------------
//offset = 97
//def create(lenght, funcbasename, applyfunc, returnfunc):
//    letters = " ".join([chr(x+offset) for x in range(lenght)])
//    applies = " ".join(["|> {} {}".format(applyfunc, chr(x+offset)) for x in range(lenght)])
//    return """let {}{} func {} =
//    {} func {}""".format(
//        funcbasename,
//        lenght,
//        letters,
//        returnfunc,
//        applies)

//file = open("GeneratedCode.fs","w")
//for i in range(1, 21):
//    file.write(create(i, "Map", "ApplyOk", "Ok"))
//    file.write("\n\n")
//    file.write(create(i, "MapError", "ApplyError", "Error"))
//    file.write("\n\n")
//-----------------------------------------------------------------------