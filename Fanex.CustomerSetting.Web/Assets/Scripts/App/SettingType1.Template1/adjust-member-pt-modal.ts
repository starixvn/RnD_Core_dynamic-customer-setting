/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/module-registrater.ts" />
/// <reference path="models.ts" />

module CustomerSetting {
    "use strict";

    interface IAdjustMemberPtModalScope extends ng.IScope {
        isShown: boolean;
        divisor: number;
        selectedValue: number;
        range: Array<KeyValuePair<number, number>>;
        updateMemberPt: (memberPt: number) => void;
        ok: () => void;
        cancel: () => void;
    }

    class AdjustMemberPtModalDirective implements ng.IDirective {
        public static $inject: string[] = ["adjustMemberPtModalService"];

        public replace: boolean = true;
        public templateUrl: string = "adjust-member-pt-modal.html";

        public link: ng.IDirectiveLinkFn = (scope: IAdjustMemberPtModalScope) => {
            this.adjustMemberPtModalService.setup(scope);
        };

        private adjustMemberPtModalService: AdjustMemberPtModalService;

        constructor(adjustMemberPtModalService: AdjustMemberPtModalService) {
            this.adjustMemberPtModalService = adjustMemberPtModalService;
        }
    }

    ModuleRegister.directive("adjustMemberPtModal", AdjustMemberPtModalDirective);

    export class AdjustMemberPtModalService {
        public static $inject: string[] = ["valueRangeService"];

        private modalScope: IAdjustMemberPtModalScope;
        private valueRangeService: ValueRangeService;

        constructor(valueRangeService: ValueRangeService) {
            this.valueRangeService = valueRangeService;
        }

        public setup(scope: IAdjustMemberPtModalScope): void {
            if (angular.isUndefined(scope)) {
                throw "Scope of the modal must not be undefied";
            }

            this.modalScope = scope;
            this.modalScope.divisor = Settingtype1Template1Config.roundingFactor;

            this.modalScope.cancel = () => {
                this.modalScope.isShown = false;
            };

            this.modalScope.ok = () => {
                this.modalScope.updateMemberPt(this.modalScope.selectedValue);
                this.modalScope.isShown = false;
            };
        }

        public show(max: number, update: (memberPt: number) => void): void {
            if (this.modalScope.isShown) {
                this.modalScope.isShown = false;
            }

            this.modalScope.selectedValue = 0;
            this.generateRange(max);
            this.modalScope.isShown = true;
            this.modalScope.updateMemberPt = update;
        }

        private generateRange(max: number): void {
            this.modalScope.range = this.valueRangeService.generateRanges(
                this.modalScope.selectedValue,
                0,
                max,
                Settingtype1Template1Config.step,
                Settingtype1Template1Config.roundingFactor);
        }
    }

    ModuleRegister.service("adjustMemberPtModalService", AdjustMemberPtModalService);

    function Unsafe($sce: ng.ISCEService) {
        return (value: string) => {
            return $sce.trustAsHtml(value);
        };
    }

    Unsafe.$inject = ["$sce"];
    ModuleRegister.filter("unsafe", Unsafe);
}