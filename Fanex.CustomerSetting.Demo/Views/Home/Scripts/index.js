angular.module('indexModule', []).controller('IndexController',
['$scope', '$http', function ($scope, $http) {
    $scope.productId = 255; $scope.superId = 1177375;
    $scope.masterId = 0; $scope.agentId = 0; $scope.memberId = 0;
    $scope.roles = [{ name: "Agent", value: 2 }, { name: "Master", value: 3 }, { name: "Super", value: 4 }, { name: "Company", value: 5}];

    $scope.languages = [{ name: "English", value: "en-US" }, { name: "繁體中文", value: "zh-TW" }, { name: "简体中文", value: "zh-CN" }, { name: "日本語", value: "ja-JP" }, { name: "ภาษาไทย", value: "th-TH" }, { name: "한국어", value: "ko-KR"}];

    $scope.roleChange = function () {
        switch ($scope.LoginForm.RoleId) {
            case 1:
                $scope.LoginForm.CustId = 73103;
                $scope.LoginForm.DisableLogin = false;
                break;
            case 2:
                $scope.LoginForm.CustId = 181595;
                $scope.LoginForm.SubAccountId = 1;
                $scope.LoginForm.DisableLogin = false;
                break;
            case 3:
                $scope.LoginForm.CustId = 68287;
                $scope.LoginForm.SubAccountId = 2;
                $scope.LoginForm.DisableLogin = false;
                break;
            case 4:
                $scope.LoginForm.CustId = 197461;
                $scope.LoginForm.SubAccountId = 3;
                $scope.LoginForm.DisableLogin = false;
                break;
            case 5:
                $scope.LoginForm.CustId = 332427;
                $scope.LoginForm.SubAccountId = 111;
                $scope.LoginForm.DisableLogin = false;
                break;
            default:
                $scope.LoginForm.CustId = "";
                $scope.LoginForm.SubAccountId = "";
                $scope.LoginForm.DisableLogin = true;
                break;
        }
    };

    $scope.login = function () {
        // Process
        $http.post("home/login",
                    {
                        RoleId: $scope.LoginForm.RoleId,
                        CustId: $scope.LoginForm.CustId,
                        SubAccountId: $scope.LoginForm.SubAccountId,
                        LanguageCode: $scope.LoginForm.LanguageCode
                    }).success(function (data) {
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
        if ($scope.LoginForm.CustId === "" || $scope.LoginForm.CustId === 0) {
            return true;
        }
        else {
            return false;
        }
    }
} ]);