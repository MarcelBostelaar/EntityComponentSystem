module RecordChainer

open ParserBase
open ParserErrors
open ResultHelperFunctions
open ParsedDataStructure

(* 
Allows users to easily create and chain record matchers and constructors.
Usage as follows
Making an entrymatcher: EntryChainer [name of entry field] [matching function of type JsonValue -> Result<'a,ErrorTrace>] [errormessage in case of error]

Chaining a matched value nested tuple:
[Result<(string*JsonValues) list, 'ErrorTrace>] |> EntryChainStarter |> entrymatcher1 |> entrymatcher 2 ... |> EntryChainFinisher

Chaining a building function:
ApplyChainStart [Result<'n -> ... -> b->'a-'>'builditem, 'errortrace>] [tuple result of chaining, see above] |> ApplyChain (n times) ... |> ApplyChainFinish
Builder function accesses elements in the reverse order that the match chain made them
*)

let chainify matcher = MatchEntryInRecord matcher |> Result.bind |> TupleSecondApplyBound

let EntryChainStarter a = (fun x -> (), x) |> Result.map <| a

let EntryChainer name matcher error = 
    let entrymatcher = MatchRecordEntry name matcher 
    MatchEntryInRecord entrymatcher |> Bind <| error |> TupleSecondApplyBound

let private _ChainFinisher_unbound (somevalue:'a*((string * ParsedData) list)) = 
    if (snd somevalue).Length = 0 
    then fst somevalue |> Ok 
    else 
        snd somevalue |> UnmatchedEntries |> RootError |> Error

let EntryChainFinisher a = Result.bind _ChainFinisher_unbound a

let private _consumeapplytuple (value: ('a -> 'b)*('rest*'a)) = 
    let f = fst value
    let x = snd value |> snd
    let rest = snd value |> fst
    f x, rest
let ApplyChain a = Result.map _consumeapplytuple a
let ApplyChainStart f=
    MakeTuple2Result f
let private _applychainfinish (value: 'a*unit) = fst value
let ApplyChainFinish x = Result.map _applychainfinish x