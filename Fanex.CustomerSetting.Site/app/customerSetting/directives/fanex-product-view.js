'use strict';

(function () {
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache',
            'fanexProductSettingService', 'fanexModelService'];
    var fanexProductViewDirective = function ($sce, $compile, $controller, $templateCache,
            fanexProductSettingService, modelService) {
        return {
            transclude: 'element',
            compile: function (element, attr, linker) {
                return function (scope, $element) {
                    var currentElement;

                    // update view
                    function update() {
                        var productViewSetting = fanexProductSettingService.errorViewSetting;
                        var queryModel = modelService.QueryModel;
                        if (angular.isDefined(queryModel)) {
                            var productId = queryModel.ProductId;
                            productViewSetting = fanexProductSettingService.getViewSetting(productId);
                        }

                        var templateUrl = productViewSetting.templateUrl;
                        if (!angular.isDefined(templateUrl)) {
                            return;
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
                            var controller = productViewSetting.controller;
                            if (angular.isDefined(controller)) {
                                controller = $controller(productViewSetting.controller, {
                                    $scope: newScope
                                });
                                clone.data('$ngControllerController', controller);
                                clone.children().data('$ngControllerController', controller);
                            }

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