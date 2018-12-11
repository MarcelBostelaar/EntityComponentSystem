module TypeBuilder

open RecordToFieldList
open System

let private buildfieldtypedefinition (field:FieldAllInfo) = field.fieldname + ": " + field.typename

let BuildTypeDefinition (typename:string) (fields : FieldAllInfo list) =
    let types = List.map buildfieldtypedefinition fields |> String.concat ";\n    "
    String.Format("type {0} = {{ \n    {1} \n}}", typename, types)

let BuildTypeConstructor (functionname:string) (fieldnames : string list)=
    let args = [1 .. fieldnames.Length] |> List.map (fun (x:int)-> "arg" + x.ToString())
    let fieldassignments = List.zip fieldnames args |> List.map (fun x -> fst x + " = " + snd x)
    ["let" ; functionname ; String.concat " " args ; "=" ; "{\n   "; String.concat ";\n    " fieldassignments ; "\n}"] |> String.concat " "

let BuildFieldMatcher field=
    let funcname = "Match" + field.fieldname
    String.Format("""let private {0} = MatchRecordEntry "{1}" {2}""", funcname, field.fieldname, field.typematcher)

let BuildMatcher (functionname:string) (matchmethodnames : string list) (matchbuildfunc : string) (constructfunc : string) =
    String.Format("let {0} = {1} {2} {3}", functionname, matchbuildfunc, constructfunc, String.concat " " matchmethodnames)

let private buildserializer variablename x =
    ["\"" + x.fieldname + "\"" ; ","; x.serializername; variablename + "." + x.fieldname] |> String.concat " "

let BuildSerialize (functionname:string) (fieldswithserializer : FieldAllInfo list) =
    let argumentname = "value"
    ["let"; functionname ; argumentname ; "="; "[" ; 
    List.map (buildserializer argumentname) fieldswithserializer |> String.concat " ; "     // field1 , serializevalue1 value.field1 ; field2 , serializevalue2 value.field2 ; 
    ; "]" ; "|> ParsedData.Record" ] |> String.concat " "

let BuildFullType typename (fields : FieldAllInfo list) =
    let definition = BuildTypeDefinition typename fields
    let constructor = BuildTypeConstructor ("Construct" + typename) (List.map (fun x -> x.fieldname) fields)
    let matchmethods = List.map BuildFieldMatcher fields
    let matcher = BuildMatcher ("Match" + typename) (List.map (fun x -> "Match" + x.fieldname) fields) ("MatchBuild" + fields.Length.ToString()) ("Construct" + typename)
    let serializer = BuildSerialize ("Serialize" + typename) fields
    [definition ; constructor ; String.concat "\n" matchmethods ; matcher ; serializer ] |> String.concat "\n\n"