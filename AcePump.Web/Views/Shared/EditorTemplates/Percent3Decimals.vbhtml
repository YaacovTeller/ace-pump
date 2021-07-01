@ModelType Decimal?

@(Html.Kendo().NumericTextBoxFor(Function(x) x) _
    .Min(0) _
    .Format("p3") _
    .Max(100) _
    .Decimals(5) _
    .Step(0.00005)
)  %