@ModelType Decimal?

@(Html.Kendo().NumericTextBoxFor(Function(x) x) _
    .Min(0) _
    .Decimals(2) _
    .Step(0.5)
) 