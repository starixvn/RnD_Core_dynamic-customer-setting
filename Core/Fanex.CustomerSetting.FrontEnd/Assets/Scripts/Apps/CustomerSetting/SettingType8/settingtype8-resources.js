/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType8Resource = {};
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["en-us"] = {
        "BetLimit": "Bet Limit",
        "Close": "Close",
        "ViewMore": "View More ..."
    };
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["zh-cn"] = {
        "BetLimit": "投注限额"
    };
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["zh-tw"] = {
        "BetLimit": "投注限額"
    };
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["th-th"] = {
        "BetLimit": "วงเงินเดิมพัน"
    };
    
    settingType8Resource["label"] = settingType8Resource["label"] || {};
    settingType8Resource["label"]["vi-vn"] = {
        "BetLimit": "Giới hạn cược"
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["en-us"] = {
        "MemberStakeLevelGroupMustBeSelected": "Bet Limit is required for Member level.",
        "Msg_10034": "This user&apos;s upline does not have Stake Level.",
        "UpdatedBetLimitSuccessfully": "Updated Bet Limit successfully.",
        "Y7StakeLevelRequired": "Each bet type must have at least one Bet Limit value to be selected."
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["zh-cn"] = {
        "Msg_10034": "这帐号的上线无投注本金级别.",
        "UpdatedBetLimitSuccessfully": "投注限额已成功更新."
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["zh-tw"] = {
        "Msg_10034": "這帳號的上線無投註本金級別.",
        "UpdatedBetLimitSuccessfully": "投注限額已成功更新."
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["th-th"] = {
        "Msg_10034": "อัปไลน์ของผู้ใช้รายนี้ไม่มีระดับเงินเดิมพัน",
        "UpdatedBetLimitSuccessfully": "ปรับข้อมูลวงเงินเดิมพันเรียบร้อยแล้ว"
    };
    
    settingType8Resource["message"] = settingType8Resource["message"] || {};
    settingType8Resource["message"]["vi-vn"] = {
        "Msg_10034": "Cấp trên của người dùng này chưa cấu hình cấp độ đặt cược.",
        "UpdatedBetLimitSuccessfully": "Cập nhật Giới hạn cược thành công."
    };

    angular.module("customerSetting").value("settingType8Resource", settingType8Resource);
})();
