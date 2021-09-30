/// <reference path="../../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../../core/module-registrater.ts" />
/// <reference path="../../models.ts" />

module CustomerSetting {
    "use strict";

    interface INoteIconTooltipScope extends ng.IScope {
        checked: boolean;
        tooltipMessage: string;
        showTooltip: Function;
        removeMessage: Function;
        tooltipType: string;
        tooltipMessageBackup: string;
    }

    class NoteIconTooltipDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "DefaultSettingBlock/NoteIcon/note-icon-tooltip.htm";

        public get scope(): any {
            return {
                tooltipMessage: "@?",
                tooltipType: "@?",
                tooltipMessageBackup: "@?"
            };
        }

        public controller: any = ["$scope", ($scope: INoteIconTooltipScope) => {
            $scope.showTooltip = () => {
                $scope.tooltipType = "warning";
                $scope.tooltipMessage = $scope.tooltipMessageBackup;
            };

            $scope.removeMessage = () => {
                $scope.tooltipType = "warning";
                $scope.tooltipMessage = null;
            };
        }];
    }

    ModuleRegister.directive("noteIconTooltip", NoteIconTooltipDirective);
}