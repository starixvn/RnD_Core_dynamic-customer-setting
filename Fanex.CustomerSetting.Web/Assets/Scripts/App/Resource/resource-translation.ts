/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />

module CustomerSetting {
    export class ResourceService {
        private label: any;
        private message: any;

        public static $inject = ["customerSettingResource"];

        constructor(private customerSettingResource: any) {
            this.setLanguage(Defaults.language);
        }

        public setLanguage(language: string): void {
            if (angular.isDefined(language)) {
                language = language.toLowerCase();
            } else {
                language = Defaults.language;
            }

            this.label = this.customerSettingResource["label"][language];
            this.message = this.customerSettingResource["message"][language];

            if (angular.isUndefined(this.label)) {
                this.label = this.customerSettingResource["label"][Defaults.language];
            }

            if (angular.isUndefined(this.label)) {
                this.label = {};
            }

            if (angular.isUndefined(this.message)) {
                this.message = this.customerSettingResource["message"][Defaults.language];
            }

            if (angular.isUndefined(this.message)) {
                this.message = {};
            }
        }

        public getLabel(key: string): string {
            var value: string = this.label[key];

            if (angular.isUndefined(value)) {
                value = key;
            }

            return value;
        }

        public getMessage(key: string): string {
            var msg: string = this.message[key];

            if (angular.isDefined(msg)) {
                msg = msg.replace(/&apos;|&#x0*27;/g, "'");
            } else {
                msg = key;
            }

            return msg;
        }

        public getFormattedMessage(key: string, ...args: string[]): string {
            var msg: string = this.getMessage(key);

            msg = msg.replace(/{(\d+)}/g, (match, index) => {
                return args[index] !== undefined ? args[index] : match;
            });

            return msg;
        }
    }

    ModuleRegister.service("resourceService", ResourceService);

    export function LabelFilter(resourceService: ResourceService) {
        return (name: string) => {
            return resourceService.getLabel(name);
        }
    }

    LabelFilter.$inject = ["resourceService"];
    ModuleRegister.filter("labelFilter", LabelFilter);
}