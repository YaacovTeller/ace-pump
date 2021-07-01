/*
 * Base file for all Soris.x.js files.
 *
 * This includes common functions for all Soris javascript like namespace
 */

(function(undefined) {
    "use strict";

    window.Soris = {
        namespace: function(namespacePath) {
            var pathParts = namespacePath.split(".");

            var current = this;
            for (var i = 0; i < pathParts.length; i++) {
                var nextStepInPath = pathParts[i];

                if (current[nextStepInPath] === undefined) current[nextStepInPath] = {};

                current = current[nextStepInPath];
            }

            return current;
        }
    };
}());
//Soris.Class, based on TypeScript inheritance style
(function (Soris, undefined) {
    "use strict";

    Soris.Class = function () { };

    Soris.Class.extend = function (ctor) {
        var base = this;
        var derived = ctor;

        //Duplicate static property access to derived
        for (var property in base) {
            if (base.hasOwnProperty(property)) derived[property] = base[property];
        }

        //Create lightweight intermediate constructor to avoid calling the base constructor
        function LightweightCtor() { }

        //Setup prototype chain
        LightweightCtor.prototype = base.prototype;
        derived.prototype = new LightweightCtor();
        derived.prototype._MyBase = LightweightCtor.prototype;

        //Setup constructor references
        derived.prototype.constructor = derived;
        derived.prototype.__FirstBaseCtor = base;
        derived.prototype._MyBaseCtor = function () {
            this._MyBaseCtor = base.prototype._MyBaseCtor;
            base.apply(this, arguments);
            this._MyBaseCtor = this.__FirstBaseCtor;
        };

        //Return derived class
        return derived;
    };
})(window.Soris);
//Soris.Utility module
(function(Soris) {
    Soris.Utility = {
        isUndefined: function(obj) {
            return typeof obj === "undefined";
        },
        argumentRequired: function(arg, name) {
            if(Soris.Utility.isUndefined(arg)) {
                throw new Error(name + " is a required argument!");
            }
        },
        getComparer: function(dataType) {
            switch(dataType) {
                case "date":
                    var intComparer = Soris.Utility.getComparer("int");
                    return function(a, b) { return intComparer(a.getTime(), b.getTime()); };
                case "int":
                    return function(a, b) { return (a === b) ? 0 : (a < b) ? -1 : 1; };
                default:
                    return Soris.Utility.getComparer("int");
            }
        },
        getParser: function(dataType) {
            switch(dataType) {
                case "date":
                    return function(value) { return new Date(value); };
                default:
                    return function(value) { return value; };
            }
        },
        isDateValid: function(text) {
            var datePattern = /^(\d{1,2})\/(\d{1,2})\/(\d{4})$/; // MM/dd/yyyy date format
            var matches = text.match(datePattern);

            if(matches === null) return false;
            var day = matches[2];
            var month = matches[1];
            var year = matches[3];

            var monthLengths = [ 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 ];
            if(year % 400 === 0 || (year % 100 !== 0 && year % 4 === 0)) monthLengths[1] = 29;

            if(day < 1 || day > monthLengths[month -1]) return false;
            if(month < 1 || month > 12) return false;
            if(year < 1) return false;

            return true;
        },
        getQstringParam: function(name) {
            if(Soris.Utility.isUndefined(window)) {
                throw new Error("cannot use getQstringParam outside of a browser window.");
            }

            if(Soris.Utility.isUndefined(Soris.Utility._qstringParams)) {
                var match,
                    search = /([^&=]+)=?([^&]*)/g,
                    decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); },
                    qstring = window.location.search.substring(1);

                qstringParams = {};
                while ((match = search.exec(qstring)) !== null) {
                    qstringParams[decode(match[1])] = decode(match[2]);
                }

                Soris.Utility._qstringParams = qstringParams;
            }

            return Soris.Utility._qstringParams[name];
        },
        loadDefaults: function(defaults, object) {
            if(Soris.Utility.isUndefined(object)) return defaults;

            for(var defaultProp in defaults) {
                if(defaults.hasOwnProperty(defaultProp)) {
                    if(Soris.Utility.isUndefined(object[defaultProp])) {
                        object[defaultProp] = defaults[defaultProp];
                    } else if(typeof defaults[defaultProp] === "object") {
                        Soris.Utility.loadDefaults(defaults[defaultProp], object[defaultProp]);
                    }
                }
            }

            return object;
        },
        redirect: function(href) {
            window.location = href;
        }
    };
})(window.Soris);
//Soris.Ui.UiComponentBase class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var UiComponentBase = Soris.Class.extend(function () {
        this.UiComponentFactory = new Soris.Ui.KendoUiComponentFactory();
        this.p_uiComponentFactory = this.UiComponentFactory;
    });

    UiComponentBase.prototype.render = function(container) {
        throw new Error("component must implement render!");
    };

    Ui.UiComponentBase = UiComponentBase;
})(window.Soris, jQuery);

