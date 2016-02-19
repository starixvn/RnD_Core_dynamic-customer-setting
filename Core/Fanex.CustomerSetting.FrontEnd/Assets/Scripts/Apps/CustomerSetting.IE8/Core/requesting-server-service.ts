/// <reference path="../../../typings/angularjs/angular.d.ts" />
/// <reference path="blockui/block-ui.ts" />
/// <reference path="module-registrater.ts" />

module CustomerSetting.IE8 {
    "use strict";

    interface IServerRequestCallback {
        (response: ng.IHttpPromiseCallbackArg<any>): void;
    }

    export class RequestService {
        public static $inject: string[] = ["$http", "resourceService", "blockUiService"];

        private $http: ng.IHttpService;
        private resourceService: ResourceService;
        private blockUiService: BlockUiService;
        private sharedScope: any;

        constructor($http: ng.IHttpService, resourceService: ResourceService, blockUiService: BlockUiService) {
            this.$http = $http;
            this.resourceService = resourceService;
            this.blockUiService = blockUiService;
        }

        public registerSharedScope(sharedScope: any): void {
            this.sharedScope = sharedScope;
        }

        public request(path: string, parameters: any, callback: IServerRequestCallback) {
            this.blockUiService.blockUi();

            this.$http
                .post(path, parameters)
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                    callback(response);
                }, (response: ng.IHttpPromiseCallbackArg<any>) => {
                    var error: Result;

                    if (response) {
                        if (response.status === 401) {
                            error = Result.notAuthorizedError;
                            error.message = this.resourceService.getMessage(error.message);
                        } else if (response.data) {
                            error = new Result(response.status, response.data.message, MessageType.error);
                        } else {
                            error = Result.systemError;
                            error.message = this.resourceService.getMessage(error.message);
                        }
                    }

                    this.sharedScope.requestMsgResult = error;
                })
                ["finally"](() => {
                    this.blockUiService.unblockUi();
                });
        }
    }

    ModuleRegister.service("requestService", RequestService);
}