@Code
    Dim leaseNamePropertyName As String
    
    If ViewData.ModelMetadata.AdditionalValues.ContainsKey("LeaseNameProperty") Then
        leaseNamePropertyName = ViewData.ModelMetadata.AdditionalValues("LeaseNameProperty")
    Else
        leaseNamePropertyName = "__LeaseNamePropertyNameProperty__Not__Configured__"
    End If
End Code

<script type="text/javascript">
    function LeaseName_Data(e) {
        var result = kendo.ui.ComboBox.requestData("#@ViewData.ModelMetadata.PropertyName");

        if (result.text == "") {
            var leaseNameObj = $("#@ViewData.ModelMetadata.PropertyName");
            var model = leaseNameObj.closest("tr").data("kendoEditable").options.model;
            var comboBox = leaseNameObj.data("kendoComboBox");

            result.text = model.@leaseNamePropertyName;
        }

        return result;
    }
</script>

@(Html.Kendo().ComboBoxFor(Function(x) x) _
    .DataTextField("LeaseName") _
    .DataValueField("LeaseId") _
    .MinLength(2) _
    .Filter(FilterType.StartsWith) _
    .AutoBind(False) _
    .Placeholder("Start typing a lease...") _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Sub(reader)
                                      reader.Action("StartsWith", "Lease")
                                      reader.Type(HttpVerbs.Post)
                                      reader.Data("LeaseName_Data")
                              End Sub) _
                        .ServerFiltering(True)
                End Sub)
    )