//Soris.Ui.UiComponentFactory class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var UiComponentFactory = Soris.Class.extend(function () {
    });

    UiComponentFactory.prototype.RenderSpinner = function (input, options) { throw "Provider must implement RenderSpinner"; };
    UiComponentFactory.prototype.RenderDatePicker = function (input, options) { throw "Provider must implement RenderDatePicker"; };
    UiComponentFactory.prototype.RenderEditor = function(input, options) { throw "Provider must implement RenderEditor"; };
    UiComponentFactory.prototype.RenderDropDown = function(input, options) { throw "Provider must implement RenderDropDown"; };
    UiComponentFactory.prototype.renderGrid = function(jqElement, options) { throw new Error("Provider must impleement RenderGrid"); };

    Ui.UiComponentFactory = UiComponentFactory;
})(window.Soris, jQuery);
//Soris.Ui.DropDownList class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var DropDownList = Ui.UiComponentBase.extend(function () {
    });

    DropDownList.prototype.Clear = function () { throw "Provider must implement Clear"; };
    DropDownList.prototype.Add = function (text, value) { throw "Provider must implement Add"; };
    
    Ui.DropDownList = DropDownList;
})(window.Soris, jQuery);
//Soris.Ui.KendoDropDownListWrapper class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var KendoDropDownListWrapper = Ui.UiComponentBase.extend(function (kendoDropDown) {
        this._KendoDropDown = kendoDropDown;
    });

    KendoDropDownListWrapper.prototype.Clear = function() {
        this._KendoDropDownList.dataSource.clear();
    };
    
    KendoDropDownListWrapper.prototype.Add = function(text, value) {
        this._KendoDropDownList.dataSource.Add({ text: text, value: value });
    };

    Ui.KendoDropDownListWrapper = KendoDropDownListWrapper;
})(window.Soris, jQuery);
//Soris.Ui.GridBase class

(function(Soris){
    "use strict";

    var Ui = Soris.namespace("Ui");

    var GridBase = Soris.Class.extend(function(options) {
        this.options = Soris.Utility.loadDefaults({}, options);
    });

    GridBase.prototype.setStaticFilters = function(staticFilters) { throw new Error("must implement setStaticFilters"); };

    Ui.GridBase = GridBase;
})(window.Soris);
//Soris.Ui.KendoGridWrapper class

