'use strict';

(function () {
    var injectParams = ['$timeout'];
    var tooltipService = function ($timeout) {
        var tooltipScope;
        var tooltipElement;
        var tooltipAlign;
        var boundingElement;

        this.calculatePosition = function () {
            var rect = boundingElement.getBoundingClientRect();
            var tooltipRect = tooltipElement[0].getBoundingClientRect();
            var scrollX = window.pageXOffset || document.body.scrollLeft;
            var scrollY = window.pageYOffset || document.body.scrollTop;
            var left = 0;
            if (tooltipAlign === 'middle') {
                left = Math.max(0, rect.left + ((rect.right - rect.left) - (tooltipRect.right - tooltipRect.left)) / 2 + scrollX);
            } else {
                left = Math.max(0, rect.left + scrollX);
            }

            var top = Math.max(0, rect.top - (tooltipRect.bottom - tooltipRect.top) + scrollY);
            tooltipElement.css({
                'top': top + 'px',
                'left': left + 'px'
            });
        };

        this.setup = function (scope, element) {
            if (angular.isDefined(tooltipScope)) {
                throw 'Only one tooltip component in document';
            }

            tooltipScope = scope;
            tooltipElement = element;
            tooltipElement.addClass('ng-hide');
        };

        this.show = function (element, message, align) {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (angular.isUndefined(align)) {
                align = 'middle';
            }

            tooltipAlign = align;
            tooltipScope.align = tooltipAlign;
            boundingElement = element[0];
            if (tooltipScope.message !== message) {
                $timeout(function () {
                    tooltipScope.message = message;
                });
            }

            if (tooltipElement.hasClass('ng-hide')) {
                tooltipElement.removeClass('ng-hide');
                this.calculatePosition();
            }
        };

        this.hide = function () {
            if (angular.isUndefined(tooltipScope)) {
                throw 'setup is required';
            }

            if (!tooltipElement.hasClass('ng-hide')) {
                tooltipElement.addClass('ng-hide');
            }
        };
    };

    tooltipService.$inject = injectParams;
    angular.module('customerSettingApp').service('tooltipService', tooltipService);
})();