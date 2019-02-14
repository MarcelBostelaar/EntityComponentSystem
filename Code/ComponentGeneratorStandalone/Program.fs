

open System
open masterfunctionality

[<EntryPoint>]
let main argv =
    MasterFunction ()
    Console.WriteLine("Press any key to continue...")
    ignore <| Console.ReadKey()
    0 // return an integer exit code
