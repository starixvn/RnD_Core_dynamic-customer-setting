/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType8Resource = {};
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["en-us"] = {
        "BetLimit": "Bet Limit"
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["en-us"] = {
        "MemberStakeLevelGroupMustBeSelected": "Bet Limit is required for Member level."
    };

    angular.module("customerSetting").value("settingType8Resource", settingType8Resource);
})();
