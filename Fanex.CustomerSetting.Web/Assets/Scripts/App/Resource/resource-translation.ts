/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/module-registrater.ts" />

module CustomerSetting {
    export class ResourceService {
        private label: any;
        private message: any;

        public static $inject: string[] = ["customerSettingResource"];

        constructor(private customerSettingResource: any) {
            this.setLanguage(Defaults.language);
        }

        public setLanguage(language: string): void {
            var defaultLanguage: string = Defaults.language.toLowerCase();

            if (angular.isDefined(language)) {
                language = language.toLowerCase();
            } else {
                language = defaultLanguage;
            }

            this.label = this.customerSettingResource["label"][language];
            this.message = this.customerSettingResource["message"][language];

            if (angular.isUndefined(this.label)) {
                this.label = this.customerSettingResource["label"][defaultLanguage];
            }

            if (angular.isUndefined(this.label)) {
                this.label = {};
            }

            if (angular.isUndefined(this.message)) {
                this.message = this.customerSettingResource["message"][defaultLanguage];
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

        public getFormattedMessage(key: string, ...args: any[]): string {
            var msg: string = this.getMessage(key);

            if (angular.isDefined(msg)) {
                msg = msg.replace(/{(\d+)}/g, (match, index) => {
                    return args[index] === (undefined || null) ? match : args[index];
                });
            }

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
    ModuleRegister.filter("label", LabelFilter);

    export function MessageFilter(resourceService: ResourceService) {
        return (name: string, ...replacements: any[]) => {
            return resourceService.getFormattedMessage.apply(resourceService, [name, replacements]);
        }
    }

    LabelFilter.$inject = ["resourceService"];
    ModuleRegister.filter("message", MessageFilter);
}