'use strict';

(function () {
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache',
            'fanexProductConfiguration', 'fanexModelService', 'fanexMessageService', 'fanexControllerService'];
    var fanexProductViewDirective = function ($sce, $compile, $controller, $templateCache,
            fanexProductConfiguration, fanexModelService, fanexMessageService, fanexControllerService) {
        function getProductSetting(productId) {
            var result;
            angular.forEach(fanexProductConfiguration.productSettings, function (productSetting) {
                if (angular.isUndefined(result) && productSetting.id === productId) {
                    result = productSetting;
                }
            });

            return result;
        }

        return {
            transclude: 'element',
            compile: function (element, attr, linker) {
                return function (scope, $element) {
                    var currentElement;

                    // update view
                    function update() {
                        var controller = fanexProductConfiguration.errorController;
                        var templateUrl = fanexProductConfiguration.errorTemplateUrl;
                        var isValid = fanexModelService.isValid();
                        if (isValid) {
                            var params = fanexModelService.getParams();
                            var productId = params.productId;
                            var productSetting = getProductSetting(productId);
                            if (angular.isDefined(productSetting)) {
                                templateUrl = productSetting.templateUrl;
                                controller = productSetting.controller;
                            }
                        }

                        templateUrl = $sce.getTrustedResourceUrl(templateUrl);
                        var template = $templateCache.get(templateUrl);
                        var newScope = scope.$new();

                        linker(newScope, function (clone) {
                            clone.html(template);
                            $element.after(clone);

                            if (currentElement) {
                                currentElement.remove();
                            }

                            var link = $compile(clone.contents());
                            currentElement = clone;
                            controller = $controller(controller, {
                                $scope: newScope
                            });
                            clone.data('$ngControllerController', controller);
                            clone.children().data('$ngControllerController', controller);
                            link(newScope);
                        });
                    }

                    update();
                    scope.$on('$locationChangeSuccess', update);
                    scope.$on('$locationChangeStart', function () {
                        fanexMessageService.hide();
                        fanexControllerService.reset();
                    });
                };
            }
        };
    };

    fanexProductViewDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexProductView', fanexProductViewDirective);
})();