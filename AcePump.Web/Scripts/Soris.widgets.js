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
//Soris.Widgets.WidgetBase class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");

    var WidgetBase = Soris.Ui.UiComponentBase.extend(function (options) {
        this._MyBaseCtor();

        this._options = options;
        this.WidgetID = options.WidgetID;
        this.id = options.WidgetID;
        this._Title = options.Title || "Untitled";
        this._requestMediator = this._resolveMediator();
        this.Header = null;

        this._Filters = options.Filters || [];
        
        this.CrossLinkOptions = {
            Enable: options.EnableCrossLink || false,
            Url: options.CrossLinkUrl || null,
            CrossLinkEventHandler: options.CrossLinkEventHandler || null,
            AdditionalParameters: (options.CrossLinkAdditionalParameters !== undefined) ? options.CrossLinkAdditionalParameters : []
        };

        this._CurrentDataSets = [];

        this._init();
    });

    WidgetBase.prototype._resolveMediator = function() {
        if(!Soris.Utility.isUndefined(this._options.mediator) && this._options.mediator !== null) return this._options.mediator;
        else return Widgets.WidgetRequestMediator.getUnmanagedMediator();
    };

    WidgetBase.prototype._init = function () {
        this._requestMediator.registerWidget(this);

        this._loadingImageHtml = "<span class=\"s-widget-loading-image\" />";
    };

    WidgetBase.prototype.loadDataSets = function(dataSets, globalResponse) {
        this._hideMask();

        this._CurrentDataSets = dataSets;
        this._CurrentGlobalResponse = globalResponse || {};

        this._RefreshDataSetSelector();
        if (this._CurrentDataSets.length > 0) {
            this._SelectDataSet(0);
        }
    };
    WidgetBase.prototype.LoadDataSets = function(dataSets, globalResponse) { return this.loadDataSets(dataSets, globalResponse); };

    WidgetBase.prototype.DisplayDataSet = function (dataSet, globalResponse) { };

    WidgetBase.prototype._RefreshDataSetSelector = function() {
        var values = [];
        
        for (var i = 0; i < this._CurrentDataSets.length; i++) {
            values.push({
                text: this._CurrentDataSets[i].Name,
                value: i
            });
        }
        
        this._DataSetSelector.LoadValues(values);
    };

    WidgetBase.prototype._SelectDataSet = function(index) {
        var set = this._CurrentDataSets[index];
        var globalResponse = this._CurrentGlobalResponse;

        this.DisplayDataSet(set.Data, globalResponse);
    };

    WidgetBase.prototype.Requery = function () {
        this._requestMediator.widgetChanged(this);
    };

    WidgetBase.prototype.getRequestData = function () {
        var filterParameters = this._GetFilterParameters();

        return {
            WidgetID: this.WidgetID,
            Parameters: filterParameters
        };
    };
    WidgetBase.prototype.GetRequestData = function() { return this.getRequestData(); };

    WidgetBase.prototype.showError = function(errorText) {
        this._mask.addClass("s-mask-error");

        var html = "<p>" + errorText + "</p>";
        this._showMaskedContent(html);
    };

    WidgetBase.prototype.showLoadingMask = function() {
        this._showMaskedContent(this._loadingImageHtml);
    };

    WidgetBase.prototype.hideLoadingMask = function() {
        this._hideMask();
    };

    WidgetBase.prototype._showMaskedContent = function(content) {
        this._maskContent.html(content);
        this._positionMask();

        this._mask.show();
        this._maskContent.show();
    };

    WidgetBase.prototype._hideMask = function() {
        this._mask.hide();
        this._maskContent.hide();
    };

    WidgetBase.prototype._GetFilterParameters = function () {
        var filterParameters = [];

        for (var i = 0; i < this._Filters.length; i++) {
            filterParameters.push({
                key: this._Filters[i].ParameterName,
                value: this._Filters[i].GetValue()
            });
        }

        return filterParameters;
    };

    WidgetBase.prototype.OnCrossLink = function(args) {
        if (this.CrossLinkOptions.Enable === true) {
            var event = $.Event("crossLink");
            event.Url = this.CrossLinkOptions.Url;
            event.Parameters = this.GetCrossLinkParameters(args);
            var globalFilters = this._Manager.GetGlobalFilterParameters();            
            $.extend(event.Parameters, globalFilters);
            
            $(this).trigger(event);
            
            if (!event.isDefaultPrevented()) {
                window.open(this._GetCrossLinkUrl(event),"_blank");
            }
        }        
    };

    WidgetBase.prototype._GetCrossLinkUrl = function(event) {
        var parameterString = "";
        for (var parameterName in event.Parameters) {
            parameterString += parameterName + "=" + encodeURIComponent(event.Parameters[parameterName]) + "&";
        }
       
        if (parameterString.length > 0) {
            return event.Url + "?" + parameterString.substring(0, parameterString.length - 1);
        } else {
            return event.Url;
        }
    };

    WidgetBase.prototype.GetCrossLinkParameters = function(args) { return {}; };

    WidgetBase.prototype.Render = function (originalDOMElement) {
        var orig = $(originalDOMElement);
        var container = $(document.createElement("div"));
        container.addClass("s-widget-container");
        container.prop("id", orig.prop("id"));

        this.RenderHeader(container);
        this.RenderWidget(container, this);
        this.RenderFilters(container);
        this.RenderFooter(container);
        this._renderMask(container);

        container.data("sorisWidget", this);
        container.data("sorisWidgetId", this._WidgetID);

        orig.replaceWith(container);
        this._AttachEvents();
    };

    WidgetBase.prototype._AttachEvents = function() {
        var w = this;

        this._AttachCrossLink();

        $(this._DataSetSelector).on("select", function(e) {
            w._SelectDataSet(e.value);
        });
    };

    WidgetBase.prototype._AttachCrossLink = function() {
        var handler;
        switch(typeof this.CrossLinkOptions.CrossLinkEventHandler) {
            case "string":
                handler = window[this.CrossLinkOptions.CrossLinkEventHandler];
                break;
                
            case "function":
                handler = this.CrossLinkOptions.CrossLinkEventHandler;
                break; 
                
            default:
                return;
        }
        
        $(this).on("crossLink", handler);
    };

    WidgetBase.prototype._renderMask = function(container) {
        container.css("position", "relative");

        this._mask = $("<div>")
            .addClass("s-widget-mask")
            .css({
                position: "absolute",
                top: 0,
                left: 0
            });

        this._maskContent = $("<div>")
            .addClass("s-widget-mask-content")
            .css({
                position: "absolute",
                top: 0,
                left: 0
            });

        container.append(this._mask);
        container.append(this._maskContent);
    };

    WidgetBase.prototype._positionMask = function () {
        var container = this._mask.parent();

        this._mask.css({
            height: container.height(),
            width: container.width()
        });

        var verticalCenter = container.height() / 2;
        var verticalOffset = this._maskContent.height() / 2;
        var horizontalCenter = container.width() / 2;
        var horizontalOffset = this._maskContent.width() / 2;
        this._maskContent.css({
            marginTop: verticalCenter - verticalOffset,
            marginLeft: horizontalCenter - horizontalOffset
        });
    };

    WidgetBase.prototype.RenderHeader = function (container) {
        this.Header = $("<div>").addClass("s-widget-header");
        this.Header.append($("<h3>" + this._Title + "</h3>"));

        this._DataSetSelector = new Soris.Ui.ShowMoreSelector();
        this._DataSetSelector.Render(this.Header);

        container.append(this.Header);
    };

    WidgetBase.prototype.RenderFilters = function (container) {
        var widget = this;
        var filterHandler = function(e) { widget.Requery(); };

        for (var i = 0; i < this._Filters.length; i++) {
            var filterContainer = $(document.createElement("span"));
            filterContainer.addClass("s-filter-container");
            filterContainer.data("sorisFilter", this._Filters[i]);

            container.append(filterContainer);
            $(this._Filters[i]).on("filter", filterHandler);
            this._Filters[i].Render(filterContainer);
        }
    };

    WidgetBase.prototype.RenderFooter = function (container) { };
    WidgetBase.prototype.RenderWidget = function (domContainer, widget) { };

    Widgets.WidgetBase = WidgetBase;
})(window.Soris, jQuery);

