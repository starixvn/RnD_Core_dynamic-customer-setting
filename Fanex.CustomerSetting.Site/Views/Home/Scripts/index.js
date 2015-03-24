'use strict';

angular.module('indexModule', []).controller('IndexController', ['$scope', '$http',
function ($scope, $http) {
    var roleType = {
        Company: 5,
        Super: 4,
        Master: 3,
        Agent: 2
    };

    $scope.productId = 255;
    $scope.companyId = 8;
    $scope.superId = 2;
    $scope.masterId = 4174;
    $scope.agentId = 1262;
    $scope.memberId = 1277;
    $scope.languages = [
        { name: "English", value: "en-US" },
        { name: "繁體中文", value: "zh-TW" },
        { name: "简体中文", value: "zh-CN" },
        { name: "日本語", value: "ja-JP" },
        { name: "ภาษาไทย", value: "th-TH" },
        { name: "한국어", value: "ko-KR" }
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
        $http.post("home/login", {
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

    $scope.disableButtonLogin = function () {
        return $scope.LoginForm.CustId === 0 || $scope.LoginForm.CustId === 0;
    };
} ]);