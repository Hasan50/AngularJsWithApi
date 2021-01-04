

(function () {
    var sickTypeFactory = function ($http) {

        var saveData = function (model) {
            return $http.post('/SickType/Save',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var getData = function (id) {
            return $http.get('/SickType/GetSickTypeDetails?id='+id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getAll = function (id) {
            return $http.get('/SickType/GetAll',
                {})
                .then(function (response) {
                    return response;
                });
        };

        var deleteData = function (id) {
            return $http.get('/SickType/Delete?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        return {
            GetAll: getAll,
            SaveData: saveData,
            GetData: getData,
            DeleteData: deleteData

        };
    };

    trackerApp.factory('sickTypeFactory', sickTypeFactory);

})();

trackerApp.controller("sickTypeController", ['$scope', '$uibModal', 'sickTypeFactory', 'languageFactory', function ($scope, $uibModal, sickTypeFactory, languageFactory) {
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
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.DataList = [];
    $scope.GetSickTypeDataList = function () {
        sickTypeFactory.GetAll().then(function (response) {
            $scope.DataListSickType = response.data;
        });
    }

    $scope.AddOrEditSickType = function (id) {
        $scope.BusinessModelSickType = {};

        if (id > 0) {
            sickTypeFactory.GetData(id).then(function (response) {
                $scope.BusinessModelSickType = response.data;
            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/SickType/Create',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };

    $scope.deleteDataSickType = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        sickTypeFactory.DeleteData(id).then(function (response) {
                            if (response.data.Success) {
                                showMessage(response.data.Message, "success");
                                $scope.GetSickTypeDataList();
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

    $scope.SaveSickType = function () {
        if (validateForm("validator")) {
            sickTypeFactory.SaveData($scope.BusinessModelSickType).then(function (response) {
                if (response.data.Success) {
    $scope.IsLoading = true;
                    showMessage(response.data.Message, "success");
                    $scope.closeModal();
                    $scope.BusinessModelSickType = {};
                    $scope.GetSickTypeDataList();
                }
                else {
    $scope.IsLoading = false;
                    showMessage(response.data.Message, "error");
                }
            });
        }
    };


}]);

