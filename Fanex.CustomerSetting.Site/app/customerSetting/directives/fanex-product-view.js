(function () {
    'use strict';
    var injectParams = ['$sce', '$compile', '$controller', '$templateCache',
            'fanexProductSettingService', 'fanexModelService'];
    var fanexProductViewDirective = function ($sce, $compile, $controller, $templateCache,
            fanexProductSettingService, modelService) {
        return {
            transclude: 'element',
            compile: function (element, attr, linker) {
                return function (scope, $element) {
                    var currentElement;
                    var currentScope;

                    function cleanup(){
                        if (currentElement) {
                            currentElement.remove();
                            currentScope.$destroy();
                            currentElement = null;
                        }
                    }

                    // update view
                    function update() {
                        cleanup();

                        var productViewSetting = fanexProductSettingService.errorViewSetting;
                        var queryModel = modelService.getQueryModel();
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

                            var link = $compile(clone.contents());
                            currentElement = clone;
                            currentScope = newScope;
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

                    scope.$on('fanex-model-changed', update);
                };
            }
        };
    };

    fanexProductViewDirective.$inject = injectParams;
    angular.module('customerSettingApp').directive('fanexProductView', fanexProductViewDirective);
})();