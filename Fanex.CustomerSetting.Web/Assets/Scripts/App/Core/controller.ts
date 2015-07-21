/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="constants.ts" />
/// <reference path="models.ts" />
/// <reference path="module-registration.ts" />
/// <reference path="resource-translation.ts" />
/// <reference path="service.ts" />

module CustomerSetting {
    @controller
    export class CustomerSettingController {
        constructor(
            private $rootScope: ng.IRootScopeService,
            private resourceTranslation: ResourceTranslationFilter) { }

        public setting: Setting;

        public customerInfo: CustomerInfo;
    }

    class SettingQuery {
        private _customerId: number;
        private _customerLevel: Level;
        private _productId: number;
        private _uplineLevel: Level;

        public get customerId(): number {
            return this._customerId;
        }

        public set customerId(custId: number) {
            if (custId <= 0) {
                throw CommonErrorMessages.negativeParam("customerId");
            }

            this._customerId = custId;
        }

        public get customerLevel(): Level {
            return this._customerLevel;
        }

        public set customerLevel(level: Level) {
            if (level < Level.member || level > Level.super) {
                throw CommonErrorMessages.invalidParam("customerLevel");
            }

            this._customerLevel = level;
        }

        public get productId(): number {
            return this._productId;
        }

        public set productId(productId: number) {
            if (productId <= 0) {
                throw CommonErrorMessages.negativeParam("customerId");
            }

            this._productId = productId;
        }

        public get uplineLevel(): Level {
            return this._uplineLevel;
        }

        public set uplineLevel(level: Level) {
            if (level < Level.agent) {
                throw CommonErrorMessages.invalidParam("uplineLevel");
            }

            this._uplineLevel = level;
        }
    }

    class SettingCommand {
    }

    class PathParamsConverter {
    }
}