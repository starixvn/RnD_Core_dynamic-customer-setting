'use strict';

(function () {
    var injectParams = ['$rootScope'];
    var fanexDirectiveLinker = function ($rootScope) {
        var directiveLinker = function () {
            var scopes = [];
            this.linkScope = function (scope) {
                scopes.push(scope);
            };

            this.getScopes = function () {
                return scopes;
            };

            $rootScope.$on('product-view-loaded', function () {
                scopes = [];
            });
        };

        return directiveLinker;
    };

    fanexDirectiveLinker.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveLinker', fanexDirectiveLinker);
})();