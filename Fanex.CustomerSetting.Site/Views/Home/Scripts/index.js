'use strict';

if (!String.prototype.format) {
    Object.defineProperty(String.prototype, 'format', {
        value: function () {
            var args = arguments;
            return this.replace(/{(\d+)}/g, function (match, number) {
                return typeof args[number] !== 'undefined'
                ? args[number]
                : match;
            });
        }
    });
}

angular.module('indexModule', []).controller('IndexController', ['$scope', '$http',
function ($scope, $http) {
    var roleType = {
        Company: 5,
        Super: 4,
        Master: 3,
        Agent: 2
    };

    var formType = {
        CompanyEditSuper: 1,
        SuperEditMaster: 2,
        MasterEditAgent: 3,
        AgentEditMember: 4,
        SuperEditMember: 5,
        MasterEditMember: 6
    };

    $scope.isMultiple = false;
    $scope.productId = 255;
    $scope.companyId = 8;
    $scope.superId = 2;
    $scope.superIds = '2,3,4';
    $scope.superNames = 'SA-RM,SA-US$,SA-TB';
    $scope.masterId = 4174;
    $scope.masterIds = '4174,4300,4356';
    $scope.masterNames = 'mrm,mbrm,MYRM';
    $scope.agentId = 1262;
    $scope.agentIds = '1262,1653,2400';
    $scope.agentNames = 'wsrm,SKRM,216COM';
    $scope.memberId = 1277;
    $scope.memberIds = '1277,1284,1290';
    $scope.memberNames = '428RM,305RM,302RM';
    $scope.languages = [
        { name: "English (en-US)", value: "en-US" },
        { name: "繁體中文 (zh-TW)", value: "zh-TW" },
        { name: "简体中文 (zh-CN)", value: "zh-CN" },
        { name: "日本語 (ja-JP)", value: "ja-JP" },
        { name: "ภาษาไทย (th-TH)", value: "th-TH" },
        { name: "한국어 (ko-KR)", value: "ko-KR" },
        { name: "Tiếng Việt (vi-VN)", value: "vi-VN" }
    ];
    $scope.roles = [];
    for (var key in roleType) {
        $scope.roles.push({
            name: key,
            value: roleType[key]
        });
    }

    $scope.roleChange = function () {
        switch ($scope.LoginForm.RoleId) {
            case roleType.Agent:
                $scope.LoginForm.CustId = $scope.agentId;
                $scope.LoginForm.SubAccountId = 1;
                $scope.LoginForm.DisableLogin = false;
                break;
            case roleType.Master:
                $scope.LoginForm.CustId = $scope.masterId;
                $scope.LoginForm.SubAccountId = 2;
                $scope.LoginForm.DisableLogin = false;
                break;
            case roleType.Super:
                $scope.LoginForm.CustId = $scope.superId;
                $scope.LoginForm.SubAccountId = 3;
                $scope.LoginForm.DisableLogin = false;
                break;
            case roleType.Company:
                $scope.LoginForm.CustId = $scope.companyId;
                $scope.LoginForm.SubAccountId = 111;
                $scope.LoginForm.DisableLogin = false;
                break;
            default:
                $scope.LoginForm.CustId = 0;
                $scope.LoginForm.SubAccountId = 0;
                $scope.LoginForm.DisableLogin = true;
                break;
        }
    };

    $scope.login = function () {
        $http.post("account/login", {
            RoleId: $scope.LoginForm.RoleId,
            CustId: $scope.LoginForm.CustId,
            SubAccountId: $scope.LoginForm.SubAccountId,
            LanguageCode: $scope.LoginForm.LanguageCode
        }).success(function () {
            $scope.LoginForm.HideForm = true;
            $('#collapseLogin').removeClass("in");
        });
    };

    $scope.$watch('disableButtonLogin()', function (val) {
        if (val !== undefined) {
            $scope.LoginForm.DisableLogin = val;
        }
    });

    $scope.view = function (selectedFormType) {
        var url;
        var rootUrl = $('#main').data('root-url');
        switch (selectedFormType) {
            case formType.CompanyEditSuper:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/editSuper/{1}/{2}?custIds={3}&custNames={4}'.format(
                        rootUrl,
                        $scope.superIds.split(',')[0],
                        $scope.productId,
                        $scope.superIds,
                        $scope.superNames);
                } else {
                    url = '{0}/CustomerSetting/#/editSuper/{1}/{2}'.format(
                        rootUrl,
                        $scope.superId,
                        $scope.productId);
                }
                break;

            case formType.SuperEditMaster:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/editMaster/{1}/{2}?custIds={3}&custNames={4}'.format(
                        rootUrl,
                        $scope.masterIds.split(',')[0],
                        $scope.productId,
                        $scope.masterIds,
                        $scope.masterNames);
                } else {
                    url = '{0}/CustomerSetting/#/editMaster/{1}/{2}'.format(
                        rootUrl,
                        $scope.masterId,
                        $scope.productId);
                }
                break;

            case formType.MasterEditAgent:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/editAgent/{1}/{2}?custIds={3}&custNames={4}'.format(
                        rootUrl,
                        $scope.agentIds.split(',')[0],
                        $scope.productId,
                        $scope.agentIds,
                        $scope.agentNames);
                } else {
                    url = '{0}/CustomerSetting/#/editAgent/{1}/{2}'.format(
                        rootUrl,
                        $scope.agentId,
                        $scope.productId);
                }
                break;

            case formType.AgentEditMember:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/editMember/{1}/{2}?custIds={3}&custNames={4}'.format(
                        rootUrl,
                        $scope.memberIds.split(',')[0],
                        $scope.productId,
                        $scope.memberIds,
                        $scope.memberNames);
                } else {
                    url = '{0}/CustomerSetting/#/editMember/{1}/{2}'.format(
                        rootUrl,
                        $scope.memberId,
                        $scope.productId);
                }
                break;

            case formType.SuperEditMember:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/superEditMember/{1}/{2}/{3}?custIds={4}&custNames={5}'.format(
                        rootUrl,
                        $scope.superId,
                        $scope.memberIds.split(',')[0],
                        $scope.productId,
                        $scope.memberIds,
                        $scope.memberNames);
                } else {
                    url = '{0}/CustomerSetting/#/superEditMember/{1}/{2}/{3}'.format(
                        rootUrl,
                        $scope.superId,
                        $scope.memberId,
                        $scope.productId);
                }
                break;

            case formType.MasterEditMember:
                if ($scope.isMultiple) {
                    url = '{0}/CustomerSetting/#/masterEditMember/{1}/{2}/{3}?custIds={4}&custNames={5}'.format(
                        rootUrl,
                        $scope.masterId,
                        $scope.memberIds.split(',')[0],
                        $scope.productId,
                        $scope.memberIds,
                        $scope.memberNames);
                } else {
                    url = '{0}/CustomerSetting/#/masterEditMember/{1}/{2}/{3}'.format(
                        rootUrl,
                        $scope.masterId,
                        $scope.memberId,
                        $scope.productId);
                }
                break;
        }

        var settingPage = $('#setting-page');
        settingPage.attr('src', url);
    };

    $scope.disableButtonLogin = function () {
        return $scope.LoginForm.CustId === 0 || $scope.LoginForm.CustId === 0;
    };
} ]);