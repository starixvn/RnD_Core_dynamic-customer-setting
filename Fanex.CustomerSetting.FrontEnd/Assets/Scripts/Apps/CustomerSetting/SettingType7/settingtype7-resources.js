/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType7Resources = {};
    
    settingType7Resources["label"] = settingType7Resources["label"] || {};
    settingType7Resources["label"]["en-us"] = {
        "ChargedCommission": "Charged Commission"
    };
    
    settingType7Resources["message"] = settingType7Resources["message"] || {};
    settingType7Resources["message"]["en-us"] = {
        "ChargedCommissionIsGreaterThanUplineChargedCommissionError": "The Charged Commission cannot be greater than Charged Commission of the Upline",
        "Msg_1": "Invalid Commission Rate.",
        "UpdatedChargedCommissionSuccessfully": "Charged Commission is updated successfully."
    };

    angular.module("customerSetting").value("settingType7Resources", settingType7Resources);
})();
