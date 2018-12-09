module ParserErrors
open System
open ParsedDataStructure

type ErrorTrace<'error>=
    | RootError of 'error
    | ParentError of 'error * ErrorTrace<'error>
    | MultiError of ErrorTrace<'error> list
    
type typemismatch = {expectedType: string; actual: ParsedData}
type namemismatch = {expectedName: string; actual: string}

type ErrorDescription=
    | String of string
    | TypeMismatch of typemismatch
    | NameMismatch of namemismatch
    | MultipleMatched of (string * ParsedData) list
    | UnmatchedEntries of (string * ParsedData) list

let CreateTypeMismatchError expected actual =
    {expectedType = expected; actual = actual} |> TypeMismatch
let CreateNameMismatchError expected actual =
    {expectedName = expected; actual = actual} |> NameMismatch
    
let CreateRootTypeMismatchErrorResult expected actual =
    CreateTypeMismatchError expected actual |> RootError |> Error
let CreateRootNameMismatchErrorResult expected actual =
    CreateNameMismatchError expected actual |> RootError |> Error
    
let RootErrorResult message = message |> RootError |> Error
let MultiErrorResult message = message |> MultiError |> Error