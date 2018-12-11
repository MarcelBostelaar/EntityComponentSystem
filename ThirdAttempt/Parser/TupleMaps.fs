module TupleMaps

let MapTupleFst f tuple =
    fst tuple |> f, snd tuple

let MapTupleSnd f tuple =
    fst tuple, f (snd tuple)