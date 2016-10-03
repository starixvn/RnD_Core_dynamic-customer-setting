/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../../typings/underscore/underscore.d.ts" />
/// <reference path="../../module-registrater.ts" />
/// <reference path="togglable-setting-block-shared-states.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export interface IToggleSettingBlockScope extends ng.IScope {
        blockIndex: number;
        sharedStates: TogglableSettingBlockSharedStates;
        settingData: DefaultSettingItem[];
        settingTypeId: number;
    }

    export class ToggleSettingBlockDirective implements ng.IDirective {
        public static $inject: string[] = ["$rootScope", "$timeout"];

        public scope: any = {
            blockIndex: "=",
            sharedStates: "=",
            settingData: "=",
            settingTypeId: "="
        };

        private initializing: boolean = true;

        public link: ng.IDirectiveLinkFn = (scope: IToggleSettingBlockScope, element: ng.IAugmentedJQuery) => {
            var container: ng.IAugmentedJQuery = angular.element(element.parent());

            container.addClass("togglable-bar");
            container.on("click", (event: BaseJQueryEventObject) => {
                var target: ng.IAugmentedJQuery = angular.element(event.target);

                if (target.hasClass("togglable-bar")) {
                    this.toggle(scope);

                    return false;
                }
            });
            element.addClass("ng-hide");

            if (!this.isToggleAllDirective(scope)) {
                scope.$watch("settingData", () => {
                    if (this.initializing) {
                        this.$timeout(() => this.initializing = false);
                    } else if (!scope.sharedStates.isExpanded(scope.blockIndex)) {
                        scope.sharedStates.expandBlock(scope.blockIndex);
                        this.triggerPageHeightChangedEvent();
                    }
                }, true);

                this.$rootScope.$on("updateSetting", (event: ng.IAngularEvent, settingTypeId: number) => {
                    if ((!settingTypeId || settingTypeId === scope.settingTypeId)
                        && scope.settingData
                        && _.some(scope.settingData, (item: DefaultSettingItem) => !item.validationState.isSuccessful())
                        && !scope.sharedStates.isExpanded(scope.blockIndex)) {
                        scope.sharedStates.expandBlock(scope.blockIndex);
                        this.triggerPageHeightChangedEvent();
                    }
                });
                this.$rootScope.$on("queriedSetting", () => {
                    this.initializing = true;
                });
            }
        }

        private $rootScope: ng.IRootScopeService;
        private $timeout: ng.ITimeoutService;

        constructor($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService) {
            this.$rootScope = $rootScope;
            this.$timeout = $timeout;
        }

        private toggle(scope: IToggleSettingBlockScope): void {
            if (this.isToggleAllDirective(scope)) {
                if (scope.sharedStates.isExpandedAll()) {
                    scope.sharedStates.collapseAllBlocks();
                } else {
                    scope.sharedStates.expandAllBlocks();
                }
            } else {
                if (scope.sharedStates.isExpanded(scope.blockIndex)) {
                    scope.sharedStates.collapseBlock(scope.blockIndex);
                } else {
                    scope.sharedStates.expandBlock(scope.blockIndex);
                }
            }

            scope.$apply();
            this.triggerPageHeightChangedEvent();
        };

        private isToggleAllDirective(scope: IToggleSettingBlockScope): boolean {
            return scope.blockIndex === null || scope.blockIndex === undefined;
        }

        private triggerPageHeightChangedEvent(): void {
            this.$rootScope.$emit("page-height-changed");
        }
    }

    ModuleRegister.directive("toggleSettingBlock", ToggleSettingBlockDirective);
}