//Soris.Widgets.DirectionChangeWidget class
(function (Soris, $, kendo, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var DirectionChangeWidget = Widgets.WidgetBase.extend(function (options) {
        this._MyBaseCtor(options);

        this._Arrow = null;
        this._Label = null;
        this._Footer = null;

        this._AmountFormatString = options.AmountFormatString || "{0}";
        this._AmountChangedFormatString = options.AmountChangedFormatString || "{0}";
        this._FooterFormatString = options.FooterFormatString || "{0} by {1} from the same time period last year.";
        this._ColorCodePositiveResult = options.ColorCodePositiveResult || false;
        this._DisplayAmountChangedText = options.DisplayAmountChangedText || false;
        this._DisplayDirectionChangeArrow = options.DisplayDirectionChangeArrow || false;
    });

    DirectionChangeWidget._DirectionUp = 1;
    DirectionChangeWidget._DirectionDown = 2;

    DirectionChangeWidget.prototype._SetPositiveResultClasses = function () {
        this._Label.addClass("s-positive-change");
        this._Label.removeClass("s-negative-change");

        this.Header.addClass("s-positive-change");
        this.Header.removeClass("s-negative-change");
    };

    DirectionChangeWidget.prototype._SetNegativeResultClasses = function () {
        this._Label.addClass("s-negative-change");
        this._Label.removeClass("s-positive-change");

        this.Header.addClass("s-negative-change");
        this.Header.removeClass("s-positive-change");
    };

    DirectionChangeWidget.prototype._SetAmountText = function (amount) {
        var formattedAmount = kendo.format(this._AmountFormatString, amount);
        this._Label.text(formattedAmount);
    };

    DirectionChangeWidget.prototype._SetFooterText = function (amountChanged, direction) {
        var formattedAmountChanged = kendo.format(this._AmountChangedFormatString, amountChanged);
        var directionText = (direction === DirectionChangeWidget._DirectionUp) ? "Up" : "Down";
        var footerText = kendo.format(this._FooterFormatString, directionText, formattedAmountChanged);

        this._Footer.text(footerText);
    };

    DirectionChangeWidget.prototype._ConfigureArrow = function (direction, positiveResult) {
        if (direction === DirectionChangeWidget._DirectionUp) {
            this._Arrow.prop("alt", "Up");
            this._Arrow.addClass("s-sprite-up-arrow");
            this._Arrow.removeClass("s-sprite-down-arrow");

        } else {
            this._Arrow.prop("alt", "Down");
            this._Arrow.addClass("s-sprite-down-arrow");
            this._Arrow.removeClass("s-sprite-up-arrow");
        }

        if (positiveResult) {
            this._Arrow.addClass("s-sprite-green");
            this._Arrow.removeClass("s-sprite-red");

        } else {
            this._Arrow.addClass("s-sprite-red");
            this._Arrow.removeClass("s-sprite-green");
        }
    };

    DirectionChangeWidget.prototype.DisplayDataSet = function (dataSet) {
        this._ConfigureArrow(dataSet.Direction, dataSet.PositiveResult);

        if (this._ColorCodePositiveResult === true) {
            if (dataSet.PositiveResult === true) this._SetPositiveResultClasses();
            else this._SetNegativeResultClasses();
        }

        this._SetAmountText(dataSet.Amount);
        this._SetFooterText(dataSet.AmountChanged, dataSet.Direction);
    };

    DirectionChangeWidget.prototype.RenderWidget = function (container) {
        var widgetContainer = $(document.createElement("div"));
        widgetContainer.addClass("s-widget");

        this._Arrow = $(document.createElement("span"));
        this._Arrow.addClass("s-sprite");
        widgetContainer.append(this._Arrow);

        this._Label = $(document.createElement("span"));
        this._Label.addClass("s-dchange-amount");
        widgetContainer.append(this._Label);

        container.addClass("s-direction-change-widget");
        container.append(widgetContainer);
    };

    DirectionChangeWidget.prototype._ApplyDisplaySettings = function() {
        if (!this._DisplayDirectionChangeArrow) this._Arrow.hide();
        if (!this._DisplayAmountChangedText) this._Footer.hide();
    };

    DirectionChangeWidget.prototype.RenderFooter = function (container) {
        this._Footer = $(document.createElement("div"));
        this._Footer.addClass("s-widget-footer");

        container.append(this._Footer);
        this._ApplyDisplaySettings();
    };

    Widgets.DirectionChangeWidget = DirectionChangeWidget;
})(window.Soris, jQuery, kendo);
//Soris.Widgets.ChartWidget class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var ChartWidget = Widgets.WidgetBase.extend(function (options) {
        this._MyBaseCtor(options);
        this.options = Soris.Utility.loadDefaults({
            ValueFormat: "{0}",
            CategoryFormat: "{0}",
            MaxCharactersInCategory: null,
            ShowLegend: false,
            CategoryDataType: "string",
            ValueDataType: "float"
        }, options);

        this._Chart = null;
        this._ChartType = options.ChartType || "pie";

        this._NoDataTemplate = null;
        this._ChartDomElement = null;

        $.extend(this.CrossLinkOptions, {
            ValuePropertyName: options.CrossLinkValueParameterName,
            CategoryPropertyName: options.CrossLinkCategoryParameterName
        });

        this.ChartFactory = new Widgets.KendoChartFactory();
    });

    ChartWidget.prototype.DisplayDataSet = function (dataSet, globalResponse) {
        if (dataSet.Data.length === 0) {
            this._ShowNoDataTemplate();

        } else {
            if (this._CategoryDataType !== "string" || this._ValueDataType !== "string") {
                this._ParseResponseDataTypes(dataSet);
            }

            this._ShowChart();
            this._Chart.LoadData(dataSet, globalResponse);
        }
    };

    ChartWidget.prototype._ParseResponseDataTypes = function(response) {
        var categoryParser = this._GetParserByDataType(this.options.CategoryDataType);
        var valueParser = this._GetParserByDataType(this.options.ValueDataType);
        
        for (var i = 0; i < response.Data.length; i++) {
            var dataItem = response.Data[i];
            dataItem.Category = categoryParser(dataItem.Category);

            for(var j=0; j < response.SeriesNames.length; j++) {
                var seriesName = response.SeriesNames[j];
                dataItem[seriesName] = valueParser(dataItem[seriesName]);
            }
        }
    };

    ChartWidget.prototype._GetParserByDataType = function(dataType) {
        return Soris.Utility.getParser(dataType);
    };

    ChartWidget.prototype.GetCrossLinkParameters = function(args) {
        var params = {};
        
        if(this.CrossLinkOptions.ValuePropertyName !== undefined) params[this.CrossLinkOptions.ValuePropertyName] = args.value;
        if(this.CrossLinkOptions.CategoryPropertyName !== undefined) params[this.CrossLinkOptions.CategoryPropertyName] = args.category;
        
        for (var i = 0; i < this.CrossLinkOptions.AdditionalParameters.length; i++) {
            var paramName = this.CrossLinkOptions.AdditionalParameters[i];
            params[paramName] = args.dataItem[paramName];
        }

        return params;
    };

    ChartWidget.prototype.RenderWidget = function(domContainer, widget) {
        this._RenderNoDataTemplate(domContainer);
        this._RenderChart(domContainer, widget);

        this._ShowNoDataTemplate();
    };

    ChartWidget.prototype._RenderNoDataTemplate = function(container) {
        var template = $(document.createElement("div"));
        template.addClass("s-no-data");
        template.text("No records matched your search criteria.  Could not create chart.");
        
        this._NoDataTemplate = template;
        container.append(template);
    };

    ChartWidget.prototype._RenderChart = function (domContainer, widget) {
        var chart = $(document.createElement("div"));
        chart.addClass("s-widget")
             .addClass("s-chart");

        switch (this._ChartType) {
            case "pie":
                domContainer.addClass("s-pie-chart-widget");
                this._Chart = this.ChartFactory.RenderPieChart(chart, widget, this.options);
                break;

            case "bar":
                domContainer.addClass("s-bar-chart-widget");
                this._Chart = this.ChartFactory.RenderBarChart(chart, widget, this.options);
                break;

            case "line":
                domContainer.addClass("s-line-chart-widget")
                            .addClass("s-scrollable-widget-container");
                this._Chart = this.ChartFactory.RenderLineChart(chart, widget, this.options);
                break;
        }

        this._ChartDomElement = chart;
        domContainer.append(chart);
    };

    ChartWidget.prototype._ShowNoDataTemplate = function() {
        this._ChartDomElement.hide();
        this._NoDataTemplate.show();
    };

    ChartWidget.prototype._ShowChart = function() {
        this._ChartDomElement.show();
        this._NoDataTemplate.hide();
    };

    Widgets.ChartWidget = ChartWidget;
})(window.Soris, jQuery);
//Soris.Widgets.ChartFactory class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var ChartFactory = Soris.Class.extend(function () {
    });

    ChartFactory.prototype.RenderPieChart = function (domWidget, widget, options) { throw "Provider must implement RenderPieChart"; };
    ChartFactory.prototype.RenderBarChart = function (domWidget, widget, options) { throw "Provider must implement RenderBarChart"; };
    ChartFactory.prototype.RenderLineChart = function (domWidget, widget, options) { throw "Provider must implement RenderLineChart"; };

    Widgets.ChartFactory = ChartFactory;
})(window.Soris, jQuery);
//Soris.Widgets.ChartBase class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var ChartBase = Soris.Class.extend(function (options) {
        this._ElideCategories = (options.MaxCharactersInCategory && (options.CategoryDataType === "string" || !options.CategoryDataType ));
        this._MaxCharactersInCategory = options.MaxCharactersInCategory;
    });

    ChartBase.prototype.LoadData = function(data, globalResponse) {
        var seriesInfo = this._parseSeriesNames(data.SeriesNames);
        data = data.Data;

        this._PrepareData(data);
        this.p_loadPreparedData(data, seriesInfo);
        this.p_setSeriesToDisplay(seriesInfo);
        this.p_displayNotes(globalResponse.notes);

        this.p_redrawChart();
    };

    ChartBase.prototype._parseSeriesNames = function(rawSeriesNames) {
        var parsedSeriesNames = [];
        for(var i=0; i<rawSeriesNames.length; i++) {
            var namePieces = rawSeriesNames[i].split("|");

            var parsedName = {
                name: (namePieces.length > 1) ? namePieces[1] : namePieces[0],
                field: namePieces[0]
            };

            parsedSeriesNames.push(parsedName);
        }

        return parsedSeriesNames;
    };

    ChartBase.prototype._PrepareData = function(data) {
        if (this._ElideCategories) {
            for (var i = 0; i < data.length; i++) {
                data[i].FullCategory = data[i].Category;
                data[i].ElidedCategory = (data[i].Category.length > this._MaxCharactersInCategory) ? data[i].Category.substring(0, this._MaxCharactersInCategory + 1) + "..." : data[i].Category;
            }
        }
    };

    ChartBase.prototype.p_loadPreparedData = function (data, seriesInfo) { throw "Chart must implement p_loadPreparedData"; };
    ChartBase.prototype.p_setSeriesToDisplay = function(seriesInfo) { throw new Error("Chart must implement p_setSeriesToDisplay"); };
    ChartBase.prototype.p_displayNotes = function(notes) { throw new Error("Chart must implement p_displayNotes"); };
    ChartBase.prototype.p_redrawChart = function () {throw new Error("Chart must implement p_redrawChart"); };

    Widgets.ChartBase = ChartBase;
})(window.Soris, jQuery);
//Soris.Widgets.KendoBarChartWrapper class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var KendoBarChartWrapper = Widgets.ChartBase.extend(function (kendoChart, options) {
        this._MyBaseCtor(options);

        this._KendoChart = kendoChart;
    });

    KendoBarChartWrapper.prototype.p_loadPreparedData = function (data, seriesInfo) {
        this._KendoChart.dataSource.data(data);
    };

    KendoBarChartWrapper.prototype.p_setSeriesToDisplay = function(seriesInfo) {
        this._KendoChart.options.series.length = 0;
        for(var i=0; i<seriesInfo.length; i++) {
            this._KendoChart.options.series.push({
                type: "column",
                categoryField: "Category",
                field: seriesInfo[i].field
            });
        }
    };

    KendoBarChartWrapper.prototype.p_displayNotes = function(notes) {
        console.warn("KendoBarChartWrapper does not display notes.");
    };

    KendoBarChartWrapper.prototype.p_redrawChart = function() {
        this._KendoChart.refresh();
        this._KendoChart.redraw();
    };

    Widgets.KendoBarChartWrapper = KendoBarChartWrapper;
})(window.Soris, jQuery);
//Soris.Widgets.KendoChartFactory class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var KendoChartFactory = Soris.Widgets.ChartFactory.extend(function () {
        this._MyBaseCtor();
    });

    KendoChartFactory.prototype.RenderPieChart = function (domWidget, widget, options) {
        var elideCategories = (options.MaxCharactersInCategory && (options.CategoryDataType === "string" || !options.CategoryDataType ));
        var kendoOptions = {
            legend: {
                labels: {template: elideCategories ?
                                        "#: dataItem.ElidedCategory #: #: kendo.toString(percentage, \"%\") #%" :
                                        "#: text #: #: kendo.toString(percentage, \"%\") #%"},
                visible: options.ShowLegend,
                position: "bottom"
                }, 
            plotArea: { padding: 0 },
            seriesDefaults: {
                labels: { visible: !options.ShowLegend, position: "center", background: "transparent", template: "#= category #: #= kendo.toString(percentage, \"%\") #%" }
            },
            series: [
                { type: "pie", padding: 15, field: "Value1", categoryField: "Category" }
            ],
            dataSource: new kendo.data.DataSource({data: []}),
            chartArea: {
                width: 250,
                height: 250
            },
            tooltip: {
                visible: options.ShowLegend,
                template: elideCategories ?
                            "#: dataItem.FullCategory #: ${kendo.toString(percentage, '%')}%" :
                            "${category}: ${kendo.toString(percentage, '%')}%"
            },            
            seriesClick: function(e) { widget.OnCrossLink(e); }
        };

        var kendoChart = domWidget.kendoChart(kendoOptions).data("kendoChart");
        return new Soris.Widgets.KendoPieChartWrapper(kendoChart, options);
    };

    KendoChartFactory.prototype.RenderLineChart = function(domWidget, widget, options) {
        var kendoOptions = {
            legend: { visible: false },
            plotArea: { padding: 0 },
            chartArea: {
                width: 500,
                height: 350
            },
            series: [
                { type: "line", padding: 15, field: "Value1", categoryField: "Category" }
            ],
            dataSource: new kendo.data.DataSource({data: []}),
            valueAxis: {
                labels: {format: options.ValueFormat },
                line: { visible: false }
            },
            categoryAxis: {
                labels: {format: options.CategoryFormat, rotation: -90 }
            },
            tooltip: {
                visible: true,
                template: "#= kendo.format(\"{0:" + options.CategoryFormat + "}\", category) #: #= kendo.format(\"" + options.ValueFormat + "\", value) #"
            },
            seriesClick: function(e) { widget.OnCrossLink(e); }   
        };

        var kendoChart = domWidget.kendoChart(kendoOptions).data("kendoChart");
        return new Soris.Widgets.KendoLineChartWrapper(kendoChart, options);
    };
    
    KendoChartFactory.prototype.RenderBarChart = function(domWidget, widget, options) {
        var kendoOptions = {
            legend: { visible: false },
            plotArea: { padding: 0 },
            chartArea: {
                width: 500,
                height: 350
            },
            series: [
                { type: "column", padding: 15, field: "Value1", categoryField: "Category" }
            ],
            dataSource: new kendo.data.DataSource({data: []}),
            valueAxis: {
                labels: {format: options.ValueFormat },
                line: { visible: false }
            },
            categoryAxis: {
                labels: {format: options.CategoryFormat, rotation: -90 }
            },
            tooltip: {
                visible: true,
                template: "#= kendo.format(\"" + options.CategoryFormat + "\", category) #: #= kendo.format(\"" + options.ValueFormat + "\", value) #"
            },
            seriesClick: function(e) { widget.OnCrossLink(e); }
        };

        var kendoChart = domWidget.kendoChart(kendoOptions).data("kendoChart");
        return new Soris.Widgets.KendoBarChartWrapper(kendoChart, options);
    };

    Widgets.KendoChartFactory = KendoChartFactory;
})(window.Soris, jQuery);
//Soris.Widgets.KendoLineChartWrapper class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");

    //Case 1036 - Add calculators to deal with different types of data evaluation scenario
    //      this will eventually expand into a WidgetResponseDataAction system where any piece
    //      code can register an action that should be performed on incoming data with
    //
    //          considerDataPoint   - tells the code about a point that came in
    //          executeResult       - performs whatever action is needed once all points are considered
    //
    //      the RequestMediator will be responsible for calling all the DataAction's and looping through
    //      the incoming data.  Each DataAction will rely on closures to access "local" data. Additionally,
    //      we'll need to create "final" actions which execute after all the DataAction's have executed.
    //      This will allow for refresh's etc to happen only once at the very end.

    var Calculator = (function(){
        function WidgetResponseDataActionManager(){
            this.calculators = [];
        }

        WidgetResponseDataActionManager.prototype.registerMaxMinCalculator = function(name, dataPointValuesCalculator) {
            this.calculators.push({
                name: name,
                min: null,
                max: null,
                getValues: dataPointValuesCalculator
            });
        };

        WidgetResponseDataActionManager.prototype.calculateResults = function(data) {
            this.executeCalculators(data);
            var results = {};

            for(var i=0; i<this.calculators.length; i++) {
                results[this.calculators[i].name] = {
                    min: this.calculators[i].min,
                    max: this.calculators[i].max
                };
            }

            return results;
        };

        WidgetResponseDataActionManager.prototype.executeCalculators = function(data) {
            for(var i=0; i<data.length; i++) {
                var dataPoint = data[i];

                for(var j=0; j<this.calculators.length; j++) {
                    var calculator = this.calculators[j];
                    var calculatedValues = calculator.getValues(dataPoint);

                    if(calculator.min === null || calculator.min > calculatedValues.min) calculator.min = calculatedValues.min;
                    if(calculator.max === null || calculator.max < calculatedValues.max) calculator.max = calculatedValues.max;
                }
            }
        };

        return WidgetResponseDataActionManager;
    }());

    // END Case 1036

    var KendoLineChartWrapper = Widgets.ChartBase.extend(function (kendoChart, options) {
        this._MyBaseCtor(options);
        var myOptions = Soris.Utility.loadDefaults({
            majorUnitCeiling: 500,
            verticalMaxCeiling: 1000
        }, options);
        this.options = Soris.Utility.loadDefaults(this.options, myOptions);

        this._KendoChart = kendoChart;
        this._MajorSteps = 10;
        this._MinimumStepPaddingPercent = 1.1;
        this._MaxOnYAxis = 20;
        this._MaxOnXAxis = 30;

        this.width = kendoChart.options.chartArea.width;
        this.dataPointWidth = (typeof options.dataPointWidth !== "undefined") ? options.dataPointWidth : 30;
        this.valueLabelPadding = (typeof options.valueLabelPadding !== "undefined") ? options.valueLabelPadding : 100;
    });

    KendoLineChartWrapper.prototype.CeilToMultiple = function ( value, multipleOf ) {
        var actualMultiplier = value / multipleOf;
        var ceiledMultiplier = Math.ceil(actualMultiplier);
        
        return multipleOf * ceiledMultiplier;
    };

    KendoLineChartWrapper.prototype.p_displayNotes = function(notes) {
        if(typeof notes === "undefined") return;

        this._ensureProperty(this._KendoChart.options.categoryAxis, "notes", { line: {}, data: []});
        this._ensureProperty(this._KendoChart.options.categoryAxis.notes, "line", {});
        this._ensureProperty(this._KendoChart.options.categoryAxis.notes, "data", []);
        this._KendoChart.options.categoryAxis.notes.data.length = 0;

        var valueParser = Soris.Utility.getParser(this.options.CategoryDataType);
        for(var i=0; i<notes.length; i++) {
            this._KendoChart.options.categoryAxis.notes.data.push({
                value: valueParser(notes[i].value),
                label: {
                    template: notes[i].text
                },
                link: notes[i].link
            });
        }

        this._KendoChart.options.categoryAxis.notes.line.length = 100;

        var wrapper = this;
        this._KendoChart.bind("noteClick", function(e) { wrapper._handleCategoryNoteClickByValue(e.value); });
    };

    KendoLineChartWrapper.prototype._handleCategoryNoteClickByValue = function(value) {
        var note = this._findCategoryNoteByValue(value);
        if(note !== null) {
            Soris.Utility.redirect(note.link);
        }
    };

    KendoLineChartWrapper.prototype._findCategoryNoteByValue = function(value) {
        var valueComparer = Soris.Utility.getComparer(this.options.CategoryDataType);
        var notes = this._KendoChart.options.categoryAxis.notes.data;
        for(var i=0; i<notes.length; i++) {
            if(valueComparer(value, notes[i].value) === 0) {
                return notes[i];
            }
        }

        return null;
    };

    KendoLineChartWrapper.prototype._ensureProperty = function(object, name, defaultValue) {
        if(typeof object[name] === "undefined") object[name] = defaultValue;
    };

    KendoLineChartWrapper.prototype.p_loadPreparedData = function (data, seriesInfo) {
        var calc = new Calculator();
        calc.registerMaxMinCalculator("yAxis", function(dataPoint) {
            var min = null,
                max = null;

            for(var i=0; i<seriesInfo.length; i++) {
                var seriesField = seriesInfo[i].field;
                var seriesValue = dataPoint[seriesField];

                if(min === null || min > seriesValue) min = seriesValue;
                if(max === null || max < seriesValue) max = seriesValue;
            }

            return {
                min: min,
                max: max
            };
        });

        if(this.options.CategoryDataType === "number") {
            calc.registerMaxMinCalculator("xAxis", function (dataPoint) {
                return {
                    max: dataPoint.Category,
                    min: dataPoint.Category
                };
            });

        } else if(this.options.CategoryDataType === "date") {
            calc.registerMaxMinCalculator("xAxis", function(dataPoint){
                var oneDayInMs = 24 * 60 * 60 * 1000;
                var daysInDataPoint = dataPoint.Category.getTime() / oneDayInMs;
                return {
                    max: daysInDataPoint,
                    min: daysInDataPoint
                };
            });

            this._KendoChart.options.categoryAxis.baseUnit = "days";

        } else {
            var dataPointCount = 0;
            calc.registerMaxMinCalculator("xAxis", function(dataPoint){
                return {
                    max: ++dataPointCount,
                    min: 0
                };
            });
        }

        var results = calc.calculateResults(data);
        var stepSize = results.yAxis.max / this._MajorSteps;
        stepSize *= this._MinimumStepPaddingPercent;

        var majorUnit = this.CeilToMultiple( stepSize, this.options.majorUnitCeiling );

        this._KendoChart.options.valueAxis.majorUnit = majorUnit;
        this._KendoChart.options.valueAxis.max = this.CeilToMultiple((results.yAxis.max + majorUnit), this.options.verticalMaxCeiling);
        
        var numberOfValuesDisplayed = (results.yAxis.max / majorUnit);
        if (numberOfValuesDisplayed > this._MaxOnYAxis) {
            var amountToSkip = data.length / this._MaxOnYAxis;
            this._KendoChart.options.valueAxis.majorUnit = majorUnit * amountToSkip;
        }

        this._configureChartAreaForData(data, results);

        this._KendoChart.dataSource.data(data);
    };

    KendoLineChartWrapper.prototype._configureChartAreaForData = function(data, results) {
        var distanceCharted = (results.xAxis.max - results.xAxis.min);
        var requiredWidth = (distanceCharted * this.dataPointWidth) + this.valueLabelPadding;
        var widthToUse = (requiredWidth > this.width) ? requiredWidth : this.width;

        this._KendoChart.options.chartArea.width = widthToUse;
    };

    KendoLineChartWrapper.prototype.p_setSeriesToDisplay = function(seriesInfo) {
        this._KendoChart.options.series.length = 0;
        for(var i=0; i<seriesInfo.length; i++) {
            this._KendoChart.options.series.push({
                type: "line",
                categoryField: "Category",
                field: seriesInfo[i].field,
                name: seriesInfo[i].name
            });
        }

        if(seriesInfo.length > 1) this._showLegend();
        else this._hideLegend();
    };

    KendoLineChartWrapper.prototype._showLegend = function (position) {
        if(typeof position === "undefined") position = "bottom";

        this._KendoChart.options.legend = {
            visible: true,
            position: position
        };
    };

    KendoLineChartWrapper.prototype._hideLegend = function() {
        this._KendoChart.options.legend = {
            visible: false
        };
    };

    KendoLineChartWrapper.prototype.p_redrawChart = function() {
        this._KendoChart.refresh();
        this._KendoChart.redraw();
    };
    Widgets.KendoLineChartWrapper = KendoLineChartWrapper;
})(window.Soris, jQuery);
//Soris.Widgets.KendoPieChartWrapper class
(function (Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    
    var KendoPieChartWrapper = Widgets.ChartBase.extend(function (kendoChart, options) {
        this._MyBaseCtor(options);

        this._KendoChart = kendoChart;
    });

    KendoPieChartWrapper.prototype.p_loadPreparedData = function (data, seriesInfo) {
        this._KendoChart.dataSource.data(data);
    };

    KendoPieChartWrapper.prototype.p_setSeriesToDisplay = function(seriesInfo) {
        if(seriesInfo.length > 1) throw new Error("KendoPieChartWrapper only supports up to 1 series!");

        this._KendoChart.options.series.length = 0;
        for(var i=0; i<seriesInfo.length; i++) {
            this._KendoChart.options.series.push({
                type: "pie",
                categoryField: "Category",
                field: seriesInfo[i].name
            });
        }
    };

    KendoPieChartWrapper.prototype.p_displayNotes = function(notes) {
        console.warn("KendoPieChartWrapper does not display notes.");
    };

    KendoPieChartWrapper.prototype.p_redrawChart = function() {
        this._KendoChart.refresh();
        this._KendoChart.redraw();
    };

    Widgets.KendoPieChartWrapper = KendoPieChartWrapper;
})(window.Soris, jQuery);
//Soris.Widgets.WidgetRequestConnectionBase class
(function(Soris) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");

    Widgets.WIDGET_REQUEST_CONNECTION_STATUS = {
        OPEN: 1,
        CLOSED: 2
    };
    var STATUS = Widgets.WIDGET_REQUEST_CONNECTION_STATUS;

    var WidgetRequestConnectionBase = Soris.Class.extend(function(){
        this._pendingReceives = [];
        this._receiveCallback = null;

        this.status = STATUS.OPEN;
    });

    WidgetRequestConnectionBase.prototype.send = function(request, callback) {
        throw new Error("type must implement send!");
    };

    /**
     * @param {function} receiveCallback - A callback function which is called every
     * time data comes in.  The callback is in the form function(err, data).
     */
    WidgetRequestConnectionBase.prototype.receive = function(receiveCallback) {
        if(this.status === STATUS.CLOSED) throw new Error("cannot receive when closed!");

        this._receiveCallback = receiveCallback;

        for(var i=0; i<this._pendingReceives.length; i++) {
            var pending = this._pendingReceives[i];
            this._receiveCallback(pending.err, pending.data);
        }

        this._pendingReceives.length = 0;
    };

    WidgetRequestConnectionBase.prototype._receiveOrPending = function(err, data) {
        if(this._receiveCallback !== null) {
            this._receiveCallback(err, data);
        } else {
            this._pendingReceives.push({
                err: err,
                data: data
            });
        }
    };

    /**
     * Called when the connection receives data.  Passes the data to the callback registered
     * with the receive function on the base class.
     *
     * @param data - The data received.
     */
    WidgetRequestConnectionBase.prototype.p_dataReceived = function(data) {
        this._receiveOrPending(null, data);
    };

    /**
     * Called when the connection receives an error.  Passes error and data to the callback
     * registered with the receive function on the base class.
     * @param err - The error
     * @param data - Any data that came with the error
     */
    WidgetRequestConnectionBase.prototype.p_errorReceived = function(err, data) {
        this._receiveOrPending(err, data);
    };

    WidgetRequestConnectionBase.prototype.close = function() {
        throw new Error("type must impelement close!");
    };

    WidgetRequestConnectionBase.prototype.onClosed = function() {
        var e = $.Event("closed");

        $(this).trigger(e);
    };

    WidgetRequestConnectionBase.prototype.sendComplete = function() {
        this.p_sendCompleted = true;

        if(this._receiveCompleted === true) this.close();
    };

    WidgetRequestConnectionBase.prototype.p_receiveComplete = function() {
        this._receiveCompleted = true;

        if(this.p_sendCompleted === true) this.close();
    };

    Widgets.WidgetRequestConnectionBase = WidgetRequestConnectionBase;
})(window.Soris);
//Soris.Widgets.AjaxWidgetRequestConnection class
(function(Soris, $) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");

    var STATUS = Widgets.WIDGET_REQUEST_CONNECTION_STATUS;
    var AjaxWidgetRequestConnection = Widgets.WidgetRequestConnectionBase.extend(function(options){
        this._MyBaseCtor(options);

        this._activeAjaxRequests = [];

        this._options = options;
    });

    AjaxWidgetRequestConnection.prototype.send = function(request) {
        if(this.status === STATUS.CLOSED) {
            throw new Error("Could not send request because the connection is closed!");
        }

        var ajaxRequest = $.ajax({
            url: this._options.url,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",
            data: JSON.stringify(request)
        });

        this._addActiveRequest(ajaxRequest);

        var conn = this;
        ajaxRequest.done(function(data, status, xhr) {
            conn.p_dataReceived(data);

            conn._removeActiveRequest(ajaxRequest);
        });

        ajaxRequest.fail(function(xhr, status, errorText) {
            conn.p_errorReceived(xhr.responseText || xhr.statusText, errorText);

            conn._removeActiveRequest(ajaxRequest);
        });
    };

    AjaxWidgetRequestConnection.prototype._addActiveRequest = function(request) {
        this._activeAjaxRequests.push(request);
    };

    AjaxWidgetRequestConnection.prototype._removeActiveRequest = function(request) {
        for(var i=0; i<this._activeAjaxRequests.length; i++) {
            if(this._activeAjaxRequests[i] === request) {
                this._activeAjaxRequests.splice(i, 1);
                break;
            }
        }

        if(this.p_sendCompleted && this._activeAjaxRequests.length === 0) this.p_receiveComplete();
    };

    AjaxWidgetRequestConnection.prototype.close = function() {
        for(var i=0; i<this._activeAjaxRequests.length; i++) {
            this._activeAjaxRequests[i].abort();
        }

        this.status = STATUS.CLOSED;
    };

    Widgets.AjaxWidgetRequestConnection = AjaxWidgetRequestConnection;
})(window.Soris, jQuery);
//Soris.Widgets.WidgetRequestMediaBase class
(function(Soris, $, undefined) {
    var Widgets = Soris.namespace("Widgets");

    var WidgetRequestMediaBase = Soris.Class.extend(function(){
    });

    WidgetRequestMediaBase.prototype.open = function() {
        throw new Error("type must implement open!");
    };

    Widgets.WidgetRequestMediaBase = WidgetRequestMediaBase;
})(window.Soris, jQuery);
//Soris.Widgets.AjaxWidgetRequestMedia class
(function(Soris, $, undefined) {
    var Widgets = Soris.namespace("Widgets");

    var AjaxWidgetRequestMedia = Widgets.WidgetRequestMediaBase.extend(function(options) {
        this.options = options;
    });

    AjaxWidgetRequestMedia.prototype.open = function() {
        return new Widgets.AjaxWidgetRequestConnection(this.options);
    };

    Widgets.AjaxWidgetRequestMedia = AjaxWidgetRequestMedia;
})(window.Soris, jQuery);
//Soris.Widgets.WidgetRequest class
(function(Soris, $, undefined) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");

    var STATUS = Widgets.WIDGET_REQUEST_CONNECTION_STATUS;
    var WidgetRequestMediator = Soris.Class.extend(function(options) {
        this.options = options;

        this._globalFilters = [];
        this._widgets = [];
        this._recordsGrid = null;
        this._currentConnection = null;

        this._init();
    });

    WidgetRequestMediator.getUnmanagedMediator = function() {
        if(Soris.Utility.isUndefined(WidgetRequestMediator._unmanagedMediator)) {
            WidgetRequestMediator._unmanagedMediator = new WidgetRequestMediator({});
        }

        return WidgetRequestMediator._unmanagedMediator;
    };

    WidgetRequestMediator.prototype._init = function() {
        var config = Widgets.PageConfiguration.resolve();

        this._requestMedia = this.options.requestMedia;
        if(Soris.Utility.isUndefined(this._requestMedia)) {
            this._requestMedia = config.resolveRequestMedia();
        }

        this.options.globalQueryUrl = config.options.globalQueryUrl;
        this.globalQueryDataQueue = (function() {
            var STATE = {
                DATA_AVAILABLE: "DATA_AVAILABLE",
                NO_DATA_AVAILABLE: "NO_DATA_AVAILABLE"
            };

            function GlobalQueryDataQueue() {
                this.queue = [];
                this.state = STATE.NO_DATA_AVAILABLE;
                this.data = null;
            }

            GlobalQueryDataQueue.prototype.enqueueWorkItem = function(workItem) {
                if(this.state === STATE.NO_DATA_AVAILABLE) {
                    this.queue.push(workItem);
                } else if(this.state === STATE.DATA_AVAILABLE) {
                    workItem(this.data);
                }
            };

            GlobalQueryDataQueue.prototype.loadData = function(globalData) {
                for(var i=0; i<this.queue.length; i++) {
                    var workItem = this.queue[i];

                    workItem(globalData);
                }
                this.queue.length = 0;

                this.data = globalData;
                this.state = STATE.DATA_AVAILABLE;
            };

            GlobalQueryDataQueue.prototype.clearData = function() {
                this.data = null;
                this.state = STATE.NO_DATA_AVAILABLE;
            };

            return new GlobalQueryDataQueue();
        }());

        var mediator = this;
        $(window).on("beforeunload", function() {
            if(mediator._currentConnection !== null) mediator._currentConnection.close();
        });
    };

    WidgetRequestMediator.prototype.cancelPendingRequests = function() {
        for(var i=0; i<this._widgets.length; i++) {
            this._widgets[i].hideLoadingMask();
        }
    };

    WidgetRequestMediator.prototype.runGlobalQuery = function(globalFilters) {
        this.globalQueryDataQueue.clearData();

        var mediator = this;
        $.ajax({
            url: this.options.globalQueryUrl,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            type: "POST",data: JSON.stringify({
                requests: [],
                globalFilters: globalFilters.asKeyValueArray
            })
        })
            .then(function(httpResponse) {
                if(httpResponse.Errors !== null && httpResponse.Errors.length > 0) {
                    mediator.showErrors(httpResponse.Errors);
                    mediator.cancelPendingRequests();

                } else {
                    mediator.globalQueryDataQueue.loadData(httpResponse);

                    var e = $.Event("globalQueryCompleted");
                    e.globalResponse = httpResponse;
                    $(mediator).trigger(e);
                }
            });
    };

    WidgetRequestMediator.prototype.showErrors = function(errors) {
        var msg = "";

        for(var i=0; i<errors.length; i++) {
            msg += errors[i] + "\n";
        }

        this.showGeneralError(msg);
    };

    WidgetRequestMediator.prototype.showGeneralError = function(message) {
        alert(message);
    };

    WidgetRequestMediator.prototype.globalChanged = function() {
        var globalFilterValues = this._getGlobalFilterValues();
        if(globalFilterValues.validationErrors.length > 0) {
            this.showGeneralError(globalFilterValues.validationErrors.join("\n"));

        } else {
            this.runGlobalQuery(globalFilterValues);
            this._ensureConnection(true);

            for(var i=0; i<this._widgets.length; i++) {
                this._widgets[i].showLoadingMask();
                this._currentConnection.send(this._createRequest(this._widgets[i], globalFilterValues));
            }

            this._refreshRecordsGrid(globalFilterValues);
        }
    };

    WidgetRequestMediator.prototype._createRequest = function(widget, globalFilterValues) {
        if(Soris.Utility.isUndefined(globalFilterValues)) globalFilterValues = this._getGlobalFilterValues();

        return {
            requests: [widget.getRequestData()],
            globalFilters: globalFilterValues.asKeyValueArray
        };
    };

    WidgetRequestMediator.prototype.widgetChanged = function(widget) {
        widget.showLoadingMask();

        this._ensureConnection(false);
        this._currentConnection.send(this._createRequest(widget));
    };

    WidgetRequestMediator.prototype._ensureConnection = function(closeExisting) {
        if(this._currentConnection !== null && this._currentConnection.status === STATUS.OPEN) {
            if(closeExisting) {
                this._currentConnection.close();
            } else {
                return;
            }
        }

        this._currentConnection = this._requestMedia.open();

        var mediator = this;
        this._currentConnection.receive(function(err, data) {
            var response = null,
                widget = null;

            if(typeof(data) === "object" && "Responses" in data && "WidgetID" in data.Responses[0]) {
                response = data.Responses[0];
                widget = mediator._getWidgetById(response.WidgetID);
            }

            if(err === null) {
                var dataSets = response.DataSets;

                mediator.globalQueryDataQueue.enqueueWorkItem(function(globalData) {
                    widget.loadDataSets(dataSets, globalData);
                });

            } else if(widget !== null && err !== "abort") {
                widget.showError(err);

            } else if(err !== "abort") {
                mediator.showGeneralError(err);
            } // else ignore err === "abort"
        });
    };

    WidgetRequestMediator.prototype._getWidgetById = function(id) {
        for(var i=0; i<this._widgets.length; i++) {
            if(this._widgets[i].id === id) return this._widgets[i];
        }

        throw new Error("could not find a widget with '" + id + "' id.  Did you call registerWidget?");
    };

    WidgetRequestMediator.prototype.registerRecordsGrid = function(recordsGrid) {
        if(!(recordsGrid instanceof Soris.Ui.GridBase)) throw new Error("recordsGrid was not a valid Widgets.RecordsGrid!");

        this._recordsGrid = recordsGrid;

        var globalFilterValues = this._getGlobalFilterValues();
        this._refreshRecordsGrid(globalFilterValues);
    };

    WidgetRequestMediator.prototype._refreshRecordsGrid = function(globalFilters) {
        if(this._recordsGrid === null) return;

        this._recordsGrid.setStaticFilters(globalFilters.asKendoArray);
    };

    WidgetRequestMediator.prototype.registerGlobalFilter = function(globalFilter) {
        Soris.Utility.argumentRequired(globalFilter.parameterName, "parameterName");
        Soris.Utility.argumentRequired(globalFilter.getValue, "getValue");

        this._globalFilters.push(globalFilter);
    };

    WidgetRequestMediator.prototype._getGlobalFilterValues = function() {
        var filters = {
            asHashtable: {},
            asKeyValueArray: [],
            asKendoArray: [],
            validationErrors: []
        };

        for(var i=0; i<this._globalFilters.length; i++) {
            var parameterName = this._globalFilters[i].parameterName;
            var value = this._globalFilters[i].getValue();

            filters.asKeyValueArray.push({
                key: parameterName,
                value: value
            });

            filters.asHashtable[parameterName] = value;

            if(!Soris.Utility.isUndefined(value) && value !== "" && value !== null) {
                filters.asKendoArray.push({
                    field: parameterName,
                    operator: "eq",
                    value: value
                });
            }

            [].push.apply(filters.validationErrors, this._globalFilters[i].validate());
        }

        return filters;
    };

    WidgetRequestMediator.prototype.registerWidget = function(widget) {
        Soris.Utility.argumentRequired(widget.getRequestData, "getRequestData");
        Soris.Utility.argumentRequired(widget.loadDataSets, "loadDataSets");
        Soris.Utility.argumentRequired(widget.showError, "showError");
        Soris.Utility.argumentRequired(widget.showLoadingMask, "showLoadingMask");
        Soris.Utility.argumentRequired(widget.id, "id");

        this._widgets.push(widget);
    };

    Widgets.WidgetRequestMediator = WidgetRequestMediator;
})(window.Soris, jQuery);
//Soris.Widgets.Filters.FilterBase class
(function (Soris, $, undefinded) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var FilterBase = Soris.Ui.UiComponentBase.extend(function (options) {
        this._MyBaseCtor();

        this._TextBefore = options.TextBefore || "";
        this._TextAfter = options.TextAfter || "";
        this._Events = options.Events || {};
        this._NonFiltering = (options.NonFiltering !== undefinded) ? options.NonFiltering : false;
        this.DefaultValue = (options.DefaultValue !== undefinded) ? options.DefaultValue : null;
        this.ParameterName = options.ParameterName || "Unknown Parameter Name";
        this.parameterName = this.ParameterName;
        this.DisplayTextParameterName = options.DisplayTextParameterName || "Unknown Display Text Parameter Name";
    });

    FilterBase.prototype.validate = function() { return []; };

    FilterBase.prototype._attachEvents = function () {
        for (var event in this._Events) {
            this.p_attachEvent(event, this._Events[event]);
        }

        var filter = this;
        this.p_attachEvent("change", function(e) {
            function runAfterCurrentThreadCompletes(f) { setTimeout(f, 0); }
            runAfterCurrentThreadCompletes(function() {
                if(!e.isDefaultPrevented()) filter.OnFilter();
            });
        });
    };

    FilterBase.prototype.getValue = function () { };
    FilterBase.prototype.GetValue = function() { return this.getValue(); };
    
    FilterBase.prototype.AddHandler = function (eventName, handler) {
        this._Events[eventName] = handler;
    };

    FilterBase.prototype.OnFilter = function() {
        if (!this._NonFiltering) {
            var event = $.Event("filter");

            $(this).trigger(event);
        }
    };

    FilterBase.prototype.Render = function(container) { this.render(container); };
    FilterBase.prototype.render = function (container) {
        this._renderStart(container);

        var filterInputField = $(document.createElement("input"));
        filterInputField.addClass("s-filter");
        container.append(filterInputField);
        this.p_renderFilter(filterInputField);
        this._attachEvents();

        this._renderEnd(container);
    };

    FilterBase.prototype._renderStart = function (container) {
        var text = document.createTextNode(this._TextBefore);
        container.append(text);
    };

    FilterBase.prototype.p_renderFilter = function (filterInputField) { throw new Error("Filter must implement p_renderFilter"); };
    FilterBase.prototype.RenderFilter = function(filterInputField) { this.p_renderFilter(filterInputField); };

    FilterBase.prototype.p_attachEvent = function (eventName, handler) { throw new Error("Filter must implement p_attachEvent"); };
    FilterBase.prototype.AttachEvent = function(eventName, handler) { this.p_attachEvent(eventName, handler); };
    
    FilterBase.prototype._renderEnd = function (container) {
        var text = document.createTextNode(this._TextAfter);
        container.append(text);
    };

    Filters.FilterBase = FilterBase;
})(window.Soris, jQuery);

