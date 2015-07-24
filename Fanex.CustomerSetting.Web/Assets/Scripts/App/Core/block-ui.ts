/// <reference path="../../../typings/angularjs/angular.d.ts" />

module CustomerSetting {
    export class BlockUiDirective {
        public get restrict(): string {
            return "A";
        }

        public get scope(): any {
            return {
                isLoading: "="
            }
        }

        public get template(): string {
            return "<div class=\"block-overlay\" data-ng-show=\"hasLoading\"></div><div class=\"block-loading\" data-ng-show=\"hasLoading\"></div>";
        }

        public get replace(): boolean {
            return true;
        }

        public link: ng.IDirectiveLinkFn = (scope: IBlockUiScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any): void => {
        }
    }

    interface IBlockUiScope extends ng.IScope {
        isLoading: boolean;
    }
}