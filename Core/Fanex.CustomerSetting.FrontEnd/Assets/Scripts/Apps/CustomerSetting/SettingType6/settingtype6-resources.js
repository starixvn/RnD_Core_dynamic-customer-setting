/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType6Resource = {};
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["en-us"] = {
        "ClickToViewCurrentWinLossLimit": "Click to view Current Win/Loss Amount",
        "CurrentWinLossAmount": "Current Win/Loss Amount",
        "DailyAutoResetLimit": "Daily auto reset limit (at 12 noon GMT +8)",
        "Losing": "Losing",
        "MaximumLoss": "Maximum Loss",
        "MaximumWin": "Maximum Win",
        "OnGivenCredit": "on given credit",
        "PercentOfNumber": "{0} of {1}",
        "Unlimited": "Unlimited",
        "WinLossLimit": "Win Loss Limit",
        "Winning": "Winning"
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["zh-cn"] = {
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["zh-tw"] = {
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["ko-kr"] = {
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["ja-jp"] = {
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["th-th"] = {
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["vi-vn"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["en-us"] = {
        "MaximumLossValueIsRequired": "Maximum Loss value is required.",
        "MaximumLossValueShouldBeInRange": "Maximum Loss value should be in the range of ({0} - {1}).",
        "MaximumWinShouldBotExceedThanUplineMaximumWin": "Maximum Win should be less than or equal to Maximum Win of Direct Upline.",
        "MaximumWinShouldNotBeLessThanZero": "Maximum Win should not be less than 0.",
        "Msg_10022": "You are not allowed to change Daily auto reset limit.",
        "Msg_10024": "Maximum Win should be less than or equal to Maximum Win of Direct Upline.",
        "Msg_10033": "Maximum Loss should be less than or equal to Maximum Loss of Direct Upline.",
        "UpdatedWinLossLimitSuccessfully": "Updated Win Loss Limit successfully."
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["zh-cn"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["zh-tw"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["ko-kr"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["ja-jp"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["th-th"] = {
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("settingType6Resource", settingType6Resource);
})();
