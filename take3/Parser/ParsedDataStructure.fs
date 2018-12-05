module ParsedDataStructure

type ParsedData =
    | String of string
    | Float of float
    | List of ParsedData list
    | Boolean of bool
    | Record of (string*ParsedData) list
    | Null