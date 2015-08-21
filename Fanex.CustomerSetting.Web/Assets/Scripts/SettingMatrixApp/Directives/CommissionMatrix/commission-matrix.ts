/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {

    export class CommissionMatrixDirective implements ng.IDirective {
        public static $inject: string[] = ["appConfigService"];

        public restrict: string;
        public templateUrl: string;
        public scope =
        {
            commissionMatrix: '=settingTypeMatrix'
        };

        private appConfigService: AppConfigService;

        constructor(appConfigService: AppConfigService) {
            this.appConfigService = appConfigService;
            this.restrict = "A";
            this.templateUrl = this.appConfigService.resolveRootUrl("app/customerSettingMatrix/directives/commissionmatrix/commission-matrix.htm");
        }
    }

    ModuleRegister.directive("commissionMatrix", CommissionMatrixDirective);

    /*Commssion Matrix Models*/

    export class CommissionItemMatrixRow {
        public customerLevel: Level;
        public commRate: number;

        constructor(commissionItemMatrixRow) {
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

        public getHeaderName(settingLevel: number) {
            switch (settingLevel) {
                case SettingLevel.product:
                    return "Commission";
                case SettingLevel.sportType:
                    return this.sportTypeName;
                case SettingLevel.betType:
                case SettingLevel.sportTypeBetType:
                    return this.betTypeName;
            }

            return "";
        }
    }

    export class CommissionMatrix extends SettingTypeMatrix {
        public commissionItemMatrices: Array<CommissionItemMatrix>;
        public customerLevels: Array<Level>;

        constructor(commissionMatrix) {
            super(commissionMatrix);

            this.commissionItemMatrices = commissionMatrix.commissionItemMatrices.map((itemMatrix: any) => new CommissionItemMatrix(itemMatrix));
        }

        public convertToDisplay() {
            this.customerLevels = this.commissionItemMatrices[0].commissionItemMatrixRows.map((row: CommissionItemMatrixRow) => row.customerLevel);

            //// super: commRate
            //// master: commRate
            //// agent: commRate
            //// member: commRate

            switch (this.settingLevel) {
                case SettingLevel.product:
                    break;
                case SettingLevel.sportType:
                    break;
                case SettingLevel.betType:
                    break;
                case SettingLevel.sportTypeBetType:
                    break;
            }
        }
    }

}