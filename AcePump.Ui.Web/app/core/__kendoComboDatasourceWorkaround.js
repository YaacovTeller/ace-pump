(function () {
    "use strict";

    // Case 1054
    //
    // Created by YY Kosbie 11/22/2015
    //
    // Angular directive which connects the dataSource of a kendo combo box to a simple scope
    // function in the controller.  Kendo's 2015 Q1 implementation of an angular combo box creates sever
    // speed issues on a page by loading and managing the DOM for many data sources simultaneously.  This causes
    // a delay of several seconds before the page becomes responsive.  Use this directive to wrap your kendo
    // angular combo box and get access to the text changed and data source.
    //
    // IMPLEMENTATION NOTES: See kendo source kendo.angular.js line 720.  The actual widget is instatiated in
    // the post-link phase of kendo directive.  This means any changes we make to the HTML in the compile
    // template or pre-link phases will be overwritten by Kendo's jQuery DOM manipulation.  Also, this means
    // our post link will run in reverse priority so a higher priority runs *later* than a lower.
    //
    // Additionally, the kendo wrapper does *not* store the kendoComboBox object in  data("kendoComboBox")
    // like a traditional kendo widget.  Instead it is in data("handler") of the HTML element which contained
    // the kendo-combo-box directive.
    //
    // USAGE:
    //      Add a <kendo-combo-datasource-workaround> wrapper element around your <ANY kendo-combo-box> element.  Add a
    //      searchTextChanged to your scope so the combo can update it's options.  In form:
    //
    //          function searchTextChanged(text) {
    //              return Promise([newOptions]);
    //          }
    //
    //      Be sure to set the k-data-source to [] to begin with.
    //

    angular
        .module("acePump.core")
        .directive("kendoComboDatasourceWorkaround", kendoComboDatasourceWorkaround);

    kendoComboDatasourceWorkaround.$inject = ["$timeout"];
    function kendoComboDatasourceWorkaround($timeout) {
        var directive = {
            priority: -100,
            restrict: "E",
            transclude: true,
            scope: {},
            template:
                "<span class=\"kendo-combo-workaround-container\" ng-keyup=\"searchTextChanged_Internal()\">" +
                    "<div ng-transclude>" +
                    "</div>" +
                "</span>",
            link: link,
            controller: ComboWorkaroundController
        };

        ComboWorkaroundController.$inject = ["$scope"];
        function ComboWorkaroundController($scope) {
            $scope.searchTextChanged_Internal = function () {
                if ($scope._searchTimeout !== null) $timeout.cancel($scope._searchTimeout);
                $scope._searchTimeout = $timeout($scope.runSearch, 100, false);
            };

            $scope.runSearch = function () {
                var searchText = $scope.getSearchText();
                if ($scope._kendoComboBox.options.minLength > searchText.length) {
                    $scope._kendoComboBox.dataSource.data([]);
                    $scope._kendoComboBox.close();
                    return;
                }

                if ($scope._reqInProgress) return;
                $scope._setReqInProgress();
                $scope.$parent
                    .searchTextChanged(searchText)
                    .then(function (newOptions) {
                        $scope._clearReqInProgress();
                        $scope._kendoComboBox.dataSource.data(newOptions);

                        if (newOptions.length > 0) $scope._kendoComboBox.open();
                    });
            };

            $scope._setReqInProgress = function () {
                $scope._reqInProgress = true;

                var arrow = $scope._wrapperElement.find(".k-select");
                kendo.ui.progress(arrow, true);
                arrow.find(".k-loading-image").css("background-size", "contain");
            };

            $scope._clearReqInProgress = function () {
                $scope._reqInProgress = false;

                var arrow = $scope._wrapperElement.find(".k-select");
                kendo.ui.progress(arrow, false);
            };

            $scope.getSearchText = function () {
                return $scope._kendoVisibleInput.val();
            };
        }

        link.$inject = ["scope", "elem", "attrs"];
        function link(scope, elem, attrs) {
            scope._wrapperElement = elem;

            scope._searchTimeout = null;
            scope._kendoComboBox = elem.find(".k-combobox select").data("handler");
            scope._kendoVisibleInput = elem.find(".k-combobox .k-input");
        }

        return directive;
    }
})();