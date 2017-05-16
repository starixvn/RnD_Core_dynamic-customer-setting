/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType15Resources = {};
    
    settingType15Resources["label"] = settingType15Resources["label"] || {};
    settingType15Resources["label"]["en-us"] = {
        "WinningShareCommission": "Winning Share Commission"
    };
    
    settingType15Resources["label"] = settingType15Resources["label"] || {};
    settingType15Resources["label"]["zh-cn"] = {
        "WinningShareCommission": "获奖赢额佣金"
    };
    
    settingType15Resources["label"] = settingType15Resources["label"] || {};
    settingType15Resources["label"]["zh-tw"] = {
        "WinningShareCommission": "獲獎贏額佣"
    };
    
    settingType15Resources["label"] = settingType15Resources["label"] || {};
    settingType15Resources["label"]["th-th"] = {
        "WinningShareCommission": "ค่าคอมมิชชั่นที่ชนะ"
    };
    
    settingType15Resources["label"] = settingType15Resources["label"] || {};
    settingType15Resources["label"]["vi-vn"] = {
        "WinningShareCommission": "Phần chia thắng cược"
    };

    angular.module("customerSetting").value("settingType15Resources", settingType15Resources);
})();
