@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<h2>Part Categories</h2>

@Html.ActionKendoButton("Back to Part List", "Index", "PartTemplate", Nothing)
@(Html.Kendo().Grid(Of PartCategoryGridRowModel) _
    .Name("categories") _
    .Filterable() _
    .Sortable() _
    .DataSource(Sub(dataSource)
                    dataSource _
                        .Ajax() _
                        .Model(Sub(model)
                                   model.Id(Function(x) x.PartCategoryID)
                               End Sub) _
                        .Read("List", "PartCategory") _
                        .Create("Create", "PartCategory") _
                        .Update("Edit", "PartCategory")
                End Sub) _
    .Columns(Sub(columns)
                 columns.Bound(Function(x) x.CategoryName)
                 columns.Command(Sub(command)
                                     command.Edit()
                                 End Sub)
             End Sub) _
    .ToolBar(Sub(toolbar)
                 toolbar.Create().Text("Add A Category")
             End Sub)
    )