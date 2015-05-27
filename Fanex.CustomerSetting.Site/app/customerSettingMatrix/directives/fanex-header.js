(function () {
    'use strict';
    var injectParams = ['fanexAppSettings'];
    var fanexHeaderDirective = function (appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: false,
            templateUrl: 'templates/fanex-header.htm',
            controller: ['$scope', function ($scope) {
                $scope.navigationItems = [{
                    Value: appSettings.settingType.PositionTaking,
                    Text: 'PT Matrix'
                }, {
                    Value: appSettings.settingType.Commission,
                    Text: 'Commission Matrix'
                }];
                $scope.activeNavigationItem = function (navigationItem) {
                    $scope.selectedNavigationItem = navigationItem;
                    $scope.changeSettingType(navigationItem.Value);
                };
                $scope.activeNavigationItem($scope.navigationItems[0]);
            } ]
        };
    };

    fanexHeaderDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexHeader', fanexHeaderDirective);
})();