(function () {
    'use strict';
    var injectParams = ['$rootScope'];
    var fanexDirectiveLinker = function ($rootScope) {
        function DirectiveLinker() {
            var scopes = [];

            function linkScope(scope) {
                scopes.push(scope);
            }

            function getScopes() {
                return scopes;
            }

            $rootScope.$on('product-view-loaded', function () {
                scopes = [];
            });

            return {
                linkScope: linkScope,
                getScopes: getScopes
            };
        }

        return DirectiveLinker;
    };

    fanexDirectiveLinker.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexDirectiveLinker', fanexDirectiveLinker);
})();