
module CustomerSetting {
    "use strict";
    export interface IUpdateDataCheckboxScope extends ng.IScope {
        click: () => void;
        settingItem: CopyWithCheckboxSettingItem,
        isMinPt: boolean;
        isFirst: boolean;
    }


    export class UpdateDataCheckboxDirective implements ng.IDirective {
        public templateUrl: string = "CopyWithCheckBoxSettingBlock/update-data-checkbox.htm";
        public replace: boolean = true;

        public get scope(): any {
            return {
                settingItem: '=',
                isMinPt: '=',
                isFirst: '='
            };
        };

        public controller: any = ["$scope", ($scope: IUpdateDataCheckboxScope) => {
            $scope.click = () => {
                $scope.settingItem.isRowCopied = false;

                if ($scope.isMinPt) {
                    $scope.settingItem.isCheckedUpdateMinPtOnly = !$scope.settingItem.isCheckedUpdateMinPtOnly;
                } else {
                    $scope.settingItem.isCheckedUpdateRow = !$scope.settingItem.isCheckedUpdateRow;
                    $scope.settingItem.isCheckedUpdateMinPtOnly = $scope.settingItem.isCheckedUpdateRow;
                }
            };
        }];
    }

    ModuleRegister.directive("updateDataCheckbox", UpdateDataCheckboxDirective);
}