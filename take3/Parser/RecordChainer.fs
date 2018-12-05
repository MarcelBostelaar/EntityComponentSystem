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
 
//type Matcher<'a> = string*JsonValue -> Result<'a,ErrorTrace>

let private chainify matcher = MatchEntryInRecord matcher |> Result.bind |> TupleSecondApplyBound

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

let MatchBuild1 func a json=
    let nested_tuples = MatchRecord json |> EntryChainStarter |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChainFinish

let MatchBuild2 func a b json=
    let nested_tuples = MatchRecord json |> EntryChainStarter |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild3 func a b c json=
    let nested_tuples = MatchRecord json |> EntryChainStarter |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

//Python generation code
//------------------------------------------------------------------------
//offset = 97
//def create(lenght):
//    letters = " ".join([chr(x+offset) for x in range(lenght)])
//    chainify_unreversed = ["chainify {} |>".format(chr(x+offset)) for x in range(lenght)]
//    chainify_unreversed.reverse()
//    chainify = " ".join(chainify_unreversed)
//    applychain = " ".join(["ApplyChain |>" for x in range(lenght)])
//    return """let MatchBuild{} func {} json=
//    let nested_tuples = MatchRecord json |> EntryChainStarter |> {} EntryChainFinisher
//    ApplyChainStart (Ok func) nested_tuples |> {} ApplyChainFinish""".format(lenght, letters, chainify, applychain)
//
//file = open("text.fs","w")
//for i in range(1, 21):
//    file.write(create(i))
//    file.write("\n\n")
//-----------------------------------------------------------------------