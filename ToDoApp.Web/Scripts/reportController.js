

(function () {
    var reportFactory = function ($http) {

       
        var getData = function (userId, fromDate,toDate) {
            return $http.get('/Report/GetMonthlyAttendanceDetailswithDate?userId=' + userId + '&fromDate=' + fromDate+'&toDate='+toDate,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAgentDetailsData = function (agentId, fromDate, toDate) {
            return $http.get('/Report/GetAgentMonthlyAttendanceDetailswithDate?agentId=' + agentId + '&fromDate=' + fromDate + '&toDate=' + toDate,
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
        var getCompanyAgentList = function () {
            return $http.get('/CompanyAgent/GetCompanyAgentList',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAll = function (fromDate, toDate, companyAgentId) {
            return $http.get('/Report/GetAll?fromDate=' + fromDate + "&toDate=" + toDate + "&companyAgentId=" + companyAgentId,
                {})
                .then(function (response) {
                    return response;
                });
        };
        return {
            GetAll: getAll,
            GetData: getData,
            GetAgentDetailsData: getAgentDetailsData,
            DeleteData: deleteData,
            GetCompanyAgentList: getCompanyAgentList

        };
    };

    trackerApp.factory('reportFactory', reportFactory);

})();

trackerApp.controller("reportController", ['$scope', '$uibModal', 'reportFactory', 'languageFactory', function ($scope, $uibModal, reportFactory, languageFactory) {
    $scope.pageSize = 10;
    $scope.activeTab = 1;
    $scope.DataList = [];
    $scope.CompanyWiseWorkingList = [];
    $scope.GetDataList = function () {
        reportFactory.GetAll($scope.filterOb.fromDate, $scope.filterOb.toDate, $scope.filterOb.companyAgentId).then(function (response) {
            $scope.DataList = response.data.EmployeeList;
            $scope.CompanyWiseWorkingList = response.data.AgentWiseCount;
        });
    }
    $scope.languageOb = {};
    $scope.GetLanguageData = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            $scope.languageOb = languageFactory.GetLanguageValue(response.data.UserLanguage);
        });
    }
    $scope.userId = null;
    $scope.GetLanguageData();
    $scope.ViewDetails = function (id) {
        $scope.BusinessModels = [];

        if (id != null) {
            $scope.userId = id;
            reportFactory.GetData(id, $scope.detailsFilterOb.fromDate, $scope.detailsFilterOb.toDate).then(function (response) {
                $scope.BusinessModels = response.data;
                
            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Report/Create',
            backdrop: 'static',
            scope: $scope,
            size: 'lg'
        });
    };
    $scope.ViewCompanyWiseDetails = function (id) {
        $scope.detailsFilterOb = {
            fromDate: "",
            toDate: "",
        }
        $scope.AgetnDetailLists = [];

        if (id != null) {
            $scope.agentId = id;
            reportFactory.GetAgentDetailsData(id, $scope.detailsFilterOb.fromDate, $scope.detailsFilterOb.toDate).then(function (response) {
                $scope.AgetnDetailLists = response.data;

            });
        }

        $scope.modalInstance = $uibModal.open({
            templateUrl: '/Report/CompanyWiseDetails',
            backdrop: 'static',
            scope: $scope,
            size: 'lg'
        });
    };
    $scope.closeModal = function () {
        $scope.modalInstance.close();
    };
    $scope.filterDetailsList=function(){
        reportFactory.GetData($scope.userId , $scope.detailsFilterOb.fromDate, $scope.detailsFilterOb.toDate).then(function (response) {
            $scope.BusinessModels = response.data;

        });
    }
    $scope.filterAgentDetailsList = function () {
        reportFactory.GetAgentDetailsData($scope.agentId, $scope.detailsFilterOb.fromDate, $scope.detailsFilterOb.toDate).then(function (response) {
            $scope.AgetnDetailLists = response.data;

        });
    }
    $scope.resetForm = function () {
        $scope.filterOb = {
            fromDate: "",
            toDate: "",
            companyAgentId: null,
        }
        $scope.GetDataList();
    }
    $scope.filterOb = {
        fromDate: "",
        toDate: "",
        companyAgentId: null,
    }
    $scope.detailsFilterOb = {
        fromDate: "",
        toDate: "",
    }

    $scope.detailsResetForm = function () {
        $scope.detailsFilterOb = {
            fromDate: "",
            toDate: "",
        }
        $scope.filterDetailsList();
    }
    $scope.agentDetailsResetForm = function () {
        $scope.detailsFilterOb = {
            fromDate: "",
            toDate: "",
        }
        $scope.filterAgentDetailsList();
    }
    $scope.companyAgentList = [];

    $scope.getCompanyAgentList = function () {
        reportFactory.GetCompanyAgentList().then(function (result) {
            $scope.companyAgentList = result.data;
        });
    }
    $scope.getCompanyAgentList();
    $scope.EmployeesExportToExcel = function () {
        window.location = "/Report/ExportToExcel?fromDate=" + $scope.filterOb.fromDate + "&toDate=" + $scope.filterOb.toDate + "&companyAgentId=" + $scope.filterOb.companyAgentId + "&activeTab=" + $scope.activeTab;
    };
    $scope.AttendanceDetialsExportToExcel = function () {
        window.location = "/Report/ExportDetailsToExcel?userId=" + $scope.userId + "&fromDate=" + $scope.detailsFilterOb.fromDate + "&toDate=" + $scope.detailsFilterOb.toDate;
    };
    $scope.AgentDetialsExportToExcel = function () {
        window.location = "/Report/ExportAgentDetailsToExcel?agentId=" + $scope.agentId + "&fromDate=" + $scope.detailsFilterOb.fromDate + "&toDate=" + $scope.detailsFilterOb.toDate;
    };
    $scope.ViewDetailsByCompany = function (id) {

    }

}]);

