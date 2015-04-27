'use strict';

(function () {
    var injectParams = ['$rootScope', '$location', 'commonService', 'appSettings', 'FormTypeFactory'];
    var modelService = function ($rootScope, $location, commonService, appSettings, FormTypeFactory) {
        function Model() {
            this.isValid = false;
            this.params = {};
            this.isMultiple = false;
            this.custIds = [];
            this.custNames = [];
        }

        Model.prototype.getMultipleInfo = function () {
            return {
                IsMultiple: this.isMultiple,
                CustIds: this.custIds,
                CustNames: this.custNames
            };
        };

        Model.prototype.reset = function () {
            this.isValid = false;
            this.params = {};
            this.isMultiple = false;
            this.custIds = [];
            this.custNames = [];
        };

        Model.prototype.load = function () {
            this.reset();

            // read form setting type
            var path = $location.path();
            var formTypes = commonService.getPropertyValues(appSettings.formType);
            angular.forEach(formTypes, function (formType) {
                if (this.isValid) {
                    return;
                }

                var formTypeObj = new FormTypeFactory(formType);
                var pattern = formTypeObj.pattern;
                var matches = pattern.match(/\/:\w+/g);
                var params = [];
                angular.forEach(matches, function (match) {
                    params.push(match.replace('/:', ''));
                });

                var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
                matches = regExp.exec(path);
                if (matches !== null && matches.length - 1 === params.length) {
                    this.isValid = true;
                    angular.forEach(params, function (param, index) {
                        this.params[param] = parseInt(matches[index + 1]);
                    }, this);

                    this.params.formType = formType;
                }
            }, this);

            // read multiple information
            this.isMultiple = false;
            var queryString = $location.search();
            var custIds = queryString.custIds;
            var custNames = queryString.custNames;
            if (angular.isDefined(custIds) && angular.isDefined(custNames)) {
                custIds = custIds.split(',').map(Number);
                custNames = custNames.split(',');
                this.isMultiple = custIds.length === custNames.length;
            }

            if (this.isMultiple) {
                this.custIds = custIds;
                this.custNames = custNames;
            }
        };

        var model = new Model();
        model.load();
        $rootScope.$on('$locationChangeStart', function () {
            model.load();
        });

        this.isValid = function () {
            return model.isValid;
        };
        this.getParams = function () {
            return model.params;
        };
        this.getMultipleInfo = function () {
            return model.getMultipleInfo();
        };
        this.createSelectModel = function () {
            if (!model.isValid) {
                return undefined;
            }

            var formType = model.params.formType;
            var formTypeObj = new FormTypeFactory(formType);
            var selectModel = formTypeObj.createModel(model.params);
            angular.extend(selectModel, {
                FormType: model.params.formType,
                ProductId: model.params.productId
            });

            return selectModel;
        };
    };

    modelService.$inject = injectParams;
    angular.module('customerSettingApp').service('modelService', modelService);
})();