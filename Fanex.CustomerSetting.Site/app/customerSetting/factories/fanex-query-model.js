'use strict';

(function () {
    var injectParams = ['$location', 'fanexAppSettings', 'fanexCommonService', 'fanexFormTypeHandler'];
    var fanexQueryModel = function ($location, appSettings, commonService, FormTypeHandler) {
        var isValid = false;
        var viewParams = {};
        var isMultiple = false;
        var custIds = [];
        var custNames = [];
        var formTypeHandler;

        function readSingleInformation() {
            var path = $location.path();
            var formTypes = commonService.getPropertyValues(appSettings.formType);
            angular.forEach(formTypes, function (formType) {
                if (isValid) {
                    return;
                }

                formTypeHandler = new FormTypeHandler(formType);
                var pattern = formTypeHandler.pattern;
                var matches = pattern.match(/\/:\w+/g);
                var queryParams = [];
                angular.forEach(matches, function (match) {
                    queryParams.push(match.replace('/:', ''));
                });

                var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
                matches = regExp.exec(path);
                if (matches !== null && matches.length - 1 === queryParams.length) {
                    isValid = true;
                    angular.forEach(queryParams, function (queryParam, index) {
                        viewParams[queryParam] = parseInt(matches[index + 1]);
                    });
                }
            });
        }

        function readMultipleInformation() {
            isMultiple = false;
            var queryString = $location.search();
            var custIdsQueryString = queryString.custIds;
            var custNamesQueryString = queryString.custNames;
            if (angular.isDefined(custIdsQueryString) && angular.isDefined(custNamesQueryString)) {
                isMultiple = true;
                custIds = custIdsQueryString.split(',').map(Number);
                custNames = custNamesQueryString.split(',');
                if (custIds.length !== custNames.length || custIds.length <= 1) {
                    isValid = false;
                }
            }
        }

        function QueryModel() {
            isValid = false;
            readSingleInformation();
            if (isValid) {
                readMultipleInformation();
            }

            if (!isValid) {
                return undefined;
            }

            var model = formTypeHandler.createModel(viewParams);
            model.getMultipleInfo = function () {
                return {
                    IsMultiple: isMultiple,
                    CustIds: custIds,
                    CustNames: custNames
                };
            };

            return model;
        }

        return QueryModel;
    };

    fanexQueryModel.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexQueryModel', fanexQueryModel);
})();