//Soris.Widgets.Filters.DateFilter class
(function (Soris, $) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");

    var DateFilter = Filters.FilterBase.extend(function (options) {
        this._MyBaseCtor(options);

        var currentYear = (new Date()).getFullYear();
        this._options = Soris.Utility.loadDefaults({
            required: true
        }, options);
    });

    DateFilter.prototype.p_renderFilter = function (filterInputField) {
        this._dateInput = filterInputField;
        this._datePicker = this.p_uiComponentFactory.RenderDatePicker(filterInputField, {
            value: this._options.initialValue,
            change: 0
        });
    };

    DateFilter.prototype.getValue = function () {
        return this._dateInput.val();
    };

    DateFilter.prototype.validate = function() {
        var errors = [];

        var rawInput = this.getValue();
        if((rawInput === "" || rawInput === null) && this._options.required) {
            errors.push(this.ParameterName + " is required.");

        } else if(rawInput !== "" && rawInput !== null && !Soris.Utility.isDateValid(rawInput)) {
            errors.push(this.ParameterName + ": " + rawInput + " is not a valid date.");
        }

        return errors;
    };

    DateFilter.prototype.p_attachEvent = function (eventName, handler) {
        this._datePicker.on(eventName, handler);
    };

    Filters.DateFilter = DateFilter;
})(window.Soris, jQuery);

