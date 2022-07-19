module Pages.Catalog

open Sutil

let view () =
  Html.div [
    Card.view ()
    Pagination.view 5
  ]