(function(Soris){
    "use strict";

    var Ui = Soris.namespace("Ui");

    var KendoGridWrapper = Ui.GridBase.extend(function(grid) {
        this._grid = grid;
        this._currentStaticFilters = [];
    });

    KendoGridWrapper.prototype.setStaticFilters = function(staticFilters) {
        var filterObject = this._grid.dataSource.filter();
        var currentlyAppliedFilters = !Soris.Utility.isUndefined(filterObject) ?
                                            filterObject.filters :
                                            [];

        this._removeStaticFilters(currentlyAppliedFilters);
        this._currentStaticFilters = staticFilters;
        this._addStaticFilters(currentlyAppliedFilters);

        this._grid.dataSource.filter(currentlyAppliedFilters);
    };

    KendoGridWrapper.prototype._removeStaticFilters = function(fromFilterCollection) {
        for(var i=0; i<this._currentStaticFilters.length; i++) {
            var staticFilter = this._currentStaticFilters[i];

            for(var j=0; j<fromFilterCollection.length; j++) {
                var filter = fromFilterCollection[j];

                if(staticFilter.field === filter.field) {
                    fromFilterCollection.splice(j, 1);
                    break; //out of the inner for loop only
                }
            }
        }
    };

    KendoGridWrapper.prototype._addStaticFilters = function(toFilterCollection) {
        for(var i=0; i<this._currentStaticFilters.length; i++) {
            toFilterCollection.push(this._currentStaticFilters[i]);
        }
    };

    Ui.KendoGridWrapper = KendoGridWrapper;
})(window.Soris);
//Soris.Ui.KendoUiComponentFactory class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var KendoUiComponentFactory = Ui.UiComponentFactory.extend(function () {
        this._MyBaseCtor();
    });

    KendoUiComponentFactory.prototype.RenderSpinner = function (input, options) {
        return $(input).kendoNumericTextBox(options);
    };

    KendoUiComponentFactory.prototype.RenderDatePicker = function (input, options) {
        return $(input).kendoDatePicker(options);
    };
    
    KendoUiComponentFactory.prototype.RenderEditor = function (input, options) {
        return input;
    };

    KendoUiComponentFactory.prototype.RenderDropDown = function(input, options) {
        return new Soris.Ui.KendoDropDownListWrapper($(input).kendoDropDown(options));
    };

    KendoUiComponentFactory.prototype.renderGrid = function(jqElement, options) {
        return new Soris.Ui.KendoGridWrapper({
            grid: jqElement.kendoGrid(options)
        });
    };

    Ui.KendoUiComponentFactory = KendoUiComponentFactory;
})(window.Soris, jQuery);
//Soris.Ui.ShowMoreSelector class
(function (Soris, $, undefined) {
    "use strict";

    var Ui = Soris.namespace("Ui");
    
    var ShowMoreSelector = Ui.UiComponentBase.extend(function () {
    });

    ShowMoreSelector.prototype._OnSelect = function(item) {
        var event = $.Event("select");
        event.text = item.text();
        event.value = item.val();

        $(this).trigger(event);
        
        if (!event.isDefaultPrevented()) {
            this._Title.text(event.text);
            this._ShowLink();
        }
    };

    ShowMoreSelector.prototype.SelectedValue = function() {
        return this._Selector.val();
    };

    ShowMoreSelector.prototype.LoadValues = function(values) {
        this._Selector.empty();
        
        for (var i = 0; i < values.length; i++) {
            this._Selector.append($("<option>")
                                    .val(values[i].value)
                                    .text(values[i].text)
                                 );
        }
        
        if (values.length > 1) {
            this._ShowLink();
        } else {
            this._ShowTitleOnly();
        }
    };

    ShowMoreSelector.prototype.Render = function(container) {
        this._Title = $("<span>");
        
        this._Selector = $("<select>")
                            .on("change", function(e) {
                                var item = $(this).find(":selected");
                                var showMoreContainer = $(this).closest(".s-show-more-container");
                                var showMore = showMoreContainer.data("sorisShowMore");

                                showMore._OnSelect(item);
                            })
                            .hide();
        
        this._Link = $("<a>See More</a>")
                                .prop("href", "#")
                                .click(function(e) {
                                    var showMoreContainer = $(this).closest(".s-show-more-container");
                                    var showMore = showMoreContainer.data("sorisShowMore");

                                    showMore._ShowSelector();
                                    e.preventDefault();
                                })
                                .hide();
        
        var showMoreContainer = $("<span>").addClass("s-show-more-container");
        showMoreContainer.append(this._Title);
        showMoreContainer.append(this._Selector);
        showMoreContainer.append(this._Link);
        showMoreContainer.data("sorisShowMore", this);

        container.append(showMoreContainer);
    };

    ShowMoreSelector.prototype._ShowSelector = function() {
        this._Selector.show();
        this._Link.hide();
    };

    ShowMoreSelector.prototype._ShowLink = function() {
        this._Link.show();
        this._Selector.hide();
    };

    ShowMoreSelector.prototype._ShowTitleOnly = function() {
        this._Title.show();
        this._Link.hide();
        this._Selector.hide();
    };
    
    Ui.ShowMoreSelector = ShowMoreSelector;
})(window.Soris, jQuery);
(function(Soris, $) {
    "use strict";

    $(window).load(function () {
        Soris.Kendo.GridHelper.InitializeAllGrids(true);
    });
}(window.Soris, jQuery));
//Soris.Kendo.GridHelper class
(function (Soris, $, undefined) {
    "use strict";

    var Kendo = Soris.namespace("Kendo");
    
    var GridHelper = function (jqObj) {
        this._JqObj = jqObj;
        this._KGrid = jqObj.data("kendoGrid");

        this._ColumnCount = null;
    };

    GridHelper.GetHelper = function (jqObj) {
        if (jqObj.length === 0) throw "cannot get helper, arg null!";

        GridHelper._LoadGrids();
        for (var i = 0; i < GridHelper._Helpers.length; i++) {
            if (GridHelper._Helpers[i]._JqObj.is(jqObj)) return GridHelper._Helpers[i];
        }

        var helper = new GridHelper(jqObj);
        GridHelper._Helpers.push(helper);

        return helper;
    };

    GridHelper._Helpers = [];
    GridHelper._GridsLoaded = false;

    GridHelper.InitializeAllGrids = function (forceReload) {
        GridHelper._LoadGrids(forceReload);

        for (var i = 0; i < GridHelper._Helpers.length; i++) {
            var helper = GridHelper._Helpers[i];

            helper.Initialize();
        }
    };

    GridHelper._LoadGrids = function (forceReload) {
        if (forceReload !== undefined && forceReload) {
            GridHelper._Helpers = [];
            GridHelper._GridsLoaded = false;
        } else if (GridHelper._GridsLoaded) {
            return;
        }

        GridHelper._GridsLoaded = true;

        var grids = $("div.k-grid").filter(function () { return $(this).data("kendoGrid") !== undefined; });
        grids.each(function () {
            GridHelper._Helpers.push(new GridHelper($(this)));
        });
    };

    GridHelper.GetHelperFromEvent = function (e) {
        var grid;
        if (e.currentTarget !== undefined) {
            grid = $(e.currentTarget).closest(".k-grid");
        } else if (e.sender !== undefined && e.sender.element !== undefined) {
            grid = e.sender.element;
        } else {
            throw "Event does not contain reference to a grid, could not get helper!";
        }

        return GridHelper.GetHelper(grid);
    };

    GridHelper.prototype.GetDataItemFromEvent = function (e) {
        var currentRow = $(e.currentTarget).closest("tr");
        var dataItem = this._KGrid.dataItem(currentRow);

        return dataItem;
    };

    GridHelper.prototype.Refresh = function () {
        this._KGrid.dataSource.read();
        this._KGrid.refresh();
    };

    GridHelper.prototype.SaveGrid = function () {
        this._KGrid.dataSource.sync();
    };

    GridHelper.prototype.Aggregate = function (callback) {
        if (typeof callback !== "function") throw "Illegal callback";

        var data = this._KGrid.dataSource.data();
        for (var i = 0; i < data.length; i++) {
            callback(data[i]);
        }
    };

    GridHelper.prototype._GetColumnCount = function () {
        if (this._ColumnCount === null) {
            this._ColumnCount = this._KGrid.table.find('.k-grid-header colgroup > col').length;
        }

        return this._ColumnCount;
    };

    GridHelper.prototype.Initialize = function () {
        this._LoadOptions();
        this._CreateFixedHeaderRow();
        this._AttachHandlers();
    };

    GridHelper.prototype._LoadOptions = function () {
        var userOptions = this._KGrid.element.data("sorisGridOptions");
        if (userOptions === undefined) userOptions = {};

        this._Options = {
            ShowNoRecordsMessage: userOptions.ShowNoRecordsMessage || false
        };
    };

    GridHelper.prototype.Grid = function () {
        return this._KGrid;
    };

    GridHelper.prototype._GetHasChangesMessageContainer = function () {
        if (this._HasChangesMessage === undefined) {
            this._HasChangesMessage = $("<span class=\"s-grid-change-message\" />");

            var toolbar = this._KGrid.element.find(".k-toolbar");
            if (toolbar.length > 0) {
                toolbar.append(this._HasChangesMessage);
            }
        }

        return this._HasChangesMessage;
    };

    GridHelper.prototype._ShowHasChangesMessage = function () {
        this._GetHasChangesMessageContainer().html("There are changes!");
    };

    GridHelper.prototype._ClearHasChangesMessage = function () {
        this._GetHasChangesMessageContainer().empty();
    };

    GridHelper.prototype.DisableEditorFor = function (e, propName) {
        var toFind = "[name=" + propName + "]";
        var editor = $(e.container).find(toFind);
        var td = editor.closest("td");
        td.prop("disabled", true);
        td.html(editor.val());
    };

    GridHelper.prototype._CreateFixedHeaderRow = function () {
        var originalHeaderClone = this._KGrid.table.find("thead").clone();

        this._FixedHeaderRow = $("<table style=\"position: fixed; top: 0px; left: 50%; display: none;\">");
        this._FixedHeaderRow.append(originalHeaderClone);
        this._JqObj.append(this._FixedHeaderRow);

        this._UpdateFixedHeader();
    };

    GridHelper.prototype._DataBoundRedrawLoop = function (callback) {
        var expectedRows = this._KGrid.dataSource.data().length;
        var actualRows = this._KGrid.table.find("tr[role=row]").length;

        if (expectedRows !== actualRows) {
            var helper = this;

            window.setTimeout(function () { helper._DataBoundRedrawLoop(callback); }, 100);
        } else {
            callback();
        }
    };

    GridHelper.prototype._UpdateFixedHeader = function () {
        this._DetectFixedHeaderPositions();
        this._CalculateFixedHeaderSize();
    };

    GridHelper.prototype._DetectFixedHeaderPositions = function () {
        this._FixedHeaderTop = this._KGrid.table.offset().top;
        this._FixedHeaderBottom = this._FixedHeaderTop + this._KGrid.table.height() - 50;
    };

    GridHelper.prototype._CalculateFixedHeaderSize = function () {
        var originalCols = this._KGrid.table.find("th");
        var cloneCols = this._FixedHeaderRow.find("th");
        for (var i = 0; i < originalCols.length; i++) {
            var cloneCol = $(cloneCols[i]);
            var originalCol = $(originalCols[i]);

            cloneCol.width(originalCol.width());
            cloneCol.text(originalCol.text());
        }

        this._FixedHeaderRow.width(this._KGrid.table.find("thead").width());
        this._FixedHeaderRow.css("margin-left", this._FixedHeaderRow.width() / 2 * -1);
    };

    GridHelper.prototype._AttachHandlers = function () {
        var helper = this;

        this._KGrid.dataSource.bind("error", function (e) {
            if (e.errors !== undefined) {
                helper.DisplayErrors(e.errors);
            } else if (e.xhr !== undefined && e.xhr.status == 500) {
                helper.DisplayError("An unrecoverable server error occured (500).  Your request was not saved.");
            } else {
                helper.DisplayError("An unrecoverable grid error occured.  Your request was not saved.");
            }
        });

        this._KGrid.bind("dataBound", function (e) {
            if (helper._KGrid.dataSource.data().length === 0) {
                helper.DisplayNoResults();
            }

            helper._DataBoundRedrawLoop(function () { helper._UpdateFixedHeader(); });
        });

        this._KGrid.bind("columnResize", function (e) {
            helper._UpdateFixedHeader();
        });

        this._KGrid.bind("edit", function (e) {
            if (e.model.isNew()) {
                var row = e.container.closest("tr");
                helper._AutoBindNewItemValues(row);
            }
        });

        $(window).bind("scroll", function () {
            var currentOffset = $(this).scrollTop();

            if (currentOffset >= helper._FixedHeaderTop && currentOffset <= helper._FixedHeaderBottom) {
                helper._FixedHeaderRow.show();

            } else if (currentOffset < helper._FixedHeaderTop || currentOffset > helper._FixedHeaderBottom) {
                helper._FixedHeaderRow.hide();
            }
        });
    };

    GridHelper.prototype._AutoBindNewItemValues = function (newRow) {
        var model = this._KGrid.dataItem(newRow);

        var cells = newRow.find("td");
        for (var i = 0; i < cells.length; i++) {
            var autoBindControl = this._ControlForAutoBinding(cells[i]);

            if (autoBindControl.length > 0) {
                var bindingProperty = $(cells[i]).data("containerFor");

                model[bindingProperty] = autoBindControl.val();
            }
        }
    };

    GridHelper.prototype._ControlForAutoBinding = function (cell) {
        var control = $(cell).find(".k-dropdown input");

        return control;
    };

    GridHelper.prototype.DisplayError = function (text) {
        this.DisplayErrors({
            Error: {
                errors: [text]
            }
        });
    };

    GridHelper.prototype.DisplayErrors = function (allErrors) {
        var msg = "";

        for (var property in allErrors) {
            var propertyErrors = allErrors[property].errors;

            for (var i = 0; i < propertyErrors.length; i++) {
                msg += property + ": " + propertyErrors[i] + "\n";
            }
        }

        window.alert(msg);

        this._KGrid.cancelChanges();
    };

    GridHelper.prototype.DisplayNoResults = function () {
        if (this._Options.ShowNoRecordsMessage) {
            var noResultsRowHtml = "<tr class=\"kendo-data-row s-empty-results\"><td colspan=\"" + this._GetColumnCount() + "\">No records found.</td></tr>";

            this._KGrid.table.append(noResultsRowHtml);
        }
    };

    Kendo.GridHelper = GridHelper;
})(window.Soris, jQuery);