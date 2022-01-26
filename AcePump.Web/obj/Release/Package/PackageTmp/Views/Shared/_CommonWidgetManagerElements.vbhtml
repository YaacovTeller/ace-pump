@ModelType AcePump.Web.Models.WidgetManagerConfigurationModel
@Imports Yesod.Widgets
@Imports AcePump.Web.Models

<script>
    var partialView = (function ($, undefined) {
        "use strict";

        ///*
        // Represents a filter in a cascading chain of filters.  Sometimes the value
        // should be used for filters down the line, sometimes it should not.
        ///*
        function CascadingFilter(jqObj) {
            this._JqObj = jqObj;
            this._UseInFilterRequests = true;
        };

        CascadingFilter.prototype.UseInFilterRequests = function () {
            this._UseInFilterRequests = true;
        };

        CascadingFilter.prototype.DoNotUseInFilterRequests = function () {
            this._UseInFilterRequests = false;
        };

        CascadingFilter.prototype.ShouldUseValueInFilterRequests = function () {
            return this._UseInFilterRequests;
        };

        CascadingFilter.prototype.GetValue = function () {
            return this._JqObj.val();
        };

        CascadingFilter.prototype.FilterName = function () {
            return this._JqObj.prop("name");
        };

        CascadingFilter.prototype.is = function (jqObj) {
            return this._JqObj.is(jqObj);
        };

        CascadingFilter.prototype.val = function (value) {
            return this._JqObj.val(value);
        };

        CascadingFilter.prototype.change = function () {
            this._JqObj.change();
        };

        function HelperClass() {
            this._CascadeChain = [];
            this._CustomerAccessList = "\"@String.Join(","c, Model.CustomersIDsThatMayBeAccessed.Values.ToList())\""
        };

        HelperClass.prototype.Initialize = function () {
            this._CustomerID = new CascadingFilter($("#CustomerID"));
            this._LeaseID = new CascadingFilter($("#LeaseID"));
            this._WellID = new CascadingFilter($("#WellID"));

            this._CascadeChain = [
                this._CustomerID, this._LeaseID, this._WellID
            ];

            this.SetCustomerAccessListFirstRun();
        };

        HelperClass.prototype.PreventComboBoxFreeText = function (combo, e) {
            if (!this.ComboBoxValueIsInList(combo)) {
                combo.value("");
                combo.select(0);
                
                if(typeof(e) !== "undefined") e.preventDefault();
            }
        };

        HelperClass.prototype.ComboBoxValueIsInList = function (combo) {
            if ((combo.value().length > 0) && (combo.selectedIndex === -1)) {
                return false;
            } else {
                return true;
            }
        };

        HelperClass.prototype.GetCascadeData = function (comboId) {
            var combo = $("#" + comboId);
            var data = { term: combo.data("kendoComboBox").input.val() };

            var foundEndOfCascade = false;
            for (var i = this._CascadeChain.length - 1; i >= 0; i--) {
                var currentFilter = this._CascadeChain[i];

                if (foundEndOfCascade && currentFilter.ShouldUseValueInFilterRequests()) data[currentFilter.FilterName()] = currentFilter.GetValue();
                else if (currentFilter.is(combo)) foundEndOfCascade = true;
            }

            return data;
        };

        HelperClass.prototype.CascadeValueChange = function (from) {
            var foundStartOfCascade = false;

            for (var i = 0; i < this._CascadeChain.length; i++) {
                if (foundStartOfCascade) this._CascadeChain[i].data("kendoComboBox").value("");
                else if (this._CascadeChain[i].is(from)) foundStartOfCascade = true;
            }
        };

        HelperClass.prototype.SetCustomerAccessListFirstRun = function () {
            var ddl = $("#CustomerID_DropDown").data("kendoDropDownList");

            if (ddl !== null) {
                var leaseDataSource = this._LeaseID._JqObj.data("kendoComboBox").dataSource;
                this._CustomerID._JqObj.on("change", function (e) { leaseDataSource.read() });
                
                this.SetCustomerAccessList(ddl.selectedIndex, ddl.dataItem().Value);
            }
        };

        HelperClass.prototype.SetCustomerAccessList = function (selectedIndex, selectedValue) {
            if (selectedIndex === 0) {
                this._CustomerID.DoNotUseInFilterRequests();
                this._CustomerID.val(this._CustomerAccessList);

            } else {
                this._CustomerID.UseInFilterRequests();
                this._CustomerID.val(selectedValue);
            }

            this._CustomerID.change();
        };

        var h = new HelperClass();
        $(window).load(function () {
            h.Initialize();
        });

        return {
            Helper: h,
            CustomerID_DropDown_Selected: function (e) {
                var index = e.item.index();
                var value = this.dataItem(index).Value;

                h.SetCustomerAccessList(index, value);
            }
        };
    })(jQuery);
