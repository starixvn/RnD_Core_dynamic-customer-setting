/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../core/module-registrater.ts" /> 

module CustomerSetting {
    class BlockUiDirective {
        public static $inject = ["$templateCache"];

        public replace: boolean = true;
        public restrict: string = "A";

        public get scope(): any {
            return {
                isLoading: "="
            }
        }

        public template: Function = () => this.$templateCache.get("BlockUI/block-ui.htm");

        constructor(private $templateCache: ng.ITemplateCacheService) { }
    }

    ModuleRegister.directive("blockUi", BlockUiDirective);
}