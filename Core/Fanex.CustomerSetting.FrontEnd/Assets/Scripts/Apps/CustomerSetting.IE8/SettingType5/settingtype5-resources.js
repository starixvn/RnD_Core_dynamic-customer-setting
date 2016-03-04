/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType5Resources = {};
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["en-us"] = {
        "AgentsShare": "Agent&apos;s Share",
        "ChargeCommission": "Charge Commission",
        "ChargeOnMemberNetWin": "Charge on Member Net Win",
        "CommForAgent": "Comm for Agent",
        "CommForMaster": "Comm for Master",
        "Given": "Given",
        "MastersShare": "Master&apos;s Share",
        "Remain": "Remain",
        "SupersShare": "Super&apos;s Share",
        "TotalCharge": "Total Charge"
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["zh-cn"] = {
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["zh-tw"] = {
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["th-th"] = {
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["vi-vn"] = {
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["en-us"] = {
        "Msg_1": "Invalid Charge Commission Rate.",
        "ShareCannotBeExceed100Percent": "The Share cannot be exceed 100%.",
        "ShareMustBeLessThanOrEqualToGivenCommissionError": "The Share must be less than or equal to the Given Commission.",
        "ShareMustBePositive": "The Share must be a positive number.",
        "UpdatedChargeCommissionSuccessfully": "Charge Commission is updated successfully."
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["zh-cn"] = {
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["zh-tw"] = {
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["th-th"] = {
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("settingType5Resources", settingType5Resources);
})();
