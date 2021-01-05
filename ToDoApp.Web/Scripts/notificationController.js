

(function () {
    var notifyFactory = function ($http) {

        var getTopNotifications = function () {
            return $http.get('/Home/GetTopNotifications',
                {})
                .then(function (response) {
                    return response;
                });
        };
        var getAllNotification = function () {
            return $http.get('/Home/GetAllNotification',
                {})
                .then(function (response) {
                    return response;
                });
        };

        return {
            GetTopNotifications: getTopNotifications,
            GetAllNotification: getAllNotification,
        };
    };

    trackerApp.factory('notifyFactory', notifyFactory);

})();

trackerApp.controller("notificationController", ['$scope', '$uibModal', 'notifyFactory', function ($scope, $uibModal, notifyFactory) {

    $scope.topNotificationList = [];
    $scope.GetTopNotifications = function () {
        notifyFactory.GetTopNotifications().then(function (response) {
            $scope.topNotificationList = response.data;
        });
    }

}]);

