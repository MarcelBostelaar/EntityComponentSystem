module ParseAndSort

open ParsedDataStructure
open RecordToFieldList
open Types
open ResultHelperFunctions
open TopologicalSortFsharp
open ParserErrors
open System

let private addfieldlistparseerror (data : Filenamepair<ParsedData>) (error : ErrorTrace<ErrorDescription>) =
    ParentError (ErrorDescription.String (String.Format("A field that is not a string was found in file {0}, all fields should be strings. Nested records, numbers, etc are not allowed", data.filename)), error)
    

let private parsesingledata (data : Filenamepair<ParsedData>) =
    FilenamepairMap RecordToFieldlist data |> FilenamepairResultExtract |> Result.mapError (addfieldlistparseerror data)

///<summary>Turns a list of key-data pairs into a list of key-parsedfield pairs, or an error if it isnt a parsedfield</summary>
let private parsedata data =
    List.map parsesingledata data |> OnlyOk

let private IDgetter (data : Filenamepair< ParsedField list>) = data.filename
///<summary>Selects the dependencies from a parsed data that is not part of the primitives given to it (IE if you give it 'int' then it will only return dependencies that arent 'int')</summary>
let private DependencySelector primitives (data : Filenamepair< ParsedField list>) = 
    let dependencies = data.data |> List.map (fun x -> x.typename) |> set //the depenencies of some data
    Set.difference dependencies primitives |> Set.toSeq //the dependencies of some data that are not a primitive

let private IDComparer (a:string) (b:string) = a=b

let private Sort primitives (data : Filenamepair<ParsedField list> seq) = 
    let dependencygetter = DependencySelector primitives
    SortTopologically IDgetter dependencygetter IDComparer data |> Result.mapError ErrorUnion.TopologicalSortError

let ParseAndSort(primitivetypenames: string Set) (data : Filenamepair<ParsedData> list)= 
    let parsed = parsedata data |> Result.mapError ErrorUnion.ParserError
    Result.bind (Sort primitivetypenames) parsed