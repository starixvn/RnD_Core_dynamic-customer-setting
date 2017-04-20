/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType14Resources = {};
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["en-us"] = {
        "AgentCommission": "Agent Commission",
        "AgentGivenCommission": "Agent Given Commission",
        "CommForAgent": "Comm For Agent",
        "CommForMaster": "Comm For Master",
        "MasterCommission": "Master Commission",
        "MasterGivenCommission": "Master Given Commission",
        "MasterRemainCommission": "Master Remain Commission",
        "MaxGivenCommission": "Max Given Commission",
        "MemberCommission": "Member Commission",
        "SuperCommission": "Super Commission",
        "SuperGivenCommission": "Super Given Commission",
        "SuperRemainCommission": "Super Remain Commission",
        "TicketCommission": "Ticket Commission"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["en-us"] = {
        "CommissionCannotBeExceed100Percent": "The Commission cannot be exceed 100%.",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "The Commission must be less than or equal to the Given Commission.",
        "CommissionMustBePositive": "The Commission must be a positive number.",
        "Msg_1": "Invalid Commission Rate.",
        "UpdatedTicketCommissionSuccessfully": "Updated Ticket Commission successfully."
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["zh-cn"] = {
        "Msg_1": "佣金率无效"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["zh-tw"] = {
        "Msg_1": "傭金率無效"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["th-th"] = {
        "Msg_1": "อัตราค่าคอมมิชชั่นไม่ถูกต้อง"
    };

    angular.module("customerSetting").value("settingType14Resources", settingType14Resources);
})();
