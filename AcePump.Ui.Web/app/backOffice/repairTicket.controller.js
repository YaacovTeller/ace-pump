(function () {
    "use strict";

    angular
        .module("acePump.backOffice")
        .controller("RepairTicketController", RepairTicketController);

    RepairTicketController.$inject = ["$http", "inventoryService", "modalService", "$q", "$scope", "$timeout", "util", "$window"];
    function RepairTicketController($http, inventoryService, modalService, $q, $scope, $timeout, util, $window) {
        $scope.CONSTANTS = {
            ERROR_HANDLED: "$scope.ERROR_HANDLED"
        };

        $scope.grid_Error = function (e) {
            if (e.errors) {
                var message = "";

                for (var propertyName in e.errors) {
                    var property = e.errors[propertyName];
                    if ("errors" in property) {
                        for (var i = 0; i < property.errors.length; i++) {
                            message += propertyName + ": " + property.errors[i] + "\n";
                        }
                    }
                }

                $scope.errorMessage = message;
                $scope._revertPartInspectionValues(e.model);
            }
        };

        /**
         * Validates that all items on the repair have been marked.  This automatically
         * saves any open editors and verifies that the save succeeded.
         * @returns {Promise} Resolves if validation passes, rejects if it fails.
         */
        $scope.validateAllItemsMarked = function () {
            var allInspectionsMarked = true;
            for (var i = 0; i < $scope.inspections.length; i++) {
                if (!$scope.inspections[i].Result) {
                    allInspectionsMarked = false;
                    $scope.inspections[i].updateFailed = true;
                }
            }

            if (allInspectionsMarked) {
                return $scope.saveAll();

            } else {
                $scope.alert("You must mark all parts on the repair ticket before you can complete it.");

                return $q.reject();
            }
        };

        /**
         * Validates the repair ticket is filled out properly and POSTs the entire page
         * to the complete repair page.
         */
        $scope.validateAndPost = function () {
            $scope.validateAllItemsMarked()
                .then(function () {
                    var element = angular.element("form[name='inspectionForm']");
                    element.submit();
                });
        };

        /**
         * Tries to save any open editors (inspections where "Convert" or "Replace" was selected and POST
         * the update to the server.
         * @returns {Promise} Resolves when all the open editors succeed in posting their values.  Rejects
         * if any of the editors fail.
         */
        $scope.saveAll = function () {
            var markCompletedPromises = [];
            for (var i = 0; i < $scope.inspections.length; i++) {
                var inspection = $scope.inspections[i];
                if (inspection._markCompletedPromise) markCompletedPromises.push(inspection._markCompletedPromise);

                if ($scope.inspections[i].inEditMode) {
                    $scope._prepareForSave($scope.inspections[i]);

                    inspection._editDeferred.resolve();

                    delete inspection._editDeferred;
                }
            }

            return $q.all(markCompletedPromises);
        };

        /**
         * A part mark may be "OK", "N/A", "Maintenance", "Convert", or "Replace".  This method updates the
         * mark and does anything else necessary for that specific mark on that part.  For example, on an an
         * assembly which was previously split, markPart unsplits it before saving the parent assemblie's new
         * mark.
         *
         * @param partInspection the inspection to mark
         * @param markAs the new mark - can be "OK", "N/A", "Maintenance", "Convert", or "Replace"
         */
        $scope.markPart = function (partInspection, markAs) {
            var markDeferred = $q.defer();
            partInspection._markCompletedPromise = markDeferred.promise;

            $q.when()
                .then(function () {
                    if (partInspection.IsSplitAssembly) {
                        return $scope.unsplitAssembly(partInspection);
                    }
                })
                .then(function () { $scope._setInspectionValuesByMark(partInspection, markAs); })
                .then(function () {
                    if ($scope._shouldSuggestAssemblySplit(partInspection, markAs)) {
                        return $scope.suggestReplaceAssembly(partInspection)
                            .catch(function () {
                                markDeferred.resolve();
                                delete partInspection._markCompletedPromise;

                                return $q.reject();
                            });
                    }
                })
                .then(function () {
                    if ($scope._shouldAllowEditingBeforePostUpdate(partInspection)) {
                        return $scope._runAllowEditAndPostLoop(partInspection)
                            .catch(function (err) {
                                if (err === "cancel") {
                                    markDeferred.resolve("cancel");
                                    return $q.reject($scope.CONSTANTS.ERROR_HANDLED);

                                } else {
                                    return $q.reject(err);
                                }
                            });

                    } else {
                        return $scope.postInspectionUpdate(partInspection)
                            .catch(function (err) {
                                $scope._revertPartInspectionValues(partInspection);
                                return $q.reject(err);
                            });
                    }
                })
                .then(function () {
                    return $scope.checkAvailabilityInInventory(partInspection);
                })
                .then(function () {
                    partInspection.updateFailed = false;

                    markDeferred.resolve();

                    delete partInspection._markCompletedPromise;
                })
                .catch(function (error) {
                    if (error !== $scope.CONSTANTS.ERROR_HANDLED) {
                        markDeferred.reject();
                        console.log("Unhandled error in markPart: " + error);
                    }
                });
        };

        $scope.checkAvailabilityInInventory = function (inspection) {
            return $q.when()
                .then(function () {
                    if ($scope.serverVars.customerUsesInventory) {
                        var url = $scope.serverVars.checkAvailabilityInInventoryUrl;
                        var partReplacedId = inspection.PartReplacedID;
                        var customerId = $scope.serverVars.customerID;

                        if ($scope.serverVars.canModifyInventory && inspection.PartReplacedID) {
                            return inventoryService.checkAvailability(url, inspection.PartReplacedID, customerId);

                        } else {
                            return false;
                        }
                    } else {
                        return false;
                    }
                })
                .then(function (available) {
                    inspection.AvailableInInventory = available;
                    inspection.showInventoryButton = $scope.showInventoryButton(inspection);
                });
        };

        /**
         * Starts a loop to allow edit / post inspection until the post succeeds or the edit is cancelled.
         * @param partInspection the inspection to edit
         * @returns A promise which resolves when the part is editing and posted and rejects when the edit is cancelled
         * @private
         */
        $scope._runAllowEditAndPostLoop = function (partInspection) {
            var whileClosure = { updateSucceeded: false };
            var i = 0;
            return qWhile(
                function () { return whileClosure.updateSucceeded; },
                function (r) {
                    return $scope.allowEditing(partInspection)
                        .catch(function (allowEditError) {
                            if (allowEditError !== $scope.CONSTANTS.ERROR_HANDLED) return $q.reject(allowEditError);
                            else return $q.resolve();
                        })
                        .then(function () {
                            return $scope.postInspectionUpdate(partInspection)
                                .then(function () {
                                    whileClosure.updateSucceeded = true;
                                    partInspection.updateFailed = false;    //POST success
                                })
                                .catch(function (postError) {
                                    partInspection.updateFailed = true;
                                    return $q.resolve();                    //swallow POST error and rerun the loop
                                });
                        });
                });
        };

        /**
         * Fill in for "missing" $q.while.  Creates a promise which continues to run the body until
         * the condition is satisfied. The promise resolves when the conidition is satisifed.
         *
         * This implementation is based on http://stackoverflow.com/a/17238793/794234
         *
         * @param A function which returns a boolean indicating if the loop is complete.  Gets the result of the previous iteration as an argument.
         * @param A function which returns a promise.  The q waits for this promise before checking the condiition and runs again if needed.  Gets the result of the previous iteration as an argument.
         */
        function qWhile(condition, body) {
            var whileDeferred = $q.defer();

            function runLoopIteration(previousIterationResult) {
                var conditionSatisfied = condition(previousIterationResult);
                if (conditionSatisfied) {
                    whileDeferred.resolve(previousIterationResult);
                } else {
                    $q.when(body(previousIterationResult))
                        .then(function (iterationResult) { runLoopIteration(iterationResult); })
                        .catch(function (error) { whileDeferred.reject(error); });
                }
            }
            runLoopIteration();

            return whileDeferred.promise;
        }

        $scope._shouldSuggestAssemblySplit = function (partInspection, markAs) {
            return markAs === "Replace" && partInspection.ReasonRepaired !== "ROUT/MAINT" && partInspection.CanBeRepresentedAsAssembly;
        };

        $scope._shouldAllowEditingBeforePostUpdate = function (partInspection) {
            return (partInspection.Result === "Replace" && partInspection.ReasonRepaired !== "ROUT/MAINT") ||
                        partInspection.Result === "Convert";
        };

        $scope._revertPartInspectionValues = function (partInspection) {
            if (typeof (partInspection._previousInspectionValues) === "undefined") return;

            $scope._copyPartInspectionValues(partInspection._previousInspectionValues, partInspection);

            delete partInspection._previousInspectionValues;
        };

        $scope._copyPartInspectionValues = function (from, to) {
            to.Result = from.Result;
            to.ReasonRepaired = from.ReasonRepaired;
            to.ReplacementQuantity = from.ReplacementQuantity;
            to.ReplacementPartTemplateNumber = from.ReplacementPartTemplateNumber;
            to.PartReplacedID = from.PartReplacedID;
        };

        $scope.postInspectionUpdate = function (partInspection) {
            partInspection.serverOperationInProgress = true;
            return $http({
                method: "POST",
                url: $scope.serverVars.updateUrl,
                responseType: "json",
                data: partInspection
            })
            .then(function (httpResponse) {
                partInspection.serverOperationInProgress = false;
                partInspection.ReplacedWithInventoryPartID = null;
                if (!$scope._handleInspectionPostErrors(httpResponse.data, partInspection)) {
                    return $q.reject($scope.CONSTANTS.ERROR_HANDLED);
                }
            });
        };

        /**
         * POSTs to /DT/RemoveInspection with the inspetion that was deleted.  Does not
         * update the $scope.inspections array.
         *
         * When the POST returns, removes all other inspections sent by the server in
         * the 'removed' array of the response.  This allows the server to remove other
         * related assemblies and still keep the grid up to date without requiring us
         * to dupilcate logic between the client and server.
         *
         * @param partInspection
         * @returns {*}
         */
        $scope.postInspectionDelete = function (partInspection) {
            partInspection.serverOperationInProgress = true;

            return $http({
                method: "POST",
                url: $scope.serverVars.removeUrl,
                responseType: "json",
                data: { PartInspectionID: partInspection.PartInspectionID }
            })
            .then(function (httpResponse) {
                partInspection.serverOperationInProgress = false;

                if (!$scope._handleInspectionPostErrors(httpResponse.data, partInspection)) {
                    $scope._revertPartInspectionValues(partInspection);
                    return $q.reject($scope.CONSTANTS.ERROR_HANDLED);
                } else if ("Changes" in httpResponse.data && "Removed" in httpResponse.data.Changes && httpResponse.data.Changes.Removed.length > 0) {
                    $scope._removeInspectionsFromDisplay(httpResponse.data.Changes.Removed);
                }
            });
        };

        $scope.alert = function (txt) {
            $window.alert(txt);
        };

        /**
         * Removes the specified inspections from the $scope.inspections array without POSTing
         * the removal to the server.  Should only be used in the removal a) came from the server
         * or b) was already POSTed somewhere else in the code.
         * @param inspection
         * @private
         */
        $scope._removeInspectionsFromDisplay = function (inspections) {
            for (var ixScopeInspections = 0; ixScopeInspections < $scope.inspections.length; ixScopeInspections++) {
                for (var ixRemovedInspections = 0; ixRemovedInspections < inspections.length; ixRemovedInspections++) {
                    if ($scope.inspections[ixScopeInspections].PartInspectionID === inspections[ixRemovedInspections].PartInspectionID) {
                        $scope.inspections.splice(ixScopeInspections, 1);
                        ixScopeInspections--;

                        inspections.splice(ixRemovedInspections, 1);
                        ixRemovedInspections = inspections.length; // continue the outer for loop
                    }
                }
            }
        };

        $scope._handleInspectionPostErrors = function (responseData, partInspection) {
            if ("Errors" in responseData && responseData.Errors !== null) {
                var msg = "Could not save " + partInspection.OriginalPartTemplateNumber + "\n";

                for (var propertyName in responseData.Errors) {
                    var propertyErrorList = responseData.Errors[propertyName].errors;

                    for (var i = 0; i < propertyErrorList.length; i++) {
                        msg += "\t- " + propertyName + ": " + propertyErrorList[i] + "\n";
                    }
                }

                $scope.alert(msg);

                return false;
            } else {
                return true;
            }
        };

        $scope._setInspectionValuesByMark = function (partInspection, markAs) {
            partInspection._previousInspectionValues = {
                Result: partInspection.Result,
                ReasonRepaired: partInspection.ReasonRepaired,
                ReplacementQuantity: partInspection.ReplacementQuantity,
                ReplacementPartTemplateNumber: partInspection.ReplacementPartTemplateNumber,
                PartReplacedID: partInspection.PartReplacedID
            };

            switch (markAs) {
                case "OK":
                    partInspection.Result = "OK";
                    partInspection.ReasonRepaired = null;
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.ReplacementQuantity = null;
                    partInspection.PartReplacedID = null;
                    partInspection.ReplacedWithInventoryPartID = null;
                    break;

                case "NA":
                    partInspection.Result = "N/A";
                    partInspection.ReasonRepaired = "Did not inspect";
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.ReplacementQuantity = null;
                    partInspection.PartReplacedID = null;
                    partInspection.ReplacedWithInventoryPartID = null;
                    break;

                case "Maintenance":
                    partInspection.Result = "Replace";
                    partInspection.ReasonRepaired = "ROUT/MAINT";
                    partInspection.ReplacementPartTemplateNumber = partInspection.OriginalPartTemplateNumber;
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.PartReplacedID = partInspection.OriginalPartTemplateID;
                    break;

                case "Convert":
                    partInspection.Result = "Convert";
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.ReplacementPartTemplateNumber = "";
                    partInspection.PartReplacedID = null;
                    break;

                case "Replace":
                    partInspection.Result = "Replace";
                    partInspection.ReasonRepaired = null;
                    partInspection.ReplacementPartTemplateNumber = partInspection.OriginalPartTemplateNumber;
                    partInspection.ReplacementQuantity = partInspection.Quantity;
                    partInspection.PartReplacedID = partInspection.OriginalPartTemplateID;
                    break;

                default:
                    throw new Error("Unknown mark: " + markAs);
            }
        };

        $scope.unsplitAssembly = function (partInspection) {
            partInspection.IsSplitAssembly = false;
            partInspection.serverOperationInProgress = true;

            var postsToResolve = [];
            for (var i = 0; i < $scope.inspections.length; i++) {
                var item = $scope.inspections[i];
                if (item.ParentAssemblyID === partInspection.PartInspectionID) {
                    var deletePromise = $scope.deleteInspection(item, i, true);
                    postsToResolve.push(deletePromise);
                }
            }

            var updatePromise = $scope.postInspectionUpdate(partInspection);
            postsToResolve.push(updatePromise);

            var usnplitPromise = $q
                .all(postsToResolve)
                .then(function () {
                    partInspection.serverOperationInProgress = false;
                });
            return usnplitPromise;
        };

        /**
         * Deletes specified inspection and posts the delete to the server.
         * @param partInspection to delete
         * @param ix index in the $scope.inspections array.  If index is supplied partInspection is ignored
         * @param doNotConfirm true skip the confirmation dialog
         * @returns {*}
         */
        $scope.deleteInspection = function (partInspection, ix, doNotConfirm) {
            if (doNotConfirm !== true) if (!$window.confirm("Are you sure you want to delete this record?")) return $q.reject("cancelled");
            if (typeof (ix) === "undefined" || ix === null) {
                ix = $scope._getInpsectionIndex(partInspection);
            }
            partInspection = $scope.inspections[ix];

            return $scope.postInspectionDelete(partInspection)
                .then(function () {
                    if ($scope.inspections[ix] !== partInspection) ix = $scope._getInpsectionIndex(partInspection);
                    $scope.inspections.splice(ix, 1);
                });
        };

        $scope._getInpsectionIndex = function (inspection) {
            for (var i = 0; i < $scope.inspections.length; i++) {
                if ($scope.inspections[i].PartInspectionID === inspection.PartInspectionID) {
                    return i;
                }
            }

            throw new Error("Could not find matching inspection");
        };

        $scope.suggestReplaceAssembly = function (partInspection) {
            var deferred = $q.defer();

            var modal = modalService.getModal("suggest-replace-assembly");
            modal.open();
            modal.then(function (reasonResolved) {
                if (reasonResolved === "ok") { //whole assembly
                    deferred.resolve();

                } else if (reasonResolved === "cancel") { //some parts
                    deferred.reject($scope.CONSTANTS.ERROR_HANDLED);
                    $scope._setInspectionValuesByMark(partInspection, "OK");
                    partInspection.IsSplitAssembly = true;

                    $scope.splitAssembly(partInspection);
                }
            }, function (reasonRejected) {
                deferred.reject(reasonRejected);
            });

            return deferred.promise;
        };

        $scope.splitAssembly = function (inspection) {
            inspection.serverOperationInProgress = true;

            var splitAssemblyHttpPromise = $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.splitAssmUrl,
                data: { partInspectionId: inspection.PartInspectionID }
            })
            .then(function (httpResponse) {
                $scope._updateInspectionSortOrdersAndAddNew(httpResponse.data.Data);
                inspection.serverOperationInProgress = false;
            });

            return splitAssemblyHttpPromise;
        };

        $scope._updateInspectionSortOrdersAndAddNew = function (updatedInspections) {
            var partInspectionIdComparer = function (a, b) { return (a.PartInspectionID > b.PartInspectionID) ? 1 : -1; };
            var inspectionsClone = $scope.inspections.slice();
            updatedInspections = updatedInspections.sort(partInspectionIdComparer);
            inspectionsClone = inspectionsClone.sort(partInspectionIdComparer);

            for (var localIx = 0, updateIx = 0; updateIx < updatedInspections.length; updateIx++) {
                var localInspection = inspectionsClone[localIx];
                var updatedInspection = updatedInspections[updateIx];

                if (localIx >= inspectionsClone.length || inspectionsClone[localIx].PartInspectionID !== updatedInspections[updateIx].PartInspectionID) {
                    $scope.inspections.push(updatedInspection);

                } else {
                    localInspection.SortOrder = updatedInspection.SortOrder;
                    localIx++;
                }
            }
        };

        $scope.syncInspectionOrder = function () {
            $http({
                url: $scope.serverVars.syncInspectionOrderUrl,
                method: "POST",
                data: { id: $scope.serverVars.deliveryTicketID }
            })
            .then(function (response) {
                $scope.inspections = response.data;
            });
        };

        $scope.allowEditing = function (partInspection) {
            var editDeferred = $q.defer();
            editDeferred.promise.finally(function () {
                partInspection.editReplacementPartTemplateNumber = false;
                partInspection.inEditMode = false;

                delete partInspection._editDeferred;
            });

            partInspection.inEditMode = true;
            if (partInspection.Result === "Convert") partInspection.editReplacementPartTemplateNumber = true;
            partInspection.showInventoryButton = $scope.showInventoryButton(partInspection);


            partInspection._editDeferred = editDeferred;
            return editDeferred.promise;
        };

        $scope.cancelEdit = function (partInspection) {
            $scope._verifyInEditMode(partInspection);

            partInspection._editDeferred.reject("cancel");

            $scope._revertPartInspectionValues(partInspection);
        };

        $scope.saveEdit = function (partInspection) {
            $scope._verifyInEditMode(partInspection);
            $scope._prepareForSave(partInspection);

            partInspection._editDeferred.resolve();
        };

        $scope._prepareForSave = function (partInspection) {
            if (partInspection.editReplacementPartTemplateNumber && "ReplacementPartInfo" in partInspection) {
                partInspection.ReplacementPartTemplateNumber = partInspection.ReplacementPartInfo.PartTemplateNumber;
                partInspection.PartReplacedID = partInspection.ReplacementPartInfo.PartTemplateID;
            }
        };

        $scope._verifyInEditMode = function (partInspection) {
            if (!partInspection.inEditMode) throw new Error("not currently in edit mode!");
            if (typeof (partInspection._editDeferred) === "undefined") throw new Error("missing deferred could not continue");
        };

        $scope.searchTextChanged = function (text) {
            var textChangedDeferred = $q.defer();

            $http({
                url: $scope.serverVars.partsByNumberReadUrl,
                method: "POST",
                data: { text: text }
            })
            .then(function (response) {
                textChangedDeferred.resolve(response.data);
            });

            return textChangedDeferred.promise;
        };

        $scope.switchRepairMode = function () {
            return util.confirm("CAUTION: Switching to tear down will delete all inspections for this ticket. Are you sure you want to continue?")
                .then(function () {
                    return $http({
                        method: "POST",
                        responseType: "json",
                        url: $scope.serverVars.switchRepairModeUrl,
                        data: { id: $scope.serverVars.deliveryTicketID }
                    })
                        .then(function (httpResponse) {
                            if (httpResponse.data.Success) {
                                $window.location.assign(httpResponse.data.RedirectUrl);
                            } else {
                                return $q.reject(httpResponse.data.Errors);
                            }
                        });
                })
                .catch(function (errors) {
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '/n';
                    }

                    $window.alert(message);
                });
        };

        $scope.usePartFromInventory = function (inspection) {
            var url = $scope.serverVars.updateUsingPartFromInventoryUrl;
            return inventoryService.usePartFromInventory(url, inspection)
                .then(function (response) {
                    inspection.ReplacedWithInventoryPartID = response.replacedWithInventoryPartID;
                    inspection.AvailableInInventory = response.availableInInventory;
                })
                .catch(function (errors) {
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '\n';
                    }
                    $window.alert(message);
                    return $scope.checkAvailabilityInInventory(inspection);
                });
        };

        $scope._shouldAllowUsingInventory = function (inspection) {
            return (inspection.Result === "Replace" || inspection.Result === "Convert");
        };

        $scope.showInventoryButton = function (inspection) {
            var show = $scope.serverVars.canModifyInventory && !inspection.CanBeRepresentedAsAssembly && !inspection.HasParentAssembly && !inspection.inEditMode && (!!inspection.ReplacedWithInventoryPartID || ($scope._shouldAllowUsingInventory(inspection) && inspection.AvailableInInventory && parseInt(inspection.ReplacementQuantity) === 1));
            return show;
        };

        $scope.checkInventoryList = function () {
            var url = $scope.serverVars.checkInventoryListUrl;
            var customerId = $scope.serverVars.customerID;
            return inventoryService.checkAvailabilityForInspections(url, customerId, $scope.inspections)
                .then(function (response) {
                    for (var i = 0; i < $scope.inspections.length; i++) {
                        var currentInspection = $scope.inspections[i];
                        for (var j = 0; j < response.length; j++) {
                            if (response[j].PartTemplateID === currentInspection.PartReplacedID) {
                                currentInspection.AvailableInInventory = true;
                                j = response.length;
                            }
                        }
                        currentInspection.showInventoryButton = $scope.showInventoryButton(currentInspection);
                    }
                });
        };

        $scope.loadInspections = function (id) {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.readUrl,
                data: { id: id }
            })
                .then(function (response) {
                    $scope.inspections = response.data.Data;
                });
        };

        $scope.loadReasonsRepaired = function () {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.reasonRepairedListUrl
            })
            .then(function (response) {
                $scope.reasonsRepaired = response.data;
            });
        };

        $scope.loadPumpFailedTemplates = function () {
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.listTemplatesUrl
            })
            .then(function (response) {
                $scope.templates = response.data;

                if ($scope.serverVars.pumpTemplateID) {
                    for (var i = 0; i < $scope.templates.length; i++) {
                        if ($scope.serverVars.pumpTemplateID === $scope.templates[i].PumpTemplateId) {
                            $scope.selectedTemplate = $scope.templates[i];
                            i = $scope.templates.length;
                        }
                    }
                }
            });
        };

        $scope.loadTemplates = function () {
            if ($scope.templatesLoaded) return $q.resolve();
            return $http({
                method: "POST",
                responseType: "json",
                url: $scope.serverVars.listTemplatesUrl
            })
                .then(function (response) {
                    $scope.templates = response.data;

                    if ($scope.serverVars.pumpTemplateID) {
                        for (var i = 0; i < $scope.templates.length; i++) {
                            if ($scope.serverVars.pumpTemplateID === $scope.templates[i].PumpTemplateId) {
                                $scope.selectedTemplate = $scope.templates[i];
                                i = $scope.templates.length;
                            }
                        }
                    }
                    $scope.templatesLoaded = true;
                });
        };

        $scope.showUpdateTemplateSelect = function () {
            $scope.templatesLoading = true;
            $scope.loadTemplates()
                .then(function () {
                    $scope.updateTemplateSelectVisible = true;
                })
                .catch(function (errors) {
                    var message = '';
                    for (var i = 0; i < errors.length; i++) {
                        message = errors[i].ErrorMessage + '\n';
                    }
                    $window.alert(message);
                })
                .finally(function () {
                    $scope.templatesLoading = false;
                });
        };

        $scope.updateTemplate = function () {
            if ($scope.selectedTemplate.PumpTemplateId !== $scope.serverVars.pumpTemplateID) {
                return util.confirm("WARNING: Changing this template number will erase the parts shown below and replace everything with new parts.  Are you sure you want to change the template number?")
                    .then(function () {
                        return $http({
                            method: "POST",
                            responseType: "json",
                            url: $scope.serverVars.updateTemplateUrl,
                            data: {
                                deliveryTicketId: $scope.serverVars.deliveryTicketID,
                                newTemplateId: $scope.selectedTemplate.PumpTemplateId
                            }
                        })
                            .then(function (httpResponse) {
                                if (httpResponse.data.Success) {
                                    $scope.serverVars.pumpTemplateID = $scope.selectedTemplate.PumpTemplateId;
                                    $scope.serverVars.pumpFailedSpecSummary = httpResponse.data.PumpFailedConciseSpecSummary;
                                    $scope.updateTemplateSelectVisible = false;

                                    return $scope.refreshInspections();

                                } else {
                                    return $q.reject(httpResponse.data.Errors);
                                }
                            });
                    })
                    .catch(function (errors) {
                        var message = '';
                        for (var i = 0; i < errors.length; i++) {
                            message = errors[i].ErrorMessage + '\n';
                        }
                        $window.alert(message);
                    });
            }
        };

        $scope.loadOriginalPartCustomerOwned = function (pumpFailedId, customerId, currentTicketDate) {
            var url = $scope.serverVars.listPartsInUseForPumpUrl;
            return inventoryService.loadOriginalPartsCustomerOwned(url, pumpFailedId, customerId, currentTicketDate)
                .then(function (response) {
                    for (var i = 0; i < $scope.inspections.length; i++) {
                        var currentInspection = $scope.inspections[i];
                        for (var j = 0; j < response.data.length; j++) {
                            if (response.data[j].TemplatePartDefID === currentInspection.TemplatePartDefID) {
                                if (response.data[j].ReplacedWithInventoryPartID !== null) {
                                    currentInspection.OriginalCustomerOwnedPartID = response.data[j].ReplacedWithInventoryPartID;
                                }
                                j = response.data.length;
                            }
                        }
                    }
                });
        };

        $scope.refreshInspections = function () {
            return $scope.loadInspections($scope.serverVars.deliveryTicketID)
                 .then(function () {
                     if ($scope.serverVars.customerUsesInventory && $scope.serverVars.canModifyInventory) {
                         return $scope.checkInventoryList();
                     }
                 })
                 .then(function () {
                     if ($scope.serverVars.customerUsesInventory) {
                         return $scope.loadOriginalPartCustomerOwned(
                             $scope.serverVars.pumpFailedID,
                             $scope.serverVars.customerID,
                             $scope.serverVars.currentTicketDate);
                     }
                 });
        };

        (function init() {
            $timeout(function () {

                $scope.refreshInspections();

                if ($scope.serverVars.reasonRepairedListUrl) {
                    $scope.loadReasonsRepaired();
                }
            }, 0, false);

        }());
    }
})();