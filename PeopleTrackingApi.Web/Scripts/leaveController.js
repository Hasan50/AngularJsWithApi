

(function () {
    var leaveFactory = function ($http) {

        var leaveApproveNotification = function (id) {
            return $http.post('/PushNotification/LeaveApproveNotification',
                { applicantId: id })
                .then(function (response) {
                    return response;
                });
        };
        var leaveRejectNotification = function (id) {
            return $http.post('/PushNotification/LeaveRejectNotification',
                { applicantId: id })
                .then(function (response) {
                    return response;
                });
        };
        var aprovedData = function (id) {
            return $http.post('/Leave/Approved',
                { id: id })
                .then(function (response) {
                    return response;
                });
        };
        var rejectedData = function (id) {
            return $http.post('/Leave/Rejected',
                { id: id })
                .then(function (response) {
                    return response;
                });
        };


        var getData = function (id) {
            return $http.get('/Leave/GetLeaveDetails?id=' + id,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAll = function (fromDate, toDate, userName) {
            return $http.get('/Leave/GetAll?fromDate=' + fromDate + "&toDate=" + toDate + "&userName=" + userName,
                {})
                .then(function (response) {
                    return response;
                });
        };
        return {
            GetAll: getAll,
            AprovedData: aprovedData,
            RejectedData: rejectedData,
            GetData: getData,
            LeaveApproveNotification: leaveApproveNotification,
            LeaveRejectNotification: leaveRejectNotification
        };
    };

    trackerApp.factory('leaveFactory', leaveFactory);

})();

trackerApp.controller("leaveController", ['$scope', '$uibModal', 'leaveFactory', 'languageFactory', function ($scope, $uibModal, leaveFactory, languageFactory) {
    $scope.pageSize = 10;
    $scope.DataList = [];
    $scope.GetDataList = function () {
        leaveFactory.GetAll($scope.filterOb.fromDate, $scope.filterOb.toDate, $scope.filterOb.userName).then(function (response) {
            $scope.DataList = response.data;
        });
    }
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.GetLanguageData();
    $scope.AddOrEdit = function (id) {
        $scope.BusinessModel = {};
        $scope.BusinessModel.StatusId = "";
        if (id > 0) {
            leaveFactory.GetData(id).then(function (response) {
                $scope.BusinessModel = response.data;
                if (response.data.IsApproved) {
                    $scope.BusinessModel.StatusId = "1";
                }
                else if (response.data.IsRejected) {
                    $scope.BusinessModel.StatusId = "0";
                } 
            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Leave/Create',
            backdrop: 'static',
            scope: $scope,
            size: 'md'
        });
    };

    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };

    $scope.resetForm = function () {
        $scope.filterOb = {
            fromDate: "",
            toDate: "",
            userName: "",
        }
        $scope.GetDataList();
    }
    $scope.filterOb = {
        fromDate: "",
        toDate: "",
        userName: "",
    }

    $scope.ExportToExcel = function () {

        window.location = "/Leave/ExportToExcel?fromDate=" + $scope.filterOb.fromDate + "&toDate=" + $scope.filterOb.toDate + "&userName=" + $scope.filterOb.userName;
    };

    $scope.deleteData = function (id) {
        $.confirm({
            title: 'Confirmation required',
            content: 'Do you really want to delete?',
            buttons: {
                ok: {
                    action: function () {
                        leaveFactory.DeleteData(id).then(function (response) {
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
        $scope.IsLoading = true;
        if ($scope.BusinessModel.StatusId == "1") {
            leaveFactory.AprovedData($scope.BusinessModel.Id).then(function (response) {
                if (response.data.Success) {
                    $scope.sendAprovePushNotification($scope.BusinessModel.UserId);
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
        if ($scope.BusinessModel.StatusId == "0"){
            leaveFactory.RejectedData($scope.BusinessModel.Id).then(function (response) {
                if (response.data.Success) {
                    $scope.sendRejectPushNotification($scope.BusinessModel.UserId);
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
    $scope.sendAprovePushNotification = function (applicantId) {
        leaveFactory.LeaveApproveNotification(applicantId).then(function (response) {
            if (response.data.Success) {
                console.log(response);
            }
        });
    }
    $scope.sendRejectPushNotification = function (applicantId) {
        leaveFactory.LeaveRejectNotification(applicantId).then(function (response) {
            if (response.data.Success) {
                console.log(response);
            }
        });
    }
    $scope.DisplaySickTypes = function () {
        $scope.modalInstanceSickType = $uibModal.open({
            templateUrl: '/SickType/Index',
            backdrop: 'static',
            scope: $scope,
            controller:"sickTypeController",
            size: 'md'
        });
    }
    $scope.closeModalSickType = function () {
        $scope.modalInstanceSickType.close();
    };

}]);

