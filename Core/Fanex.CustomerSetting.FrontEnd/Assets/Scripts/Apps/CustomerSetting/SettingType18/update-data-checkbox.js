var CustomerSetting;
(function (CustomerSetting) {
    "use strict";
    var UpdateDataCheckboxDirective = /** @class */ (function () {
        function UpdateDataCheckboxDirective() {
            this.templateUrl = "update-data-checkbox.htm";
            this.replace = true;
            this.controller = ["$scope", function ($scope) {
                    $scope.click = function () {
                        $scope.settingItem.isRowCopied = false;
                        if ($scope.isMinPt) {
                            $scope.settingItem.isCheckedUpdateMinPtOnly = !$scope.settingItem.isCheckedUpdateMinPtOnly;
                        }
                        else {
                            $scope.settingItem.isCheckedUpdateRow = !$scope.settingItem.isCheckedUpdateRow;
                            $scope.settingItem.isCheckedUpdateMinPtOnly = $scope.settingItem.isCheckedUpdateRow;
                        }
                    };
                }];
        }
        Object.defineProperty(UpdateDataCheckboxDirective.prototype, "scope", {
            get: function () {
                return {
                    settingItem: '=',
                    isMinPt: '=',
                    isFirst: '='
                };
            },
            enumerable: true,
            configurable: true
        });
        ;
        return UpdateDataCheckboxDirective;
    }());
    CustomerSetting.UpdateDataCheckboxDirective = UpdateDataCheckboxDirective;
    CustomerSetting.ModuleRegister.directive("updateDataCheckbox", UpdateDataCheckboxDirective);
})(CustomerSetting || (CustomerSetting = {}));
//# sourceMappingURL=update-data-checkbox.js.map