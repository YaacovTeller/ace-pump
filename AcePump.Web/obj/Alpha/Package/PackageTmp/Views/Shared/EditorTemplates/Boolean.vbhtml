@ModelType Nullable(Of Boolean)
@Html.CheckBox("", If(Model.HasValue, Model.Value, False))