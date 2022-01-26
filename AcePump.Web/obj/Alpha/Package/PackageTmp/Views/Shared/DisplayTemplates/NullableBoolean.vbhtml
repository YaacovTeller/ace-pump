@ModelType Nullable(Of Boolean)

@If Model.HasValue AndAlso Model.Value Then
    @<text>Yes</text>
Else
    @<text>No</text>
End If