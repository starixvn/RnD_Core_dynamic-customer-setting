/// <reference path="../../../typings/angularjs/angular.d.ts" />
var AppSetting = (function () {
    function AppSetting() {
    }
    AppSetting.resolveUrl = function (relativeUrl) {
        if (AppSetting.resolveUrl === null) {
            var body = ng.element(document.body);
            AppSetting.rootUrl = body.attr('data-root-url');
        }
        AppSetting.rootUrl = AppSetting.rootUrl || '';
        relativeUrl = relativeUrl || '';
        return AppSetting.rootUrl + relativeUrl;
    };
    AppSetting.defaultLanguage = 'en-US';
    AppSetting.commonErrorCodes = {
        Success: 0,
        ErrorValue: -1,
        NoChange: -2,
        AccessDenied: -3
    };
    AppSetting.rootUrl = null;
    return AppSetting;
})();
//# sourceMappingURL=app-setting.js.map