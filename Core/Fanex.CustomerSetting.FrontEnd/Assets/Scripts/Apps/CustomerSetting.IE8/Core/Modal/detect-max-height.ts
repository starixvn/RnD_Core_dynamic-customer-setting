/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../helpers.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class DetectMaxHeightDirective implements ng.IDirective {
        public static $inject: string[] = ["$window", "$timeout"];

        public restrict: string = "A";

        public link: ng.IDirectiveLinkFn = (scope: ng.IScope, element: ng.IAugmentedJQuery) => {
            var container: ng.IAugmentedJQuery = angular.element(element.parent());
            var debounceResize: any = _.debounce(() => {
                if (!element.hasClass("ng-hide")) {
                    this.calculateHeight(element, container);
                }
            }, 100);

            this.calculateHeight(element, container);
            container.css({ "height": "auto" });
            angular.element(this.$window).on("resize", debounceResize);
        };

        private $window: ng.IWindowService;
        private $timeout: ng.ITimeoutService;

        constructor($window: ng.IWindowService, $timeout: ng.ITimeoutService) {
            this.$window = $window;
            this.$timeout = $timeout;
        }

        private calculateHeight(element: ng.IAugmentedJQuery, container: ng.IAugmentedJQuery): void {
            this.$timeout(() => {
                var targetContainerHeight: number = container.parent()[0].clientHeight;
                var maxHeigth: number = targetContainerHeight * 0.6;
                var marginTop: number = targetContainerHeight * 0.2 - 35;

                element.css({
                    "max-height": maxHeigth + "px"
                });
                container.css({ "margin-top": marginTop + "px" });
            });
        }
    }

    ModuleRegister.directive("detectMaxHeight", DetectMaxHeightDirective);
}