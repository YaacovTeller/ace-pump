@ModelType AcePump.Web.Models.DashboardConfigurationModel

@Imports Yesod.Widgets
@Imports Yesod.Widgets.Models
@Imports Yesod.Kendo
@Imports AcePump.Domain.DataSource
@Imports AcePump.Web.WidgetQueries
@Imports AcePump.Web
@Imports AcePump.Web.Models

@Code
    ViewData("Title") = "Customer Dashboard"
End Code

<link type"text/css" rel="Stylesheet" href="@Url.Content("~/Content/Soris/soris.widget.default.css")"/>
<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.All.min.js")"></script>
<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.Kendo.Svg.js")"></script>
<script type="text/javascript">
    function AveragePumpRuntime_CrossLink(e) {
        var id;
        if($('input[name=runtimeType]:checked').val() === "@CInt(RuntimeType.ByLease)") {
            id = e.Parameters.LeaseID;
            e.Url = "@Url.Action("LeaseDashboard", "History")";
        } else {
            id = e.Parameters.WellID;;
            e.Url = "@Url.Action("WellDashboard", "History")";
        }

        e.Parameters = {id: id};
    }

    function TpFailingParts_CrossLink(e) {
        if($("[name=partType]:checked").val() === "@CInt(PartQueryType.ByCategory)") {
            e.preventDefault();
        }
    }
</script>

<h2>Customer Dashboard</h2>

@Html.Partial("_CommonWidgetManagerElements", Model.ManagerConfiguration)

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
                   End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("AvgDailyRepairCost") _
    .Title("Average Daily Repair Cost") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", True)
             End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("AvgDailyPumpCost") _
    .Title("Average Daily Pump Cost") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", False)
             End Sub)
)

@*@(Html.Widget().DirectionChange() _
    .Id("ExpectedPullsThisMonth") _
    .Title("Expected Pulls This Month")
)*@

@(Html.Widget().Chart(Of String) _
    .Id("TpReasonsRepaired") _
    .Title("Top Reasons Repaired") _
    .ChartType(ChartType.Pie) _
    .ShowLegend(True) _
    .Filters(Sub(filters)
                     filters.NumericSpinner(Sub(spinner)
                                                    spinner.Min(1)
                                                    spinner.Max(5)
                                                    spinner.ParameterName("NumberToShow")
                                                    spinner.TextBefore("Show the top")
                                                    spinner.TextAfter("reasons repaired.")
                                                    spinner.DefaultValue(3)
                                            End Sub)
             End Sub) _
     .CrossLink(Sub(crossLink)
                        crossLink.Action("ReasonRepairedDashboard", "History")
                        crossLink.CategoryParameterName("reasonRepaired")
                End Sub)
)

@(Html.Widget().Chart(Of String) _
    .Id("TpFailingPart") _
    .Title("Top Failing Parts") _
    .ChartType(ChartType.Pie) _
    .ShowLegend(True) _
    .MaxCharactersInCategory(20) _
    .Filters(Sub(filters)
                 filters.NumericSpinner(Sub(spinner)
                                            spinner.Min(1)
                                            spinner.Max(5)
                                            spinner.ParameterName("NumberToShow")
                                            spinner.TextBefore("Show the top")
                                            spinner.TextAfter("failing parts.")
                                            spinner.DefaultValue(3)
                                        End Sub)

                 filters.RadioButtons(Sub(radio)
                                          radio.ParameterName("partType")
                                          radio.DefaultValue(CInt(PartQueryType.ByPart))
                                          radio.Options(Sub(options)
                                                            options.Add().Text("By Part").Value(CInt(PartQueryType.ByPart))
                                                            options.Add().Text("By Category").Value(CInt(PartQueryType.ByCategory))
                                                        End Sub)
                                      End Sub)
             End Sub) _
    .CrossLink(Sub(link)
                   link.Action("PartDashboard", "History")
                   link.AdditionalParameters("PartTemplateID")
                   link.Events(Sub(events)
                                   events.CrossLink("TpFailingParts_CrossLink")
                               End Sub)
               End Sub)
)

