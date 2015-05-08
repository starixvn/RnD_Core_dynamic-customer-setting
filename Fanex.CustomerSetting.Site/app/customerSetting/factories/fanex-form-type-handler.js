'use strict';

(function () {
    var injectParams = ['fanexAppSettings'];
    var fanexFormTypeHandler = function (appSettings) {
        var CompanyEditSuperHandler = function () {
            this.formType = appSettings.formType.CompanyEditSuper;
            this.pattern = '/editSuper/:superId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Company,
                    TargetCustId: params.superId,
                    TargetRoleId: appSettings.roleType.Super
                };
            };
        };

        var SuperEditMasterHandler = function () {
            this.formType = appSettings.formType.SuperEditMaster;
            this.pattern = '/editMaster/:masterId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.masterId,
                    TargetRoleId: appSettings.roleType.Master
                };
            };
        };

        var MasterEditAgentHandler = function () {
            this.formType = appSettings.formType.MasterEditAgent;
            this.pattern = '/editAgent/:agentId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.agentId,
                    TargetRoleId: appSettings.roleType.Agent
                };
            };
        };

        var AgentEditMemberHandler = function () {
            this.formType = appSettings.formType.AgentEditMember;
            this.pattern = '/editMember/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: 0,
                    RoleId: appSettings.roleType.Agent,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var SuperEditMemberHandler = function () {
            this.formType = appSettings.formType.SuperEditMember;
            this.pattern = '/superEditMember/:superId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: params.superId,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var MasterEditMemberHandler = function () {
            this.formType = appSettings.formType.MasterEditMember;
            this.pattern = '/masterEditMember/:masterId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    FormType: this.formType,
                    ProductId: params.productId,
                    CustId: params.masterId,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        function FormTypeHandler(formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return new CompanyEditSuperHandler();

                case appSettings.formType.SuperEditMaster:
                    return new SuperEditMasterHandler();

                case appSettings.formType.MasterEditAgent:
                    return new MasterEditAgentHandler();

                case appSettings.formType.SuperEditMember:
                    return new SuperEditMemberHandler();

                case appSettings.formType.MasterEditMember:
                    return new MasterEditMemberHandler();

                case appSettings.formType.AgentEditMember:
                    return new AgentEditMemberHandler();
            }
        }

        return FormTypeHandler;
    };

    fanexFormTypeHandler.$inject = injectParams;
    angular.module('customerSettingApp').factory('fanexFormTypeHandler', fanexFormTypeHandler);
})();