/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="module-registration.ts" />

module CustomerSetting {
    @directive
    export class BlockUiDirective {
        public get restrict(): string {
            return "A";
        }

        public get template(): string {
            return "<div class=\"block-overlay\" data-ng-show=\"hasLoading\"></div><div class=\"block-loading\" data-ng-show=\"hasLoading\"></div>";
        }

        public get replace(): boolean {
            return true;
        }
    }

    interface IBlockUiScope extends ng.IScope {
        isLoading: boolean;
    }
}