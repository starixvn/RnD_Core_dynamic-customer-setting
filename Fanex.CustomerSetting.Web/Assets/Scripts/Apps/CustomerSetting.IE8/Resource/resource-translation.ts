/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/common.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class ResourceService {
        public static $inject: string[] = ["customerSettingResource", "parametersService"];

        private resource: any;
        private label: any;
        private message: any;

        constructor(customerSettingResource: any, parametersService: ParametersService) {
            this.resource = customerSettingResource;
            this.setLanguage(parametersService.getLanguage() || Defaults.language);
        }

        public getLabel(key: string): string {
            var value: string = this.label[key];

            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            } else {
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
                msg = msg.replace(/{(\d+)}/g, (match: string, index: number) => {
                    return args[index] === (undefined || null) ? match : args[index];
                });
            }

            return msg;
        }

        /* tslint:disable:no-string-literal */
        private setLanguage(language: string): void {
            var defaultLanguage: string = Defaults.language.toLowerCase();

            if (angular.isDefined(language)) {
                language = language.toLowerCase();
            } else {
                language = defaultLanguage;
            }

            this.label = this.resource["label"][language];
            this.message = this.resource["message"][language];

            if (angular.isUndefined(this.label)) {
                this.label = this.resource["label"][defaultLanguage];
            }

            if (angular.isUndefined(this.label)) {
                this.label = {};
            }

            if (angular.isUndefined(this.message)) {
                this.message = this.resource["message"][defaultLanguage];
            }

            if (angular.isUndefined(this.message)) {
                this.message = {};
            }
        }
        /* tslint:enable:no-string-literal */
    }

    ModuleRegister.service("resourceService", ResourceService);

    export function LabelFilter(resourceService: ResourceService) {
        return (name: string) => {
            return resourceService.getLabel(name);
        };
    }

    LabelFilter.$inject = ["resourceService"];
    ModuleRegister.filter("label", LabelFilter);

    export function MessageFilter(resourceService: ResourceService) {
        return (name: string, ...replacements: any[]) => {
            return resourceService.getFormattedMessage.apply(resourceService, [name, replacements]);
        };
    }

    MessageFilter.$inject = ["resourceService"];
    ModuleRegister.filter("message", MessageFilter);
}