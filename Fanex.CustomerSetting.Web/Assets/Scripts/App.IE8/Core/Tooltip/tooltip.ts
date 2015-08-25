/// <reference path="../../../typings/angularjs/angular.d.ts" />
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

            this.$compile(tooltip)(scope);
            element.parent().prepend(tooltip);

            scope.$watch("message", (newMessage: string, oldMessage: string) => {
                if (newMessage && newMessage !== oldMessage) {
                    this.$timeout(() => {
                        var elementRect: ClientRect = element[0].getBoundingClientRect();
                        var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                        var scrollX: number = this.$window.pageXOffset;
                        var scrollY: number = this.$window.pageYOffset;
                        var top: number = Math.max(0, elementRect.top - tooltipRect.height + scrollY);
                        var left: number = Math.max(0, elementRect.left + (elementRect.width - tooltipRect.width) / 2 + scrollX);

                        if (this.$window.innerWidth < left + tooltipRect.width) {
                            left = elementRect.left - tooltipRect.width + elementRect.width / 2;
                            tooltip.addClass("right");
                        }

                        tooltip.css({
                            "top": top + "px",
                            "left": left + "px"
                        });
                    });
                }
            });
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
    }

    ModuleRegister.directive("tooltip", Tooltip);

    function TooltipMessage($injector: ng.auto.IInjectorService) {
        return (result: ValidationResult, resourceServiceName: string, fieldName: string) => {
            var message: string = "";

            if (result
                && !result.isSuccessful
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

    function HighlightingField() {
        return (result: ValidationResult, fieldName: string) => {
            var mustHighlighted: boolean = false;

            if (result
                && !result.isSuccessful
                && (!fieldName || (fieldName && fieldName === result.fieldName))) {
                mustHighlighted = true;
            }

            return mustHighlighted;
        };
    }

    ModuleRegister.filter("highlightingField", HighlightingField);
}