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
            ptMatrix: '=settingTypeMatrix'
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

        constructor(ptMatrix: any) {
            super(ptMatrix);

            this.isAllowMasterAutoPT = ptMatrix.isAllowMasterAutoPT;
            this.ptItemMatrices = ptMatrix.ptItemMatrices.map((itemMatrix: any) => new PTItemMatrix(itemMatrix));
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
    }

    export class PTItemMaxtrixRow {
        public customerLevel: Level;

        public superPT: number;
        public autoSuperPT: number;
        public isAutoSuperPT: boolean;

        public masterPT: number;
        public autoMasterPT: number;
        public isAutoMasterPT: boolean;

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

            this.agentPT = ptItemMaxtrixRow.agentPT;

            this.minPT = ptItemMaxtrixRow.minPT;
        }
    }
}