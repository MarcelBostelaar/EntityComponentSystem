module CSharpClassBuilder

open Types
open System

let private ExtraIndent (code : string) = "\t" + code.Replace("\n", "\n\t")


let private generateOneField (parsedfield : ParsedField) = 
    String.Format("public {0} {1};", parsedfield.typename, parsedfield.name)
let private generateFieldDefinitions (item : ParsedField list) = List.map generateOneField item

let private generateSingleParameter (parsedfield : ParsedField) = String.Format("{0} {1}", parsedfield.typename, parsedfield.name)
let private generateSingleAssignment (parsedfield : ParsedField) =  String.Format("this.{0} = {1};", parsedfield.name, parsedfield.name)
let private generateConstructor (item : Filenamepair<(ParsedField list)>) = 
    let parameters = List.map generateSingleParameter (item.data)
    let assignments = List.map generateSingleAssignment (item.data)
    String.Format("public {0}({1}){{\n{2}\n}}", item.filename, String.concat ", " parameters, ExtraIndent <| String.concat "\n" assignments)


let private generateCopyAction (primitives : string Set) (parsedfield : ParsedField)  =
    match primitives.Contains parsedfield.typename with
    | true -> String.Format("this.{0}", parsedfield.name)
    | false -> String.Format("this.{0}.DeepCopy()", parsedfield.name)


let private generateDeepCopy (item : Filenamepair<(ParsedField list)>) (primitives : string Set) =
    let copyactions = List.map (generateCopyAction primitives) item.data 
    let constructionstring = String.Format("return new {0}(\n{1}\n);", item.filename, ExtraIndent <| String.concat ",\n" copyactions)
    String.Format("public {0} DeepCopy(){{\n{1}\n}}", item.filename, ExtraIndent constructionstring)

let private ClassWriter (item : Filenamepair<(ParsedField list)>) (primitives : string Set)= 
    let fields = generateFieldDefinitions item.data
    let constructorstring = generateConstructor item
    let deepcopy = generateDeepCopy item primitives

    let all = List.concat [fields; [""; constructorstring; ""; deepcopy]]
    String.Format("class {0}{{\n{1}\n}}", item.filename, ExtraIndent <| String.concat "\n" all)

let FullWriter (primitives : string Set) (_namespace : string) (item : Filenamepair<ParsedField list>)=
    let makefullclass item = String.Format("namespace {0}\n{{\n{1}\n}}", _namespace, ExtraIndent <| ClassWriter item primitives)
    FilenamepairBind makefullclass item