

(function () {
    var companyAgentFactory = function ($http) {

        var saveData = function (model) {
            return $http.post('/CompanyAgent/Save',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var updateData = function (model) {
            return $http.post('/CompanyAgent/Update',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };

        var getData = function (id) {
            return $http.get('/CompanyAgent/GetCompanyDetails?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var deleteData = function (id) {
            return $http.get('/CompanyAgent/Delete?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAll = function () {
            return $http.get('/CompanyAgent/GetAll',
                {})
                .then(function (response) {
                    return response;
                });
        };
        return {
            GetAll: getAll,
            SaveData: saveData,
            UpdateData: updateData,
            GetData: getData,
            DeleteData: deleteData

        };
    };

    trackerApp.factory('companyAgentFactory', companyAgentFactory);

})();

trackerApp.controller("companyAgentController", ['$scope', '$uibModal', 'companyAgentFactory', 'languageFactory', function ($scope, $uibModal, companyAgentFactory, languageFactory) {
    $scope.ErrorList = [];
    $scope.pageSize = 10;
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
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.DataList = [];
    $scope.GetDataList = function () {
        companyAgentFactory.GetAll().then(function (response) {
            $scope.DataList = response.data;
        });
    }

    $scope.AddOrEdit = function (id) {
        $scope.BusinessModel = { IsActive: true };

        if (id > 0) {
            companyAgentFactory.GetData(id).then(function (response) {
                $scope.BusinessModel = response.data;
                $scope.address.name = response.data.Address;
                document.getElementById("officeStartTime").value = response.data.OfficeStartTime;
                document.getElementById("officeEndTime").value = response.data.OfficeEndTime;
                $scope.address.components.location.lat = response.data.GeofanceLat;
                $scope.address.components.location.long = response.data.GeofanceLng;
            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/CompanyAgent/Create',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };


    $scope.EmployeesExportToExcel = function () {
        window.location = "/CompanyAgent/ExportToExcel";
    };
    $scope.ChangeStartTime = function (event) {
        console.log(event);
    }
    function onChangeEventHandler(event) {
        console.log(event);
    }
    $scope.deleteData = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        companyAgentFactory.DeleteData(id).then(function (response) {
                            if (response.data.Success) {
                                showMessage(response.data.Message, "success");
                                $scope.GetDataList();
                                $scope.$apply();
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
            $scope.IsLoading = true;
            var officeStartTimeValue = document.getElementById("officeStartTime").value;
            var officeEndTimeValue = document.getElementById("officeEndTime").value;
            var updateObject = { ...$scope.BusinessModel, OfficeStartTime: officeStartTimeValue, OfficeEndTime: officeEndTimeValue };
            updateObject.GeofanceLat = $scope.address.components.location.lat;
            updateObject.GeofanceLng = $scope.address.components.location.long;
            updateObject.Address = $scope.address.name;
            companyAgentFactory.SaveData(updateObject).then(function (response) {
                if (response.data.Success) {
                    showMessage(response.data.Message, "success");
                    $scope.closeModal();
                    $scope.BusinessModel = {};
                    $scope.GetDataList();
                }
                else {
                    showMessage(response.data.Message, "error");
                }
                $scope.IsLoading = false;
            });
        }
    };


}]);

