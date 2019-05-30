module <%= namespace %>

open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn

type Counter = {Value: int}

let getInitCounter() : Task<Counter> = task { return { Value = 42 } }

let webApp = router {
    get "/api/init" (fun next ctx ->
        task {
            let! counter = getInitCounter()
            return! json counter next ctx
        })
}

let app = application {
    url "http://localhost:8080/"
    use_router webApp
    memory_cache
    use_gzip
}


[<EntryPoint>]
let main argv =
    run app
    0 // return an integer exit code