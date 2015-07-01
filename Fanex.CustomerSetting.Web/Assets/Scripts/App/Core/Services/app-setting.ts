/// <reference path="../../../../typings/tsd.d.ts" />
module CustomerSetting {
    export class AppSetting {
        static defaultLanguage: string = 'en-US';
        static commonErrorCodes = {
            Success: 0,
            ErrorValue: -1,
            NoChange: -2,
            AccessDenied: -3
        };
        static rootUrl: string = null;

        static resolveUrl(relativeUrl: string): string {
            if (AppSetting.resolveUrl === null) {
                var body = ng.element(document.body);

                AppSetting.rootUrl = body.attr('data-root-url');
            }

            AppSetting.rootUrl = AppSetting.rootUrl || '';
            relativeUrl = relativeUrl || '';

            return AppSetting.rootUrl + relativeUrl;
        }
    }
}