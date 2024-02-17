@ModelType AcePump.Web.Models.DashboardConfigurationModel
@Imports AcePump.Web.Models


@Imports Yesod.Widgets
@Imports Yesod.Widgets.Models
@Imports Yesod.Kendo
@Imports AcePump.Domain.DataSource
@Imports AcePump.Web.WidgetQueries
@Imports AcePump.Web
@Imports AcePump.Web.Models

@Code
    ViewData("Title") = "Lease Dashboard"
    
    Dim queryOnLoad = Model.ManagerConfiguration.LeaseID.HasValue
End Code

@Section PreloadLibs
    <script src="@Url.Content("~/Scripts/kendo/jszip.min.js")" type="text/javascript"></script>
End Section

<link type="text/css" rel="Stylesheet" href="@Url.Content("~/Content/Soris/soris.widget.default.css")"/>
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
            leaseId: $("#LeaseID").val(),
            startDate: JSON.stringify( startDate ),
            endDate: JSON.stringify( endDate )
        };
     }

     var view = (function($, undefined){
        var HelperClass = function() {
        };

        HelperClass.prototype.Initialize = function() {
            this._LeaseName = $("#lease-name");
            this._btnExportExcel = $(".k-grid-excel-workaround");
            this._recordsGrid = $("#recordsGrid").data("kendoGrid");
            
            this._AttachHandlers();
            this._renderExcelIcon();
        };

        HelperClass.prototype._renderExcelIcon = function() {
            this._btnExportExcel
                .prepend(
                    $("<span>").addClass("k-icon").addClass("k-i-excel")
                );
        };

        HelperClass.prototype._AttachHandlers = function() {
            var h = this;
            $(Soris.Widgets.WidgetRequestMediator.getUnmanagedMediator()).on("globalQueryCompleted", function (e) {
                h._WidgetManager_QueryCompleted(e);
            });

            this._btnExportExcel.click(function(e){
                h._recordsGrid.saveAsExcel();

                h._setExcelInProgress();
                e.preventDefault();
            });

            this._recordsGrid.bind("excelExport", function(e) {
                var ixTicketDate = 4;
                var ixCost  = 5;
                var sheet = e.workbook.sheets[0];

                for (var i = 1; i < sheet.rows.length; i++) {
                    sheet.rows[i].cells[ixTicketDate].format = "yy-MM-dd";
                    sheet.rows[i].cells[ixCost].format = "$#####.00";
                }

                sheet.columns[ixCost].width = 13.57; //100px

                h._setExcelComplete();
            });
        };

        HelperClass.prototype._setExcelInProgress = function() {
            kendo.ui.progress(this._btnExportExcel, true);
        };

        HelperClass.prototype._setExcelComplete = function() {
            kendo.ui.progress(this._btnExportExcel, false);
        };

        HelperClass.prototype._WidgetManager_QueryCompleted = function(e) {
            this._UpdateLeaseInfo(e.GlobalResponse);
        };
        
        HelperClass.prototype._UpdateLeaseInfo = function(globalResponse) {
            if(globalResponse && globalResponse.LeaseName !== undefined) this._LeaseName.text(globalResponse.LeaseName);
        };

        var h = new HelperClass();
        $(window).load(function() { h.Initialize(); });
        return {
            Helper: h
        };
    })(jQuery);
</script>

<h2>Details of <span id="lease-name">@Model.ManagerConfiguration.LeaseName</span> Lease</h2>

@Html.Partial("_CommonWidgetManagerElements", Model.ManagerConfiguration)

@Html.HiddenFor(Function(model) model.ManagerConfiguration.CustomerID)

@(Html.Widget().PageConfiguration() _
                  .RequestMedia(Sub(media)
                                        media.Url(Url.Action("WidgetQuery", "History"))
                                        media.Type("ajax")
                                End Sub) _
                .QueryOnLoad(queryOnLoad) _
                .GlobalQueryUrl(Url.Action("GlobalQuery","History"))
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
                                                        ext.DisplayTextParameterName("LeaseName")
                                                End Sub)
                       End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("TotalSpent") _
    .Title("Total Spent At This Lease") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", True)
             End Sub)
)

@(Html.Widget().DirectionChange() _
    .Id("SpentOnPumpRepair") _
    .Title("Spent On Pumps At This Lease") _
    .AmountFormatString("{0:c}") _
    .AmountChangedFormatString("{0:c}") _
    .Filters(Sub(f)
                     f.StaticValue("includeNonPumpCost", False)
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

@(Html.Widget().DirectionChange() _
    .Id("TotalPulls") _
    .Title("Total Pulls")
)

@(Html.Widget().Chart(Of String) _
    .Id("TpReasonsRepaired") _
    .Title("Top Reasons Repaired") _
    .ChartType(ChartType.Pie) _
    .ShowLegend(False) _
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

@(Html.Widget.Chart(Of Date) _
    .Id("DailyRepairCostPastMonth") _
    .Title("Daily Repair Cost Past Month") _
    .ChartType(ChartType.Line) _
    .CategoryFormat("{0:d}") _
    .ValueFormat("{0:c}") _
    .Filters(Sub(x)
                     x.StaticValue("staticStartDate", Date.Now.AddMonths(-1).Ticks.ToString())
                     x.StaticValue("staticEndDate", Date.Now.Ticks.ToString())
             End Sub)
)

@(Html.Kendo().Grid(Of PartInspectionDashboardGridRowModel)() _
                    .Name("recordsGrid") _
                    .Filterable() _
                    .Sortable() _
                    .Pageable() _
                    .ToolBar(Sub(s)
                                 s.Custom().Text("Export to Excel").HtmlAttributes(New With {.class = "k-grid-excel-workaround"})
                             End Sub) _
                    .Excel(Sub(excel)
                               excel.FileName("Lease Dashboard Export " & Date.Now.ToShortDateString() & ".xlsx")
                               excel.Filterable(False)
                               excel.AllPages(True)
                           End Sub) _
                    .AutoBind(queryOnLoad) _
                    .Columns(Sub(c)
            c.Bound(Function(pi) pi.DeliveryTicketID).Title("Ticket #").Filterable(FilterableType.NumericId) _
                .ClientTemplate(
                "#=DeliveryTicketID#" &
                "#if(IsSignificantDesignChange){#<i title=""Significant design change"" class=""status-flag fa fa-exclamation""></i>#}#"
                )
            c.Bound(Function(pi) pi.ApiNumber).Title("API #")
            c.Bound(Function(pi) pi.LeaseName)
            c.Bound(Function(pi) pi.WellNumber)
            c.Bound(Function(pi) pi.TicketDate).Format("{0:d}")
            c.Bound(Function(pi) pi.Cost).Format("{0:c}").Filterable(FilterableType.RoundedDecimal)
            c.Template(@@<text></text>).ClientTemplate(
                                        "<a href=""" & Url.Action("Details", "DeliveryTicket") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">View Ticket</a>"
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