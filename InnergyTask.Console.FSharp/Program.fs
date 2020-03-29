open System

type Material = { Id:string; Name:string; Supplies:Supply list }
and Supply = { Warehouse:string; Quantity:Int64 }

let stockReader readLine =
   let splitSupply (str:string) =
      str.Split(',') |> fun arr -> { Warehouse=arr.[0]; Quantity=(int64 arr.[1]) }
   let splitSupplies (str:string) =
      str.Split('|') |> Array.map splitSupply |> Array.toList
   let splitMaterial (str:string) =
      str.Split(';') |> fun arr -> { Id=arr.[1]; Name=arr.[0]; Supplies=splitSupplies arr.[2] }

   seq { while true do yield readLine () }
   |> Seq.takeWhile (fun s -> not <| String.IsNullOrWhiteSpace(s))
   |> Seq.filter (fun s -> not <| s.StartsWith("#"))
   |> Seq.map splitMaterial
   |> Seq.toList

let stockWriter writeLine materials =
   let writeSupply (m, s) = sprintf "%s: %i" m.Id s.Quantity |> writeLine
   let writeWarehouse (wh, total, lst) =
      sprintf "%s (total %i)" wh total |> writeLine
      lst |> List.sortBy (fun (m, _) -> m.Id) |> List.iter writeSupply
      writeLine String.Empty

   materials
   |> List.collect (fun m -> List.map (fun s -> (m, s)) m.Supplies)
   |> List.groupBy (fun (_, s) -> s.Warehouse)
   |> List.map (fun (wh, lst) -> (wh, List.sumBy (fun (_, s) -> s.Quantity) lst, lst))
   |> List.sortByDescending (fun (wh, total, _) -> total, wh)
   |> List.iter writeWarehouse

[<EntryPoint>]
let main argv =
   let readConsole _ = Console.ReadLine()
   let writeConsole (line:string) = Console.WriteLine(line)
   let stock = stockReader readConsole
   stock |> stockWriter writeConsole
   0