@Imports AcePump.Web.Areas.Employees.Models.DisplayDtos
@Imports Yesod.Kendo
@Imports AcePump.Common

@Code
    ViewData("Title") = "Index"
End Code

<h2>Delivery Ticket List</h2>

<link type="text/css" rel="Stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css"></link>
<script type="text/javascript">
    var view = (function($, undefined) {
        "use strict";

        var DEFAULT_TOGGLE_STATE_NAME = "either";
        var TOGGLE_STATES = {
            "true": {
                name: "true",
                className: "toggle-true",
                filter: {
                    operator: "eq",
                    value: true
                },
                nextStateName: "false"
            },
            "false": {
                name: "false",
                className: "toggle-false",
                filter: {
                    operator: "eq",
                    value: false
                },
                nextStateName: "either"
            },
            "either": {
                name: "either",
                className: "toggle-either",
                nextStateName: "true"
            }
        };

        var FilterModes = {
            IdSearch: 0,
            General: 1
        };

        var HelperClass = function() {
            this._FilterMode = FilterModes.General;
            this._InReadRepeat = false;
        };

        HelperClass.prototype.Initialize = function() {
            this._DtGrid = $(".k-grid").data("kendoGrid");

            this._InitializeFilterState();
            this._AttachHandlers();
        };

        HelperClass.prototype._InitializeFilterState = function() {
            this._InitializeToggleStateNames();
            var initialFilters = [];

            var h = this;
            $(".toggle-filter").each(function(ix, el) {
                var initialStateName = $(this).data("initialStateName");
                if(typeof(initialStateName) === "undefined") initialStateName = DEFAULT_TOGGLE_STATE_NAME;
                else initialStateName = initialStateName.toString();

                var state = TOGGLE_STATES[initialStateName];

                h._SetToggleState(this, state, true);
                if(state.filter) {
                    initialFilters.push({
                        field: $(this).data("field"),
                        operator: state.filter.operator,
                        value: state.filter.value
                    });
                }
            });

            if(initialFilters.length > 0) {
                this._DtGrid.dataSource.filter({
                    filters: initialFilters,
                    logic: "and"
                });
            }
        };

        HelperClass.prototype._InitializeToggleStateNames = function() {
            this._ToggleStateClassNames = [];

            for(var stateName in TOGGLE_STATES) {
                if(TOGGLE_STATES.hasOwnProperty(stateName)) {
                    this._ToggleStateClassNames.push(TOGGLE_STATES[stateName].className);
                }
            }
        };

        HelperClass.prototype._SetToggleState = function(el, state, doNotAutoFilter) {
            $(el)
                .data("stateName", state.name)
                .removeClass(this._ToggleStateClassNames.join(" "))
                .addClass(state.className);

            if(!doNotAutoFilter) {
                var field = $(el).data("field");
                var currentFilter = this._DtGrid.dataSource.filter();
                if(!currentFilter) currentFilter = {
                    filters: [],
                    logic: "and"
                };

                for(var i=0; i<currentFilter.filters.length; i++) {
                    if(currentFilter.filters[i].field === field) {
                        currentFilter.filters.splice(i, 1);
                        break;
                    }
                }

                if(state.filter) {
                    currentFilter.filters.push({
                        field: field,
                        operator: state.filter.operator,
                        value: state.filter.value
                    });
                }

                this._DtGrid.dataSource.filter(currentFilter);
            }
        };

        HelperClass.prototype._AttachHandlers = function() {
            var h = this;

            this._DtGrid.dataSource.bind("requestStart", function(e) {
                if(e.type === "read" && !h._InReadRepeat) {
                    var dataSource = e.sender;
                    h.DetectFilterMode(dataSource.filter());

                    if(h._FilterMode === FilterModes.IdSearch) {
                        e.preventDefault();

                        h._InReadRepeat = true;
                        dataSource.one("requestEnd", function(e) { h._InReadRepeat = false; });
                        dataSource.read();
                    }
                }
            });

            $(".toggle-filter").click(function(e) {
                var field = $(this).data("field");
                var oldStateName = $(this).data("stateName");
                var oldState = TOGGLE_STATES[oldStateName];
                var newState = TOGGLE_STATES[oldState.nextStateName];

                h._SetToggleState(this, newState);
            });
        };

        HelperClass.prototype.DetectFilterMode = function(filters) {
            if(filters === null || filters === undefined) {
                this.FilterMode = FilterModes.General;
                return;
            }

            var found = false;
            for(var i = 0; i < filters.filters.length; i++) {
                var f = filters.filters[i];

                if(f.field === "DeliveryTicketID" && f.operator === "eq") found = true;
            }

            if(found) this._FilterMode = FilterModes.IdSearch;
            else this._FilterMode = FilterModes.General;

            this._UpdateFilterMode();
        };

        HelperClass.prototype._UpdateFilterMode = function() {
            var h = this;

            switch(this._FilterMode) {
                case FilterModes.IdSearch:
                    $(".toggle-filter").each(function(ix, el) {
                        h._SetToggleState(el, TOGGLE_STATES.either);
                    });
                    break;

                case FilterModes.General:
                    break;
            }
        };

        var h = new HelperClass();
        $(window).load(function(){h.Initialize()});
        return {
            Helper: h
        };
    })(jQuery);

    function toggle_Click(e) {
        var grid = $("#deliveryTickets").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var deliveryTicket = grid.dataItem(currentRow);

        if(!deliveryTicket.IsClosed) {
            var yes = confirm("Do you really want to close delivery ticket #" + deliveryTicket.DeliveryTicketID);

            if (yes) {
                 $.ajax({
                    dataType: "json",
                    url: "@Url.Action("CloseAjax", "DeliveryTicket")",
                     data: { id: deliveryTicket.DeliveryTicketID },
                    type: "POST",
                    success: function(result) {
                        alert('Ticket successfully closed.');
                        deliveryTicket.IsClosed = true;
                        grid.dataSource.read();
                    },
                    error: function(data) {
                        alert('Could not close the ticket. Please make sure that the correct ticket is selected.');
                    }
                }); 
            }
        } else {
            document.location = "@Url.Action("Reopen")/" + deliveryTicket.DeliveryTicketID;
        }
    }

    function markReadyForQuickbooks_Click(e){
        var grid = $("#deliveryTickets").data("kendoGrid");
        var currentRow = $(e.currentTarget).closest("tr");
        var deliveryTicket = grid.dataItem(currentRow);

        var deliveryticketID =  deliveryTicket.DeliveryTicketID;
            $.ajax({
                dataType: "json",
                url: "@Url.Action("MarkAsReadyForQuickbooks", "DeliveryTicket")",
                data: {id:deliveryticketID},
                type: "POST",
                success: function(result) {
                    alert('Marked successfully for QuickBooks.');
                    grid.dataSource.read();
                },
                error: function(data) {
                    alert('Could not mark the ticket as ready for QuickBooks. Please check that the ticket is closed.');
                }
            });
    }

    function deliveryTickets_AdditionalData(e) {
        var hideClosed = $("#chkHideClosed").prop("checked");
        var hideQuotes = $("#chkHideQuotes").prop("checked");

        return {
            hideClosed: hideClosed,
            hideQuotes: hideQuotes
        };
    }

    function deliveryTickets_DataBound(e) {
        $(".toggle").click(toggle_Click);
        $(".markReadyForQuickbooks").click(markReadyForQuickbooks_Click);

        var grid = this;

        grid.tbody.find(">tr").each(function(){
            var deliveryTicket = grid.dataItem(this);

            if(deliveryTicket.IsClosed) {
                disableEditButtons($(this));
            }
        });
    }

    function disableEditButtons(row) {
        var editButton = row.find(".k-grid-EditTicket");
        var repairButton = row.find(".k-grid-RepairPump");
        var deleteButton = row.find(".k-grid-delete");

        editButton.addClass("k-state-disabled").removeClass("k-grid-EditTicket");
        repairButton.addClass("k-state-disabled").removeClass("k-grid-RepairPump");
        deleteButton.addClass("k-state-disabled").removeClass("k-grid-delete");
    }

    $(document).ready(function() {
        $("#chkHideClosed").click(function() {
            var grid = $("#deliveryTickets").data("kendoGrid");

            grid.dataSource.read();
        });

        $("#chkHideQuotes").click(function() {
            var grid = $("#deliveryTickets").data("kendoGrid");

            grid.dataSource.read();
        });
    });
