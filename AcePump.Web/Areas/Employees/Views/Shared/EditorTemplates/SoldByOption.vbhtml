
@(Html.TypeManagerDropDownFor(Function(x) x, "SoldByOption", False))

<script type="text/javascript">
    var model = $("#SoldByOptionID").closest("tr").data("kendoEditable").options.model;
    $("#SoldByOptionID").data("kendoDropDownList").value(model.SoldByOptionID);
</script>
