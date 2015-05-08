'use strict';

(function () {
    var injectParams = ['$rootScope', 'fanexQueryModel'];
    var fanexModelService = function ($rootScope, QueryModel) {
        var queryModel = new QueryModel();
        $rootScope.$on('$locationChangeStart', function () {
            queryModel = new QueryModel();
        });

        return {
            QueryModel: queryModel
        };
    };

    fanexModelService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexModelService', fanexModelService);
})();