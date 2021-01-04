

(function () {
    var myClientsFactory = function ($http) {

        var saveData = function (model) {
            return $http.post('/MyClients/Save',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };

        var getMyClients = function () {
            return $http.get('/MyClients/GetCompanyList',
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getData = function (id) {
            return $http.get('/MyClients/Get?id='+id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var deleteData = function (id) {
            return $http.get('/MyClients/Delete?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAll = function () {
            return $http.get('/MyClients/GetAll',
                {})
                .then(function (response) {
                    return response;
                });
        };
        return {
            GetAll: getAll,
            SaveData: saveData,
            GetData: getData,
            GetMyClients: getMyClients,
            DeleteData: deleteData

        };
    };

    trackerApp.factory('myClientsFactory', myClientsFactory);

})();

trackerApp.controller("myClientsController", ['$scope', '$uibModal', 'myClientsFactory', function ($scope, $uibModal, myClientsFactory) {
    $scope.pageSize = 10;
    $scope.ErrorList = [];
    var validateForm = function (group) {
        $scope.ErrorList = [];
        $("." + group).removeClass("invalid-field");
        var notFound = true;
        $("." + group).each(function () {
            if ($(this).hasClass("ng-invalid")) {
                $(this).addClass("invalid-field");
                var message = $(this).parent().prev("label").clone().children().remove().end().text();
                $scope.ErrorList.push({ Message: message + " is required." });
                notFound = false;
            }
        });
        return notFound;
    };
    //
    $scope.options = {
        types: ['(cities)'],
        componentRestrictions: { country: 'FR' }
    };
    $scope.address = {
        name: '',
        place: '',
        components: {
            placeId: '',
            streetNumber: '',
            street: '',
            city: '',
            state: '',
            countryCode: '',
            country: '',
            postCode: '',
            district: '',
            location: {
                lat: '',
                long: ''
            }
        }
    };


    //
    $scope.DataList = [];
    $scope.GetDataList = function () {
        myClientsFactory.GetAll().then(function (response) {
            $scope.DataList = response.data;
        });
    }

    $scope.AddOrEdit = function (id) {
        $scope.BusinessModel = { Id: 0, IsActive:true};

        if (id > 0) {
            myClientsFactory.GetData(id).then(function (response) {
                $scope.BusinessModel = response.data;
                $scope.address.name = response.data.Address;

            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/MyClients/ClientCreate',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };

    $scope.ClientsExportToExcel = function () {
        window.location = "/MyClients/ClientsExportToExcel";
    };

    $scope.deleteData = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        myClientsFactory.DeleteData(id).then(function (response) {
                            if (response.data.Success) {
                                showMessage(response.data.Message, "success");
                                $scope.GetDataList();
                            } else {
                                showMessage(response.data.Message, "error");
                            }
                        });
                    }
                },
                cancel: function () {
                    // nothing to do
                }
            }
        });
    };

    $scope.IsLoading = false;

    $scope.Save = function () {
        if (validateForm("validator")) {
            var updateObject = { ...$scope.BusinessModel };
            updateObject.Address = $scope.address.name;
            myClientsFactory.SaveData(updateObject).then(function (response) {
                if (response.data.Success) {
    $scope.IsLoading = true;
                    showMessage(response.data.Message, "success");
                    $scope.closeModal();
                    $scope.BusinessModel = {};
                    $scope.GetDataList();
                }
                else {
    $scope.IsLoading = false;
                    showMessage(response.data.Message, "error");
                }
            });
        }
    };


}]);

