module CustomerSettingMatrix {
    "use strict";

    export class QuerySettingMatrix {
        public settingTypeId: number;
        public productId: number;
        public customerName: string;

        constructor(settingTypeId: number, productId: number, customerName: string) {
            this.settingTypeId = settingTypeId;
            this.productId = productId;
            this.customerName = customerName;
        }
    }
} 