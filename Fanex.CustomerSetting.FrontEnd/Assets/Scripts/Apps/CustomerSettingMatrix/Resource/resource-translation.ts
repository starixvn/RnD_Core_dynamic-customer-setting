/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="../helper-services.ts" />
/// <reference path="../models.ts" />
/// <reference path="../module-register.ts" />

module CustomerSettingMatrix {
    "use strict";

    export class ResourceService {
        public static $inject: string[] = ["$window"];

        private resources: any;
        private label: any;
        private message: any;

        constructor($window: ng.IWindowService) {
            /* tslint:disable:no-string-literal */
            this.resources = $window["resources"];
            this.label = this.resources["label"]["en-us"];
            this.message = this.resources["message"]["en-us"];
            /* tslint:enable:no-string-literal */
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
        return (name: string) => {
            return resourceService.getLabel(name);
        };
    }
    LabelFilter.$inject = ["resourceService"];
    ModuleRegister.filter("message", MessageFilter);
}