module CustomerSettingMatrix {
    export enum Level {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    export class ProductName {
        public productId: number;
        public name: string;

        constructor(product: any) {
            this.productId = product.productId;
            this.name = product.name;
        }
    }

    export class Customer {
        public id: number;
        public name: string;
        public currency: number;
        public level: Level;

        constructor(customer: any) {
            this.id = customer.id;
            this.name = customer.name;
            this.currency = customer.currency;
            this.level = customer.level.value;
        }
    }

    export class SettingItemMatrix {
        public betTypeId: number;
        public betTypeName: string;
        public sportTypeId: number;
        public sportTypeName: string;

        constructor(settingItemMatrix: any) {
            this.betTypeId = settingItemMatrix.betTypeId;
            this.betTypeName = settingItemMatrix.betTypeName;
            this.sportTypeId = settingItemMatrix.sportTypeId;
            this.sportTypeName = settingItemMatrix.sportTypeId;
        }
    }

    export class SettingTypeMatrix {
        public settingLevel: number;

        constructor(settingTypeMatrix: any) {
            this.settingLevel = settingTypeMatrix.settingLevel;
        }
    }

    export class SettingType {
        public id: number;
        public name: string;

        constructor(settingTypeId: number, settingTypeName: string) {
            this.id = settingTypeId;
            this.name = settingTypeName;
        }
    }

    /*Commssion Matrix Models*/
    export class CommissionItemMatrixRow {
        public customerLevel: Level;
        public commRate: number;

        constructor(commissionItemMatrixRow) {
            this.customerLevel = commissionItemMatrixRow.customerLevel.value;
            this.commRate = commissionItemMatrixRow.commRate;
        }
    }

    export class CommissionItemMatrix extends SettingItemMatrix {
        public commissionItemMatrixRows: Array<CommissionItemMatrixRow>;

        constructor(commissionItemMatrix: any) {
            super(commissionItemMatrix);

            this.commissionItemMatrixRows = commissionItemMatrix.commissionItemMatrixRows;
        }
    }

    export class CommissionMatrix extends SettingTypeMatrix {
        public commissionItemMatrices: Array<CommissionItemMatrix>;

        constructor(commissionMatrix) {
            super(commissionMatrix);

            this.commissionItemMatrices = commissionMatrix.commissionItemMatrices;
        }
    }

    /*Position Taking Matrix Models*/

    export class PTMatrix extends SettingTypeMatrix {
        public isAllowMasterAutoPT: boolean;
        public pTItemMatrices: Array<PTItemMatrix>;

        constructor(pTMatrix: any) {
            super(pTMatrix);

            this.isAllowMasterAutoPT = pTMatrix.isAllowMasterAutoPT;
            this.pTItemMatrices = pTMatrix.pTItemMatrices;
        }
    }

    export class PTItemMatrix extends SettingItemMatrix {
        public pTItemMatrixRows: Array<PTItemMaxtrixRow>;
        public isLive: boolean;

        constructor(pTItemMatrix: any) {
            super(pTItemMatrix);

            this.pTItemMatrixRows = pTItemMatrix.pTItemMatrixRows;
            this.isLive = pTItemMatrix.isLive;
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

        constructor(pTItemMaxtrixRow) {
            this.customerLevel = pTItemMaxtrixRow.customerLevel.value;

            this.superPT = pTItemMaxtrixRow.superPT;
            this.autoSuperPT = pTItemMaxtrixRow.autoSuperPT;
            this.isAutoSuperPT = pTItemMaxtrixRow.isAutoSuperPT;

            this.masterPT = pTItemMaxtrixRow.masterPT;
            this.autoMasterPT = pTItemMaxtrixRow.autoMasterPT;
            this.isAutoMasterPT = pTItemMaxtrixRow.isAutoMasterPT;

            this.agentPT = pTItemMaxtrixRow.agentPT;

            this.minPT = pTItemMaxtrixRow.minPT;
        }
    }
}