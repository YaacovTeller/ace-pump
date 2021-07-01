(function () {
    "use strict";

    angular
        .module("app.tickets")
        .controller("DetailsController", DetailsController);

    DetailsController.$inject = ["$http", "$ionicLoading", "$ionicPopup", "$q", "$scope", "$stateParams", "ticketsRepository"];
    function DetailsController($http, $ionicLoading, $ionicPopup, $q, $scope, $stateParams, ticketsRepository) {
        var vm = this;

        vm.currentTicket = ticketsRepository.get($stateParams.deliveryTicketId);

        vm.deleteImage = deleteImage;
        vm.editNote = editNote;
        vm.getSmallImageSrc = getSmallImageSrc;
        vm.getLargeImageSrc = getLargeImageSrc;
        vm.getPdfHref = getPdfHref;

        init();

        function init() {
            getTicketPictures();
        }

        function getTicketPictures() {
            $ionicLoading.show();
            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images",
                method: "GET"
            })
                .then(function (httpResponse) {
                    vm.images = httpResponse.data;
                    $ionicLoading.hide();
                });

        }

        function getSmallImageSrc(image) {
            return "/api/deliverytickets/" + image.DeliveryTicketID + "/smallImages/" + image.DeliveryTicketImageUploadID;
        }

        function getLargeImageSrc(image) {
            return "/api/deliverytickets/" + image.DeliveryTicketID + "/largeImages/" + image.DeliveryTicketImageUploadID;
        }

        function deleteImage(image) {
            return $ionicPopup.confirm({
                title: "Delete Image",
                template: "Are you sure you want to delete this image?"
            })
                .then(function (result) {
                    $ionicLoading.show();
                    if (result) {
                        $http({
                            url: "/api/deliverytickets/" + image.DeliveryTicketID + "/images/" + image.DeliveryTicketImageUploadID,
                            method: "DELETE"
                        })
                            .then(function (httpResponse) {
                                var ix = vm.images.indexOf(image);
                                vm.images.splice(ix, 1);
                                $ionicLoading.hide();
                            });
                    } else {
                        $ionicLoading.hide();
                    }
                })
                .catch(function (err) {
                    $ionicLoading.hide();
                    $ionicPopup
                        .alert({
                            title: "Delete Failed",
                            template: err
                        });
                });
        }

        function editNote(image) {
            $scope.currentImage = image;
            $scope.data = {};
            

            return $ionicPopup.show({
                title: "Please enter a note for this image",
                template: "<input type='text' ng-model='data.newNote' ng-value='currentImage.Note'></input>",
                scope: $scope,
                buttons: [
                    { text: 'Cancel', onTap: function (e) { return false; } },
                    {
                        text: '<b>Save</b>',
                        type: 'button-positive',
                        onTap: function (e) {
                            return $scope.data.newNote || true;
                        }
                    }
                ]
            })
                .then(function (result) {
                    if (result) {
                        if (image.Note !== $scope.data.newNote) {
                            return updateImageNote(image, $scope.data.newNote);
                        }
                    }
                })
                .catch(function (err) {
                    $ionicPopup.alert("Could not change note for this image. " + err);
                });
        }

        function updateImageNote(image, newNote) {
            $ionicLoading.show();
            return $http({
                url: "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/images/" + image.DeliveryTicketImageUploadID + "/Note",
                method: "PATCH",
                params: {
                    note: newNote
                }
            })
                .then(function (httpResponse) {
                    $ionicLoading.hide();
                    image.Note = httpResponse.data.note;
                });
        }

       function getPdfHref() {
            return "/api/deliverytickets/" + vm.currentTicket.DeliveryTicketID + "/pdf";
        }
    }
})();