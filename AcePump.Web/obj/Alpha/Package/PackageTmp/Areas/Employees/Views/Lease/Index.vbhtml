@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.min.js")"></script>

<h2>Lease Locations</h2>

<p>
    @Html.ActionKendoButton("Back to Well Locations", "Index", "Well")
</p>

@(Html.Kendo().Grid(Of LeaseGridRowModel)() _
    .Name("LeaseLocations") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .ToolBar(Sub(t) t.Create()) _
    .Columns(Sub(c)
                     c.Bound(Function(lease) lease.LeaseID)
                     c.Bound(Function(lease) lease.LocationName)
                     c.Command(Sub(com)
                                       com.Edit()
                               End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model)
                                       model.Id(Function(id) id.LeaseID)
                                       model.Field(Function(id) id.LeaseID).Editable(False)
                               End Sub) _
                        .Read("List", "Lease") _
                        .Update("Edit", "Lease") _
                        .Create("KendoCreate", "Lease")
                End Sub)
    )