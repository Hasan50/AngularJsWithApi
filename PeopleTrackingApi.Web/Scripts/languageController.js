



trackerApp.controller("languageController", ['$scope','$rootScope', '$uibModal', 'languageFactory', function ($scope,$rootScope, $uibModal, languageFactory) {



    $scope.selectedLanguageSrc ="https://preview.keenthemes.com/metronic/theme/html/demo13/dist/assets/media/svg/flags/195-france.svg"
    $scope.SetLanguage = function (lang) {
        languageFactory.SetUserLanguage(lang).then(function (response) {
            if (response.data == "en") {
                $scope.selectedLanguageSrc = "https://preview.keenthemes.com/metronic/theme/html/demo13/dist/assets/media/svg/flags/226-united-states.svg"
                
            } else {
                $scope.selectedLanguageSrc = "https://preview.keenthemes.com/metronic/theme/html/demo13/dist/assets/media/svg/flags/195-france.svg" 
            }
            window.location.reload();
        });
    };
    $scope.GetLanguage = function () {
        languageFactory.GetUserLanguage().then(function (response) {
            if (response.data.UserLanguage == "en") {
                $scope.selectedLanguageSrc = "https://preview.keenthemes.com/metronic/theme/html/demo13/dist/assets/media/svg/flags/226-united-states.svg"
                
            } else {
                $scope.selectedLanguageSrc = "https://preview.keenthemes.com/metronic/theme/html/demo13/dist/assets/media/svg/flags/195-france.svg"
            }
        });
    };
    $scope.GetLanguage();
}]);

