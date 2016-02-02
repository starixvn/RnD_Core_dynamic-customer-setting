module CustomerSettingMatrix {
    "use strict";

    export class QuerySettingMatrix {
        public settingTypeId: number;
        public productId: number;
        public customerName: string;
        public sportTypeId: number;
        public betTypeId: number;
        public isLive: boolean;
        public showFilter: boolean;

        constructor(
            settingTypeId: number,
            productId: number,
            customerName: string,
            sportTypeId: number,
            betTypeId: number,
            isLive: boolean,
            showFilter: boolean) {
            this.settingTypeId = settingTypeId;
            this.productId = productId;
            this.customerName = customerName;
            this.sportTypeId = sportTypeId;
            this.betTypeId = betTypeId;
            this.isLive = isLive;
            this.showFilter = showFilter;
        }
    }
} 