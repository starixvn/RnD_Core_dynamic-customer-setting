'use strict';

(function () {
    var injectParams = ['$timeout', 'fanexMessageService'];
    var fanexErrorController = function ($timeout, messageService) {
        messageService.showError('Please check your URL');
    };

    fanexErrorController.$inject = injectParams;
    angular.module('customerSettingApp').controller('FanexErrorController', fanexErrorController);
} ());