module ListResultMap

open ResultHelperFunctions

let ResultListMap (value : Result<'a,'b> list) : Result<'a list, 'b list> =
    match AllOk value with
    | true -> ExtractOks value |> Ok
    | false -> ExtractErrors value |> Error