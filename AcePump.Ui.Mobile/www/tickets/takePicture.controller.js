(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("TakePictureController", TakePictureController);

    TakePictureController.$inject = ["$", "$cordovaCamera", "$cordovaTransfer", "$http", "$ionicHistory", "$ionicLoading", "$ionicPopup", "$q", "$scope", "$state", "$stateParams", "Upload", "ticketsRepository"];
    function TakePictureController($, $cordovaCamera, $cordovaTransfer, $http, $ionicHistory, $ionicLoading, $ionicPopup, $q, $scope, $state, $stateParams, Upload, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);

        vm.takePicture = takePicture;
        vm.savePicture = savePicture;

        init();

        function init() {
            takePicture();
        }

        function takePicture() {
            var options = {
                quality: 50,
                destinationType: Camera.DestinationType.DATA_URL,
                sourceType: Camera.PictureSourceType.CAMERA,
                encodingType: Camera.EncodingType.JPEG
            };

            $cordovaCamera.getPicture(options).then(function (imageData) {
                vm.imgData = imageData;
                vm.imgURI = "data:image/jpeg;base64," + imageData;                
                var dimensions = getDimensions(vm.imgURI);
            }, function (err) {
                $ionicPopup
                    .alert({
                        title: "Camera Failure",
                        template: err
                    })
                    .then(function () {
                        $ionicHistory.goBack();
                    });
            });
        }

        function getDimensions(imgUri) {
            var imgContainer = $("<div>")
                .css({
                    visibility: "hidden",
                    position: "absolute"
                })
                .append($("<img>").attr("src", imgUri));

            document.body.append(imgContainer);
            var img = imgContainer[0].querySelector("img");


            var dimensions = {
                width: img.clientWidth,
                height: img.clientHeight
            };

            imgContainer.remove();

            return dimensions;
        }

        function savePicture() {
            $ionicLoading.show({
                template: "Uploading image <progress max=\"100\" value=\"0\" id=\"progress_bar_con\"> </progress >"
            })
                .then(function () {
                    return Upload.upload({
                        url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images",
                        method: "PUT",
                        data: {
                            file: vm.imgData
                        }
                    })
                        .then(
                            function uploadSuccess(e) {
                                $ionicLoading.hide();
                                $ionicPopup
                                    .alert({
                                        title: "Upload Picture",
                                        template: "Upload was successful."
                                    })
                                    .then(function () {
                                        $ionicHistory.goBack();
                                    });
                            },
                            function uploadFail() {
                                $ionicLoading.hide();
                                $ionicPopup
                                    .alert({
                                        title: "Upload Picture",
                                        template: "Picture upload failed, please try again."
                                    })
                                    .then(function () {
                                        $ionicHistory.goBack();
                                    });
                            },
                            function updateProgress(e) {
                                var percentComplete = parseInt(100.0 * e.loaded / e.total);
                                document.getElementById("progress_bar_con").value = percentComplete;
                            });
                });
        }
    }
})();