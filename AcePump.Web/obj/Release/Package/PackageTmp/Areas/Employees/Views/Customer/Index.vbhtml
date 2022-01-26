@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos

@Code
    ViewData("Title") = "Index"
End Code

<h2>Customer List</h2>

<p>
    @Html.ActionKendoButton("Create New", "Create")
</p>

<script type="text/javascript">
    function onAdditionalData() {
        return {
            term: $("#jumpToCustomer").val()
        };
    }

    function jumpToCustomer_SelectCustomer(e) {
        document.location = "@Url.Action("Details")/" + this.dataItem(e.item.index()).Id;
    }

    function grid_Select(e) {
        document.location = "@Url.Action("Details")/" + this.dataItem($(e.currentTarget).closest("tr")).CustomerID;
    }

    function edit_Click(e) {
        document.location="@Url.Action("Edit")/" + this.dataItem($(e.currentTarget).closest("tr")).CustomerID;
    }
    function viewDiscounts_Click(e) {
        document.location = "@Url.Action("PriceList")/" + this.dataItem($(e.currentTarget).closest("tr")).CustomerID;
    }    
</script>

Jump To Customer: @(Html.Kendo().AutoComplete() _
                        .Name("jumpToCustomer") _
                        .DataTextField("Name") _
                        .MinLength(2) _
                        .Placeholder("Start typing a customer name...") _
                        .Events(Sub(e)
                                        e.Select("jumpToCustomer_SelectCustomer")
                                End Sub) _
                        .DataSource(Sub(dataSource)
                                            dataSource _
                                            .Read(Function(reader) reader.Action("StartsWith", "Customer") _
                                                                         .Data("onAdditionalData")) _
                                            .ServerFiltering(True)
                                    End Sub)
                    )
                    
@(Html.Kendo().Grid(Of CustomerGridRowModel)() _
    .Name("customers") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                 c.Bound(Function(cust) cust.CustomerName)
                 c.Bound(Function(cust) cust.Address1)
                 c.Bound(Function(cust) cust.Address2)
                 c.Bound(Function(cust) cust.City)
                 c.Bound(Function(cust) cust.State)
                 c.Bound(Function(cust) cust.Zip)
                 c.Bound(Function(cust) cust.Phone)
                 c.Bound(Function(cust) cust.Website)
                 c.Bound(Function(cust) cust.CountyName)
                 c.Bound(Function(cust) cust.APINumberRequired) _
                     .ClientTemplate("<input type=""checkbox"" disabled=""disabled"" #= APINumberRequired ? checked=""checked"" : """" # />") _
                     .Title("APINumber Required") _
                     .Filterable(Function(filterable) filterable.Messages(Function(m) m.IsFalse("Required")) _
                                                                .Messages(Function(m) m.IsTrue("Not Required")) _
                                                                .Messages(Function(m) m.Info("Show Customers that have APINumbers on wells")))
                 c.Bound(Function(cust) cust.UsesInventory) _
                     .ClientTemplate("<input type=""checkbox"" disabled=""disabled"" #= UsesInventory ? checked=""checked"" : """" # />") _
                     .Filterable(Function(filterable) filterable.Messages(Function(m) m.IsFalse("Does not use inventory")) _
                                                                .Messages(Function(m) m.IsTrue("Uses inventory")) _
                                                                .Messages(Function(m) m.Info("Show Customers that use inventory")))


                 If User.IsInRole("AcePumpAdmin") Then
                     c.Bound(Function(cust) cust.PayUpFront) _
                     .ClientTemplate("<input type=""checkbox"" disabled=""disabled"" #= PayUpFront ? checked=""checked"" : """" # />") _
                     .Filterable(Function(filterable) filterable.Messages(Function(m) m.IsFalse("Does not pay up front")) _
                                                                .Messages(Function(m) m.IsTrue("Must pay up front")) _
                                                                .Messages(Function(m) m.Info("Show Customers that must pay up front")))
                 End If

                 c.Command(Sub(com)
                               com.Custom("Details").Click("grid_Select")
                               com.Custom("Edit").Click("edit_Click")
                               com.Custom("View Price List").Click("viewDiscounts_Click")
                           End Sub)
             End Sub) _
    .DataSource(Sub(dataSource)
                    dataSource _
                    .Ajax() _
                    .Model(Sub(model) model.Id(Function(id) id.CustomerID)) _
                    .Read("List", "Customer")
                End Sub)
    )