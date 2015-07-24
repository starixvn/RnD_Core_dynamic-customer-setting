module CustomerSetting {
    export enum Level {
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

    export enum MessageType {
        success = 0,
        warning = 1,
        error = 2
    }

    export class ErrorCodes {
        public static get internalError(): number {
            return -1;
        }

        public static get noChange(): number {
            return -2;
        }

        public static get accessDenied(): number {
            return -3;
        }
    }

    export class CommonErrorMessages {
        public static negativeParam(paramName: string): string {
            return paramName + " must be greater than zero.";
        }

        public static invalidParam(paramName: string): string {
            return paramName + " has taken an invalid value.";
        }

        public static wrongCustomerLevel(): string {
            return "Wrong customer level.";
        }

        public static customerIdsNotMatchCustomerNames(): string {
            return "The number of customer IDs does not match with the number of customer names.";
        }
    }

    export class DefaultAppSettings {
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
    }
}