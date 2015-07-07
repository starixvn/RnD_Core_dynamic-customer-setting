module CustomerSetting.Helpers {
    export class BlockUIDirective implements ng.IDirective {
        public static $inject: string[] = ["$compile", "$templateCache"];

        public restrict: string = "A";
        public template: string = "<div class=\"loading-container\" data-ng-show=\"hasLoading\">" +
        "<div class=\"block-overlay\"></div>" +
        "<div class=\"block-loading\"></div>" +
        "</div>";

        constructor(
            private $compile: ng.ICompileService,
            private $templateCache: ng.ITemplateCacheService) { }

        public link: ng.IDirectiveLinkFn = (scope: ng.IScope, element: ng.IAugmentedJQuery, attrs: ng.IAttributes, ctrl: any): void => {
        }
    }

    interface BlockUIScope extends ng.IRootScopeService {
        isLoading: boolean;
    }

    class BlockUIService implements ng.IServiceProvider {
        public static $inject: string[] = ["$rootScop"];

        constructor(private blockUIScope: BlockUIScope) { }

        public setup(scope: BlockUIScope): void {
            if (ng.isDefined(this.blockUIScope)) {
                throw "Only one Block UI is allowed in one app.";
            }

            this.blockUIScope = scope;
        }

        public show(): void {
            if (angular.isUndefined(this.blockUIScope)) {
                throw "Setup is required.";
            }

            this.blockUIScope.isLoading = true;
        }

        public hide(): void {
            if (angular.isUndefined(this.blockUIScope)) {
                throw "Setup is required.";
            }

            this.blockUIScope.isLoading = false;
        }

        public $get(): BlockUIService {
            return this;
        }
    }
} 