//Soris.Widgets.Filters.OptionListFilterBase class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var OptionListFilterBase = Filters.FilterBase.extend(function (options) {
        this._MyBaseCtor(options);

        this.Options = options.Options || [];
        this.FilterInputField = null;
    });

    OptionListFilterBase.prototype.p_renderFilter = function (filterInputField) {
        this.FilterInputField = filterInputField;

        this.StartRenderFilter();

        for (var i = 0; i < this.Options.length; i++) {
            this.RenderOption(this.Options[i]);
        }

        this.EndRenderFilter();
    };

    OptionListFilterBase.prototype.StartRenderFilter = function () { };
    OptionListFilterBase.prototype.RenderOption = function (option) { };
    OptionListFilterBase.prototype.EndRenderFilter = function () { };

    Filters.OptionListFilterBase = OptionListFilterBase;
})(window.Soris, jQuery);

//Soris.Widgets.Filters.RadioButtonsFilter class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var RadioButtonsFilter = Filters.OptionListFilterBase.extend(function (options) {
        this._MyBaseCtor(options);


    });

    RadioButtonsFilter.prototype.StartRenderFilter = function () {
        var newFilterField = $(document.createElement("span"));

        this.FilterInputField.replaceWith(newFilterField);
        this.FilterInputField = newFilterField;
    };

    RadioButtonsFilter.prototype.RenderOption = function (option) {
        var optionEl = document.createElement("input");
        optionEl.type = "radio";
        optionEl.value = option.Value;
        optionEl.name = this.ParameterName;

        if (option.Value === this.DefaultValue) optionEl.checked = true;

        var label = $(document.createElement("label"));
        label.append(optionEl);
        label.append(document.createTextNode(option.Text));

        this.FilterInputField.append(label);
    };

    RadioButtonsFilter.prototype.p_attachEvent = function (eventName, handler) {
        this.FilterInputField.on(eventName, handler);
    };

    RadioButtonsFilter.prototype.getValue = function () {
        return this.FilterInputField.find("input:checked").val();
    };

    Filters.RadioButtonsFilter = RadioButtonsFilter;
})(window.Soris, jQuery);
//Soris.Widgets.Filters.DropDownListFilter class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var DropDownListFilter = Filters.OptionListFilterBase.extend(function (options) {
        this._MyBaseCtor(options);


    });

    DropDownListFilter.prototype.StartRenderFilter = function () {
        var newFilterField = $(document.createElement("select"));

        this.FilterInputField.replaceWith(newFilterField);
        this.FilterInputField = newFilterField;
    };

    DropDownListFilter.prototype.RenderOption = function (option) {
        var optionEl = document.createElement("option");
        optionEl.value = option.Value;
        optionEl.text = option.Text;

        this.FilterInputField.append(optionEl);
    };

    DropDownListFilter.prototype.GetValue = function () {
        return this.FilterInputField.val();
    };

    DropDownListFilter.prototype.AttachEvent = function (eventName, handler) {
        this.FilterInputField.on(eventName, handler);
    };

    Filters.DropDownListFilter = DropDownListFilter;
})(window.Soris, jQuery);

