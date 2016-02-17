/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../core/helper-services.ts" />
/// <reference path="../core/module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    export class ResourceService {
        public static $inject: string[] = ["customerSettingResource", "parametersService"];

        private resource: any;
        private defaultLabel: any;
        private defaultMessage: any;
        private label: any;
        private message: any;

        constructor(customerSettingResource: any, parametersService: ParametersService) {
            this.resource = customerSettingResource;
            this.setLanguage(parametersService.getLanguage() || Defaults.language);
        }

        public getLabel(key: string): string {
            var value: string = this.label[key];

            if (angular.isUndefined(value)) {
                value = this.defaultLabel[key] || key;
            }

            if (angular.isDefined(value)) {
                value = value.replace(/&apos;|&#x0*27;/g, "'");
            }

            return value;
        }

        public getFormattedLabel(key: string, ...args: any[]): string {
            var label: string = this.getMessage(key);

            if (angular.isDefined(label)) {
                label = label.replace(/{(\d+)}/g, (match: string, index: number) => {
                    return args[index] === (undefined || null) ? match : args[index];
                });
            }

            return label;
        }

        public getMessage(key: string): string {
            var msg: string = this.message[key];

            if (angular.isUndefined(msg)) {
                msg = this.defaultMessage[key] || key;
            }

            if (angular.isDefined(msg)) {
                msg = msg.replace(/&apos;|&#x0*27;/g, "'");
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
            var label = this.resource["label"] || {};
            var message = this.resource["message"] || {};

            if (angular.isDefined(language)) {
                language = language.toLowerCase();
            } else {
                language = defaultLanguage;
            }

            this.defaultLabel = label[defaultLanguage] || {};
            this.defaultMessage = message[defaultLanguage] || {};

            this.label = label[language] || this.defaultLabel;
            this.message = message[language] || this.defaultMessage;
        }
        /* tslint:enable:no-string-literal */
    }

    ModuleRegister.service("resourceService", ResourceService);

    function LabelFilter(resourceService: ResourceService): Function {
        return (name: string) => {
            return resourceService.getLabel(name);
        };
    }

    LabelFilter.$inject = ["resourceService"];
    ModuleRegister.filter("label", LabelFilter);

    function MessageFilter(resourceService: ResourceService): Function {
        return (name: string, ...replacements: any[]) => {
            replacements.unshift(name)

            return resourceService.getFormattedMessage.apply(resourceService, replacements);
        };
    }

    MessageFilter.$inject = ["resourceService"];
    ModuleRegister.filter("message", MessageFilter);
}