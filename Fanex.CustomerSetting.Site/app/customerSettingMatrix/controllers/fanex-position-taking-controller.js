(function () {
    'use strict';
    var injectParams = ['$scope', '$http', 'fanexAppSettings', 'fanexResourceService'];
    var fanexPositionTakingController = function ($scope, $http, appSettings, resourceService) {
        $scope.products = [
            { Id: 250, Name: 'Product-250' },
            { Id: 251, Name: 'Product-251' },
            { Id: 252, Name: 'Product-252' },
            { Id: 253, Name: 'Product-253' },
            { Id: 254, Name: 'Product-254' },
            { Id: 255, Name: 'Product-255' }
        ];
        $scope.product = $scope.products[0];
        $scope.userName = '428RM';
        $scope.submit = function () {
            var product = $scope.product;
            var productId = product.Id;
            var productName = product.Name;
            resourceService.ProductName = productName;
            if (angular.isUndefined(productId)) {
                $scope.showMessage('Please select a product!');
                return;
            }

            var userName = $scope.userName;
            if (angular.isUndefined(userName) || userName === '') {
                $scope.showMessage('Please input username!');
                return;
            }

            $scope.hideMessage();
            submit(productId, userName);
        };

        $scope.$on('fanex-user-clicked', function (event, userName) {
            $scope.userName = userName;
            $scope.submit();
        });

        $scope.showMessage = function (message) {
            $scope.customer = undefined;
            $scope.settings = undefined;
            $scope.message = message;
        };

        $scope.hideMessage = function () {
            $scope.message = undefined;
        };

        function submit(productId, userName) {
            var queryModel = {
                ProductId: productId,
                UserName: userName
            };

            var url = appSettings.rootUrl + '/CustomerSettingMatrix/PositionTaking';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                $scope.customer = response.Customer;
                $scope.settings = response.Settings;
            });
        }
    };

    fanexPositionTakingController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPositionTakingController', fanexPositionTakingController);
})();