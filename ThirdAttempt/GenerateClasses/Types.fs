module Types

type ParsedField = {name: string; typename:string}
type FieldAllInfo = {fieldname: string; typename:string; typematcher: string ;serializername: string}
type TypeDescription = {typename: string; fields: FieldAllInfo list}