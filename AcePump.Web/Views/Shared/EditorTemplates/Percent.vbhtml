@ModelType Double?

@(Html.Kendo().NumericTextBoxFor(Function(x) x) _
    .Min(0) _
    .Format("p2") _
    .Max(100) _
    .Decimals(4) _
    .Step(0.0005)
)  %