//Soris.Widgets.Filters.ExternalFilter class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var ExternalFilter = Filters.FilterBase.extend(function (options) {
        this._MyBaseCtor(options);

        if (options.JqInput === undefined) throw "JqInput is a required option.";
        this._JqInput = options.JqInput;
        this._JqContainer = (options.JqContainer !== undefined) ? options.JqContainer : options.JqInput;
    });

    ExternalFilter.prototype.p_renderFilter = function (filterInputField) {
        this._JqContainer.replaceAll(filterInputField);
        this._JqContainer.addClass("s-filter");



        var cmb = this._JqInput.data("kendoComboBox");
        if (cmb !== undefined) {
            this.p_attachEvent = this._kendoAttachEvent;

            var textFromQstring = Soris.Utility.getQstringParam(this.DisplayTextParameterName);
            if(textFromQstring !== "") {
                cmb.input.val(textFromQstring);
            }
        } else {
            this.p_attachEvent = this._jqAttachEvent;
        }
    };
    
    ExternalFilter.prototype.p_attachEvent = function (eventName, handler) {
        throw new Error("no attach event defined!");
    };

    ExternalFilter.prototype._jqAttachEvent = function(eventName, handler) {
        this._JqInput.on(eventName, handler);
    };

    ExternalFilter.prototype._kendoAttachEvent = function(eventName, handler) {
        this._JqInput.data("kendoComboBox").bind(eventName, handler);
    };

    ExternalFilter.prototype.getValue = function () {
        return this._JqInput.val();
    };

    ExternalFilter.prototype.GetText = function() {
        var cmb = this._JqInput.data("kendoComboBox");
        if (cmb !== undefined) {
            return cmb.text();
        }
    };
    
    Filters.ExternalFilter = ExternalFilter;
})(window.Soris, jQuery);

