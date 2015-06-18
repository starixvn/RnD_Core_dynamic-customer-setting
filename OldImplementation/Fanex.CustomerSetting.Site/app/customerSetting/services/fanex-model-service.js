(function () {
    'use strict';
    var injectParams = ['$rootScope', 'fanexQueryModel'];
    var fanexModelService = function ($rootScope, QueryModel) {
        var queryModel;
        function createModel() {
            try {
                queryModel = new QueryModel();
            }
            catch (error) {
                if (typeof console !== 'undefined') {
                    console.error(error);
                }
            }
        }

        $rootScope.$on('$locationChangeStart', function () {
            createModel();
            $rootScope.$emit('fanex-model-changed', queryModel);
        });

        return {
            getQueryModel: function () {
                return queryModel;
            }
        };
    };

    fanexModelService.$inject = injectParams;
    angular.module('customerSettingApp').service('fanexModelService', fanexModelService);
})();