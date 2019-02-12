module CSharpClassBuilder

open Types
open System

let private ExtraIndent (code : string) = "\t" + code.Replace("\n", "\n\t")


let private generateOneField (parsedfield : ParsedField) = 
    String.Format("public {0} {1};", parsedfield.typename, parsedfield.name)
let private generateFieldDefinitions (item : string * (ParsedField list)) = List.map generateOneField (snd item)

let private generateSingleParameter (parsedfield : ParsedField) = String.Format("{0} {1}", parsedfield.typename, parsedfield.name)
let private generateSingleAssignment (parsedfield : ParsedField) =  String.Format("this.{0} = {1};", parsedfield.name, parsedfield.name)
let private generateConstructor (item : string * (ParsedField list)) = 
    let parameters = List.map generateSingleParameter (snd item)
    let assignments = List.map generateSingleAssignment (snd item)
    String.Format("public {0}({1}){{\n{2}\n}}", fst item, String.concat ", " parameters, ExtraIndent <| String.concat "\n" assignments)


let private generateCopyAction (primitives : string Set) (parsedfield : ParsedField)  =
    match primitives.Contains parsedfield.typename with
    | true -> String.Format("this.{0}", parsedfield.name)
    | false -> String.Format("this.{0}.DeepCopy()", parsedfield.name)


let private generateDeepCopy (item : string * (ParsedField list)) (primitives : string Set) =
    let copyactions = List.map (generateCopyAction primitives) (snd item) 
    let constructionstring = String.Format("return new {0}(\n{1}\n);", (fst item), ExtraIndent <| String.concat ",\n" copyactions)
    String.Format("public {0} DeepCopy(){{\n{1}\n}}", fst item, ExtraIndent constructionstring)

let private ClassWriter (item : string * (ParsedField list)) (primitives : string Set)= 
    let fields = generateFieldDefinitions item
    let constructorstring = generateConstructor item
    let deepcopy = generateDeepCopy item primitives

    let all = List.concat [fields; [""; constructorstring; ""; deepcopy]]
    String.Format("class {0}{{\n{1}\n}}", fst item, ExtraIndent <| String.concat "\n" all)

let FullWriter (primitives : string Set) (_namespace : string) (item : string * (ParsedField list))=
    String.Format("namespace {0}\n{{\n{1}\n}}", _namespace, ExtraIndent <| ClassWriter item primitives)