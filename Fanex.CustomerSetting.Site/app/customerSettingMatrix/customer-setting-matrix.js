/*! fanex-customer-setting-matrix v0.0.1 | (c) 2015 Nexcel Solutions Vietnam */

'use strict';

(function () {
    angular.module('customerSettingMatrixApp', []);
} ());



'use strict';

(function () {
    var injectParams = ['$scope', '$http'];
    var fanexPositionTakingController = function ($scope, $http) {
        $scope.products = [
            { id: 250, name: 'Product-250' },
            { id: 251, name: 'Product-251' },
            { id: 252, name: 'Product-252' },
            { id: 253, name: 'Product-253' },
            { id: 254, name: 'Product-254' },
            { id: 255, name: 'Product-255' }
        ];
        $scope.product = $scope.products[0];
        $scope.userName = '428RM';
        $scope.submit = function () {
            var product = $scope.product;
            var productId = product.id;
            if (angular.isUndefined(productId)) {
                $scope.message = 'Please select a product!';
                return;
            }

            var userName = $scope.userName;
            if (angular.isUndefined(userName) || userName === '') {
                $scope.message = 'Please input username!';
                return;
            }

            $scope.message = undefined;
            submit(productId, userName);
        };

        var body = angular.element(document.body);
        var rootUrl = body.attr('data-root-url');
        function submit(productId, userName) {
            var queryModel = {
                ProductId: productId,
                UserName: userName
            };

            var url = rootUrl + '/CustomerSettingMatrix/PositionTaking';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                console.log(response);
            });
        }
    };

    fanexPositionTakingController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPositionTakingController', fanexPositionTakingController);
})();