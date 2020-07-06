/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../common.ts" />
/// <reference path="../../module-registrater.ts" />

module CustomerSetting {
    "use strict";

    export class CopyingWithCheckboxState {
        public messageKey: string;
        public type: string;
        public positionModel: PositionModel;
        public displayingSeconds: number;

        constructor(positionModel?: PositionModel, displayingSeconds: number = 5) {
            this.positionModel = positionModel || PositionModel.relative;
            this.displayingSeconds = displayingSeconds;
        }

        public isShowingMessage(): boolean {
            return !!this.messageKey || this.type === "none";
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

    export enum CopyMode {
        minPt = 1,
        row = 2,
        group = 3
    }

    export interface ICopySettingWithCheckboxScope extends ng.IScope {
        copy: (isForceCopy: boolean) => void;
        hideTooltip: () => void;
        copyingState: CopyingWithCheckboxState;
        settingTypeId: number;
        settingItems: Array<Array<PositionTalkingPartialMinPt>>;
        isIconActive: boolean;
    }

    export class CopySettingWithCheckboxDirective implements ng.IDirective {
        public static $inject: string[] = ["$rootScope", "copyingSettingWithCheckboxService"];

        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/CopySettingWithCheckbox/copy-setting-with-checkbox.htm";

        public get scope(): any {
            return {
                settingTypeId: "=",
                settingItems: "="
            };
        }

        public controller: any = ["$scope", ($scope: ICopySettingWithCheckboxScope) => {
            $scope.copyingState = new CopyingWithCheckboxState();
            this.copyingSettingWithCheckboxService.register($scope);
            $scope.isIconActive = false;
            this.$rootScope.$on("copySettingType18", () => {
                $scope.copy(true);
            });

            $scope.copy = (isForceCopy: boolean = false) => {
                if ($scope.settingItems && $scope.settingItems.length > 1) {
                    var items = ArrayHelpers.join2DimensionalArray($scope.settingItems);
                    var source = items[0];
                    var targets = items.slice(1);
                    if (!source.isCheckedUpdateRow || (source.isRowCopied && !$scope.isIconActive) || (isForceCopy && !source.isRowCopied))
                        return;
                    if (isForceCopy || !source.isRowCopied) {
                        if (source.validate()) {
                            this.copyingSettingWithCheckboxService.hideTooltips($scope);
                            var copyingResult = source.copySettingTo(targets);
                            if (copyingResult.numberOfCopiedItems > 0) {
                                source.isRowCopied = true;
                                $scope.isIconActive = true;
                            }

                            if (!copyingResult.status) {
                                $scope.copyingState.fail();
                            } else if (copyingResult.numberOfCopiedItems > 0) {
                                $scope.copyingState.succeed();
                            } else {
                                $scope.copyingState.none();
                                return;
                            }
                        }
                    }
                    else {
                        source.isRowCopied = false;
                        $scope.isIconActive = false;
                        source.removeCopyDisabled(targets);
                        this.copyingSettingWithCheckboxService.hideTooltips($scope);
                    }
                }
            };

            $scope.hideTooltip = () => {
                $scope.copyingState.none();
            };
        }];

        protected copyingSettingWithCheckboxService: CopyingSettingWithCheckboxService;

        private $rootScope: ng.IRootScopeService;

        constructor($rootScope: ng.IRootScopeService, copyingSettingWithCheckboxService: CopyingSettingWithCheckboxService) {
            this.copyingSettingWithCheckboxService = copyingSettingWithCheckboxService;
            this.$rootScope = $rootScope;
        }
    }

    ModuleRegister.directive("copySettingWithCheckbox", CopySettingWithCheckboxDirective);

    class CopyingSettingWithCheckboxService {
        public static $inject: string[] = ["$rootScope"];

        private scopes: Array<ICopySettingWithCheckboxScope>;

        private $rootScope: ng.IRootScopeService;

        constructor($rootScope: ng.IRootScopeService) {
            this.$rootScope = $rootScope;
            this.scopes = new Array<ICopySettingWithCheckboxScope>();

            this.$rootScope.$on("updateSetting", () => {
                this.scopes.forEach((registeredScope: ICopySettingWithCheckboxScope) => {
                    if (registeredScope.copyingState.isShowingMessage()) {
                        var items: Array<PositionTalkingPartialMinPt> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();

                        items.forEach((item: PositionTalkingPartialMinPt) => {
                            item.resetHighlightState();
                        });
                    }
                });
            });
        }

        public register(scope: ICopySettingWithCheckboxScope): void {
            var isExisting: boolean = this.scopes.filter((registeredScope: ICopySettingWithCheckboxScope) => {
                return registeredScope.$id === scope.$id;
            }).length > 0;

            if (!isExisting) {
                this.scopes.push(scope);
            }
        }

        public hideTooltips(scope: ICopySettingWithCheckboxScope): void {
            var settingTypeId: number = scope.settingTypeId;

            this.scopes.forEach((registeredScope: ICopySettingWithCheckboxScope) => {
                if (registeredScope.copyingState.isShowingMessage()) {
                    var registeredSettingTypeId: number = registeredScope.settingTypeId;

                    if (registeredSettingTypeId === settingTypeId) {
                        var items: Array<PositionTalkingPartialMinPt> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();
                        items.forEach((item: PositionTalkingPartialMinPt) => {
                            item.resetHighlightState();
                        });
                    }
                }
            });
        }
    }

    ModuleRegister.service("copyingSettingWithCheckboxService", CopyingSettingWithCheckboxService);
}