(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexCommissionHandler'];
    var fanexCommissionMatrixDirective = function ($compile, appSettings, CommissionHandler) {
        return {
            restrict: 'AE',
            scope: true,
            templateUrl: 'templates/fanex-commission-matrix.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('settings', function (newSettings, oldSettings) {
                    if (newSettings !== oldSettings) {
                        update();
                    }
                });

                function update() {
                    var customer = $scope.customer;
                    var settings = $scope.settings;
                    var roleId = customer.RoleId;
                    var settingLevel = customer.SettingLevel;
                    var commissionHandler = new CommissionHandler(roleId, settingLevel);
                    $scope.tables = commissionHandler.createTables(settings);
                }
            } ]
        };
    };

    fanexCommissionMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexCommissionMatrix', fanexCommissionMatrixDirective);
})();