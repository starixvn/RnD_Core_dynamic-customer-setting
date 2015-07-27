/// <reference path="../../../../typings/angularjs/angular.d.ts" />
/// <reference path="../../core/module-registrater.ts" />

module CustomerSetting {
    class InlineMessge implements ng.IDirective {
        public get scope(): any {
            return {
                message: "="
            }
        }

        public templateUrl: string = "InlineMessage/inline-message.htm";
    }
}