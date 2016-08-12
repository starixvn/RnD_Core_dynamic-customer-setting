/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../../typings/underscore/underscore.d.ts" />
/// <reference path="../../module-registrater.ts" />
/// <reference path="togglable-setting-block-shared-states.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export interface IToggleSettingBlockScope extends ng.IScope {
        toggle: () => void;
        isExpanded: boolean;
        blockIndex: number;
        sharedStates: TogglableSettingBlockSharedStates;
        settingData: DefaultSettingItem[];
        settingTypeId: number;
    }

    export class ToggleSettingBlockDirective implements ng.IDirective {
        public static $inject: string[] = ["$rootScope", "$timeout"];
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/ToggleSettingBlock/toggle-setting-block.htm";

        public scope: any = {
            blockIndex: "=",
            sharedStates: "=",
            settingData: "=",
            settingTypeId: "="
        };

        private initializing: boolean = true;

        public link: ng.IDirectiveLinkFn = (scope: IToggleSettingBlockScope) => {
            scope.toggle = () => {
                if (isNaN(scope.blockIndex)) {
                    if (scope.isExpanded) {
                        scope.sharedStates.collapseAllBlocks();
                    } else {
                        scope.sharedStates.expandAllBlocks();
                    }

                    scope.isExpanded = scope.sharedStates.isExpandedAll();
                } else {
                    if (scope.isExpanded) {
                        scope.sharedStates.collapseBlock(scope.blockIndex);
                    } else {
                        scope.sharedStates.expandBlock(scope.blockIndex);
                    }

                    scope.isExpanded = scope.sharedStates.isExpanded(scope.blockIndex);
                }
            };

            scope.$watch("sharedStates", (sharedStates: TogglableSettingBlockSharedStates) => {
                if (isNaN(scope.blockIndex)) {
                    scope.isExpanded = sharedStates.isExpandedAll();
                } else {
                    scope.isExpanded = scope.sharedStates.isExpanded(scope.blockIndex);
                }
            }, true);

            if (!isNaN(scope.blockIndex)) {
                scope.$watch("settingData", () => {
                    if (this.initializing) {
                        this.$timeout(() => this.initializing = false);
                    } else {
                        scope.sharedStates.expandBlock(scope.blockIndex);
                        scope.isExpanded = true;
                    }
                }, true);

                this.$rootScope.$on("updateSetting", (event: ng.IAngularEvent, settingTypeId: number) => {
                    if (settingTypeId === scope.settingTypeId
                        && scope.settingData
                        && _.some(scope.settingData, (item: DefaultSettingItem) => !item.validationState.isSuccessful())) {
                        scope.sharedStates.expandBlock(scope.blockIndex);
                        scope.isExpanded = true;
                    }
                });
            }
        }

        private $rootScope: ng.IRootScopeService;
        private $timeout: ng.ITimeoutService;

        constructor($rootScope: ng.IRootScopeService, $timeout: ng.ITimeoutService) {
            this.$rootScope = $rootScope;
            this.$timeout = $timeout;
        }
    }

    ModuleRegister.directive("toggleSettingBlock", ToggleSettingBlockDirective);
}