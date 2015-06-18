(function () {
    'use strict';
    var injectParams = ['$compile', 'fanexAppSettings', 'fanexPositionTakingHandler'];
    var fanexPositionTakingMatrixDirective = function ($compile, appSettings, PositionTakingHandler) {
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
                    $scope.disableAutoMPT = customer.DisableAutoMPT;
                    var settings = $scope.settings;
                    var roleId = customer.RoleId;
                    var settingLevel = customer.SettingLevel;
                    var positionTakingHandler = new PositionTakingHandler(roleId, settingLevel);
                    $scope.groups = positionTakingHandler.createGroups(settings);
                }
            } ]
        };
    };

    fanexPositionTakingMatrixDirective.$inject = injectParams;
    angular.module('customerSettingMatrixApp').directive('fanexPositionTakingMatrix', fanexPositionTakingMatrixDirective);
})();