/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    interface TooltipScope extends ng.IScope {
        message: string;
        showOnFocus: boolean;
        isShown: boolean;
    }

    class Tooltip implements ng.IDirective {
        public static $inject: string[] = ["$window", "$document", "$compile", "$timeout", "$templateCache"];

        public restrict: string = "A";
        public priority: number = 100;

        public get scope(): any {
            return {
                message: "=",
                showOnFocus: "="
            }
        }

        public link: ng.IDirectiveLinkFn = (scope: TooltipScope, element: ng.IAugmentedJQuery) => {
            var template = this.$templateCache.get("Tooltip/tooltip.htm");
            var tooltip = angular.element(template);

            this.$compile(tooltip)(scope);
            element.parent().prepend(tooltip);

            scope.$watch("message", message => {
                this.$timeout(() => {
                    var elementRect: ClientRect = element[0].getBoundingClientRect();
                    var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                    var scrollX: number = this.$window.pageXOffset;
                    var scrollY: number = this.$window.pageYOffset;
                    var top: number = Math.max(0, elementRect.top - tooltipRect.height + scrollY);
                    var left: number = Math.max(0, elementRect.left + (elementRect.width - tooltipRect.width) / 2 + scrollX);

                    tooltip.css({
                        "top": top + "px",
                        "left": left + "px"
                    });
                });
            });

            if (scope.showOnFocus) {
                element.on("focus", () => {
                    scope.isShown = true;
                    scope.$apply();
                });

                element.on("focusout", () => {
                    scope.isShown = false;
                    scope.$apply();
                });
            } else {
                scope.isShown = true;
            }
        }

        constructor(
            private $window: ng.IWindowService,
            private $document: ng.IDocumentService,
            private $compile: ng.ICompileService,
            private $timeout: ng.ITimeoutService,
            private $templateCache: ng.ITemplateCacheService) { }
    }

    ModuleRegister.directive("tooltip", Tooltip);
}