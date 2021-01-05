

(function () {
    var userFactory = function ($http) {
        
        var updateData = function (model) {
            return $http.post('/Account/UpdateUser',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var changePassword = function (model) {
            return $http.post('/Account/ChangePassword',
                { json: model })
                .then(function (response) {
                    return response;
                });
        };
        var getUserClaim = function () {
            return $http.get('/Account/GetUserClaims',
                {})
                .then(function (response) {
                    return response;
                });
        };
       
        return {
            UpdateData: updateData,
            GetUserClaim: getUserClaim,
            ChangePassword: changePassword
        };
    };

    trackerApp.factory('userFactory', userFactory);

})();

trackerApp.controller("userController", ['$scope', '$uibModal', 'userFactory', function ($scope, $uibModal, userFactory) {

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
    $scope.BusinessModel = {};
    $scope.GetDataList = function () {
        userFactory.GetUserClaim().then(function (response) {
            $scope.BusinessModel = response.data;
        });
    }
    $scope.GetDataList();


    $scope.Update = function () {
        if (validateForm("validator")) {
            var updateObject = { ...$scope.BusinessModel };
            userFactory.UpdateData(updateObject).then(function (response) {
                if (response.data.Success) {
                    showMessage(response.data.Message, "success");
                    $scope.GetDataList();
                }
                else {
                    showMessage(response.data.Message, "error");
                };
            });
        }
    };
    $scope.ChangePassword = function () {
        if (validateForm("validator")) {
            if ($scope.ChangePassModel.NewPassword != $scope.ChangePassModel.ConfirmPassword)
                return showMessage("New Password and Confirm password does not match", "error");
            var updateObject = { ...$scope.ChangePassModel, UserName: $scope.BusinessModel.UserName };
            userFactory.ChangePassword(updateObject).then(function (response) {
                if (response.data.Success) {
                    showMessage(response.data.Message, "success");
                    $scope.ChangePassModel = {};
                }
                else {
                    showMessage(response.data.Message, "error");
                }
            });
        };
    };
}]);

