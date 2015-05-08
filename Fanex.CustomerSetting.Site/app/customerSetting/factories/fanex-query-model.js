'use strict';

(function () {
    var injectParams = ['$location', 'fanexAppSettings', 'fanexCommonService', 'fanexFormTypeHandler'];
    var fanexQueryModel = function ($location, appSettings, commonService, FormTypeHandler) {
        function QueryModel() {
            var isValid = false;
            var params = {};
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
                            params[queryParam] = parseInt(matches[index + 1]);
                        });
                    }
                });
            }

            function readMultipleInformation() {
                isMultiple = false;
                var queryString = $location.search();
                var _custIds = [];
                var _custNames = [];
                var custIdsQueryString = queryString.custIds;
                var custNamesQueryString = queryString.custNames;
                if (angular.isDefined(custIdsQueryString) && angular.isDefined(custNamesQueryString)) {
                    isMultiple = true;
                    _custIds = custIdsQueryString.split(',').map(Number);
                    _custNames = custNamesQueryString.split(',');
                    if (_custIds.length !== _custNames.length || _custIds.length <= 1) {
                        isValid = false;
                    }
                }

                if (isMultiple) {
                    custIds = _custIds;
                    custNames = _custNames;
                }
            }

            function createModel() {
                if (isValid) {
                    return formTypeHandler.createModel(params);
                }

                return undefined;
            }

            readSingleInformation();
            if (isValid) {
                readMultipleInformation();
            }

            var model = createModel();
            model.isValid = function () {
                return isValid;
            };

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