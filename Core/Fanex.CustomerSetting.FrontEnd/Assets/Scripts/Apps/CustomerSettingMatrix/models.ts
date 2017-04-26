module CustomerSettingMatrix {
    "use strict";

    export enum LevelValue {
        company = 5,
        super = 4,
        master = 3,
        agent = 2,
        member = 1
    }

    export enum SettingLevel {
        product = 1,
        sportType = 2,
        betType = 3,
        sportTypeBetType = 4
    }

    export class Level {
        public displayName: string;
        public value: number;

        constructor(level: any) {
            this.displayName = level.displayName;
            this.value = level.value;
        }
    }

    export class Product {
        public id: number;
        public name: string;

        constructor(product: any) {
            this.id = product.id;
            this.name = product.name;
        }
    }

    export class Customer {
        public id: number;
        public name: string;
        public currency: number;
        public level: LevelValue;
        public levelName: string;

        constructor(customer: any) {
            this.id = customer.id;
            this.name = customer.name;
            this.currency = customer.currency;
            this.level = customer.level.value;
            this.levelName = customer.level.displayName;
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
            this.sportTypeName = settingItemMatrix.sportTypeName;
        }
    }

    export class SettingTypeMatrix {
        public settingLevel: number;
        public productId: number;
        public productName: string;

        constructor(settingTypeMatrix: any) {
            this.settingLevel = settingTypeMatrix.settingLevel;
            this.productId = settingTypeMatrix.productId;
            this.productName = settingTypeMatrix.productName;
        }

        public get isSportTypeBetTypeSettingLevel(): boolean {
            return this.settingLevel === SettingLevel.sportTypeBetType;
        }

        public groupDataBySettingLevel() {
            throw new Error("groupDataBySettingLevel not implemented");
        }
    }

    export class SettingType {
        static PositionTaking = new SettingType(1, "PositionTaking");
        static Commission = new SettingType(3, "Commission");
        static ChargeCommission = new SettingType(5, "Charge Commission");
        static TicketCommission = new SettingType(14, "Ticket Commission");
        public id: number;
        public name: string;

        constructor(settingTypeId: number, settingTypeName: string) {
            this.id = settingTypeId;
            this.name = settingTypeName;
        }
    }
}