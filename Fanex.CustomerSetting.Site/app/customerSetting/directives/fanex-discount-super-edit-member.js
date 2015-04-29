'use strict';

(function () {
    var injectParams = ['fanexCommonService', 'fanexResourceService', 'fanexControllerService', 'fanexAppSettings'];
    var fanexSuperEditMemberDiscountDirective = function (commonService, resourceService, controllerService, appSettings) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountSuperEditMember'
            },
            templateUrl: 'templates/fanex-discount-super-edit-member.htm',
            controller: ['$scope', function ($scope) {
                controllerService.linkDiscount($scope);
                var discount = $scope.discount;
                $scope.name = controllerService.getDisplayName(appSettings.settingType.Discount, $scope.discount);
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexSuperEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexSuperEditMemberDiscount', fanexSuperEditMemberDiscountDirective);
})();