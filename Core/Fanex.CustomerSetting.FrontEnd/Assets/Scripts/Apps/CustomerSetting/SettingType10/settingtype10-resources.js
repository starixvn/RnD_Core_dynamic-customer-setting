/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType11Resources = {};
    
    settingType11Resources["label"] = settingType11Resources["label"] || {};
    settingType11Resources["label"]["en-us"] = {
        "Commission": "Commission",
        "CommissionForAgent": "Comm for Agent",
        "CommissionForMaster": "Comm for Master",
        "CommissionForMember": "Comm for Member",
        "Group": "Group",
        "UserGroup": "User Group"
    };
    
    settingType11Resources["message"] = settingType11Resources["message"] || {};
    settingType11Resources["message"]["en-us"] = {
        "CommissionCannotBeNegative": "Commission cannot be negative.",
        "CommissionCannotExceedMaxValue": "Commission cannot exceed Max Value.",
        "CommissionMustBeLessThan": "Commission must be less than {0}",
        "InvalidUserGroup": "Invalid user group.",
        "Msg_1": "Invalid Commission Rate.",
        "UnableToChangeUserGroup": "You are not allowed to change to User Group.",
        "UpdatedCommissionSuccessfully": "Updated Commission successfully."
    };

    angular.module("customerSetting").value("settingType11Resources", settingType11Resources);
})();
