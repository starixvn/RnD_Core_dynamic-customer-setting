'use strict';

(function () {
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache',
                        'productConfiguration', 'modelService', 'messageService', 'controllerService'];
    var customerSettingProductDirective = function ($sce, $compile, $controller, $templateCache,
                        productConfiguration, modelService, messageService, controllerService) {
        function getProductSetting(productId) {
            var result;
            angular.forEach(productConfiguration.productSettings, function (productSetting) {
                if (angular.isUndefined(result) && productSetting.id === productId) {
                    result = productSetting;
                }
            });

            return result;
        }

        return {
            restrict: 'AE',
            link: function (scope, element) {
                scope.$on('$locationChangeStart', function () {
                    messageService.hide();
                    controllerService.reset();
                });
                scope.$on('$locationChangeSuccess', function () {
                    update();
                });

                function update() {
                    var controller = productConfiguration.errorController;
                    var templateUrl = productConfiguration.errorTemplateUrl;

                    var isValid = modelService.isValid();
                    if (isValid) {
                        var params = modelService.getParams();
                        var productId = params.productId;
                        var productSetting = getProductSetting(productId);
                        if (angular.isDefined(productSetting)) {
                            templateUrl = productSetting.templateUrl;
                            controller = productSetting.controller;
                        }
                    }

                    templateUrl = $sce.getTrustedResourceUrl(templateUrl);
                    var template = $templateCache.get(templateUrl);
                    element.html(template);
                    controller = $controller(controller, {
                        $scope: scope,
                        $template: template
                    });
                    element.data('$ngControllerController', controller);
                    element.children().data('$ngControllerController', controller);
                    var link = $compile(element.contents());
                    link(scope);
                }
            }
        };
    };

    customerSettingProductDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('customerSettingProduct', customerSettingProductDirective);
})();