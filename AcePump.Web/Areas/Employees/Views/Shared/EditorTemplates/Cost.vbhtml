@ModelType Decimal

@(Html.Kendo().CurrencyTextBoxFor(Function(x) x) _
                            .Format("{0:c}") _
                            .Decimals(2) _
                            .Step(1) _
                            .Min(0) _
                            .Max(99999)
)