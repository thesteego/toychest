//  A simple mutable web server

module WebServer

#if INTERACTIVE
#r "System.Net.dll"
#r "../../packages/Suave/lib/net40/Suave.dll"
#endif

//  Encapsulate everything in 
open Suave
open System.Net

let private started = ref false
let private mainWebPart = ref (Successful.OK "Welcome")

let update(newPart) = lock mainWebPart (fun () -> mainWebPart := newPart)

let start() = 
    if not started.Value then
        lock started (fun () -> started := true)
        async {
                startWebServer defaultConfig (fun(ctx : HttpContext) ->
                    async {
                        let part = lock mainWebPart (fun () -> mainWebPart.Value)
                        return! part ctx
                    })
        } |> Async.Start

start()
