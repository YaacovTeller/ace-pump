@ModelType Decimal?

@(Html.Kendo().CurrencyTextBoxFor(Function(x) x) _
      .HtmlAttributes(New With {.style = "width:100%"}) _
      .Min(0)
)


