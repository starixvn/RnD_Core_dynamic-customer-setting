'use strict';

(function () {
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexDiscountMasterEditMemberDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountMasterEditMember',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-master-edit-member.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);
                
                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexDiscountMasterEditMemberDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexDiscountMasterEditMember', fanexDiscountMasterEditMemberDirective);
})();