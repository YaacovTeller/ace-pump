@Code
    Dim customerNamePropertyName As String
    
    If ViewData.ModelMetadata.AdditionalValues.ContainsKey("CustomerNameProperty") Then
        customerNamePropertyName = ViewData.ModelMetadata.AdditionalValues("CustomerNameProperty")
    Else
        customerNamePropertyName = "__CustomerNamePropertyNameProperty__Not__Configured__"
    End If
End Code

<script type="text/javascript">
    function CustomerName_Data(e) {
        var result = kendo.ui.ComboBox.requestData("#@ViewData.ModelMetadata.PropertyName");

        if (result.text == "") {
            var customerNameObj = $("#@ViewData.ModelMetadata.PropertyName");
            var model = customerNameObj.closest("tr").data("kendoEditable").options.model;
            var comboBox = customerNameObj.data("kendoComboBox");

            result.text = model.@customerNamePropertyName;
        }

        return result;
    }
</script>

@(Html.Kendo().ComboBoxFor(Function(x) x) _
    .DataTextField("Name") _
    .DataValueField("Id") _
    .MinLength(2) _
    .Filter(FilterType.StartsWith) _
    .AutoBind(False) _
    .Placeholder("Start typing a customer...") _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Sub(reader)
                                      reader.Action("StartsWith", "Customer")
                                      reader.Type(HttpVerbs.Post)
                                      reader.Data("CustomerName_Data")
                              End Sub) _
                        .ServerFiltering(True)
                End Sub)
    )
