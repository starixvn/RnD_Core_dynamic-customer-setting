/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    export class CommissionMatrixDirective implements ng.IDirective {
        public restrict: string;
        public templateUrl: string;
        public scope =
        {
            commissionMatrix: "=settingTypeMatrix"
        };

        constructor() {
            this.restrict = "A";
            this.templateUrl = "Directives/CommissionMatrix/commission-matrix.htm";
        }
    }

    ModuleRegister.directive("commissionMatrix", CommissionMatrixDirective);

    /*Commssion Matrix Models*/

    export class CommissionItemMatrixRow {
        public customerLevel: Level;
        public commRate: number;

        constructor(commissionItemMatrixRow: any) {
            this.customerLevel = new Level(commissionItemMatrixRow.customerLevel);
            this.commRate = commissionItemMatrixRow.commRate;
        }
    }

    export class CommissionItemMatrix extends SettingItemMatrix {
        public commissionItemMatrixRows: Array<CommissionItemMatrixRow>;

        constructor(commissionItemMatrix: any) {
            super(commissionItemMatrix);

            this.commissionItemMatrixRows = commissionItemMatrix.commissionItemMatrixRows.map((itemMatrixRow: any) => new CommissionItemMatrixRow(itemMatrixRow));
        }

        public getItemMatrixName(settingLevel: number, settingTypeId: number) {
            switch (settingLevel) {
                case SettingLevel.product:
                    return this.getCommissionName(settingTypeId);
                case SettingLevel.sportType:
                    return this.sportTypeName;
                case SettingLevel.betType:
                case SettingLevel.sportTypeBetType:
                    return this.betTypeName;
            }

            return "";
        }

        private getCommissionName(settingTypeId: number) {
            switch (settingTypeId) {
                case SettingType.Commission.id:
                    return SettingType.Commission.name;
                case SettingType.ChargeCommission.id:
                    return SettingType.ChargeCommission.name;
                case SettingType.TicketCommission.id:
                    return SettingType.TicketCommission.name;
            }

            return "";
        }
    }

    export class CommissionMatrix extends SettingTypeMatrix {
        public commissionItemMatrices: Array<CommissionItemMatrix>;
        public customerLevels: Array<Level>;
        public commissionItemMatrixGroups: Array<CommissionItemMatrixGroup>;
        public settingTypeId: number;

        constructor(commissionMatrix: any, settingTypeId: number) {
            super(commissionMatrix);

            this.commissionItemMatrices = commissionMatrix.commissionItemMatrices.map((itemMatrix: any) => new CommissionItemMatrix(itemMatrix));
            this.settingTypeId = settingTypeId;
        }

        public groupDataBySettingLevel() {
            this.customerLevels = this.commissionItemMatrices[0].commissionItemMatrixRows.map((row: CommissionItemMatrixRow) => row.customerLevel);
            this.commissionItemMatrixGroups = new Array<CommissionItemMatrixGroup>();

            switch (this.settingLevel) {
                case SettingLevel.product:
                case SettingLevel.sportType:
                case SettingLevel.betType:
                    var commissionItemMatrixGroup = new CommissionItemMatrixGroup();
                    commissionItemMatrixGroup.commissionItemMatrices = this.commissionItemMatrices;
                    commissionItemMatrixGroup.settingLevel = this.settingLevel;
                    this.commissionItemMatrixGroups.push(commissionItemMatrixGroup);
                    break;

                case SettingLevel.sportTypeBetType:
                    var sportId: number = 0;
                    var commissionItemLength = this.commissionItemMatrices.length;
                    var currentCommissionMatrixGroup: CommissionItemMatrixGroup;
                    for (var i = 0; i < commissionItemLength; i++) {
                        var commissionItem = this.commissionItemMatrices[i];
                        if (commissionItem.sportTypeId !== sportId) {
                            currentCommissionMatrixGroup = new CommissionItemMatrixGroup();
                            currentCommissionMatrixGroup.id = commissionItem.sportTypeId;
                            currentCommissionMatrixGroup.commissionItemMatrices = new Array<CommissionItemMatrix>();
                            currentCommissionMatrixGroup.name = commissionItem.sportTypeName;
                            currentCommissionMatrixGroup.settingLevel = this.settingLevel;
                            sportId = commissionItem.sportTypeId;
                            this.commissionItemMatrixGroups.push(currentCommissionMatrixGroup);
                        }

                        currentCommissionMatrixGroup.commissionItemMatrices.push(commissionItem);
                    }

                    break;
            }
        }
    }

    export class CommissionItemMatrixGroup {
        public id: number;
        public name: string;
        public settingLevel: number;
        public commissionItemMatrices: Array<CommissionItemMatrix>;
    }
}