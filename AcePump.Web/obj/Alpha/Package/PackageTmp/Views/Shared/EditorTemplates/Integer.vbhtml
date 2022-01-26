@ModelType Integer?

@(Html.Kendo().IntegerTextBoxFor(Function(x) x) _
      .HtmlAttributes(New With {.style = "width:100%"}) _
      .Min(0) _
      .Max(Integer.MaxValue)
)