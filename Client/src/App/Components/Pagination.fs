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
  type private pageIndex =
    { Value     : int
      PageCount : int }
  type PageIndex =
    private
    | PageIndex of pageIndex
    member x.PageCount =
      let (PageIndex i) = x
      i.PageCount
    member x.Value =
      let (PageIndex i) = x
      i.Value
    member x.SetValue newValue =
      let (PageIndex i) = x
      match i.PageCount >= newValue && newValue >= 1 with
      | true -> Some (PageIndex { i with PageCount = newValue })
      | false -> None

  type Model = { CurrentPage : PageIndex }

  let init pageCount () =
    { CurrentPage = PageIndex { Value = 1 ; PageCount = pageCount } }
open Model

type private Msg =
  | IncrementPageIndex
  | DecrementPageIndex
  | GoToFirstPage
  | GoToLastPage

let private updateCurrentPage m newPageIndex =
      match newPageIndex with
      | Some pi -> { m with CurrentPage = pi }
      | None -> m

let private update msg model =
  match msg with
  | IncrementPageIndex ->
        model.CurrentPage.Value + 1
        |> model.CurrentPage.SetValue
        |> updateCurrentPage model
  | DecrementPageIndex -> 
        model.CurrentPage.Value - 1
        |> model.CurrentPage.SetValue
        |> updateCurrentPage model
  | GoToFirstPage ->
        model.CurrentPage.SetValue 1
        |> updateCurrentPage model
  | GoToLastPage ->
        model.CurrentPage.PageCount
        |> model.CurrentPage.SetValue
        |> updateCurrentPage model

let private getCurrentPage (m : Model.Model) = m.CurrentPage.Value

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

