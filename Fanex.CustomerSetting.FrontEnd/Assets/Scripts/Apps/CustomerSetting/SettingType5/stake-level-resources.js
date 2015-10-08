/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var stakeLevelResources = {};
    
    stakeLevelResources["label"] = stakeLevelResources["label"] || {};
    stakeLevelResources["label"]["en-us"] = {
        "BetLimit": "Bet Limit"
    };
    
    stakeLevelResources["message"] = stakeLevelResources["message"] || {};
    stakeLevelResources["message"]["en-us"] = {
    };

    angular.module("customerSetting").value("stakeLevelResources", stakeLevelResources);
})();
