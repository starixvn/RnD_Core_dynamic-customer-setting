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
        settingItems: Array<Array<CopyWithCheckboxSettingItem>>;
        settingItemsOneDirection: Array<CopyWithCheckboxSettingItem>;
        isIconActive: boolean;
        isSporttypeLevel: boolean;
        copySporttypeActivated: boolean;
        numOfCopyBettypeActivated: number;
    }

    export class CopySettingWithCheckboxDirective implements ng.IDirective {
        public static $inject: string[] = ["$rootScope", "copyingSettingWithCheckboxService"];

        public replace: boolean = true;
        public templateUrl: string = "CopyWithCheckBoxSettingBlock/CopySettingWithCheckbox/copy-setting-with-checkbox.htm";

        public get scope(): any {
            return {
                settingTypeId: "=",
                settingItems: "=",
                isSporttypeLevel: "=",
                
            };
        }

        public controller: any = ["$scope", ($scope: ICopySettingWithCheckboxScope) => {
            $scope.copyingState = new CopyingWithCheckboxState();
            this.copyingSettingWithCheckboxService.register($scope);
            $scope.isIconActive = false;
            $scope.copySporttypeActivated = false;
            $scope.numOfCopyBettypeActivated = 0;
            $scope.settingItemsOneDirection = ArrayHelpers.join2DimensionalArray($scope.settingItems);
            this.$rootScope.$on("copySettingTypeWithCheckbox" + $scope.settingTypeId, (event: ng.IAngularEvent, bettypeId: number) => {
                if ($scope.isIconActive && $scope.settingItemsOneDirection[0].betTypeId == bettypeId) {
                    $scope.copy(true);
                }
            });
            this.$rootScope.$on("updateSporttypeCopyState" + $scope.settingTypeId, (event: ng.IAngularEvent, copySporttypeActivated: boolean) => {
                if (!$scope.isSporttypeLevel) {
                    $scope.copySporttypeActivated = copySporttypeActivated;
                    $scope.isIconActive = false;
                }
            });
            this.$rootScope.$on("updateBettypeCopyState" + $scope.settingTypeId, (event: ng.IAngularEvent, copyBettypeActivated: boolean) => {
                $scope.numOfCopyBettypeActivated += copyBettypeActivated ? 1 : -1;
            });

            $scope.copy = (isSourceChanged: boolean = false) => {
                if (!$scope.isSporttypeLevel && $scope.copySporttypeActivated)
                    return;
                if ($scope.isSporttypeLevel && $scope.numOfCopyBettypeActivated > 0)
                    return;
                if ($scope.settingItems && $scope.settingItems.length > 1) {
                    //var items = ArrayHelpers.join2DimensionalArray($scope.settingItems);
                    var source = $scope.settingItemsOneDirection[0];
                    var targets = $scope.settingItemsOneDirection.slice(1);
                    //if (!source.isCheckedUpdateRow || (source.isRowCopied && !$scope.isIconActive) || (isSourceChanged && !source.isRowCopied))
                    //    return;
                    if (isSourceChanged || (!$scope.isIconActive && source.isCheckedUpdateRow)) {
                        if (source.validate()) {
                            this.copyingSettingWithCheckboxService.hideTooltips($scope);
                            var copyingResult = source.copySettingTo(targets);
                            if (copyingResult.numberOfCopiedItems > 0) {
                                //source.isRowCopied = true;
                                $scope.isIconActive = true;

                                if (copyingResult.status)
                                    $scope.copyingState.succeed();
                                else
                                    $scope.copyingState.fail();
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

                    if ($scope.isSporttypeLevel) {
                        $scope.copySporttypeActivated = $scope.isSporttypeLevel ? $scope.isIconActive : $scope.copySporttypeActivated;
                        $scope.$emit("updateSporttypeCopyState" + $scope.settingTypeId, $scope.copySporttypeActivated);
                    }
                    else {
                        if (!isSourceChanged)
                            $scope.$emit("updateBettypeCopyState" + $scope.settingTypeId, $scope.isIconActive);
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
                        var items: Array<CopyWithCheckboxSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();

                        items.forEach((item: CopyWithCheckboxSettingItem) => {
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
                        var items: Array<CopyWithCheckboxSettingItem> = ArrayHelpers.join2DimensionalArray(registeredScope.settingItems);

                        registeredScope.copyingState.none();
                        items.forEach((item: CopyWithCheckboxSettingItem) => {
                            item.resetHighlightState();
                        });
                    }
                }
            });
        }
    }

    ModuleRegister.service("copyingSettingWithCheckboxService", CopyingSettingWithCheckboxService);
}