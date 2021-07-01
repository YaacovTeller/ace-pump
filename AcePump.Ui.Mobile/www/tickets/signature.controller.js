(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("SignatureController", SignatureController);

    SignatureController.$inject = ["$http", "$ionicLoading", "$ionicHistory", "$q", "$rootScope", "$scope", "$stateParams", "ticketsRepository"];
    function SignatureController($http, $ionicLoading, $ionicHistory, $q, $rootScope, $scope, $stateParams, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);
        
        vm.clearForm = clearForm;
        vm.saveSignature = saveSignature;
        vm.resign = resign;
        vm.goBack = goBack;

        vm.signatureForm = {};
        vm.signaturePad = {};
        vm.lineItems = {};
        vm.signatureData = {};

        init();


        function init() {
            loadSignatureData();
            loadLineItems();
        }

        function clearForm() {
            vm.signatureForm.$setPristine();
            vm.signatureForm.$setUntouched();
        }

        function saveSignature() {
            $ionicLoading.show();

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/sign",
                method: "PATCH",
                data: vm.signatureData
            })
                .then(function (httpResponse) {
                    vm.currentTicket.HasSignature = true;
                    vm.clearForm();
                })
                .finally(function () {
                    $ionicLoading.hide();
                });

        }

        function resign() {
            vm.signatureData.Signature = null;
            vm.signatureData.SignatureName = "";
            vm.signatureData.SignatureCompanyName = "";
            vm.signatureData.SignatureDate = new Date();
            vm.currentTicket.HasSignature = false;
        }

        function loadSignatureData() {
            $ionicLoading.show();
            vm.signatureData = {};

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/signatureData",
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.signatureData = httpResponse.data;    
                    vm.currentTicket.HasSignature = !!vm.signatureData.Signature;  
                    if (!vm.currentTicket.HasSignature) {
                        vm.signatureData.SignatureDate = new Date();
                    }
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }    

        function loadLineItems() {
            $ionicLoading.show();
            vm.lineItems = {};

            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/lineItems",                
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.lineItems = httpResponse.data;

                    vm.signatureData.salesTotal = 0;
                    vm.signatureData.taxOwed = 0;

                    for (var i = 0; i < vm.lineItems.length; i++) {
                        vm.signatureData.salesTotal += vm.lineItems[i].LineTotal;
                        vm.signatureData.taxOwed += vm.lineItems[i].LineTotal * (vm.lineItems[i].LineIsTaxable ? vm.signatureData.SalesTaxRate : 0);                     
                    }

                    vm.signatureData.grandTotal = vm.signatureData.salesTotal + vm.signatureData.taxOwed;
                })
                .finally(function () {
                    $ionicLoading.hide();
                });
        }     

        function goBack() {
            $ionicHistory.backView().stateParams = { deliveryTicketId: vm.currentTicket.DeliveryTicketID };
            $rootScope.$ionicGoBack();
        }        
    }
})();