//Soris.Widgets.Filters.NumericSpinnerFilter class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");
    
    var NumericSpinnerFilter = Filters.FilterBase.extend(function (options) {
        this._MyBaseCtor(options);

        this._Max = (options.Max !== undefined) ? options.Max : null;
        this._Min = (options.Min !== undefined) ? options.Min : null;
        this._Format = options.Format || "#";
        this._Decimals = (options.Decimals !== undefined) ? options.Decimals : 0;

        this._Spinner = null;

        if (this.DefaultValue === null) this.DefaultValue = 0;
    });

    NumericSpinnerFilter.prototype.GetValue = function () {
        return this._Spinner.val();
    };

    NumericSpinnerFilter.prototype.p_renderFilter = function (filterInputField) {
        var spinnerOptions = {
            format: this._Format,
            min: this._Min,
            max: this._Max,
            decimals: this._Decimals,
            value: this.DefaultValue
        };

        this._Spinner = this.UiComponentFactory.RenderSpinner(filterInputField, spinnerOptions);
    };

    NumericSpinnerFilter.prototype.p_attachEvent = function (eventName, handler) {
        this._Spinner.on(eventName, handler);
    };

    Filters.NumericSpinnerFilter = NumericSpinnerFilter;
})(window.Soris, jQuery);
//Soris.Widgets.Filters.StaticValueFilter class
(function (Soris, $, undefined) {
    "use strict";

    var Filters = Soris.namespace("Widgets.Filters");

    var StaticValueFilter = Filters.FilterBase.extend(function (options) {
        this._MyBaseCtor(options);
    });

    StaticValueFilter.prototype.Render = function (container) { };

    StaticValueFilter.prototype.getValue = function () {
        return this.DefaultValue;
    };

    StaticValueFilter.prototype.p_renderFilter = function (filterInputField) {
        filterInputField.prop("type", "hidden");
    };

    StaticValueFilter.prototype.p_attachEvent = function (eventName, handler) {
        //do nothing
    };

    Filters.StaticValueFilter = StaticValueFilter;
})(window.Soris, jQuery);

