'use strict';

(function () {
    var injectParams = ['appSettings'];
    var componentService = function (appSettings) {
        this.inArray = function (value, array) {
            var index = -1;
            angular.forEach(array, function (arrayValue, arrayIndex) {
                if (index === -1 && arrayValue === value) {
                    index = arrayIndex;
                }
            });

            return index;
        };

        var generateOptions = function (min, max, step, factor) {
            var options = [];
            while (max >= min) {
                options.push(max);
                max -= step;
                max = Math.round(max * factor) / factor;
            }
            return options;
        };

        var DiscountSelect = function (min, max, value) {
            this.min = min;
            this.value = value;
            this.options = generateOptions(min, max, 0.0005, 10000);
        };

        var BetLimitInput = function (value, limit) {
            this.error = '';
            this.value = value;
            this.limit = limit;
        };

        var PositionTakingSelect = function (min, max, value) {
            var options = generateOptions(min, max, 1, 1);
            var selectIndex = -1;
            angular.forEach(options, function (option, index) {
                if (selectIndex === -1 && option === value) {
                    selectIndex = index;
                }
            });

            if (selectIndex === -1) {
                if (value < min) {
                    options.push(value);
                    options = options.length - 1;
                } else if (value > max) {
                    options.unshift(value);
                    selectIndex = 0;
                }
            }

            this.options = options;
            this.value = this.options[selectIndex];
        };

        this.createPositionTakingSelect = function (min, max, value) {
            return new PositionTakingSelect(min, max, value);
        };

        this.parseDiscount = function (data, formType) {
            var parseCompanySelectSuperDiscount = function (data) {
                return new DiscountSelect(0, data.DefaultMaxDiscount, data.SuperDiscount);
            };

            var parseSuperSelectMasterDiscount = function (data) {
            };

            var parseMasterSelectAgentDiscount = function (data) {
            };

            var parseSuperSelectMemberDiscount = function (data) {
            };

            var parseMasterSelectMemberDiscount = function (data) {
            };

            var parseAgentSelectMemberDiscount = function (data) {
            };

            switch (formType) {
                case 1:
                    return parseCompanySelectSuperDiscount(data);

                case 2:
                    break;

                case 3:
                    break;

                case 4:
                    break;

                case 5:
                    break;

                case 6:
                    break;
            }
        };

        this.parseBetLimit = function (data, formType) {
            var parseCompanySelectSuperBetLimit = function (data) {
                var result = {};
                result.minBet = new BetLimitInput(data.MinBet, data.MinBetMin);
                result.maxBet = new BetLimitInput(data.MaxBet, appSettings.defaultMaxBet);
                result.maxPerMatch = new BetLimitInput(data.MaxPerMatch, appSettings.defaultMaxBet);
                return result;
            };

            var parseSuperSelectMasterBetLimit = function (data) {
            };

            var parseMasterSelectAgentBetLimit = function (data) {
            };

            var parseSuperSelectMemberBetLimit = function (data) {
            };

            var parseMasterSelectMemberBetLimit = function (data) {
            };

            var parseAgentSelectMemberBetLimit = function (data) {
            };

            switch (formType) {
                case 1:
                    return parseCompanySelectSuperBetLimit(data);

                case 2:
                    return parseSuperSelectMasterBetLimit(data);

                case 3:
                    return parseMasterSelectAgentBetLimit(data);

                case 4:
                    return parseSuperSelectMemberBetLimit(data);

                case 5:
                    return parseMasterSelectMemberBetLimit(data);

                case 6:
                    return parseAgentSelectMemberBetLimit(data);
            }
        };

        this.parsePositionTaking = function (data, formType) {
            var parseCompanySelectSuperPositionTaking = function (data) {
                var result = {};
                result.superMaxPT = new PositionTakingSelect(0, data.DefaultMaxPT, data.S4);
                result.masterMaxPT = new PositionTakingSelect(0, data.S4, data.M4);
                result.agentMaxPT = new PositionTakingSelect(0, data.M4, data.A4);
                result.superMinPT = new PositionTakingSelect(0, data.S4, data.N4);
                return result;
            };

            switch (formType) {
                case 1:
                    return parseCompanySelectSuperPositionTaking(data);
            }
        };
    };

    componentService.$inject = injectParams;
    angular.module('customerSettingApp').service('componentService', componentService);
})();