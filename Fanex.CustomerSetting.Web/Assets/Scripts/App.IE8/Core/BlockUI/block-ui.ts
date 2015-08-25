/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    class BlockUiDirective implements ng.IDirective {
        public replace: boolean = true;

        public scope: any = {
            isLoading: "="
        };

        public templateUrl: string = "BlockUI/block-ui.htm";
    }

    ModuleRegister.directive("blockUi", BlockUiDirective);
}