module App

open Browser.Dom
open Sutil
open System

[<RequireQualifiedAccess>]
type Page =
  | Home
  | Catalog


module Router =
  open Browser.Types
  let parseUrl (location : Location) =
    if location.hash.Length > 1
    then location.hash.Substring 1
    else ""

  let parseRoute (location : Location) : Page =
    let hash = parseUrl location
    match hash.ToLower() with
    | "home" -> Page.Home
    | "catalog" -> Page.Catalog
    | _ -> Page.Home

type Model = { CurrentPage : Page }
let getCurrentPage m = m.CurrentPage

type Msg =
  | SetPage of Page

let viewPage page =
  match page with
  | Page.Home -> Pages.Home.view ()
  | Page.Catalog -> Pages.Catalog.view ()

let init () = { CurrentPage = Page.Home }, Cmd.none

let update msg model =
  match msg with
  | SetPage p -> 
    window.location.href <- "#" + (string p).ToLower()
    { model with CurrentPage = p }, Cmd.none

let view () =
  let model, dispatch = () |> Store.makeElmish init update ignore

  let pageObs : IObservable<Page> = model |> Store.map getCurrentPage |> Store.distinct

  let routerSub = Navigable.listenLocation Router.parseRoute (dispatch << SetPage)

  Html.div [
    Bind.fragment pageObs <| viewPage
  ]

view () |> Program.mountElement "sutil-app"


