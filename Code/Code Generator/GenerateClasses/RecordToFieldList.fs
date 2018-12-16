module RecordToFieldList

open ParserBase
open ResultHelperFunctions
open ParserErrors
open ResultMap
open Types

let private buildfield x = {name= fst x; typename = snd x}

let private flattentupleresultinsnd tuple =
    MakeTuple2Result (Ok (fst tuple)) (snd tuple)

let private recordentrystring entry =
    TupleMaps.MapTupleSnd MatchString entry |> flattentupleresultinsnd

let private allentriesstring entries =
    let allresults = List.map recordentrystring entries
    match AllOk allresults with
    | true -> ExtractOks allresults |> Ok
    | false -> ExtractErrors allresults |> MultiError |> ParentErrorResult (ErrorDescription.String "Some entries were not strings, all should be strings")

let RecordToFieldlist parseddata =
    MatchRecord parseddata |> Bind allentriesstring (ErrorDescription.String "In AllEntriesStrings") |> Map1 (List.map buildfield)