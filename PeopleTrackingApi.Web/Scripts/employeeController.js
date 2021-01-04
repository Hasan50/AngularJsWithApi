

(function () {
    var employeeFactory = function ($http) {

        var saveData = function (model, employeeModel) {
            return $http.post('/Employee/CreateEmployee',
                { json: model, employeeCompanyAgent: employeeModel })
                .then(function (response) {
                    return response;
                });
        };
        var updateData = function (model, employeeModel) {
            return $http.post('/Employee/Update',
                { json: model, employeeCompanyAgent: employeeModel })
                .then(function (response) {
                    return response;
                });
        };
        var getCompanyAgentList = function () {
            return $http.get('/CompanyAgent/GetCompanyAgentList',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getCompanyAgentWithCompanyList = function (companyId) {
            return $http.get('/CompanyAgent/GetCompanyAgentListWithCompany?CompanyId=' + companyId,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getData = function (id) {
            return $http.get('/Employee/Get?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var deleteData = function (id) {
            return $http.get('/Employee/DeleteEmployee?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getAll = function () {
            return $http.get('/Employee/GetAll',
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
            GetCompanyAgentList: getCompanyAgentList,
            GetCompanyAgentWithCompanyList: getCompanyAgentWithCompanyList,
            DeleteData: deleteData

        };
    };

    trackerApp.factory('employeeFactory', employeeFactory);

})();

trackerApp.controller("employeeController", ['$scope', '$uibModal', 'employeeFactory', 'languageFactory', function ($scope, $uibModal, employeeFactory, languageFactory) {

    $scope.pageSize = 10;

    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.IsSuperAdmin = false;
    $scope.DataList = [];
    $scope.GetDataList = function () {
        employeeFactory.GetAll().then(function (response) {
            $scope.DataList = response.data;
        });
    }
    $scope.AddOrEdit = function (id) {
        $scope.BusinessModel = { Id: id, IsActive: true };
        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Employee/EmployeeCreate',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };


    $scope.EmployeesExportToExcel = function () {
        window.location = "/Employee/EmployeeExportToExcel";
    };

    $scope.deleteData = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        employeeFactory.DeleteData(id).then(function (response) {
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
}]);
trackerApp.controller("employeeCreateController", ['$scope', '$uibModal', 'employeeFactory', 'languageFactory', function ($scope, $uibModal, employeeFactory, languageFactory) {
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
    $scope.initModel = function () {
        $scope.BusinessModel = { ...$scope.$parent.BusinessModel };
        if ($scope.BusinessModel.Id > 0) {
            employeeFactory.GetData($scope.BusinessModel.Id).then(function (response) {
                $scope.BusinessModel = response.data;
                if ($scope.IsSuperAdmin) {
                    $scope.getCompanyAgentList(0);
                }
                $scope.tagChange = response.data.CompanyAgentName;
                $scope.address.name = response.data.GeofanceLocation;
                $scope.address.components.location.lat = response.data.GeofanceLat;
                $scope.address.components.location.long = response.data.GeofanceLng;
                $scope.BusinessModel.UserFullName = response.data.UserName;
                $scope.BusinessModel.CompanyAgentId = response.data.CompanyAgentId.toString();
               
            });
        }
    }
    $scope.initModel();
    $scope.listAutocomplete = [];
    $scope.$watch("inputText", function (newVal, oldVal) {
        console.log("something is happening with input text:" + newVal);
    })

    $scope.$watch("tagChange", function (newVal, oldVal) {
        console.log("something is happening with tag change:" + newVal);
    })

    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.IsSuperAdmin = false;

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };
    $scope.companyAgentList = [];
    $scope.getCompanyAgentList = function (value) {
        $scope.listAutocomplete = [];
        $scope.IsSuperAdmin = value == 0 ? true : false;
        if ($scope.IsSuperAdmin) {
            employeeFactory.GetCompanyAgentWithCompanyList($scope.BusinessModel.CompanyId).then(function (result) {
                $scope.companyAgentList = result.data;
                $scope.companyAgentList.forEach(function (ob) {
                    $scope.listAutocomplete.push(ob.AgentName);
                });
            });
        } else {
            employeeFactory.GetCompanyAgentList().then(function (result) {
                $scope.companyAgentList = result.data;
                $scope.companyAgentList.forEach(function (ob) {
                    $scope.listAutocomplete.push(ob.AgentName);
                });
            });
        }

    }
    $scope.IsLoading = false;

    $scope.Save = function () {
        $scope.IsLoading = true;
        if (validateForm("validator")) {
            var updateObject = { ...$scope.BusinessModel };
            var tempTagList = $scope.tagChange.split(',').filter(function (tag) {
                return tag !== '';
            });
            var EmployeeCompanyAgentList = [];
            tempTagList.forEach(function (m) {
                $scope.companyAgentList.forEach(function (ob) {
                    if (m === ob.AgentName) {
                        EmployeeCompanyAgentList.push({
                            EmployeeUserId: $scope.BusinessModel.Id,
                            CompanyAgentId: ob.Id
                        })
                    }
                });
            })

            if (updateObject.Id == 0) {
                employeeFactory.SaveData(updateObject, EmployeeCompanyAgentList).then(function (response) {
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
            } else {
                employeeFactory.UpdateData(updateObject, EmployeeCompanyAgentList).then(function (response) {
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
        }
    };
    //$scope.Update = function () {
    //    var updateObject = { ...$scope.BusinessModel };
    //    var tempTagList = $scope.tagChange.split(',').filter(function (tag) {
    //        return tag !== '';
    //    });
    //    var EmployeeCompanyAgentList = [];
    //    tempTagList.forEach(function (m) {
    //        $scope.companyAgentList.forEach(function (ob) {
    //            if (m === ob.AgentName) {
    //                EmployeeCompanyAgentList.push({
    //                    EmployeeUserId: $scope.BusinessModel.Id,
    //                    CompanyAgentId: ob.Id
    //                })
    //            }
    //        });
    //    })
    //    employeeFactory.UpdateData(updateObject).then(function (response) {
    //        if (response.data.Success) {
    //            showMessage(response.data.Message, "success");
    //            $scope.closeModal();
    //            $scope.BusinessModel = {};
    //            $scope.GetDataList();
    //        }
    //        else {
    //            showMessage(response.data.Message, "error");
    //        }
    //    });
    //};

}]);

