@ModelType AcePump.Web.Models.DashboardConfigurationModel

@Imports Yesod.Widgets
@Imports Yesod.Widgets.Models
@Imports Yesod.Kendo
@Imports AcePump.Domain.DataSource
@Imports AcePump.Web.WidgetQueries
@Imports AcePump.Web
@Imports AcePump.Web.Models

@Code
    ViewData("Title") = "Part Dashboard"
End Code

<link type"text/css" rel="Stylesheet" href="@Url.Content("~/Content/Soris/soris.widget.default.css")"/>
<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.All.min.js")"></script>

<script type="text/javascript">
    function AllPartRuntimesGroupedByWell_CrossLink(e) {
        if($('input[name=runtimeType]:checked').val() === "@CInt(RuntimeType.ByLease)") {
            e.Parameters.locationName = e.Parameters.category;
            e.Url = "@Url.Action("LeaseDashboard", "History")";
        } else {
            e.Parameters.wellNumber = e.Parameters.category;
            e.Url = "@Url.Action("WellDashboardByNumber", "History")";
        }

        delete e.Parameters.category;
    }

    function PartTemplateID_AdditionalData() {
        return {
            categoryId: $("#CategoryID").val(),
            term: $("#ManagerConfiguration_PartTemplateID").data("kendoComboBox").input.val()
        };
    }
</script>

<h2>Part Dashboard</h2>

@Html.Partial("_CommonWidgetManagerElements", Model.ManagerConfiguration)

@(Html.Kendo().ComboBoxFor(Function(model) model.ManagerConfiguration.CategoryID) _
    .DataTextField("CategoryName") _
    .DataValueField("CategoryID") _
    .MinLength(2) _
    .Filter(FilterType.StartsWith) _
    .Events(Sub(e) e.Change("function(e) { partialView.Helper.PreventComboBoxFreeText(this, e); }")) _
    .Placeholder("Start typing...") _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Read(Sub(read)
                                      read.Action("CategorySearch", "History")
                                      read.Type(HttpVerbs.Post)
                              End Sub) _
                        .ServerFiltering(True)
                End Sub)
)

@(Html.Kendo().ComboBoxFor(Function(model) model.ManagerConfiguration.PartTemplateID) _
                                        .DataTextField("PartDescription") _
                                        .DataValueField("PartTemplateID") _
                                        .MinLength(2) _
                                        .Filter(FilterType.Contains) _
                                        .Text(Model.ManagerConfiguration.PartTemplateNumber) _
                                        .Events(Sub(e) e.Change("function(e) { partialView.Helper.PreventComboBoxFreeText(this, e); }")) _
                                        .AutoBind(False) _
                                        .Placeholder("Start typing a part number...") _
                                        .CascadeFrom("CategoryID") _
                                        .DataSource(Sub(dataSource)
                                                        dataSource _
                                            .Read(Sub(read)
                                                      read.Action("PartSearch", "History")
                                                      read.Type(HttpVerbs.Post)
                                                      read.Data("PartTemplateID_AdditionalData")
                                                  End Sub) _
                                            .ServerFiltering(True)
                                                    End Sub)
    )


@(Html.Widget().PageConfiguration() _
    .RequestMedia(Sub(media)
                          media.Url(Url.Action("WidgetQuery", "History"))
                          media.Type("ajax")
                  End Sub) _
    .QueryOnLoad(True) _
    .GlobalQueryUrl(Url.Action("GlobalQuery", "History"))
)


