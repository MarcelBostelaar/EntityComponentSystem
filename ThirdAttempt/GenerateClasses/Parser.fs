module Parser

open ParserBase
open ResultHelperFunctions
open ParserErrors
open ResultMap

type field = {name: string; typename:string}
let private buildfield x = {name= fst x; typename = snd x}

let private maptuplesnd f tuple =
    fst tuple, f (snd tuple)

let private flattentupleresultinsnd tuple =
    MakeTuple2Result (Ok (fst tuple)) (snd tuple)

let private recordentrystring entry =
    maptuplesnd MatchString entry |> flattentupleresultinsnd

let private allentriesstring entries =
    let allresults = List.map recordentrystring entries
    match AllOk allresults with
    | true -> ExtractOks allresults |> Ok
    | false -> ExtractErrors allresults |> MultiError |> ParentErrorResult (ErrorDescription.String "Some entries were not strings, all should be strings")

let RecordToFieldlist parseddata =
    MatchRecord parseddata |> Bind allentriesstring (ErrorDescription.String "In AllEntriesStrings") |> Map1 (List.map buildfield)