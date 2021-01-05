

(function () {
    var menuFactory = function ($http) {

        var saveData = function (model) {
            return $http.post('/MyClients/Save',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };

        var getMyCompany = function () {
            return $http.get('/MyClients/GetUserCompany',
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
            GetMyCompany: getMyCompany,
            DeleteData: deleteData

        };
    };

    trackerApp.factory('menuFactory', menuFactory);

})();

trackerApp.controller("menuController", ['$scope', '$uibModal', '$location', 'menuFactory', function ($scope, $uibModal, $location, menuFactory) {
   // $scope.activeLink = window.location.pathname.split("/")[1]; 

    $scope.activeLink = 'Home';


    $scope.myCompany = {};
    $scope.GetDataList = function () {
        menuFactory.GetMyCompany().then(function (response) {
            $scope.myCompany = response.data;
        });
    }
    $scope.GetDataList();

}]);