</script>

@Select Case Model.CustomerSelectionType
    Case SelectionType.Any
        @(Html.Kendo().ComboBoxFor(Function(model) model.CustomerID) _
            .DataTextField("Name") _
            .DataValueField("Id") _
            .MinLength(2) _
            .Placeholder("Start typing a customer name...") _
            .Filter(FilterType.StartsWith) _
            .AutoBind(False) _
            .Events(Sub(e) e.Select("function() { partialView.Helper.CascadeValueChange(this); }") _
                            .Change("function() { partialView.Helper.PreventComboBoxFreeText(this); }")) _
            .DataSource(Sub(dataSource)
                            dataSource.Read(Sub(read)
                                                read.Action("StartsWith", "Customer")
                                                read.Data("function() { return partialView.Helper.GetCascadeData(""CustomerID""); }")
                                                read.Type(HttpVerbs.Post)
                                            End Sub)
                            dataSource.ServerFiltering(True)
                        End Sub)
        ) 
    
Case SelectionType.AccessListOnly
    @Html.HiddenFor(Function(model) model.CustomerID)
    @(Html.Kendo().DropDownList _
            .Name("CustomerID_DropDown") _
            .Events(Sub(e) e.Select("partialView.CustomerID_DropDown_Selected") _
                            .Change("function() { partialView.Helper.PreventComboBoxFreeText(this); }")) _
            .Items(Sub(items)
                       items.Add().Text("All").Value("0")
                       
                       For Each entry As KeyValuePair(Of String, Integer) In Model.CustomersIDsThatMayBeAccessed
                           items.Add().Text(entry.Key).Value(entry.Value)
                       Next
                   End Sub)
        )
End Select

@Code
    Dim leaseBuilder = Html.Kendo().ComboBoxFor(Function(model) model.LeaseID) _
        .Name("LeaseID") _
        .DataTextField("Name") _
        .DataValueField("Id") _
        .Filter(FilterType.Contains) _
        .AutoBind(False) _
        .Text(Model.LeaseName) _
        .Events(Sub(e) e.Change("function() { partialView.Helper.PreventComboBoxFreeText(this); }")) _
        .DataSource(Sub(dataSource)
                            dataSource.Read(Sub(read)
                                                    read.Action("LeaseSearch", "History")
                                                    read.Data("function() { return partialView.Helper.GetCascadeData(""LeaseID""); }")
                                                    read.Type(HttpVerbs.Post)
                                            End Sub)
                            dataSource.ServerFiltering(True)
                    End Sub)
    
    If Model.CustomerSelectionType = SelectionType.Any Then
        leaseBuilder.CascadeFrom("CustomerID")
    End If
    
    leaseBuilder.Render()
End Code

@Select Case Model.WellSelectionType
    Case SelectionType.Any
        @(Html.Kendo().ComboBoxFor(Function(model) model.WellID) _
                            .Name("WellID") _
                            .DataTextField("WellNumber") _
                            .DataValueField("WellId") _
                            .Filter(FilterType.Contains) _
                            .CascadeFrom("LeaseID") _
                            .AutoBind(False) _
                            .Text(Model.WellNumber) _
                            .DataSource(Sub(dataSource)
                                            dataSource.Read(Sub(read)
                                                                read.Action("WellSearch", "History")
                                                                read.Data("function() { return partialView.Helper.GetCascadeData(""WellID""); }")
                                                                read.Type(HttpVerbs.Post)
                                                            End Sub)
                                            dataSource.ServerFiltering(True)
                                        End Sub)
        )
        @<script>
            jQuery(function () {
                //when something is typed and the datasource has not loaded yet, then it will delay the change event until the data is loaded.
                "use strict";

                var dataLoaded = false;

                $("#WellID").data("kendoComboBox").bind("dataBound", function (e) {
                    dataLoaded = true;
                });

                $("#WellID").data("kendoComboBox").bind("dataBinding", function (e) {
                    dataLoaded = false;
                });

                $("#WellID").bind("change", function (e) {                    
                    if (e._inActualChange) return;
                    
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    e.stopPropagation();

                    if (!dataLoaded) {
                        $("#WellID").data("kendoComboBox").one("dataBound", function (e) {
                            triggerActualChange();
                        });
                    } else {
                        triggerActualChange();
                    }

                    function triggerActualChange() {
                        //here ensure value chosen is in list, trigger change only if it's in the list, and if it's not, then select first item in list.
                        e._inActualChange = true;
                        var comboBox = $("#WellID").data("kendoComboBox");
                        if (comboBox.value() && comboBox.selectedIndex == -1) {
                            comboBox.value('');
                            comboBox.select(0);                            
                        } else {
                            $(this).trigger("change", e);
                        }                        
                    }
                });
            });
        </script>
End Select
