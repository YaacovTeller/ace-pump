@ModelType AcePump.Web.Models.DashboardConfigurationModel
@Imports AcePump.Web.Models

@Imports Yesod.Widgets
@Imports Yesod.Widgets.Models
@Imports Yesod.Kendo
@Imports AcePump.Domain.DataSource
@Imports AcePump.Web.WidgetQueries
@Imports AcePump.Web

@Code
    ViewData("Title") = "Well Dashboard"
    Dim queryOnLoad = Model.ManagerConfiguration.WellID.HasValue
End Code

<link type"text/css" rel="Stylesheet" href="@Url.Content("~/Content/Soris/soris.widget.default.css")"/>
<script type="text/javascript" src="@Url.Content("~/Scripts/Soris.All.min.js")"></script>

<script type="text/javascript">
      @Code
          Dim CustomerIdString As New MvcHtmlString("")
        Select Case Model.ManagerConfiguration.CustomerSelectionType
            Case SelectionType.Any
                  CustomerIdString = New MvcHtmlString("")
            
            Case SelectionType.AccessListOnly
                  CustomerIdString = New MvcHtmlString("customerId: $(""#CustomerID"").val(),")
            
            Case SelectionType.None
                  CustomerIdString = New MvcHtmlString("customerId: " & Model.ManagerConfiguration.CustomerID.Value & ",")
        End Select
    End Code
    
    function deliveryTickets_AdditionalData() {
        var startDate = $(".k-datepicker:first input").data("kendoDatePicker").value();
        var endDate = $(".k-datepicker:last input").data("kendoDatePicker").value();
         
        return {
            @CustomerIdString
            wellId: $("#WellID").val(),
            startDate: JSON.stringify( startDate ),
            endDate: JSON.stringify( endDate )
        };
    }

    var view = (function($, undefined){
        var HelperClass = function() {
        };

        HelperClass.prototype.Initialize = function() {
            this._WellNumber = $(".dashboard-well-title-area ol li:nth(0)");
            this._LeaseName = $(".dashboard-well-title-area ol li:nth(1)");
            this._CustomerName = $(".dashboard-well-title-area ol li:nth(2)");
            
            this._AttachHandlers();
        };

        HelperClass.prototype._AttachHandlers = function() {
            var h = this;
            $(Soris.Widgets.WidgetRequestMediator.getUnmanagedMediator()).on("globalQueryCompleted", function (e) {
                h._WidgetManager_QueryCompleted(e);
            });
        };

        HelperClass.prototype._WidgetManager_QueryCompleted = function(e) {
            this._UpdateWellInfo(e.globalResponse);
        };

        HelperClass.prototype._UpdateWellInfo = function(globalResponse) {
            this._WellNumber.text(globalResponse.WellNumber);
            this._LeaseName.text(globalResponse.LeaseName);
            this._CustomerName.text(globalResponse.CustomerName);
        };

        var h = new HelperClass();
        $(window).load(function() { h.Initialize(); });
        return {
            Helper: h
        };
    })(jQuery);
</script>

<h2>Well Dashboard</h2>

<div class="dashboard-well-title-area">
    <ol>
        <li class="well-number">@Model.ManagerConfiguration.WellNumber</li>
        <li>@Model.ManagerConfiguration.LeaseName</li>
        <li>@Model.ManagerConfiguration.CustomerName</li>
    </ol>
</div>

@Html.Partial("_CommonWidgetManagerElements", Model.ManagerConfiguration)

@(Html.Widget().PageConfiguration() _
    .RequestMedia(Sub(media)
                          media.Url(Url.Action("WidgetQuery", "History"))
                          media.Type("ajax")
                  End Sub) _
    .QueryOnLoad(queryOnLoad) _
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
                                                                ext.NonFiltering(True)
                                                                ext.DisplayTextParameterName("CustomerName")
                                                        End Sub)
                                   
                                   Case SelectionType.AccessListOnly
                                       filters.External(Sub(ext)
                                                                ext.ParameterName("CustomerID")
                                                                ext.JqInput("$(""#CustomerID"")")
                                                                ext.JqContainer("$(""#CustomerID_DropDown"").closest("".k-dropdown"")")
                                                                ext.TextBefore("Customer")
                                                                ext.NonFiltering(True)
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
                                                        ext.NonFiltering(True)
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
    .Id("TotalSpent") _
    .Title("Total Spent On This Well") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", True)
             End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("SpentOnPumpRepair") _
    .Title("Spent On Pumps") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", False)
             End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("TotalPulls") _
    .Title("Total Pulls")
)

@(Html.Widget().DirectionChange() _
    .Id("DailyCostOfCurrentRun") _
    .Title("Daily Cost of Current Run") _
    .AmountFormatString("{0:c}") _
    .FooterFormatString("Total cost of repairs at the well divided over the length of the current run.")
)@*Case 805 - Runtime Widgets Temporarily Hidden

    @(Html.Widget().Chart(Of String)() _
        .Id("PartsInWellRuntimeBreakdown") _
        .Title("Part Runtime") _
        .ValueFormat("{0} days") _
        .ChartType(ChartType.Bar) _
        .CategoryFormat("{0} days") _
        .CrossLink(Sub(crossLink)
                           crossLink.Action("PartDashboard", "History")
                           crossLink.AdditionalParameters("PartTemplateID")
                   End Sub)
    )
*@

@(Html.Kendo().Grid(Of DeliveryTicketsDashboardGridRowModel) _
    .Name("recordsGrid") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .AutoBind(queryOnLoad) _
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

