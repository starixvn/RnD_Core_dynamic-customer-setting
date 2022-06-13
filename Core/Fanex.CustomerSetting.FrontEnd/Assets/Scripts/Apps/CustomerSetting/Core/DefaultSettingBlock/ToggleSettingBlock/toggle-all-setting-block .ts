/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../module-registrater.ts" />
/// <reference path="togglable-setting-block-shared-states.ts" />

module CustomerSetting {
    "use strict";

    export interface IToggleAllSettingBlockScope extends ng.IScope {
        sharedStates: TogglableSettingBlockSharedStates;
    }

    export class ToggleAllSettingBlockDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/ToggleSettingBlock/toggle-all-setting-block.htm";
        public static $inject: string[] = ["$rootScope", "$timeout"];

        public get scope(): any {
            return {
                sharedStates: "=",
            };
        }

        private initializing: boolean = true;

        public link: ng.IDirectiveLinkFn = (scope: IToggleAllSettingBlockScope, element: ng.IAugmentedJQuery) => {
            var container: ng.IAugmentedJQuery = angular.element(element.parent());
            container.addClass("togglable-bar");
            container.on("click", (event: BaseJQueryEventObject) => {
                var target: ng.IAugmentedJQuery = angular.element(event.target);

                if (target.hasClass("togglable-bar")) {
                    this.toggle(scope);

                    return false;
                }
            });

            this.$rootScope.$on("toggleSettingType", (event: ng.IAngularEvent, isExpandAll: boolean, settingTypeIndex: number) => {
                if (isExpandAll) {
                    scope.sharedStates.expandBlock(settingTypeIndex);
                }
                else {
                    scope.sharedStates.collapseBlock(settingTypeIndex);
                }
            });
        }

        private $rootScope: ng.IRootScopeService;
        private $timeout: ng.ITimeoutService;

        constructor($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService) {
            this.$rootScope = $rootScope;
            this.$timeout = $timeout;
        }

        private toggle(scope: IToggleAllSettingBlockScope): void {
            if (scope.sharedStates.isExpandedAll()) {
                scope.sharedStates.collapseAllBlocks();
            } else {
                scope.sharedStates.expandAllBlocks();
            }

            this.$rootScope.$emit("toggleAllSettingTypes", scope.sharedStates.isExpandedAll());

            scope.$apply();
            this.triggerPageHeightChangedEvent();
        };

        private triggerPageHeightChangedEvent(): void {
            this.$rootScope.$emit("page-height-changed");
        }
    }

    ModuleRegister.directive("toggleAllSettingBlock", ToggleAllSettingBlockDirective);
}