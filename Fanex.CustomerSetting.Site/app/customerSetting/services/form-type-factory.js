'use strict';

(function () {
    var injectParams = ['appSettings'];
    var formTypeFactory = function (appSettings) {
        var CompanyEditSuper = function () {
            this.formType = appSettings.formType.CompanyEditSuper;
            this.pattern = '/editSuper/:superId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: 0,
                    RoleId: appSettings.roleType.Company,
                    TargetCustId: params.superId,
                    TargetRoleId: appSettings.roleType.Super
                };
            };
        };

        var SuperEditMaster = function () {
            this.formType = appSettings.formType.SuperEditMaster;
            this.pattern = '/editMaster/:masterId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: 0,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.masterId,
                    TargetRoleId: appSettings.roleType.Master
                };
            };
        };

        var MasterEditAgent = function () {
            this.formType = appSettings.formType.MasterEditAgent;
            this.pattern = '/editAgent/:agentId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: 0,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.agentId,
                    TargetRoleId: appSettings.roleType.Agent
                };
            };
        };

        var AgentEditMember = function () {
            this.formType = appSettings.formType.AgentEditMember;
            this.pattern = '/editMember/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: 0,
                    RoleId: appSettings.roleType.Agent,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var SuperEditMember = function () {
            this.formType = appSettings.formType.SuperEditMember;
            this.pattern = '/superEditMember/:superId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: params.superId,
                    RoleId: appSettings.roleType.Super,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var MasterEditMember = function () {
            this.formType = appSettings.formType.MasterEditMember;
            this.pattern = '/masterEditMember/:masterId/:memberId/:productId';
            this.createModel = function (params) {
                return {
                    CustId: params.masterId,
                    RoleId: appSettings.roleType.Master,
                    TargetCustId: params.memberId,
                    TargetRoleId: appSettings.roleType.Member
                };
            };
        };

        var FormTypeFactory = function (formType) {
            switch (formType) {
                case appSettings.formType.CompanyEditSuper:
                    return new CompanyEditSuper();

                case appSettings.formType.SuperEditMaster:
                    return new SuperEditMaster();

                case appSettings.formType.MasterEditAgent:
                    return new MasterEditAgent();

                case appSettings.formType.SuperEditMember:
                    return new SuperEditMember();

                case appSettings.formType.MasterEditMember:
                    return new MasterEditMember();

                case appSettings.formType.AgentEditMember:
                    return new AgentEditMember();
            }
        };

        return FormTypeFactory;
    };

    formTypeFactory.$inject = injectParams;
    angular.module('customerSettingApp').factory('FormTypeFactory', formTypeFactory);
})();