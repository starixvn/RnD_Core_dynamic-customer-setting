'use strict';

(function () {
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache', 'fanexProductConfiguration', 'fanexModelService'];
    var fanexProductViewDirective = function ($sce, $compile, $controller, $templateCache, productConfiguration, modelService) {
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
            transclude: 'element',
            compile: function (element, attr, linker) {
                return function (scope, $element) {
                    var currentElement;

                    // update view
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

                        scope.$emit('product-view-loaded');
                    }

                    scope.$on('$locationChangeSuccess', update);
                };
            }
        };
    };

    fanexProductViewDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexProductView', fanexProductViewDirective);
})();