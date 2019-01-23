module TupleMaps

let MapTupleFst f tuple =
    fst tuple |> f, snd tuple

let MapTupleSnd f tuple =
    fst tuple, f (snd tuple)

let ResultTupleRight (x : 'c * Result<'a,'b>)=
    match snd x with
    | Ok i -> Ok (fst x, i)
    | Error i -> Error i

let ResultTupleLeft (x : Result<'a,'b> * 'c)=
    match fst x with
    | Ok i -> Ok (i, snd x)
    | Error i -> Error i