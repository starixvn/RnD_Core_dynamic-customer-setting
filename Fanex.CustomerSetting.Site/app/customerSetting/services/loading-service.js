'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var loadingService = function ($rootScope) {
        this.block = function () {
            $rootScope.hasLoading = true;
        };

        this.unblock = function () {
            $rootScope.hasLoading = false;
        };
    };

    loadingService.$inject = injectParams;
    angular.module('customerSettingApp').service('loadingService', loadingService);
})();