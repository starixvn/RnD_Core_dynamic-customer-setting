/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../../../typings/underscore/underscore.d.ts" />
/// <reference path="../../module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export interface ICopySettingScope extends ng.IScope {
        copy: () => void;
        showError: boolean;
        settingTypeId: number;
        settingItems: Array<Array<DefaultSettingItem>>;
    }

    export class CopySettingDirective implements ng.IDirective {
        public static $inject: string[] = ["copyingSettingService"];

        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/CopySetting/copy-setting.htm";

        public scope: any = {
            settingTypeId: "=",
            settingItems: "="
        };

        public controller: any = ["$scope", ($scope: ICopySettingScope) => {
            this.copyingSettingService.register($scope);

            $scope.showError = false;

            $scope.copy = () => {
                this.copyingSettingService.hideSportCopyingError($scope);

                if ($scope.settingItems && $scope.settingItems.length > 1) {
                    var items = ArrayHelpers.join2DimensionalArray($scope.settingItems);
                    var source = items[0];
                    var targets = items.slice(1);

                    if (source.validate()) {
                        if (!source.copySettingTo(targets)) {
                            $scope.showError = true;
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
                _.forEach(this.scopes, (registeredScope: ICopySettingScope) => {
                    if (registeredScope.showError) {
                        var items: Array<DefaultSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.showError = false;

                        _.forEach(items, (item: DefaultSettingItem) => {
                            item.validationState = undefined;
                        });
                    }
                });
            });
        }

        public register(scope: ICopySettingScope): void {
            var isExisting: boolean = _.filter(this.scopes, (registeredScope: ICopySettingScope) => {
                return registeredScope.$id === scope.$id;
            }).length > 0;

            if (!isExisting) {
                this.scopes.push(scope);
            }
        }

        public hideSportCopyingError(scope: ICopySettingScope): void {
            var settingTypeId: number = scope.settingTypeId;

            _.forEach(this.scopes, (registeredScope: ICopySettingScope) => {
                if (registeredScope.showError) {
                    var registeredSettingTypeId: number = registeredScope.settingTypeId;

                    if (registeredSettingTypeId === settingTypeId) {
                        var items: Array<DefaultSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.showError = false;
                        _.forEach(items, (item: DefaultSettingItem) => {
                            item.validationState = undefined;
                        });
                    }
                }
            });
        }
    }

    ModuleRegister.service("copyingSettingService", CopyingSettingService);
}