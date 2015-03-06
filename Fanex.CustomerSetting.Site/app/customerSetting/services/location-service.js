'use strict';

(function () {
    var injectParams = ['$location', 'appSettings'];
    var locationService = function ($location, appSettings) {
        var path = $location.path();
        var formSettings = [
            {
                formType: appSettings.formType.CompanyEditSuper,
                pattern: '/editSuper/:superId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMaster,
                pattern: '/editMaster/:masterId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditAgent,
                pattern: '/editAgent/:agentId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMember,
                pattern: '/superEditMember/:superId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditMember,
                pattern: '/masterEditMember/:masterId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.AgentEditMember,
                pattern: '/agentEditMember/:masterId/:memberId/:productId'
            }
        ];

        var self = this;
        self.isValid = false;
        angular.forEach(formSettings, function (formSetting) {
            if (self.isValid) {
                return;
            }

            var formType = formSetting.formType;
            var pattern = formSetting.pattern;
            var matches = pattern.match(/\/:\w+/g);
            var params = [];
            angular.forEach(matches, function (match) {
                params.push(match.replace('/:', ''));
            });

            var regExp = new RegExp('^' + pattern.replace(/\/:\w+/g, '/(\\d+)') + '$');
            matches = regExp.exec(path);
            if (matches !== null && matches.length - 1 === params.length) {
                self.isValid = true;
                angular.forEach(params, function (param, index) {
                    self[param] = parseInt(matches[index + 1]);
                });
                self.formType = formType;
            }
        });

        this.createSelectModel = function () {
            if (!this.isValid) {
                return undefined;
            }

            var custId;
            var roleId;
            var targetCustId;
            var targetRoleId;
            switch (this.formType) {
                case appSettings.formType.CompanyEditSuper:
                    targetCustId = this.superId;
                    targetRoleId = appSettings.roleType.Super;
                    break;

                case appSettings.formType.SuperEditMaster:
                    targetCustId = this.masterId;
                    targetRoleId = appSettings.roleType.Master;
                    break;

                case appSettings.formType.MasterEditAgent:
                    targetCustId = this.agentId;
                    targetRoleId = appSettings.roleType.Agent;
                    break;

                case appSettings.formType.SuperEditMember:
                    custId = this.superId;
                    roleId = appSettings.roleType.Super;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.MasterEditMember:
                    custId = this.masterId;
                    roleId = appSettings.roleType.Master;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.AgentEditMember:
                    custId = this.agentId;
                    roleId = appSettings.roleType.Agent;
                    targetCustId = this.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;
            }

            var model = {
                CustId: custId,
                RoleId: roleId,
                TargetCustId: targetCustId,
                TargetRoleId: targetRoleId,
                FormType: this.formType,
                ProductId: this.productId
            };
            return model;
        };
    };

    locationService.$inject = injectParams;
    angular.module('customerSettingApp').service('locationService', locationService);
})();