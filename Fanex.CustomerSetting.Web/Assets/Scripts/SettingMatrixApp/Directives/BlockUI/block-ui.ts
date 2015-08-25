/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    class BlockUiDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                isLoading: "="
            };
        }

        public templateUrl: string = "Directives/BlockUI/block-ui.htm";
    }

    ModuleRegister.directive("blockUi", BlockUiDirective);
}