@(Html.Widget().Chart(Of Date) _
    .Id("DailyRepairCostPastMonth") _
    .Title("Total Daily Repair Cost - Past Month") _
    .ChartType(ChartType.Line) _
    .CategoryFormat("d") _
    .ValueFormat("{0:c}") _
    .Filters(Sub(x)
                     x.StaticValue("staticStartDate", Date.Now.AddMonths(-1).Ticks.ToString())
                     x.StaticValue("staticEndDate", Date.Now.Ticks.ToString())
             End Sub)
)

@(Html.Widget().Chart(Of Date) _
    .Id("DailyRepairCostPast6Months") _
    .Title("Total Daily Repair Cost - Past 6 Months") _
    .ChartType(ChartType.Line) _
    .CategoryFormat("d") _
    .ValueFormat("{0:c}") _
    .Filters(Sub(x)
                     x.StaticValue("staticStartDate", Date.Now.AddMonths(-6).Ticks.ToString())
                     x.StaticValue("staticEndDate", Date.Now.Ticks.ToString())
             End Sub)
)

@(Html.Widget().Chart(Of Date) _
    .Id("DailyRepairCostPast12Months") _
    .Title("Total Daily Repair Cost - Past Year") _
    .ChartType(ChartType.Line) _
    .CategoryFormat("d") _
    .ValueFormat("{0:c}") _
        .Filters(Sub(x)
                         x.StaticValue("staticStartDate", Date.Now.AddMonths(-12).Ticks.ToString())
                         x.StaticValue("staticEndDate", Date.Now.Ticks.ToString())
                 End Sub)
)

@(Html.Widget().Chart(Of String) _
    .Id("RepairCostByReason") _
    .Title("Repair Cost By Reason") _
    .ChartType(ChartType.Bar) _
    .ValueFormat("{0:c}") _
    .CrossLink(Sub(crossLink)
                       crossLink.Action("ReasonRepairedDashboard", "History")
                       crossLink.CategoryParameterName("reasonRepaired")
               End Sub)
)
@* Case 805 - Runtime Widgets Temporarily Hidden
@(Html.Widget.Chart(Of String) _
    .Id("AveragePumpRuntime") _
    .Title("Average Pump Runtime") _
    .ChartType(ChartType.Bar) _
    .ValueFormat("{0} days") _
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
                       link.AdditionalParameters("LeaseID", "WellID")
                       link.Events(Sub(events)
                                           events.CrossLink("AveragePumpRuntime_CrossLink")
                                   End Sub)
               End Sub)
)
*@
@(Html.Widget().Chart(Of String) _
    .Id("PullsByQuarter") _
    .Title("Pulls") _
    .ChartType(ChartType.Pie) _
    .ShowLegend(True) _
    .Filters(Sub(filters)
                     filters.RadioButtons(Sub(radio)
                                                  radio.ParameterName("period")
                                                  radio.DefaultValue(CInt(TimePeriod.Quarter))
                                                  radio.Options(Sub(options)
                                                                        options.Add().Text("By Quarter").Value(CInt(TimePeriod.Quarter))
                                                                        options.Add().Text("By Month").Value(CInt(TimePeriod.Month))
                                                                End Sub)
                                          End Sub)
             End Sub)
)

@(Html.Kendo().Grid(Of DeliveryTicketsDashboardGridRowModel) _
    .Name("recordsGrid") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                     c.Bound(Function(dt) dt.DeliveryTicketID).Filterable(FilterableType.NumericId)
                     c.Bound(Function(dt) dt.CustomerName)
                     c.Bound(Function(dt) dt.LeaseName)
                     c.Bound(Function(dt) dt.WellNumber)
                     c.Bound(Function(dt) dt.TicketDate).Format("{0:d}")
                     c.Bound(Function(dt) dt.Cost).Format("{0:c}").Filterable(FilterableType.RoundedDecimal)
                     c.Template(@@<text></text>).ClientTemplate(
                                    "<a href=""" & Url.Action("Details", "DeliveryTicket") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">View Ticket</a>"
                                  )
    End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.DeliveryTicketID)) _
                        .Read(Sub(read)
                                      read.Action("DeliveryTickets", "History")
                                      read.Type(HttpVerbs.Post)
                              End Sub)
                End Sub) _
    .AsWidgetRecordsGrid()
)