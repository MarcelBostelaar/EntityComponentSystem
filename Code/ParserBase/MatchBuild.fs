module MatchBuild

open RecordChainer
open ParserBase
open System

let GenerateMatchBuild lenght =
    String.Format(
"""let MatchBuild{0} func {1} data =
    let nested_tuples = MatchRecord data |> EntryChainStarter |> {2} EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> {3} ApplyChainFinish""",
    lenght.ToString(),
    List.map (fun x -> "entry" + x.ToString()) [1 .. lenght] |> String.concat " ",
    List.map (fun x -> "chainify entry" + x.ToString() + " |>") [lenght .. -1 .. 1] |> String.concat " ",
    List.map (fun x -> "ApplyChain |>") [1 .. lenght] |> String.concat " "
    )

let MatchBuild1 func a data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChainFinish

let MatchBuild2 func a b data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild3 func a b c data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild4 func a b c d data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild5 func a b c d e data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild6 func a b c d e f data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild7 func a b c d e f g data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild8 func a b c d e f g h data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild9 func a b c d e f g h i data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild10 func a b c d e f g h i j data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild11 func a b c d e f g h i j k data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild12 func a b c d e f g h i j k l data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild13 func a b c d e f g h i j k l m data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild14 func a b c d e f g h i j k l m n data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild15 func a b c d e f g h i j k l m n o data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild16 func a b c d e f g h i j k l m n o p data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify p |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild17 func a b c d e f g h i j k l m n o p q data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify q |> chainify p |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild18 func a b c d e f g h i j k l m n o p q r data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify r |> chainify q |> chainify p |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild19 func a b c d e f g h i j k l m n o p q r s data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify s |> chainify r |> chainify q |> chainify p |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish

let MatchBuild20 func a b c d e f g h i j k l m n o p q r s t data=
    let nested_tuples = MatchRecord data |> EntryChainStarter |> chainify t |> chainify s |> chainify r |> chainify q |> chainify p |> chainify o |> chainify n |> chainify m |> chainify l |> chainify k |> chainify j |> chainify i |> chainify h |> chainify g |> chainify f |> chainify e |> chainify d |> chainify c |> chainify b |> chainify a |> EntryChainFinisher
    ApplyChainStart (Ok func) nested_tuples |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChain |> ApplyChainFinish