module Pagination

open Sutil
open Sutil.Attr
open Sutil.Styling
open Feliz

let style = [
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
    Css.width (length.px 30)
    Css.cursorPointer
  ]
  rule ".selected" [
    Css.backgroundColor "#dddddd"
  ]
]

let view () =
  Html.nav [
    Html.ul [
      Html.li [ Html.text "«" ]
      Html.li [ Html.text "1" ]
      Html.li [ class' "selected" ; Html.text "2" ]
      Html.li [ Html.text "3" ]
      Html.li [ Html.text "»" ]
    ]
  ] |> withStyle style

