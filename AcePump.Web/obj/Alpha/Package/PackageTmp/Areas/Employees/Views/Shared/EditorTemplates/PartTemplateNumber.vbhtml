@Code
    Dim partTemplateNumberPropertyName As String

    If ViewData.ModelMetadata.AdditionalValues.ContainsKey("PartTemplateNumberProperty") Then
        partTemplateNumberPropertyName = ViewData.ModelMetadata.AdditionalValues("PartTemplateNumberProperty")
    Else
        partTemplateNumberPropertyName = "__PartTemplateNumberProperty__Not__Configured__"
    End If
End Code

<script type="text/javascript">
    function PartTemplateNumber_Data(e) {
        var result = kendo.ui.ComboBox.requestData("#@ViewData.ModelMetadata.PropertyName");

        if (result.text == "") {
            var partTemplateNumberObj = $("#@ViewData.ModelMetadata.PropertyName");
            var model = partTemplateNumberObj.closest("tr").data("kendoEditable").options.model;
            var comboBox = partTemplateNumberObj.data("kendoComboBox");

            result.text = model.@partTemplateNumberPropertyName;
        }

        return result;
    }
</script>

@(Html.Kendo().ComboBoxFor(Function(x) x) _
                            .DataTextField("PartTemplateNumber") _
                            .DataValueField("PartTemplateID") _
                            .MinLength(2) _
                            .Filter(FilterType.StartsWith) _
                            .AutoBind(False) _
                            .Placeholder("Start typing a part number...") _
                            .DataSource(Sub(dataSource)
                                            dataSource _
                                            .Read(Sub(reader)
                                                      reader.Action("StartsWith", "PartTemplate")
                                                      reader.Type(HttpVerbs.Post)
                                                      reader.Data("PartTemplateNumber_Data")
                                                  End Sub) _
                                                .ServerFiltering(True)
                                        End Sub)
    )


<script type="text/javascript">
    (function () {
        var cmbJq = $("#@Html.IdFor(Function(x) x)");
        var cmb = cmbJq.data("kendoComboBox");
        var visibleInput = $(cmb.input);
        visibleInput.dblclick(function (e) { visibleInput.select(); });

        var grid = cmbJq.closest(".k-grid");
        if(grid.length > 0) {
            grid.data("kendoGrid").bind("edit", function(e) {
                var partTemplateNumber = e.model.@partTemplateNumberPropertyName;
                
                if(partTemplateNumber !== "" && typeof(partTemplateNumber) !== "undefined") {
                    visibleInput.val(partTemplateNumber);
                }
            });
        }
    }());
</script>