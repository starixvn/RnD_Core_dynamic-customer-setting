(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexSettingLevelHandler', 'fanexRoleTypeHandler'];
    var fanexPositionTakingMatrixDirective = function ($compile, appSettings, SettingLevelHandler, RoleTypeHandler) {
        return {
            restrict: 'AE',
            scope: true,
            templateUrl: 'templates/fanex-position-taking-matrix.htm',
            controller: ['$scope', function ($scope) {
                $scope.$watch('settings', function (newSettings, oldSettings) {
                    if (newSettings !== oldSettings) {
                        update();
                    }
                });

                function update() {
                    var customer = $scope.customer;
                    var settings = $scope.settings;
                    var settingLevel = customer.SettingLevel;
                    var settingLevelHandler = new SettingLevelHandler(settingLevel);
                    var roleId = customer.RoleId;
                    var roleTypeHandler = new RoleTypeHandler(roleId);
                    var settingTables = roleTypeHandler.generateTables(settingLevel, settings);
                    var settingGroups = settingLevelHandler.generateGroups(settingTables);
                    $scope.disableAutoMPT = customer.DisableAutoMPT;
                    $scope.groups = settingGroups;
                }
            } ]
        };
    };

    fanexPositionTakingMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexPositionTakingMatrix', fanexPositionTakingMatrixDirective);
})();