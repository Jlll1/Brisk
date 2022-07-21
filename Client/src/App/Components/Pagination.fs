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


module private Model =
  type private pageCount = { Value : int }
  type PageCount =
    private
    | PageCount of pageCount
    member x.Value =
      let (PageCount i) = x
      i.Value

  type Model =
    { CurrentPage : int
      TotalPages  : PageCount }

  let init pageCount () =
    { CurrentPage = 1
      TotalPages = PageCount { Value = pageCount } }
open Model

type private Msg =
  | IncrementPageIndex
  | DecrementPageIndex
  | GoToFirstPage
  | GoToLastPage

let private update msg model =
  let updateModel msg m =
    match msg with
    | IncrementPageIndex ->
      { m with CurrentPage = model.CurrentPage + 1 }
    | DecrementPageIndex ->
      { m with CurrentPage = model.CurrentPage - 1 }
    | GoToFirstPage ->
      { m with CurrentPage = 1 }
    | GoToLastPage ->
      { m with CurrentPage = model.TotalPages.Value }

  let validateModelChanges m =
    let currentPageIsBetween1AndTotalPages =
      1 <= m.CurrentPage && m.CurrentPage <= m.TotalPages.Value

    if currentPageIsBetween1AndTotalPages
    then m
    else model

  model |> updateModel msg |> validateModelChanges


let private getCurrentPage (m : Model.Model) = m.CurrentPage

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

