module Pagination

open Sutil
open Sutil.Attr
open Sutil.Styling
open Feliz

let private style = [
  rule "nav" [
    Css.displayFlex
    Css.justifyContentCenter
    Css.fontSize (length.em 1.5)
  ]
  rule "ul" [
    Css.listStyleTypeNone
    Css.displayFlex
    Css.margin 0
    Css.padding 0
  ]
  rule "li" [
    Css.positionRelative
    Css.textAlignCenter
    Css.width (length.px 40)
    Css.cursorPointer
  ]
  rule ".indicator" [
    Css.cursorNone
  ]
]


type private Model = 
  { CurrentPage : int
    PageCount   : int }

let private getCurrentPage m = m.CurrentPage

type private Msg =
  | IncrementPageIndex
  | DecrementPageIndex
  | GoToFirstPage
  | GoToLastPage


let private init pageCount () = 
  { CurrentPage = 1 
    PageCount = pageCount }

let private update msg model =
  match msg with
  | IncrementPageIndex -> 
      { model with CurrentPage = model.CurrentPage + 1 }
  | DecrementPageIndex -> 
      { model with CurrentPage = model.CurrentPage - 1 }
  | GoToFirstPage ->
      { model with CurrentPage = 1 }
  | GoToLastPage ->
      { model with CurrentPage = model.PageCount }

let view pageCount =
  let model, dispatch = () |> Store.makeElmishSimple (init pageCount) update ignore

  Html.nav [
    Html.ul [
      Html.li [ Html.text "«" ; onClick(fun _ -> dispatch GoToFirstPage) [] ]
      Html.li [ Html.text "<" ; onClick(fun _ -> dispatch DecrementPageIndex) [] ]
      Html.li [
        class' "indicator"
        Bind.fragment(model |> Store.map getCurrentPage) <| fun p -> text $"{p}"
      ]
      Html.li [ Html.text ">" ; onClick(fun _ -> dispatch IncrementPageIndex) [] ]
      Html.li [ Html.text "»" ; onClick(fun _ -> dispatch GoToLastPage) [] ]
    ]
  ] |> withStyle style

