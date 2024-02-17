@ModelType AcePump.Web.Models.DashboardConfigurationModel

@Imports Yesod.Widgets
@Imports Yesod.Widgets.Models
@Imports Yesod.Kendo
@Imports AcePump.Domain.DataSource
@Imports AcePump.Web.WidgetQueries
@Imports AcePump.Web
@Imports AcePump.Web.Models

@Code
    ViewData("Title") = "Reason Repaired Dashboard"
End Code

<link type"text/css" rel="Stylesheet" href="@Url.Content("~/Content/Soris/soris.widget.default.css")"/>
<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.All.min.js")"></script>

<h2>Reason Repaired Dashboard</h2>

@Html.Partial("_CommonWidgetManagerElements", Model.ManagerConfiguration)

@(Html.TypeManagerDropDownFor(Function(Model) Model.ManagerConfiguration.ReasonRepaired, "ReasonRepaired"))

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
                             ext.ParameterName("ReasonRepaired")
                             ext.JqInput("$(""#ManagerConfiguration_ReasonRepaired"")")
                             ext.JqContainer("$(""#ManagerConfiguration_ReasonRepaired"").closest("".k-dropdown"")")
                             ext.TextBefore("Reason Repaired")
                     End Sub)
                   End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("PumpsReplacedNotRepaired") _
    .Title("Pumps Replaced and not Repaired")
)

@(Html.Widget().DirectionChange() _
    .Id("AverageRepairCost") _
    .Title("Average Repair Cost") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}")
)

@(Html.Widget().DirectionChange() _
    .Id("SpentOnPumpRepair") _
    .Title("Total Spent") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", False)
             End Sub)
)

@(Html.Widget().Chart(Of String) _
    .Id("LeasesWhereThisHappens") _
    .Title("Leases Where This Happens") _
    .ChartType(ChartType.Pie) _
    .Filters(Sub(filters)
                     filters.NumericSpinner(Sub(spinner)
                                                    spinner.Min(1)
                                                    spinner.Max(5)
                                                    spinner.ParameterName("NumberToShow")
                                                    spinner.TextBefore("Show the top")
                                                    spinner.TextAfter("leases where this happens.")
                                                    spinner.DefaultValue(2)
                                            End Sub)
             End Sub) _
    .CrossLink(Sub(crossLink)
                       crossLink.Action("LeaseDashboard", "History")
                       crossLink.CategoryParameterName("locationName")
               End Sub)
)

@(Html.Widget.Chart(Of String) _
    .Id("AssociatedFailures") _
    .Title("Associated Failures") _
    .ChartType(ChartType.Bar) _
    .Description("Pumps that were {0} also had these problems.") _
    .Filters(Sub(filters)
                     filters.NumericSpinner(Sub(spinner)
                                                    spinner.Min(5)
                                                    spinner.Max(30)
                                                    spinner.DefaultValue(10)
                                                    spinner.ParameterName("NumberToShow")
                                                    spinner.TextBefore("Show the top")
                                                    spinner.TextAfter("associated failures.")
                                            End Sub)
             End Sub) _
    .CrossLink(Sub(crossLink)
                       crossLink.Action("ReasonRepairedDashboard", "History")
                       crossLink.CategoryParameterName("reasonRepaired")
               End Sub)
)

@(Html.Widget.Chart(Of String) _
    .Id("PartsWithThisFailure") _
    .Title("Parts With This Failure") _
    .ChartType(ChartType.Bar) _
    .Filters(Sub(filters)
                 filters.NumericSpinner(Sub(spinner)
                                            spinner.Min(5)
                                            spinner.Max(30)
                                            spinner.DefaultValue(10)
                                            spinner.ParameterName("NumberToShow")
                                            spinner.TextBefore("Show the top")
                                            spinner.TextAfter("parts with this failure.")
                                        End Sub)
             End Sub) _
    .CrossLink(Sub(crossLink)
                   crossLink.Action("PartDashboard", "History")
                   crossLink.AdditionalParameters("PartTemplateID")
               End Sub)
)

@(Html.Kendo().Grid(Of PartInspectionDashboardGridRowModel) _
                .Name("recordsGrid") _
                .Filterable() _
                .Sortable() _
                .Pageable() _
                .Columns(Sub(c)
        c.Bound(Function(dt) dt.DeliveryTicketID).Filterable(FilterableType.NumericId) _
            .ClientTemplate(
            "#=DeliveryTicketID#" &
            "#if(IsSignificantDesignChange){#<i title=""Significant design change"" class=""status-flag fa fa-exclamation""></i>#}#"
            )
        c.Bound(Function(dt) dt.CustomerName)
        c.Bound(Function(dt) dt.LeaseName)
        c.Bound(Function(dt) dt.WellNumber)
        c.Bound(Function(dt) dt.PartDescription)
        c.Bound(Function(dt) dt.ReasonRepaired)
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