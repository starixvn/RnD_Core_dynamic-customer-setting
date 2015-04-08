'use strict';

(function () {
    var injectParams = ['commonService', 'resourceService', 'controllerService', 'appSettings'];
    var masterEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=masterEditMemberDiscount'
            },
            templateUrl: 'templates/master-edit-member-discount.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    masterEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('masterEditMemberDiscount', masterEditMemberDiscountDirective);
})();