

(function () {
    var dashboardFactory = function ($http) {

        var getCompanyAgentList = function () {
            return $http.get('/CompanyAgent/GetCompanyAgentList',
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getAttendance = function (companyAgentId, date) {
            return $http.get('/Attendance/GetCompanyAgentAttendanceFeed?companyAgentId=' + companyAgentId + "&date=" + date,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getOfflineAttendance = function (companyAgentId, date) {
            return $http.get('/Attendance/GetCompanyAgentAttendanceOfflineFeed?companyAgentId=' + companyAgentId + "&date=" + date,
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getEmployeeMovementDetails = function (userId) {
            return $http.get('/Attendance/GetMovementDetails?userId=' + userId,
                {})
                .then(function (response) {
                    return response;
                });
        };

        var getLocationBarData = function (userId,date) {
            return $http.get('/Attendance/GetLocationBarData?userId=' + userId+'&date='+date,
                {})
                .then(function (response) {
                    return response;
                });
        };

        return {
            GetCompanyAgentList: getCompanyAgentList,
            GetAttendance: getAttendance,
            GetEmployeeMovementDetails: getEmployeeMovementDetails,
            GetLocationBarData: getLocationBarData,
            GetOfflineAttendance: getOfflineAttendance

        };
    };

    trackerApp.factory('dashboardFactory', dashboardFactory);

})();

trackerApp.controller("dashboardController", ['$scope', '$uibModal', 'dashboardFactory', 'languageFactory', 'RESOURCE_URL', function ($scope, $uibModal, dashboardFactory, languageFactory, RESOURCE_URL) {

   
}]);


