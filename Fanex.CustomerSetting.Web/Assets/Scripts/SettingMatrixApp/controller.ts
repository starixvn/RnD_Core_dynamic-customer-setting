/// <reference path="../../typings/angularjs/angular.d.ts" />
/// <reference path="helper-services.ts" />
/// <reference path="models.ts" />
/// <reference path="module-register.ts" />
/// <reference path="parameters.ts" />
/// <reference path="directives/ptmatrix/pt-matrix.ts" />
/// <reference path="directives/commissionmatrix/commission-matrix.ts" />


module CustomerSettingMatrix {
    "use strict";

    class CustomerSettingMaxtrixController {
        public static $inject: string[] = [
            "$scope",
            "$http",
            "$timeout",
            "$window",
            "appConfigService"];

        public isUIblocked: boolean;

        
        public productName: ProductName;
        public customerName: string;
        public settingTypeId: number;

        public productNames: Array<ProductName>;
        public settingTypes: Array<SettingType>;

        public customerLine: Array<Customer>;
        public settingTypeMatrix: SettingTypeMatrix;
        

        private get querySettingMatrixPath(): string {
            return "ViewCustomerSettingMatrix/QuerySettingMatrix?";
        }

        private get queryProductNamesPath(): string {
            return "ViewCustomerSettingMatrix/QueryProductNames";
        }

        private $scope: ng.IScope;
        private $http: ng.IHttpService;
        private $timeout: ng.ITimeoutService;
        private appConfigService: AppConfigService;
        private $window: ng.IWindowService;


        constructor(
            $scope: ng.IScope,
            $http: ng.IHttpService,
            $timeout: ng.ITimeoutService,
            $window: ng.IWindowService,
            appConfigService: AppConfigService) {

            this.$scope = $scope;
            this.$http = $http;
            this.$timeout = $timeout;
            this.$window = $window;

            this.appConfigService = appConfigService;
            this.settingTypes = [new SettingType(1, "PT Matrix"), new SettingType(3, "Commission Matrix")];
            this.settingTypeId = this.settingTypes[0].id;
            this.customerName = this.$window["customerName"];
            this.queryProductNames();
        }

        public updateSettingTypeView(settingTypeId: number) {
            this.settingTypeId = settingTypeId;
            this.customerLine = [];
        }

        public queryProductNames(): void {
            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(this.queryProductNamesPath), {})
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                var productNames = JSON.parse(response.data);

                this.productNames = productNames.map((productName: any) => new ProductName(productName));
                this.productName = this.productNames[3];
                this.querySettingMatrix();
            })
                .finally(() => {
                this.isUIblocked = false;
            });
        }

        public querySettingMatrix(): void {
            var querySettingMatrix = new QuerySettingMatrix(
                this.settingTypeId,
                this.productName.productId,
                this.customerName);

            this.isUIblocked = true;

            this.$http
                .post(this.appConfigService.resolveRootUrl(this.querySettingMatrixPath), querySettingMatrix)
                .then((response: ng.IHttpPromiseCallbackArg<any>) => {
                var customerSettingMatrix = JSON.parse(response.data);

                this.customerLine = customerSettingMatrix.customerLine.map((customer: any) => new Customer(customer));
                switch (this.settingTypeId) {
                    case 1:
                        this.settingTypeMatrix = new PTMatrix(customerSettingMatrix.settingTypeMatrix);
                        break;
                    case 3:
                        this.settingTypeMatrix = new CommissionMatrix(customerSettingMatrix.settingTypeMatrix);
                        break;
                }
                this.settingTypeMatrix.convertToDisplay();
            })
                .finally(() => {
                this.isUIblocked = false;
            });
        }
    }

    ModuleRegister.controller("customerSettingMatrixController", CustomerSettingMaxtrixController);
}