/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../resource/resource-translation.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface ITooltipScope extends ng.IScope {
        message: string;
    }

    class Tooltip implements ng.IDirective {
        public static $inject: string[] = ["$window", "$document", "$compile", "$timeout", "$templateCache"];

        public restrict: string = "A";
        public priority: number = 100;

        public scope: any = {
            message: "@"
        };

        public link: ng.IDirectiveLinkFn = (scope: ITooltipScope, element: ng.IAugmentedJQuery) => {
            var template = this.$templateCache.get("Tooltip/tooltip.htm");
            var tooltip = angular.element(template);
            var debounceResize = _.debounce(() => {
                if (!tooltip.hasClass("ng-hide")) {
                    this.calculatePosition(element, tooltip);
                }
            }, 100);

            this.$compile(tooltip)(scope);
            element.parent().prepend(tooltip);

            scope.$watch("message", (newMessage: string, oldMessage: string) => {
                if (newMessage && newMessage !== oldMessage) {
                    this.calculatePosition(element, tooltip);
                }
            });

            angular.element(this.$window).on("resize", debounceResize);
        };

        private $window: ng.IWindowService;
        private $document: ng.IDocumentService;
        private $compile: ng.ICompileService;
        private $timeout: ng.ITimeoutService;
        private $templateCache: ng.ITemplateCacheService;

        constructor(
            $window: ng.IWindowService,
            $document: ng.IDocumentService,
            $compile: ng.ICompileService,
            $timeout: ng.ITimeoutService,
            $templateCache: ng.ITemplateCacheService) {
            this.$window = $window;
            this.$document = $document;
            this.$compile = $compile;
            this.$timeout = $timeout;
            this.$templateCache = $templateCache;
        }

        private calculatePosition(element: ng.IAugmentedJQuery, tooltip: ng.IAugmentedJQuery): void {
            this.$timeout(() => {
                var elementRect: ClientRect = element[0].getBoundingClientRect();
                var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                var scrollX: number = this.$window.pageXOffset || 0;
                var scrollY: number = this.$window.pageYOffset || 0;
                var elementWidth: number = elementRect.right - elementRect.left;
                var tooltipWidth: number = tooltipRect.right - tooltipRect.left;
                var tooltipHeight: number = tooltipRect.bottom - tooltipRect.top;
                var top: number = Math.max(0, elementRect.top - tooltipHeight + scrollY);
                var left: number = Math.max(0, elementRect.left + (elementWidth - tooltipWidth) / 2 + scrollX);
                var windowWidth: number = this.$document.find("body")[0].clientWidth;

                if (windowWidth < left + tooltipWidth) {
                    left = elementRect.left - tooltipWidth + elementWidth / 2;
                    left = Math.min(left, windowWidth - tooltipWidth - 5);
                    tooltip.addClass("tooltip--right");
                }

                tooltip.css({
                    "top": top + "px",
                    "left": left + "px"
                });
            });
        }
    }

    ModuleRegister.directive("tooltip", Tooltip);

    function TooltipMessage($injector: ng.auto.IInjectorService): Function {
        return (result: ValidationResult, resourceServiceName: string, fieldName: string) => {
            var message: string = "";

            if (result
                && !result.isSuccessful()
                && !result.isWarning
                && (!fieldName || (fieldName && fieldName === result.fieldName))) {
                var resourceService: ResourceService = $injector.get<ResourceService>(resourceServiceName);

                if (resourceService) {
                    message = resourceService.getFormattedMessage(result.message, result.msgParams);
                }
            }

            return message;
        };
    }

    TooltipMessage.$inject = ["$injector"];
    ModuleRegister.filter("tooltipMessage", TooltipMessage);

    function HighlightingField(): Function {
        return (result: ValidationResult, fieldName: string) => {
            var mustHighlighted: boolean = false;

            if (result
                && !result.isSuccessful()
                && (!fieldName || (fieldName && fieldName === result.fieldName))) {
                mustHighlighted = true;
            }

            return mustHighlighted;
        };
    }

    ModuleRegister.filter("highlightingField", HighlightingField);
}