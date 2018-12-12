module KahnsAlgorithm

open System.Collections.Generic

type Node<'T> = {value: 'T; mutable Dependencies: Node<'T> ref list; mutable in_degree: int}

let private refnode_indegree_zero x =
    (!x).in_degree = 0

let private refnode_indegree_notzero x =
    (!x).in_degree <> 0

let private add_value_in_degree_ref value x =
    (!x).in_degree <- (!x).in_degree + value

let private dereference x = !x

let private KahnsAlgorithm (nodes : Node<'a> ref list) =
    let no_incoming = new List<Node<'a> ref>(List.where refnode_indegree_zero nodes) //all nodes with no incoming dependencies
    let tosort = new List<Node<'a> ref>(List.where refnode_indegree_notzero nodes) //all other nodes
    let sorted = new List<Node<'a>>() //all the no-incoming nodes that have been ranked
    ignore <| while no_incoming.Count <> 0 do
                let selected = !(no_incoming.Item 0) //get first element in to no incoming list
                ignore <| List.map (add_value_in_degree_ref -1) selected.Dependencies //decrement the in_degree in the nodes it depends on
                let nowzeroin = List.where refnode_indegree_zero selected.Dependencies //get all nodes that now have an in_dregee of zero (we remove element selected from the graph)
                no_incoming.AddRange(nowzeroin) //add all the now zero incoming nodes to the end of the list of no incoming nodes
                ignore <| tosort.RemoveAll(fun x -> List.contains x nowzeroin) //remove them from the to sort list
                ignore <| sorted.Add(selected) //add the selected node to the end of the sorted list
                ignore <| no_incoming.RemoveAt(0) //remove the selected node from the no incoming list
    match tosort.Count with
    | 0 -> Seq.toList sorted |> Ok //no elements left to sort, no circular dependencies present
    | _ -> Seq.map dereference tosort |> Seq.toList |> Error //there are elements to sort that have a circular dependency, return these as an error

let private TopologicalSortNodes (nodes : Node<'a> ref list) =
    let getvalue x = x.value
    let resultokmap = ResultMap.Map1 <| List.map getvalue
    let resulterrormap = ResultMap.MapError1 <| List.map getvalue
    KahnsAlgorithm nodes |> resultokmap |> resulterrormap


let private tupleid x =
    let i, _, _ = x
    i
let private tupledependencies x =
    let _, i, _ = x
    i
let private tuplenode x =
    let _, _, i = x
    i

let private adddependencies (listofnodes : ('id*'id list*Node<'somevalue> ref) list) (owntuple : 'id*'id list*Node<'somevalue> ref)=
    let id, dependencies, self = owntuple
    let dependencyitems = List.where <| (fun i -> tupleid i |> List.contains <| dependencies) <| listofnodes
    let nodesonly = List.map tuplenode dependencyitems
    (!self).Dependencies <- nodesonly
    for i in nodesonly do
        (!i).in_degree <- (!i).in_degree + 1


let private BuildTree (values : 'a list) (identifier_choser : 'a -> 'id) (dependency_choser : 'a -> 'id list) =
    let noded = (fun x -> identifier_choser x, dependency_choser x, ref {value = x; Dependencies = []; in_degree = 0}) |> List.map <| values
    ignore <| List.map (adddependencies noded) noded
    List.map tuplenode noded

let TopologicalSort (values : 'a list) (identifier_choser : 'a -> 'id) (dependency_choser : 'a -> 'id list) =
    BuildTree values identifier_choser dependency_choser |> TopologicalSortNodes