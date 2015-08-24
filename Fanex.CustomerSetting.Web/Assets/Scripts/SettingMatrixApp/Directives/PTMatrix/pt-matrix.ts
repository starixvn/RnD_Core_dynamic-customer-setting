/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../helper-services.ts" />
/// <reference path="../../models.ts" />
/// <reference path="../../module-register.ts" />

module CustomerSettingMatrix {

    export class PtMatrix implements ng.IDirective {
        public static $inject: string[] = ["appConfigService"];

        public restrict: string;
        public templateUrl: string;
        public scope =
        {
            ptMatrix: '=settingTypeMatrix',
            productName: '='
        };

        private appConfigService: AppConfigService;

        constructor(appConfigService: AppConfigService) {
            this.appConfigService = appConfigService;
            this.restrict = "A";
            this.templateUrl = this.appConfigService.resolveRootUrl("app/customerSettingMatrix/directives/ptmatrix/pt-matrix.htm");
        }
    }

    ModuleRegister.directive("ptMatrix", PtMatrix);

    export class PTMatrix extends SettingTypeMatrix {
        public isAllowMasterAutoPT: boolean;
        public ptItemMatrices: Array<PTItemMatrix>;
        public ptItemMatrixGroups: Array<PTItemMatrixGroup>;

        constructor(ptMatrix: any) {
            super(ptMatrix);

            this.isAllowMasterAutoPT = ptMatrix.isAllowMasterAutoPT;
            this.ptItemMatrices = ptMatrix.ptItemMatrices.map((itemMatrix: any) => new PTItemMatrix(itemMatrix));
        }

        public convertToDisplay() {
            this.ptItemMatrixGroups = new Array<PTItemMatrixGroup>();

            switch (this.settingLevel) {
                case SettingLevel.product:
                case SettingLevel.sportType:
                case SettingLevel.betType:
                    var ptItemMatrixGroup = new PTItemMatrixGroup();
                    ptItemMatrixGroup.ptItemMatrices = this.ptItemMatrices;
                    ptItemMatrixGroup.settingLevel = this.settingLevel;
                    this.ptItemMatrixGroups.push(ptItemMatrixGroup);
                    break;

                case SettingLevel.sportTypeBetType:
                    var sportId: number = 0;
                    var ptItemLength = this.ptItemMatrices.length;
                    var currentPTMatrixGroup: PTItemMatrixGroup;
                    for (var i = 0; i < ptItemLength; i++) {
                        var ptItem = this.ptItemMatrices[i];
                        if (ptItem.sportTypeId != sportId) {
                            currentPTMatrixGroup = new PTItemMatrixGroup();
                            currentPTMatrixGroup.id = ptItem.sportTypeId;
                            currentPTMatrixGroup.ptItemMatrices = new Array<PTItemMatrix>();
                            currentPTMatrixGroup.name = ptItem.sportTypeName;
                            currentPTMatrixGroup.settingLevel = this.settingLevel;
                            sportId = ptItem.sportTypeId;
                            this.ptItemMatrixGroups.push(currentPTMatrixGroup);
                        }

                        currentPTMatrixGroup.ptItemMatrices.push(ptItem);
                    }

                    break;
            }
        }

        
    }

    export class PTItemMatrix extends SettingItemMatrix {
        public ptItemMatrixRows: Array<PTItemMaxtrixRow>;
        public isLive: boolean;

        constructor(ptItemMatrix: any) {
            super(ptItemMatrix);

            this.ptItemMatrixRows = ptItemMatrix.ptItemMatrixRows.map((itemMatrixRow: any) => new PTItemMaxtrixRow(itemMatrixRow));
            this.isLive = ptItemMatrix.isLive;
        }

        public getItemMatrixName(settingLevel: number, productName: ProductName) {
            switch (settingLevel) {
                case SettingLevel.product:
                    return productName.name + (this.isLive ? " (Live)" : "");
                case SettingLevel.sportType:
                    return this.sportTypeName;
                case SettingLevel.betType:
                case SettingLevel.sportTypeBetType:
                    return this.betTypeName;
            }

            return "";
        }
    }

    export class PTItemMaxtrixRow {
        public customerLevel: Level;

        public superPT: number;
        public autoSuperPT: number;
        public isAutoSuperPT: boolean;

        public masterPT: number;
        public autoMasterPT: number;
        public isAutoMasterPT: boolean;
        public isMasterLevel: boolean;

        public agentPT: number;

        public minPT: number;

        constructor(ptItemMaxtrixRow) {
            this.customerLevel = new Level(ptItemMaxtrixRow.customerLevel);

            this.superPT = ptItemMaxtrixRow.superPT;
            this.autoSuperPT = ptItemMaxtrixRow.autoSuperPT;
            this.isAutoSuperPT = ptItemMaxtrixRow.isAutoSuperPT;

            this.masterPT = ptItemMaxtrixRow.masterPT;
            this.autoMasterPT = ptItemMaxtrixRow.autoMasterPT;
            this.isAutoMasterPT = ptItemMaxtrixRow.isAutoMasterPT;
            this.isMasterLevel = this.customerLevel.value == LevelValue.master;

            this.agentPT = ptItemMaxtrixRow.agentPT;

            this.minPT = ptItemMaxtrixRow.minPT;
        }
    }

    export class PTItemMatrixGroup {
        public id: number;
        public name: string;
        public settingLevel: number;
        public ptItemMatrices: Array<PTItemMatrix>;
    }
}