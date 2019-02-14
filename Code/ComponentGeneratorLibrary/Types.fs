module Types

type ParsedField = {name: string; typename:string}
type FieldAllInfo = {fieldname: string; typename:string; typematcher: string ;serializername: string}
type TypeDescription = {typename: string; fields: FieldAllInfo list}
type Filenamepair<'a> = {filename: string; data: 'a}

let FilenamepairMap f x = {filename= x.filename; data = f x.data}
let FilenamepairResultExtract (x:Filenamepair<Result<'a,'b>>) = 
    match x.data with
    | Ok i -> Ok {filename= x.filename; data = i}
    | Error i -> Error i

let FilenamepairBind (f: Filenamepair<'a> -> 'b) x =
    {filename= x.filename; data = f x}