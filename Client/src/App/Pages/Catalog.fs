module Pages.Catalog

open Sutil

type private Model = { CurrentPage : int }

type private Msg =
  | PaginationEvent of Pagination.PaginationEvent

let private init () =
  { CurrentPage = 1 }

let private update msg model =
  match msg with
  | PaginationEvent pe ->
    match pe with
    | Pagination.CurrentPageChanged p -> 
      { model with CurrentPage = p }

let private getCurrentPage m = m.CurrentPage
let view () =
  let model, dispatch = () |> Store.makeElmishSimple init update ignore 

  Html.div [
    Bind.fragment(model |> Store.map getCurrentPage) <| fun p -> text $"{p}"
    Card.view ()
    Pagination.view (fun pe -> dispatch (PaginationEvent pe)) 5
  ]
