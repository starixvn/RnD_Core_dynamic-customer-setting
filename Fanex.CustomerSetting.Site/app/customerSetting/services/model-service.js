'use strict';

(function () {
    var injectParams = ['$rootScope', '$location', 'appSettings'];
    var modelService = function ($rootScope, $location, appSettings) {
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
                formType: appSettings.formType.AgentEditMember,
                pattern: '/editMember/:memberId/:productId'
            },
            {
                formType: appSettings.formType.SuperEditMember,
                pattern: '/superEditMember/:superId/:memberId/:productId'
            },
            {
                formType: appSettings.formType.MasterEditMember,
                pattern: '/masterEditMember/:masterId/:memberId/:productId'
            }
        ];

        var self = this;
        function loadParams() {
            var isValidModel = false;
            var modelParams = {};
            var path = $location.path();
            angular.forEach(formSettings, function (formSetting) {
                if (isValidModel) {
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
                    isValidModel = true;
                    angular.forEach(params, function (param, index) {
                        modelParams[param] = parseInt(matches[index + 1]);
                    });
                    modelParams.formType = formType;
                }
            });

            self.isValid = isValidModel;
            self.params = modelParams;
        }

        this.createSelectModel = function () {
            var isValid = this.isValid;
            var params = this.params;
            if (!isValid) {
                return undefined;
            }

            var custId;
            var roleId;
            var targetCustId;
            var targetRoleId;
            switch (params.formType) {
                case appSettings.formType.CompanyEditSuper:
                    targetCustId = params.superId;
                    targetRoleId = appSettings.roleType.Super;
                    break;

                case appSettings.formType.SuperEditMaster:
                    targetCustId = params.masterId;
                    targetRoleId = appSettings.roleType.Master;
                    break;

                case appSettings.formType.MasterEditAgent:
                    targetCustId = params.agentId;
                    targetRoleId = appSettings.roleType.Agent;
                    break;

                case appSettings.formType.SuperEditMember:
                    custId = params.superId;
                    roleId = appSettings.roleType.Super;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.MasterEditMember:
                    custId = params.masterId;
                    roleId = appSettings.roleType.Master;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;

                case appSettings.formType.AgentEditMember:
                    custId = params.agentId;
                    roleId = appSettings.roleType.Agent;
                    targetCustId = params.memberId;
                    targetRoleId = appSettings.roleType.Member;
                    break;
            }

            return {
                CustId: custId,
                RoleId: roleId,
                TargetCustId: targetCustId,
                TargetRoleId: targetRoleId,
                FormType: params.formType,
                ProductId: params.productId
            };
        };

        $rootScope.$on('$locationChangeStart', function () {
            loadParams();
        });
        
        loadParams();
    };

    modelService.$inject = injectParams;
    angular.module('customerSettingApp').service('modelService', modelService);
})();