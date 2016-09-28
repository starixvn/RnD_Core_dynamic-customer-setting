/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../resource/resource-translation.ts" />
/// <reference path="../../helpers.ts" />
/// <reference path="../../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface IFieldHighlightingStateScope extends ng.IScope {
        tooltipMessage: string;
        tooltipType: string;
        highlightType: string,
        fieldName: string;
        highlightState: {};
    }

    class FieldHighlightingDirective implements ng.IDirective {
        public static $inject: string[] = ["$window", "$compile", "$timeout", "$templateCache"];

        public restrict: string = "A";
        public priority: number = 100;

        public get scope(): any {
            return {
                tooltipMessage: "@",
                tooltipType: "@",
                fieldName: "@",
                highlightState: "="
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: IFieldHighlightingStateScope, element: ng.IAugmentedJQuery) => {
            var template: {} = this.$templateCache.get("DefaultSettingBlock/HighlightField/tooltip.htm");
            var tooltip: ng.IAugmentedJQuery = angular.element(template);
            var debounceResize: any = debounce(() => {
                if (scope.tooltipMessage) {
                    this.calculateTooltipPosition(element);
                }
            }, 100);

            this.$compile(tooltip)(scope);
            element.parent().prepend(tooltip);

            scope.$watch("tooltipMessage", () => {
                var fieldName: string = scope.fieldName;
                var highlightState: {} = scope.highlightState;

                scope.highlightType = null;

                if (scope.tooltipMessage) {
                    this.calculateTooltipPosition(element);
                    scope.highlightType = scope.tooltipType || "error";
                }

                if (scope.highlightType && highlightState && fieldName) {
                    highlightState[fieldName] = NotificationType[scope.highlightType];
                }
            });

            if (scope.highlightState) {
                scope.$watch("highlightState", (highlightState: NotificationType[]) => {
                    var fieldHighlightState = (highlightState || {})[scope.fieldName];
                    var style: string = fieldHighlightState && fieldHighlightState !== NotificationType.none
                        ? NotificationType[fieldHighlightState]
                        : null;

                    this.resetElementBorder(element);

                    if (style) {
                        element.addClass("setting-field--" + style);
                    }
                }, true);
            }

            element.on("change", () => {
                scope.tooltipMessage = null;

                if (scope.highlightState) {
                    var fieldHighlightState = scope.highlightState[scope.fieldName];

                    if (fieldHighlightState && fieldHighlightState !== NotificationType.none) {
                        scope.highlightState[scope.fieldName] = NotificationType.none;
                    }
                }

                scope.$apply();
            });

            angular.element(this.$window).on("resize scroll", debounceResize);
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

        private calculateTooltipPosition(target: ng.IAugmentedJQuery): void {
            this.$timeout(() => {
                var tooltip: ng.IAugmentedJQuery = angular.element(target[0].previousElementSibling);
                var elementRect: ClientRect = target[0].getBoundingClientRect();
                var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                var scrollX: number = this.$window.pageXOffset;
                var scrollY: number = this.$window.pageYOffset;
                var top: number = Math.max(0, elementRect.top - tooltipRect.height + scrollY + 5);
                var left: number = elementRect.left + (elementRect.width - tooltipRect.width) / 2 + scrollX;

                if (this.$window.innerWidth < left + tooltipRect.width) {
                    left = elementRect.left - tooltipRect.width + elementRect.width / 2;
                    left = Math.min(left, this.$window.innerWidth - tooltipRect.width - 5);
                    tooltip.addClass("tooltip--right");
                }

                if (left < 0) {
                    left = elementRect.left + (elementRect.width / 2);
                    tooltip.addClass("tooltip--left");
                }

                if (top !== tooltipRect.top || left !== tooltipRect.left) {
                    tooltip.css({
                        "top": top + "px",
                        "left": left + "px"
                    });
                }
            });
        }

        private resetElementBorder(element: ng.IAugmentedJQuery): void {
            if (element.hasClass("setting-field--error")) {
                element.removeClass("setting-field--error");
            }

            if (element.hasClass("setting-field--warning")) {
                element.removeClass("setting-field--success");
            }

            if (element.hasClass("setting-field--success")) {
                element.removeClass("setting-field--success");
            }
        }
    }

    ModuleRegister.directive("fieldHighlighting", FieldHighlightingDirective);

    function TooltipMessage($injector: ng.auto.IInjectorService, $filter: ng.IFilterService): Function {
        return (result: ValidationResult, resourceServiceName: string, fieldName: string, customFilterName: string = null) => {
            var message: string = null;

            if (result
                && result.message
                && (!fieldName || (fieldName && fieldName === result.fieldName))) {
                var resourceService: ResourceService = $injector.get<ResourceService>(resourceServiceName);
                var params: string[] = (result.msgParams || []).map((param: any) => {
                    if (typeof param === "number") {
                        var value = $filter("number")(param);

                        if (customFilterName) {
                            value = $filter(customFilterName)(value);
                        }

                        return value;
                    } else {
                        return param;
                    }
                });

                if (resourceService) {
                    params.unshift(result.message)
                    message = resourceService.getFormattedMessage.apply(resourceService, params);
                }
            }

            return message;
        };
    }

    TooltipMessage.$inject = ["$injector", "$filter"];
    ModuleRegister.filter("tooltipMessage", TooltipMessage);
}