</script>

<style>
    .toggle-filter.toggle-true {
        color: Green;
    }
    
    .toggle-filter.toggle-false {
        color: Red;
    }
    
    .toggle-filter.toggle-either {
        color: rgba(180, 180, 180, 1);
    }
    
    .toggle-filter i {
        font-size: 20px;
        border: none;
    }
    
    .toggle-filter {
        cursor: pointer;
        
        background-image: none,linear-gradient(to bottom,rgba(255,255,255,0) 0,rgba(255,255,255,.6) 100%);
        background-position: 50% 50%;
        
        border: 1px solid #c5c5c5;
        border-radius: 4px;
        
        margin-right: 5px;
        padding: 5px 20px 5px 7px;
    }
    
    .toggle-filter:hover {
        border-color: #b6b6b6;
        background-color: #bcb4b0;
    }
    
    .toggle-filter:focus {
        outline: 0;
    }
    
    .status-flag {
        font-size: 18px;
        margin-left: 5px;
    }
    
    .filter-bar {
        padding: 10px;
        display: inline;
        min-width: 150px;
    }
</style>
<p>
    @Html.ActionKendoButton("Create New Delivery Ticket", "Create")
</p>

<fieldset class="filter-bar">
    <legend>Filters</legend>
    <button class="toggle-filter" data-field="IsClosed" data-initial-state-name="false"><i class="fa fa-check-square-o"></i> Closed</button>
    <button class="toggle-filter" data-field="IsSigned"><i class="fa fa-clipboard"></i> Signed</button>
    <button class="toggle-filter" data-field="Quote" data-initial-state-name="false"><i class="fa fa-usd"></i> Quote</button>
    <button class="toggle-filter" data-field="IsDownloadedToQb"><i class="fa fa-download"></i> In QuickBooks&trade;</button>    
    <button class="toggle-filter" data-field="IsReadyForQb"><i class="fa fa-tag"></i> Ready for QuickBooks&trade;</button>