@(Html.Widget().GlobalFilterPanel() _
    .GlobalFilters(Sub(filters)
                       Select Case Model.ManagerConfiguration.CustomerSelectionType
                           Case SelectionType.Any
                               filters.External(Sub(ext)
                                                    ext.ParameterName("CustomerID")
                                                    ext.JqInput("$(""#CustomerID"")")
                                                    ext.JqContainer("$(""#CustomerID"").closest("".k-combobox"")")
                                                    ext.TextBefore("Customer")
                                                    ext.DisplayTextParameterName("CustomerName")
                                                End Sub)

                           Case SelectionType.AccessListOnly
                               filters.External(Sub(ext)
                                                    ext.ParameterName("CustomerID")
                                                    ext.JqInput("$(""#CustomerID"")")
                                                    ext.JqContainer("$(""#CustomerID_DropDown"").closest("".k-dropdown"")")
                                                    ext.TextBefore("Customer")
                                                    ext.DisplayTextParameterName("CustomerName")
                                                End Sub)

                           Case SelectionType.None
                               filters.StaticValue("CustomerID", String.Join(","c, Model.ManagerConfiguration.CustomersIDsThatMayBeAccessed.Values.ToList()))
                       End Select

                       filters.External(Sub(ext)
                                            ext.ParameterName("LeaseID")
                                            ext.JqInput("$(""#LeaseID"")")
                                            ext.JqContainer("$(""#LeaseID"").closest("".k-combobox"")")
                                            ext.TextBefore("Lease")
                                            ext.DisplayTextParameterName("LeaseName")
                                        End Sub)

                       filters.External(Sub(ext)
                                            ext.ParameterName("WellID")
                                            ext.JqInput("$(""#WellID"")")
                                            ext.JqContainer("$(""#WellID"").closest("".k-combobox"")")
                                            ext.TextBefore("Well")
                                            ext.DisplayTextParameterName("WellNumber")
                                        End Sub)

                       filters.External(Sub(ext)
                                            ext.ParameterName("CategoryID")
                                            ext.JqInput("$(""#ManagerConfiguration_CategoryID"")")
                                            ext.JqContainer("$(""#ManagerConfiguration_CategoryID"").closest("".k-combobox"")")
                                            ext.TextBefore("Category")
                                            ext.DisplayTextParameterName("CategoryName")
                                        End Sub)

                       filters.External(Sub(ext)
                                            ext.ParameterName("PartTemplateID")
                                            ext.JqInput("$(""#ManagerConfiguration_PartTemplateID"")")
                                            ext.JqContainer("$(""#ManagerConfiguration_PartTemplateID"").closest("".k-combobox"")")
                                            ext.TextBefore("Part")
                                            ext.DisplayTextParameterName("PartTemplateNumber")
                                        End Sub)
                   End Sub)
)
@*
Case 805 - Runtime Widgets Temporarily Hidden
@(Html.Widget().DirectionChange() _
    .Id("AveragePartRuntime") _
    .Title("Average Runtime")
)
*@
@(Html.Widget().DirectionChange() _
    .Id("TotalFailures") _
    .Title("Total Failures")
)

@(Html.Widget().Chart(Of String) _
    .Id("TpReasonsRepaired") _
    .Title("Top Reasons Repaired") _
    .ChartType(ChartType.Pie) _
    .Filters(Sub(filters)
                     filters.NumericSpinner(Sub(spinner)
                                                    spinner.Min(1)
                                                    spinner.Max(5)
                                                    spinner.ParameterName("NumberToShow")
                                                    spinner.TextBefore("Show the top")
                                                    spinner.TextAfter("reasons repaired.")
                                                    spinner.DefaultValue(2)
                                            End Sub)
             End Sub) _
    .CrossLink(Sub(crossLink)
                       crossLink.Action("ReasonRepairedDashboard", "History")
                       crossLink.CategoryParameterName("reasonRepaired")
               End Sub)         
)
@*Case 805 - Runtime Widgets Temporarily Hidden
@(Html.Widget.Chart(Of String) _
    .Id("AllPartRuntimesGroupedByWell") _
    .Title("Runtime") _
    .ChartType(ChartType.Bar) _
    .Filters(Sub(filters)
                     filters.RadioButtons(Sub(radio)
                                                  radio.ParameterName("runtimeType")
                                                  radio.DefaultValue(CInt(RuntimeType.ByLease))
                                                  radio.Options(Sub(options)
                                                                        options.Add().Text("By Lease").Value(CInt(RuntimeType.ByLease))
                                                                        options.Add().Text("By Well").Value(CInt(RuntimeType.ByWell))
                                                                End Sub)
                                          End Sub)
             End Sub) _
    .CrossLink(Sub(link)
                       link.CategoryParameterName("category")
                       link.Events(Sub(events)
                                           events.CrossLink("AllPartRuntimesGroupedByWell_CrossLink")
                                   End Sub)
               End Sub)
)
*@

@(Html.Kendo().Grid(Of PartInspectionDashboardGridRowModel) _
    .Name("recordsGrid") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                     c.Bound(Function(dt) dt.DeliveryTicketID).Filterable(FilterableType.NumericId)
                     c.Bound(Function(dt) dt.CustomerName)
                     c.Bound(Function(dt) dt.LeaseName)
                     c.Bound(Function(dt) dt.WellNumber)
                     c.Bound(Function(dt) dt.PartDescription)
                     c.Bound(Function(dt) dt.TicketDate).Format("{0:d}")
                     c.Template(@@<text></text>).ClientTemplate(
                                    "<a href=""" & Url.Action("RepairDetails", "DeliveryTicket") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">View Ticket</a>"
                                  )
    End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.DeliveryTicketID)) _
                        .Read(Sub(read)
                                      read.Action("RepairTickets", "History")
                                      read.Type(HttpVerbs.Post)
                              End Sub)
                End Sub) _
    .AsWidgetRecordsGrid()
)