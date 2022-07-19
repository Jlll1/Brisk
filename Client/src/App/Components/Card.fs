module Card
open Sutil.Styling
open Sutil
open Sutil.Attr
open Feliz

let private style = [
  rule "figure" [
    Css.width (length.px 250)
    Css.height (length.px 325)
    Css.overflowHidden
    Css.borderRadius (length.px 10)
  ]
  rule "figcaption" [
    Css.padding (length.px 5)
    Css.margin 0
  ]
  rule "figcaption h3" [
    Css.fontSize (length.em 1.3)
    Css.fontWeight 700
  ]
  rule ".image" [
    Css.height (length.percent 80)
    Css.backgroundImageUrl "https://images.pexels.com/photos/34299/herbs-flavoring-seasoning-cooking.jpg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
    Css.backgroundSizeCover
    Css.backgroundRepeatNoRepeat
    Css.backgroundPosition "50 50"
  ]
]

let view () =
  Html.figure [
    Html.div [ class' "image" ]
    Html.figcaption [
      Html.h3 "Test Item"
      Html.p "$1.50"
    ]
  ] |> withStyle style
