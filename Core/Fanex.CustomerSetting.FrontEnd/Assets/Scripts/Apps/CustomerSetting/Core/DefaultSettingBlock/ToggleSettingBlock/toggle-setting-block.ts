/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../module-registrater.ts" />
/// <reference path="togglable-setting-block-shared-states.ts" />

module CustomerSetting {
    "use strict";

    export interface IToggleSettingBlockScope extends ng.IScope {
        toggle: () => void;
        isExpanded: boolean;
        blockIndex: number;
        sharedStates: TogglableSettingBlockSharedStates;
        settingData: DefaultSettingItem[];
    }

    export class ToggleSettingBlockDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/ToggleSettingBlock/toggle-setting-block.htm";

        public get scope(): any {
            return {
                blockIndex: "=",
                sharedStates: "=",
                settingData: "="
            };
        }

        public link: ng.IDirectiveLinkFn = (scope: IToggleSettingBlockScope) => {
            if (isNaN(scope.blockIndex)) {
                scope.isExpanded = scope.sharedStates.isExpandedAll();
            } else {
                scope.isExpanded = scope.sharedStates.isExpanded(scope.blockIndex);
            }

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

            if (isNaN(scope.blockIndex)) {
                scope.$watch("sharedStates", (sharedStates: TogglableSettingBlockSharedStates) => {
                    scope.isExpanded = sharedStates.isExpandedAll();
                }, true);
            } else {
                scope.$watch("settingData", () => {
                    scope.sharedStates.expandBlock(scope.blockIndex);
                    scope.isExpanded = true;
                }, true);
            }
        }
    }

    ModuleRegister.directive("toggleSettingBlock", ToggleSettingBlockDirective);
}