//Soris.Widgets.GlobalFilterPanel class
(function(Soris, $) {
    "use strict";

    var Widgets = Soris.namespace("Widgets");
    var Filters = Soris.namespace("Widgets.Filters");
    var Ui = Soris.namespace("Ui");

    var GlobalFilterPanel = Soris.Ui.UiComponentBase.extend(function(options) {
        this._filters = [];
        this._options = Soris.Utility.loadDefaults({
            GlobalFilters: []
        }, options);

        this._requestMediator = this._resolveMediator();

        this._processOptions();
    });

    GlobalFilterPanel.prototype._processOptions = function() {
        for(var i=0; i<this._options.GlobalFilters.length; i++) {
            this.add(this._options.GlobalFilters[i]);
        }
    };

    GlobalFilterPanel.prototype._resolveMediator = function() {
        if(!Soris.Utility.isUndefined(this._options.mediator) && this._options.mediator !== null) return this._options.mediator;
        else return Widgets.WidgetRequestMediator.getUnmanagedMediator();
    };

    GlobalFilterPanel.prototype.add = function(filter) {
        this._filters.push(filter);
        this._requestMediator.registerGlobalFilter(filter);
    };

    GlobalFilterPanel.prototype.render = function(container) {
        container = $(container);
        this._createDateFilters();

        var filterPanel = $(document.createElement("div"));
        filterPanel.addClass("s-global-filter-panel");
        filterPanel.prop("id", container.prop("id"));

        container.append(filterPanel);

        this._renderGlobalFilters(filterPanel);

        filterPanel.data("sorisGlobalFilterPanel", this);
    };

    GlobalFilterPanel.prototype._createDateFilters = function () {
        var panel = this;
        var changeHandler = function() { panel._requestMediator.globalChanged(); };

        this._startDateFilter = new Filters.DateFilter({
            TextBefore: "From",
            ParameterName: "StartDate",
            initialValue: this._findInitialStartDate(),
            change: changeHandler
        });
        this.add(this._startDateFilter);

        this._endDateFilter = new Filters.DateFilter({
            TextBefore: "To",
            ParameterName: "EndDate",
            initialValue: this._findInitialEndDate(),
            change: changeHandler
        });
        this.add(this._endDateFilter);
    };

    GlobalFilterPanel.prototype._findInitialEndDate = function () {
        var qstringEndDate = Soris.Utility.getQstringParam("EndDate");
        var defaultEndDate = new Date();

        if(!Soris.Utility.isUndefined(this._options.endDate)) return this._options.endDate;
        else if(!Soris.Utility.isUndefined(qstringEndDate)) return qstringEndDate;
        else return defaultEndDate;
    };

    GlobalFilterPanel.prototype._findInitialStartDate = function() {
        var qstringStartDate = Soris.Utility.getQstringParam("StartDate");
        var thisYear = (new Date()).getFullYear();
        var defaultStartDate = new Date(thisYear, 0, 1);

        if(!Soris.Utility.isUndefined(this._options.startDate)) return this._options.startDate;
        else if(!Soris.Utility.isUndefined(qstringStartDate)) return qstringStartDate;
        else return defaultStartDate;
    };

    GlobalFilterPanel.prototype._renderGlobalFilters = function(container) {
        var mediator = this._requestMediator;
        var handler = function() { mediator.globalChanged(); };

        for(var i=0; i<this._filters.length; i++) {
            this._filters[i].render(container);
            $(this._filters[i]).on("filter", handler);
        }
    };

    Widgets.GlobalFilterPanel = GlobalFilterPanel;
})(window.Soris, jQuery);
//Soris.Widgets.PageConfiguration class
(function(Soris, $) {
    var Widgets = Soris.namespace("Widgets");

    var PageConfiguration = Soris.Class.extend(function(options) {
        this.options = Soris.Utility.loadDefaults({
                queryOnLoad: false,
                globalQueryUrl: "/GlobalQuery"
            },
            options);

        this._registerLoadHandler();
    });

    PageConfiguration.resolve = function() {
        if(Soris.Utility.isUndefined(PageConfiguration._config)) {
            PageConfiguration._config = new Widgets.EmptyPageConfiguration();
        }

        return PageConfiguration._config;
    };

    PageConfiguration.create = function(options) {
        PageConfiguration._config = new PageConfiguration(options);

        return PageConfiguration._config;
    };

    PageConfiguration.prototype.resolveRequestMedia = function() {
        switch(this.options.requestMedia.type){
            case "ajax":
                return new Widgets.AjaxWidgetRequestMedia(this.options.requestMedia);
            default:
                throw new Error("Unknown request media type");
        }
    };

    PageConfiguration.prototype._registerLoadHandler = function() {
        var options = this.options;

        $(window).load(function() {
            if(options.queryOnLoad) {
                setTimeout(function() {
                    Widgets.WidgetRequestMediator.getUnmanagedMediator().globalChanged();
                }, 250);
            }
        });
    };

    Widgets.PageConfiguration = PageConfiguration;
})(window.Soris, jQuery);
//Soris.Widgets.EmptyPageConfiguration class
(function(Soris) {
    var Widgets = Soris.namespace("Widgets");

    var EmptyPageConfiguration = Widgets.PageConfiguration.extend(function() {
        this.options = {
            queryOnLoad: false
        };
    });

    EmptyPageConfiguration.DEFAULT_QUERY_URL = "/query";

    EmptyPageConfiguration.prototype.resolveRequestMedia = function() {
        return new Widgets.AjaxWidgetRequestMedia({
            url: EmptyPageConfiguration.DEFAULT_QUERY_URL
        });
    };

    Widgets.EmptyPageConfiguration = EmptyPageConfiguration;
})(window.Soris);
//jQuery extensions
(function ($, undefined) {
    "use strict";

    $.fn.sorisDirectionChangeWidget = function (options) {
        var internalOptions = $.extend({
            WidgetID: "Missing DirectionChangeWidget ID",
            Title: "Untitled Widget",
            Filters: []
        }, options);

        this.each(function () {
            var widget = new Soris.Widgets.DirectionChangeWidget(internalOptions);

            widget.Render(this);
        });
    };

    $.fn.sorisChartWidget = function (options) {
        var internalOptions = $.extend({
            WidgetID: "Missing ChartWidget ID",
            Title: "Untitled Widget",
            Filters: []
        }, options);

        this.each(function () {
            var widget = new Soris.Widgets.ChartWidget(internalOptions);

            widget.Render(this);
        });
    };

    $.fn.sorisGlobalFilterPanel = function (options) {
        var internalOptions = $.extend({}, options);

        this.each(function () {
            var filterPanel = new Soris.Widgets.GlobalFilterPanel(internalOptions);

            filterPanel.render(this);
        });
    };
})(jQuery);