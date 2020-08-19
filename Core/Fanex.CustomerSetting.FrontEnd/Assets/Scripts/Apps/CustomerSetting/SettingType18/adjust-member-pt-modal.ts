/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/defaultsettingblock/copysetting/copy-setting.ts" />
/// <reference path="../core/settingtype/setting-type-base-directive.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    class PositionTakingOnMemberDirective implements ng.IDirective {
        public static $inject: string[] = ["$q", "updatingSettingTypePromiseChain", "adjustMemberPtModalService"];

        public controller: any = ["$scope", ($scope: ISettingTypeDirectiveSharedScope) => {
            this.updatingSettingTypePromiseChain.register($scope.settingType.id, (settingPartialCommand: SettingPartialCommand) => {
                return this.updatePositionTakingOnMembers(settingPartialCommand);
            });
        }];

        private $q: ng.IQService;

        private updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain;
        private adjustMemberPtModalService: AdjustMemberPtModalService;

        constructor(
            $q: ng.IQService,
            updatingSettingTypePromiseChain: UpdatingSettingTypePromiseChain,
            adjustMemberPtModalService: AdjustMemberPtModalService) {
            this.$q = $q;
            this.updatingSettingTypePromiseChain = updatingSettingTypePromiseChain;
            this.adjustMemberPtModalService = adjustMemberPtModalService;
        }

        public updatePositionTakingOnMembers(settingPartialCommand: SettingPartialCommand): ng.IPromise<void> {
            var deferred: ng.IDeferred<void> = this.$q.defer<void>();

            this.adjustMemberPtModalService.show(
                settingPartialCommand.settingType.settingLevel,
                settingPartialCommand.customerLevel,
                settingPartialCommand.settingType.id,
                settingPartialCommand.settingType.settingItems,
                () => {
                    deferred.resolve();
                });

            return deferred.promise;
        }
    }

    ModuleRegister.directive("positionTakingOnMember", PositionTakingOnMemberDirective);

    interface IAdjustMemberPtModalScope extends ng.IScope {
        customerLevel: Level;
        settingLevel: SettingLevel;
        settingTypeId: number;
        settingItems: ISettingItem[];
        avaiSettingLevels: Object;
        avaiCustomerLevels: Object;
        sportGroups: DefaultSettingItem[][];
        ranges: Array<Array<KeyValuePair<number, number>>>;
        isShown: boolean;
        divisor: number;
        submitChanges: () => void;
        ok: () => void;
        cancel: () => void;
    }

    class AdjustMemberPtModalDirective implements ng.IDirective {
        public static $inject: string[] = ["adjustMemberPtModalService"];

        public get scope(): any {
            return {};
        }

        public replace: boolean = true;
        public templateUrl: string = "adjust-member-pt-modal.htm";

        public link: ng.IDirectiveLinkFn = (scope: IAdjustMemberPtModalScope) => {
            this.adjustMemberPtModalService.setup(scope);

            scope.avaiSettingLevels = SettingLevel;
            scope.avaiCustomerLevels = Level;

            scope.$watch("settingItems", (settingItems: ISettingItem[]) => {
                if (angular.isArray(settingItems)) {
                    if (scope.settingLevel === SettingLevel.sportTypeBetType) {
                        scope.sportGroups = this.groupSettingItemsBySportType(settingItems);
                    } else {
                        scope.sportGroups = <DefaultSettingItem[][]>[settingItems];
                    }
                }
            });
        };

        private adjustMemberPtModalService: AdjustMemberPtModalService;

        constructor(adjustMemberPtModalService: AdjustMemberPtModalService) {
            this.adjustMemberPtModalService = adjustMemberPtModalService;
        }

        private groupSettingItemsBySportType(settingItems: ISettingItem[]): DefaultSettingItem[][] {
            var currentSportTypeId: number = -1;
            var result: Array<Array<DefaultSettingItem>> = new Array<Array<DefaultSettingItem>>();
            var group: Array<DefaultSettingItem>;

            settingItems.forEach((settingItem: ISettingItem) => {
                var item: DefaultSettingItem = <DefaultSettingItem>settingItem;

                if (currentSportTypeId !== item.sportTypeId) {
                    if (group) {
                        result.push(group);
                    }

                    currentSportTypeId = item.sportTypeId;
                    group = new Array<DefaultSettingItem>();
                }

                group.push(item);
            });

            if (group) {
                result.push(group);
            }

            return result;
        }
    }

    ModuleRegister.directive("adjustMemberPtModal", AdjustMemberPtModalDirective);

    class AdjustMemberPtModalService {
        public static $inject: string[] = ["$rootScope", "valueRangeService"];

        private $rootScope: ng.IRootScopeService;
        private modalScope: IAdjustMemberPtModalScope;
        private valueRangeService: ValueRangeService;

        constructor($rootScope: ng.IRootScopeService, valueRangeService: ValueRangeService) {
            this.$rootScope = $rootScope;
            this.valueRangeService = valueRangeService;
        }

        public setup(scope: IAdjustMemberPtModalScope): void {
            if (angular.isUndefined(scope)) {
                throw "Scope of the modal must not be undefied";
            }

            this.modalScope = scope;
            this.modalScope.divisor = SettingType18Config.roundingFactor;

            this.modalScope.cancel = () => {
                this.modalScope.isShown = false;
                this.resolvePromise();
            };

            this.modalScope.ok = () => {
                this.modalScope.isShown = false;

                if (this.modalScope.customerLevel === Level.master) {
                    this.modalScope.settingItems.forEach((item: ISettingItem) => {
                        (<MasterPositionTakingPartialMinPT>item).isUpdateSuperPTOnMembers = true;
                    });
                } else if (this.modalScope.customerLevel === Level.agent) {
                    this.modalScope.settingItems.forEach((item: ISettingItem) => {
                        (<AgentPositionTakingPartialMinPT>item).isUpdateMasterPTOnMembers = true;
                    });
                }

                this.resolvePromise();
            };
        }

        public show(
            settingLevel: SettingLevel,
            customerLevel: Level,
            settingTypeId: number,
            settingItems: Array<ISettingItem>,
            submitChanges: () => void): void {
            if (this.modalScope.isShown) {
                this.modalScope.isShown = false;
            }

            this.modalScope.submitChanges = submitChanges;

            if (this.convertData(customerLevel, settingItems)) {
                this.modalScope.settingLevel = settingLevel;
                this.modalScope.customerLevel = customerLevel;
                this.modalScope.settingTypeId = settingTypeId;
                this.modalScope.isShown = true;
            } else if (this.modalScope.submitChanges) {
                this.modalScope.submitChanges();
            }
        }

        private convertData(customerLevel: Level, settingItems: Array<ISettingItem>): boolean {
            var hasChanged: boolean = false;
            var changedItems: Array<DefaultSettingItem> = new Array<DefaultSettingItem>();

            this.modalScope.ranges = new Array<Array<KeyValuePair<number, number>>>();

            if (customerLevel === Level.master) {
                settingItems.forEach((settingItem: DefaultSettingItem) => {
                    var item: MasterPositionTakingPartialMinPT = <MasterPositionTakingPartialMinPT>settingItem;

                    if (item.isCheckedUpdateRow && !item.iS3 && item.iS3 !== (<MasterPositionTakingPartialMinPT>item.oldSetting).iS3) {
                        changedItems.push(item);
                        this.modalScope.ranges.push(this.valueRangeService.generateRanges(
                            item.superPTOnMembers,
                            0,
                            item.s4 - item.m3,
                            SettingType18Config.step,
                            SettingType18Config.roundingFactor));
                    }
                });
            } else if (customerLevel === Level.agent) {
                settingItems.forEach((settingItem: DefaultSettingItem) => {
                    var item: AgentPositionTakingPartialMinPT = <AgentPositionTakingPartialMinPT>settingItem;

                    if (item.isCheckedUpdateRow &&!item.iM2 && item.iM2 !== (<AgentPositionTakingPartialMinPT>item.oldSetting).iM2) {
                        changedItems.push(item);
                        this.modalScope.ranges.push(this.valueRangeService.generateRanges(
                            item.masterPTOnMembers,
                            0,
                            item.m3 - item.a2,
                            SettingType18Config.step,
                            SettingType18Config.roundingFactor));
                    }
                });
            }

            if (changedItems.length > 0) {
                this.modalScope.settingItems = changedItems;
                hasChanged = true;
            }

            return hasChanged;
        }

        private resolvePromise(): void {
            this.$rootScope.$emit("updateSetting");

            if (this.modalScope.submitChanges) {
                this.modalScope.submitChanges();
            }
        }
    }

    ModuleRegister.service("adjustMemberPtModalService", AdjustMemberPtModalService);

    interface ICopyPositionTakingOnMemberScope extends ICopySettingScope {
        customerLevel: Level;
    }

    class CopyPositionTakingOnMemberDirective extends CopySettingDirective {
        public get scope(): any {
            return {
                customerLevel: "=",
                settingTypeId: "=",
                settingItems: "="
            };
        }

        public controller: any = ["$scope", ($scope: ICopyPositionTakingOnMemberScope) => {
            this.copyingSettingService.register($scope);

            $scope.copyingState = new CopyingState(PositionModel.fixed);
            $scope.copyingState.none();

            $scope.copy = () => {
                this.copyingSettingService.hideTooltips($scope);

                if ($scope.settingItems && $scope.settingItems.length > 1) {
                    var items: DefaultSettingItem[] = ArrayHelpers.join2DimensionalArray($scope.settingItems);
                    var source: DefaultSettingItem = items[0];
                    var targets: DefaultSettingItem[] = items.slice(1);
                    var numberOfCopiedItems: number = 0;
                    var successful: boolean = true;

                    if ($scope.customerLevel === Level.master) {
                        numberOfCopiedItems = targets.filter((item: DefaultSettingItem) => {
                            var validItem: boolean = (<MasterPositionTakingPartialMinPT>item).copyPtOnMembersFrom(<MasterPositionTakingPartialMinPT>source);

                            successful = successful ? validItem : successful;

                            return validItem ? item.hasHighlightField(NotificationType.success) : validItem;
                        }).length;
                    } else if ($scope.customerLevel === Level.agent) {
                        numberOfCopiedItems = targets.filter((item: DefaultSettingItem) => {
                            var validItem: boolean = (<AgentPositionTakingPartialMinPT>item).copyPtOnMembersFrom(<AgentPositionTakingPartialMinPT>source);

                            successful = successful ? validItem : successful;

                            return validItem ? item.hasHighlightField(NotificationType.success) : validItem;
                        }).length;
                    }

                    if (!successful) {
                        $scope.copyingState.fail();
                    } else if (numberOfCopiedItems > 0) {
                        $scope.copyingState.succeed();
                    } else {
                        $scope.copyingState.none();
                    }
                }
            };

            $scope.hideTooltip = () => {
                $scope.copyingState.none();
            };
        }];
    }

    ModuleRegister.directive("copyPositionTakingOnMember", CopyPositionTakingOnMemberDirective);
}