</fieldset>

@(Html.Kendo().Grid(Of DeliveryTicketGridRowViewModel)() _
    .Name("deliveryTickets") _
    .Filterable() _
    .Sortable() _
    .Pageable() _
    .Columns(Sub(c)
                     c.Bound(Function(dt) dt.DeliveryTicketID).Filterable(FilterableType.NumericId) _
                                .ClientTemplate(
                                    "#=DeliveryTicketID#" &
                                    "#if(IsClosed){#<i class=""status-flag fa fa-check-square-o""></i>#}#" &
                                    "#if(IsSigned){#<i class=""status-flag fa fa-clipboard""></i>#}#" &
                                    "#if(Quote){#<i class=""status-flag fa fa-usd""></i>#}#" &
                                    "#if(IsDownloadedToQb){#<i class=""status-flag fa fa-download""></i>#}#" &
                                    "#if(IsReadyForQb){#<i class=""status-flag fa fa-tag""></i>#}#"
                                )
                     c.Bound(Function(dt) dt.CustomerName)
                     c.Bound(Function(dt) dt.LocationName)
                     c.Bound(Function(dt) dt.WellNumber)
                     c.Bound(Function(dt) dt.TicketDate).Format("{0:d}")
                     c.Template(@@<text></text>).ClientTemplate(
                                        "# if(IsClosed) { #" & _
                                        "<a class=""k-button toggle"">Reopen</a>" & _
                                        "# } else { #" & _
                                            "<a class=""k-button toggle"">Close</a>" & _
                                        "# } #" & _
                                        "<a href=""" & Url.Action("Details") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">Details</a>" & _
                                        "<a href=""" & Url.Action("Edit") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">Edit</a>" & _
                                        "<a href=""" & Url.Action("Repair") & "/#=DeliveryTicketID#"" target=""_blank"" class=""k-button k-button-icontext"">Repair</a>" & _
                                        "# if(IsClosed && InvoiceStatus===" & AcePumpInvoiceStatuses.None & "){#<a class=""k-button markReadyForQuickbooks"">Mark as Ready For Quickbooks</a>#}#"
                                      )
                     c.Command(Sub(com)
                                       com.Destroy().Text("Delete")
                               End Sub)
                     c.Bound(Function(dt) dt.ReasonStillOpen)
    End Sub) _
    .Events(Sub(events)
                    events.DataBound("deliveryTickets_DataBound")
            End Sub) _
    .DataSource(Sub(dataSource)
                        dataSource _
                        .Ajax() _
                        .Model(Sub(model) model.Id(Function(id) id.DeliveryTicketID)) _
                        .Read(Sub(read)
                                      read.Action("List", "DeliveryTicket")
                                      read.Type(HttpVerbs.Post)
                                      read.Data("deliveryTickets_AdditionalData")
                              End Sub) _
                        .Destroy("Delete", "DeliveryTicket") _
                        .PageSize(100)
                End Sub)
    )

    
<script id="command-template" type="text/x-kendo-template">
    
</script>​

<script>
    $(document).ready(function () {
        var grid = $("#deliveryTickets").data("kendoGrid");
        var wrapper = $('<div class="k-pager-wrap k-grid-pager pagerTop"/>').insertBefore(grid.element.children("table"));
        grid.pagerTop = new kendo.ui.Pager(wrapper, $.extend({}, grid.options.pageable, { dataSource: grid.dataSource }));
        grid.element.height("").find(".pagerTop").css("border-width", "0 0 1px 0");
    });
</script>