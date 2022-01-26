@ModelType Double?

@(Html.Kendo().NumericTextBoxFor(Function(x) x) _
    .HtmlAttributes(New With {.style = "width:100%"})
)