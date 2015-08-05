/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="settingtype1-template1.ts" />

module CustomerSetting {
    interface AdjustMemberPtModalScope extends ng.IScope {
        isShown: boolean;
        divisor: number;
        selectedValue: number;
        range: Array<KeyValuePair<number, number>>;
        update: (memberPt: number) => void;
        ok: () => void;
        cancel: () => void;
    }

    class AdjustMemberPtModalDirective implements ng.IDirective {
        public replace: boolean = true;
        public templateUrl: string = "adjust-member-pt-modal.html";

        public link: ng.IDirectiveLinkFn = (scope: AdjustMemberPtModalScope) => {
            this.adjustMemberPtModalService.setup(scope);
        }

        constructor(private adjustMemberPtModalService: AdjustMemberPtModalService) { }
    }

    angular.module("customerSetting").directive("adjustMemberPtModal", ["adjustMemberPtModalService", function () {
        var args = Array.prototype.concat.apply([null], arguments);

        return new (Function.prototype.bind.apply(AdjustMemberPtModalDirective, args));
    }]);

    export class AdjustMemberPtModalService {
        private modalScope: AdjustMemberPtModalScope;

        constructor(private valueRangeGenerator: ValueRangeGenerator) { }

        public setup(scope: AdjustMemberPtModalScope): void {
            if (angular.isUndefined(scope)) {
                throw "Scope of the modal must not be undefied";
            }

            this.modalScope = scope;
            this.modalScope.divisor = Settingtype1Template1Config.roundingFactor;
            this.modalScope.cancel = () => {
                this.modalScope.isShown = false;
            }
            this.modalScope.ok = () => {
                this.modalScope.update(this.modalScope.selectedValue);
                this.modalScope.isShown = false;
            }
        }

        public show(max: number, update: (memberPt: number) => void): void {
            if (this.modalScope.isShown) {
                this.modalScope.isShown = false;
            }

            this.modalScope.selectedValue = 0;
            this.generateRange(max);
            this.modalScope.isShown = true;
            this.modalScope.update = update;
        }

        private generateRange(max: number): void {
            this.modalScope.range = this.valueRangeGenerator.generateRanges(
                this.modalScope.selectedValue,
                0,
                max,
                Settingtype1Template1Config.step,
                Settingtype1Template1Config.roundingFactor);
        }
    }

    angular.module("customerSetting").service("adjustMemberPtModalService", ["valueRangeGenerator", AdjustMemberPtModalService]);
}