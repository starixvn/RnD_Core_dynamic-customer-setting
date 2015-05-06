'use strict';

(function () {
    var injectParams = ['fanexDiscountService', 'fanexResourceService'];
    var fanexSuperEditMemberDiscountDirective = function (discountService, resourceService) {
        return {
            restrict: 'AE',
            replace: true,
            scope: {
                discount: '=fanexDiscountSuperEditMember',
                settingLevel: '='
            },
            templateUrl: 'templates/fanex-discount-super-edit-member.htm',
            controller: ['$scope', function ($scope) {
                discountService.linkScope($scope);

                $scope.name = resourceService.getDisplayName($scope.settingLevel, $scope.discount);
                var discount = $scope.discount;
                $scope.masterDiscount = discount.MasterDiscount;
                $scope.agentDiscount = discount.AgentDiscount;
                $scope.memberDiscount = discount.PlayerDiscount;
            } ]
        };
    };

    fanexSuperEditMemberDiscountDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexSuperEditMemberDiscount', fanexSuperEditMemberDiscountDirective);
})();