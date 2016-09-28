/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    export class CopyingState {
        public messageKey: string;
        public type: string;

        public isFailed(): boolean {
            return !!this.messageKey || this.type === "error";
        }

        public succeed(): void {
            this.messageKey = "CopiedSuccessfully";
            this.type = "success";
        }

        public fail(): void {
            this.messageKey = "CopyingFailed";
            this.type = "error";
        }

        public none(): void {
            this.messageKey = null;
            this.type = "none";
        }
    }

    export interface ICopySettingScope extends ng.IScope {
        copy: () => void;
        copyingState: CopyingState;
        settingTypeId: number;
        settingItems: Array<Array<DefaultSettingItem>>;
    }

    export class CopySettingDirective implements ng.IDirective {
        public static $inject: string[] = ["copyingSettingService"];

        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/CopySetting/copy-setting.htm";

        public get scope(): any {
            return {
                settingTypeId: "=",
                settingItems: "="
            };
        }

        public controller: any = ["$scope", ($scope: ICopySettingScope) => {
            $scope.copyingState = new CopyingState();

            this.copyingSettingService.register($scope);

            $scope.copy = () => {
                this.copyingSettingService.hideSportCopyingError($scope);

                if ($scope.settingItems && $scope.settingItems.length > 1) {
                    var items = ArrayHelpers.join2DimensionalArray($scope.settingItems);
                    var source = items[0];
                    var targets = items.slice(1);

                    if (source.validate()) {
                        var copyingResult = source.copySettingTo(targets);

                        if (!copyingResult.status) {
                            $scope.copyingState.fail();
                        } else if (copyingResult.numberOfCopiedItems > 0) {
                            $scope.copyingState.succeed();
                        } else {
                            $scope.copyingState.none();
                        }
                    }
                }
            };
        }];

        protected copyingSettingService: CopyingSettingService;

        constructor(copyingSettingService: CopyingSettingService) {
            this.copyingSettingService = copyingSettingService;
        }
    }

    ModuleRegister.directive("copySetting", CopySettingDirective);

    class CopyingSettingService {
        public static $inject: string[] = ["$rootScope"];

        private scopes: Array<ICopySettingScope>;

        private $rootScope: ng.IRootScopeService;

        constructor($rootScope: ng.IRootScopeService) {
            this.$rootScope = $rootScope;
            this.scopes = new Array<ICopySettingScope>();

            this.$rootScope.$on("updateSetting", () => {
                this.scopes.forEach((registeredScope: ICopySettingScope) => {
                    if (registeredScope.copyingState.isFailed()) {
                        var items: Array<DefaultSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();

                        items.forEach((item: DefaultSettingItem) => {
                            item.resetValidationState();
                        });
                    }
                });
            });
        }

        public register(scope: ICopySettingScope): void {
            var isExisting: boolean = this.scopes.filter((registeredScope: ICopySettingScope) => {
                return registeredScope.$id === scope.$id;
            }).length > 0;

            if (!isExisting) {
                this.scopes.push(scope);
            }
        }

        public hideSportCopyingError(scope: ICopySettingScope): void {
            var settingTypeId: number = scope.settingTypeId;

            this.scopes.forEach((registeredScope: ICopySettingScope) => {
                if (registeredScope.copyingState.isFailed()) {
                    var registeredSettingTypeId: number = registeredScope.settingTypeId;

                    if (registeredSettingTypeId === settingTypeId) {
                        var items: Array<DefaultSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();
                        items.forEach((item: DefaultSettingItem) => {
                            item.resetValidationState();
                        });
                    }
                }
            });
        }
    }

    ModuleRegister.service("copyingSettingService", CopyingSettingService);
}