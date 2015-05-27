(function () {
    'use strict';
    var injectParams = ['$scope', '$http', 'fanexAppSettings'];
    var fanexPageController = function ($scope, $http, appSettings) {
        $scope.pageUrl = 'templates/fanex-page.htm';
        $scope.changeSettingType = function (settingType) {
            $scope.settingType = settingType;
            $scope.message = undefined;
            $scope.customer = undefined;
            $scope.settings = undefined;
        };
        $scope.productId = 0;
        $scope.userName = '428RM';
        $scope.submitForm = function (productId, userName) {
            $scope.productId = productId;
            $scope.userName = userName;
            var url = appSettings.rootUrl + '/CustomerSettingMatrix/Query';
            var queryModel = {
                ProductId: productId,
                UserName: userName,
                SettingType: $scope.settingType
            };
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                $scope.customer = response.Customer;
                $scope.settings = response.Settings;
            });
        };
        $scope.selectUserName = function (userName) {
            $scope.userName = userName;
            $scope.submitForm($scope.productId, $scope.userName);
        };
        $scope.showMessage = function (message) {
            $scope.customer = undefined;
            $scope.settings = undefined;
            $scope.message = message;
        };
        $scope.hideMessage = function () {
            $scope.message = undefined;
        };
    };

    fanexPageController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPageController', fanexPageController);
})();