/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../module-registrater.ts" />

module CustomerSetting {
    class BlockUiDirective implements ng.IDirective {
        public replace: boolean = true;

        public get scope(): any {
            return {
                isLoading: "="
            }
        }

        public templateUrl: string = "BlockUI/block-ui.htm";
    }

    ModuleRegister.directive("blockUi", BlockUiDirective);
}