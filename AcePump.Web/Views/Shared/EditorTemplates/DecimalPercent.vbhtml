@ModelType Decimal?

@(Html.Kendo().NumericTextBoxFor(Function(x) x) _
    .Min(0) _
    .Max(1) _
    .Decimals(4) _
    .Step(0.0005) _
    .Format("p2")
)  %