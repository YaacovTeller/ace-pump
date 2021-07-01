@ModelType Nullable(Of Date)
@Html.Encode(If(Model.HasValue, Model.Value.ToShortDateString(), String.empty))
    