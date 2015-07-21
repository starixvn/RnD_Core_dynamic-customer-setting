/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="constants.ts" />
/// <reference path="module-registration.ts" />

module CustomerSetting {
    @service
    export class ResourceService {
        private label: any;
        private message: any;

        constructor(private customerSettingResource: any) {
            this.setLanguage(DefaultAppSettings.language);
        }

        public setLanguage(language: string): void {
            language = language.toLowerCase();

            this.label = this.customerSettingResource["label"][language];
            this.message = this.customerSettingResource["message"][language] || {};

            if (ng.isUndefined(this.label)) {
                this.label = this.customerSettingResource["label"][DefaultAppSettings.language];
            }

            if (ng.isUndefined(this.label)) {
                this.label = {};
            }

            if (ng.isUndefined(this.message)) {
                this.message = this.customerSettingResource["message"][DefaultAppSettings.language];
            }

            if (ng.isUndefined(this.message)) {
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

            if (angular.isDefined(value)) {
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

    @filter
    export class ResourceTranslationFilter {
        constructor(resourceService: ResourceService) {
            return (name: string) => {
                return resourceService.getLabel(name);
            }
        }
    }
}