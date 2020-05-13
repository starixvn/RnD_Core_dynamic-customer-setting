/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="settingtype17-resource.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface IStakeIconTooltipScope extends ng.IScope {
        checked: boolean;
        items: Array<SettingType17StakeRange>;
        tooltipMessage: string;
        showTooltip: Function;
        removeMessage: Function;
        tooltipType: string;
        tooltipMessageBackup: string;
        maxChoice: number;
    }

    class SettingType17StakeIconTooltipDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "settingtype17-stake-icon-tooltip.htm";

        public get scope(): any {
            return {
                items: "=",
                tooltipMessage: "@?",
                tooltipType: "@?",
                tooltipMessageBackup: "@?",
                maxChoice: "="
            };
        }

        public controller: any = ["$scope", ($scope: IStakeIconTooltipScope) => {
            $scope.showTooltip = () => {
                $scope.tooltipType = "warning";
                $scope.tooltipMessage = $scope.tooltipMessageBackup;
            };

            $scope.removeMessage = () => {
                $scope.tooltipType = "warning";
                $scope.tooltipMessage = null;
            };

            $scope.$watch("items", () => {
                var checkedCount = 0;
                $scope.items.forEach((item: SettingType17StakeRange) => {
                    if (item.selected) {
                        checkedCount++;
                    }
                });

                if (checkedCount >= $scope.maxChoice) {
                    $scope.items.forEach((item: SettingType17StakeRange) => {
                        if (!item.selected) {
                            item.disabled = true;
                        }
                    });
                }
                else {
                    $scope.items.forEach((item: SettingType17StakeRange) => {
                        item.disabled = false;
                    });
                }
            }, true);
        }];
    }

    ModuleRegister.directive("settingType17StakeIconTooltip", SettingType17StakeIconTooltipDirective);
}