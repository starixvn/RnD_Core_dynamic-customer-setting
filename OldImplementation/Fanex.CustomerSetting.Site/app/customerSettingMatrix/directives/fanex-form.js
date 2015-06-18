(function () {
    'use strict';
    var injectParams = ['$http', 'fanexAppSettings', 'fanexResourceService'];
    var fanexFormDirective = function ($http, appSettings, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: false,
            templateUrl: 'templates/fanex-form.htm',
            controller: ['$scope', function ($scope) {
                $scope.products = [
                    { Id: 250, Name: 'Product-250' },
                    { Id: 251, Name: 'Product-251' },
                    { Id: 252, Name: 'Product-252' },
                    { Id: 253, Name: 'Product-253' },
                    { Id: 254, Name: 'Product-254' },
                    { Id: 255, Name: 'Product-255' }
                ];
                $scope.product = $scope.products[0];
                $scope.submit = function () {
                    var product = $scope.product;
                    var productId = product.Id;
                    var productName = product.Name;
                    resourceService.ProductName = productName;
                    var userName = $scope.userName;

                    if (angular.isUndefined(productId)) {
                        $scope.showMessage('Please select a product!');
                        return;
                    }

                    if (angular.isUndefined(userName) || userName === '') {
                        $scope.showMessage('Please input username!');
                        return;
                    }

                    $scope.hideMessage();
                    $scope.submitForm(productId, userName);
                };
            } ]
        };
    };

    fanexFormDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexForm', fanexFormDirective);
})();