module KahnsAlgorithm

type Node<'T> = {value: 'T; mutable Dependencies: 'T list; mutable in_degree: int}

let KahnsAlgorithm (nodes : Node<'a> list) =
    let mutable no_incoming = List.where (fun x -> x.in_degree = 0) nodes
    let mutable tosort = List.where (fun x -> not <| List.contains x no_incoming) nodes
    let mutable sorted = []
    while tosort.Length <> 0 do
        //sorted := sorted :: [no_incoming.Head]
        //no_incoming := no_incoming.Tail
        //Do in C#?