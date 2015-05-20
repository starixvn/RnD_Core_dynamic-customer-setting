'use strict';

(function () {
    var body = angular.element(document.body);
    var rootUrl = body.attr('data-root-url');
    angular.module('customerSettingMatrixApp').value('fanexAppSettings', {
        rootUrl: rootUrl,
        roleType: {
            Member: 1,
            Agent: 2,
            Master: 3,
            Super: 4,
            Company: 5
        }
    });
})();