(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexBreadcrumbDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: true,
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
                        case appSettings.roleType.Super:
                            break;

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

                        default:
                            throw 'Not implemented yet.';
                    }

                    lines.push({ Id: customer.CustId, Name: customer.UserName });
                    $scope.lines = lines;
                }

                $scope.show = function (userName) {
                    $scope.$emit('fanex-user-clicked', userName);
                };
            } ]
        };
    };

    fanexBreadcrumbDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexBreadcrumb', fanexBreadcrumbDirective);
})();