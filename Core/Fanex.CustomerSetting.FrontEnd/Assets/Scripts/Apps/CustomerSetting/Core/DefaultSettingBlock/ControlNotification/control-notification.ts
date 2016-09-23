/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../resource/resource-translation.ts" />
/// <reference path="../../helpers.ts" />
/// <reference path="../../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    interface IControlNotification {
        message: string;
        type: NotificationType,
        fieldName: string;
        validationState: ValidationResult;
        highlightState: NotificationType[];
        resourceServiceName: string;
        customFilterName: string;
    }

    interface IControlNotificationScope extends ng.IScope {
        controlNotification: IControlNotification;
        displayingMessage: string;
        type: string;
    }

    class ControlNotificationDirective implements ng.IDirective {
        public static $inject: string[] = ["$window", "$compile", "$timeout", "$templateCache"];

        public restrict: string = "A";
        public priority: number = 100;

        public get scope(): any {
            return {
                controlNotification: "="
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: IControlNotificationScope, element: ng.IAugmentedJQuery) => {
            var template: {} = this.$templateCache.get("DefaultSettingBlock/ControlNotification/tooltip.htm");
            var tooltip: ng.IAugmentedJQuery = angular.element(template);
            var debounceResize: any = debounce(() => {
                if (!tooltip.hasClass("ng-hide")) {
                    this.calculateTooltipPosition(element, tooltip);
                }
            }, 100);

            this.$compile(tooltip)(scope);
            element.parent().prepend(tooltip);

            scope.$watchGroup(["controlNotification.message", "controlNotification.validationState"], () => {
                // @@TODO: do we need to keep track of state of error field?

                this.detechDisplayingMessage(scope);
                this.resetElementBorder(element);

                if (scope.type) {
                    element.addClass("setting-field--" + scope.type);
                }
            });

            scope.$watchCollection("controlNotification.highlightState", () => {
                var fieldHighlightState = scope.controlNotification.highlightState[scope.controlNotification.fieldName];
                var style: string = fieldHighlightState && fieldHighlightState !== NotificationType.none
                    ? NotificationType[fieldHighlightState]
                    : null;

                if (style) {
                    this.resetElementBorder(element);
                    element.addClass("setting-field--" + style);
                }
            });

            element.on("change", () => {
                // @@TODO: need to hide the tooltip when user changes data of field? 
                var fieldHighlightState = scope.controlNotification.highlightState[scope.controlNotification.fieldName];

                if (fieldHighlightState && fieldHighlightState !== NotificationType.none && tooltip.hasClass("ng-hide")) {
                    this.resetElementBorder(element);
                    scope.controlNotification.highlightState[scope.controlNotification.fieldName] = NotificationType.none;
                }
            });

            angular.element(this.$window).on("resize scroll", debounceResize);
        };

        private $window: ng.IWindowService;
        private $compile: ng.ICompileService;
        private $timeout: ng.ITimeoutService;
        private $templateCache: ng.ITemplateCacheService;
        private $injector: ng.auto.IInjectorService;
        private $filter: ng.IFilterService;

        constructor(
            $window: ng.IWindowService,
            $compile: ng.ICompileService,
            $timeout: ng.ITimeoutService,
            $templateCache: ng.ITemplateCacheService,
            $injector: ng.auto.IInjectorService,
            $filter: ng.IFilterService) {
            this.$window = $window;
            this.$compile = $compile;
            this.$timeout = $timeout;
            this.$templateCache = $templateCache;
            this.$injector = $injector;
            this.$filter = $filter;
        }

        private calculateTooltipPosition(target: ng.IAugmentedJQuery, tooltip: ng.IAugmentedJQuery): void {
            this.$timeout(() => {
                var elementRect: ClientRect = target[0].getBoundingClientRect();
                var tooltipRect: ClientRect = tooltip[0].getBoundingClientRect();
                var scrollX: number = this.$window.pageXOffset;
                var scrollY: number = this.$window.pageYOffset;
                var top: number = Math.max(0, elementRect.top - tooltipRect.height + scrollY + 5);
                var left: number = Math.max(0, elementRect.left + (elementRect.width - tooltipRect.width) / 2 + scrollX);

                if (this.$window.innerWidth < left + tooltipRect.width) {
                    left = elementRect.left - tooltipRect.width + elementRect.width / 2;
                    left = Math.min(left, this.$window.innerWidth - tooltipRect.width - 5);
                    tooltip.addClass("tooltip--right");
                }

                if (top !== tooltipRect.top || left !== tooltipRect.left) {
                    tooltip.css({
                        "top": top + "px",
                        "left": left + "px"
                    });
                }
            });
        }

        private detechDisplayingMessage(scope: IControlNotificationScope): void {
            var config: IControlNotification = scope.controlNotification;

            if (config && config.message && config.message.length > 0) {
                scope.displayingMessage = config.message;
                scope.type = NotificationType[config.type] || "success";
            } else if (config.validationState && !config.validationState.isSuccessful) {
                scope.displayingMessage = this.getErrorMessage(config);
                scope.type = "error";
            } else {
                scope.displayingMessage = null;
            }
        }

        private getErrorMessage(config: IControlNotification): string {
            var errorMessage: string = null;

            if (config.validationState && (!config.fieldName || config.fieldName === config.validationState.fieldName)) {
                var resourceService: ResourceService = this.$injector.get<ResourceService>(config.resourceServiceName);
                var params: string[] = (config.validationState.msgParams || []).map((param: any) => {
                    if (typeof param === "number") {
                        var value = this.$filter("number")(param);

                        if (config.customFilterName) {
                            value = this.$filter(config.customFilterName)(value);
                        }

                        return value;
                    } else {
                        return param;
                    }
                });

                if (resourceService) {
                    params.unshift(config.validationState.message)
                    errorMessage = resourceService.getFormattedMessage.apply(resourceService, params);
                }
            }

            return errorMessage;
        }

        private resetElementBorder(element: ng.IAugmentedJQuery): void {
            if (element.hasClass("setting-field--error")) {
                element.remove("setting-field--error");
            }

            if (element.hasClass("setting-field--warning")) {
                element.remove("setting-field--success");
            }

            if (element.hasClass("setting-field--success")) {
                element.remove("setting-field--success");
            }
        }
    }

    ModuleRegister.directive("controlNotification", ControlNotificationDirective);
}