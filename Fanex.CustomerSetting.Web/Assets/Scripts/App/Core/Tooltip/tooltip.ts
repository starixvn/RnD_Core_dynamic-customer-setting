/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../resource/resource-translation.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface ITooltipScope extends ng.IScope {
        message: string;
    }

    class Tooltip implements ng.IDirective {
        public static $inject: string[] = ["$window", "$compile", "$timeout", "$templateCache"];

        public restrict: string = "A";
        public priority: number = 100;

        public get scope(): any {
            return {
                message: "@"
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: ITooltipScope, element: ng.IAugmentedJQuery) => {
            var template = this.$templateCache.get("Tooltip/tooltip.htm");
            var tooltip = angular.element(template);
            var debounceResize = this.debounce(() => {
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
        private $compile: ng.ICompileService;
        private $timeout: ng.ITimeoutService;
        private $templateCache: ng.ITemplateCacheService;

        constructor(
            $window: ng.IWindowService,
            $compile: ng.ICompileService,
            $timeout: ng.ITimeoutService,
            $templateCache: ng.ITemplateCacheService) {
            this.$window = $window;
            this.$compile = $compile;
            this.$timeout = $timeout;
            this.$templateCache = $templateCache;
        }

        private calculatePosition(element: ng.IAugmentedJQuery, tooltip: ng.IAugmentedJQuery): void {
            this.$timeout(() => {
                var elementRect: ClientRect = element[0].getBoundingClientRect();
                var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                var scrollX: number = this.$window.pageXOffset;
                var scrollY: number = this.$window.pageYOffset;
                var top: number = Math.max(0, elementRect.top - tooltipRect.height + scrollY + 5);
                var left: number = Math.max(0, elementRect.left + (elementRect.width - tooltipRect.width) / 2 + scrollX);

                if (this.$window.innerWidth < left + tooltipRect.width) {
                    left = elementRect.left - tooltipRect.width + elementRect.width / 2;
                    left = Math.min(left, this.$window.innerWidth - tooltipRect.width - 5);
                    tooltip.addClass("right");
                }

                if (top !== tooltipRect.top || left !== tooltipRect.left) {
                    tooltip.css({
                        "top": top + "px",
                        "left": left + "px"
                    });
                }
            });
        }

        private debounce(func: Function, waitTime: number): any {
            var timeout: ng.IPromise<void>;
            var timestamp: number;
            var context: any;
            var args: IArguments;

            var runLater = () => {
                var lastRun: number = new Date().getTime() - timestamp;

                if (lastRun > 0 && lastRun < waitTime) {
                    timeout = this.$timeout(runLater, waitTime - lastRun);
                } else {
                    timeout = null;
                    func.apply(context, args);
                    context = null;
                    args = null;
                }
            };

            context = this;
            args = arguments;

            return () => {
                timestamp = new Date().getTime();

                if (!timeout) {
                    timeout = this.$timeout(runLater, waitTime);
                }
            };
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