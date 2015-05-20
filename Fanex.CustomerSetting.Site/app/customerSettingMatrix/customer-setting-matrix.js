/*! fanex-customer-setting-matrix v0.0.1 | (c) 2015 Nexcel Solutions Vietnam */

'use strict';

(function () {
    angular.module('customerSettingMatrixApp', []);
} ());

/*jshint -W101*/

angular.module('customerSettingMatrixApp').run(['$templateCache', function($templateCache) {
  'use strict';

  $templateCache.put('templates/fanex-breadcrumb.htm',
    "<ol><li data-ng-repeat=\"line in lines\" data-ng-switch on=$last data-ng-class=\"{'active': $last}\"><a href=# ng-switch-when=false data-ng-bind=line.Name data-ng-click=show(line.Name)></a> <span ng-switch-when=true data-ng-bind=line.Name></span></li></ol>"
  );

} ]);

/*jshint -W101*/

'use strict';

(function () {
    var injectParams = ['$scope', '$http', 'fanexAppSettings'];
    var fanexPositionTakingController = function ($scope, $http, appSettings) {
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

        $scope.$on('fanex-user-clicked', function(event, userName){
            $scope.userName = userName;
            $scope.submit();
        });

        function submit(productId, userName) {
            var queryModel = {
                ProductId: productId,
                UserName: userName
            };

            var url = appSettings.rootUrl + '/CustomerSettingMatrix/PositionTaking';
            var request = $http.post(url, queryModel);
            request.success(function (response) {
                console.log(response);
                $scope.customer = response.Customer;
            });
        }
    };

    fanexPositionTakingController.$inject = injectParams;
    angular.module('customerSettingMatrixApp').controller('FanexPositionTakingController', fanexPositionTakingController);
})();

'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexBreadcrumbDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                customer: '=fanexBreadcrumb'
            },
            templateUrl: 'templates/fanex-breadcrumb.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('customer', function (newCustomer, oldCustomer) {
                    if (newCustomer !== oldCustomer) {
                        render();
                    }
                });

                function render() {
                    var lines = [];
                    var customer = $scope.customer;
                    switch (customer.RoleId) {
                        case appSettings.roleType.Master:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            break;

                        case appSettings.roleType.Agent:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            lines.push({ Id: customer.MasterId, Name: customer.MasterName });
                            break;

                        case appSettings.roleType.Member:
                            lines.push({ Id: customer.SuperId, Name: customer.SuperName });
                            lines.push({ Id: customer.MasterId, Name: customer.MasterName });
                            lines.push({ Id: customer.AgentId, Name: customer.AgentName });
                            break;
                    }

                    $scope.lines = lines;
                }

                $scope.show = function(userName){
                    $scope.$emit('fanex-user-clicked', userName);
                };
            }]
        };
    };

    fanexBreadcrumbDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexBreadcrumb', fanexBreadcrumbDirective);
})();

'use strict';

(function () {
    var body = angular.element(document.body);
    var rootUrl = body.attr('data-root-url');
    angular.module('customerSettingMatrixApp').value('fanexAppSettings', {
        rootUrl: rootUrl,
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4,
            Company: 5
        }
    });
})();