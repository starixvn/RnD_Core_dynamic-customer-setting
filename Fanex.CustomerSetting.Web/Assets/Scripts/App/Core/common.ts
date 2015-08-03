/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="helpers.ts" />

module CustomerSetting {
    export enum SettingLevel {
        product = 1,
        sportType = 2,
        betType = 3,
        sportTypeBetType = 4
    }

    export enum MessageType {
        success = 0,
        warning = 1,
        error = 2
    }

    export class Exceptions {
        public static negativeParam(paramName: string): string {
            return StringFormater.format(paramName, "{0} must be greater than zero.");
        }

        public static invalidParams(): string {
            return "The request parameters have get some invalid values.";
        }

        public static invalidParam(paramName: string): string {
            return StringFormater.format(paramName, "{0} has taken an invalid value.");
        }

        public static wrongCustomerLevel(): string {
            return "Wrong customer level.";
        }

        public static customerIdsNotMatchCustomerNames(): string {
            return "The number of customer IDs does not match with the number of customer names.";
        }
    }

    export class Defaults {
        public static get language(): string {
            return "en-US";
        }

        public static get maxNumberValue(): number {
            return 99999999999;
        }

        public static get maxFieldLength(): number {
            return 14;
        }

        public static get percentageDivider(): number {
            return 100;
        }

        public static get displayingMsgTime(): number {
            return 3000;
        }
    }

    export class KeyValuePair<T, K> {
        constructor(
            public key: T,
            public value: K) { }
    }
}