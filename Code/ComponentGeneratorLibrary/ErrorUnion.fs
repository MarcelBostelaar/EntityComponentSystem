module ErrorUnion

open ParserErrors

type ErrorUnion<'sortID, 'sortValue, 'traceerrordescription> =
    | ParserError of ErrorTrace<'traceerrordescription>
    | TopologicalSortError of TopologicalSortFsharp.Error<'sortID,'sortValue>