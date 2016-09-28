/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType7Resource = {};
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["en-us"] = {
        "BetLimit": "Bet Limit"
    };
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["zh-cn"] = {
        "BetLimit": "投注限额"
    };
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["zh-tw"] = {
        "BetLimit": "投注限額"
    };
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["th-th"] = {
        "BetLimit": "วงเงินเดิมพัน"
    };
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["vi-vn"] = {
        "BetLimit": "Giới hạn cược"
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["en-us"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "Each bet type must have one Bet Limit value to be selected.",
        "Msg_10034": "This user&apos;s upline does not have Stake Level.",
        "UpdatedBetLimitSuccessfully": "Updated Bet Limit successfully."
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["zh-cn"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每项投注类别需至少选择一个投注限额.",
        "Msg_10034": "这帐号的上线无投注本金级别.",
        "UpdatedBetLimitSuccessfully": "投注限额已成功更新."
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["zh-tw"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每項投注類別需至少選擇一個投注限額.",
        "Msg_10034": "這帳號的上線無投註本金級別.",
        "UpdatedBetLimitSuccessfully": "投注限額已成功更新."
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["th-th"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "การเดิมพันแต่ละประเภทต้องมีค่าวงเงินเดิมพันหนึ่งค่าให้เลือก",
        "Msg_10034": "อัปไลน์ของผู้ใช้รายนี้ไม่มีระดับเงินเดิมพัน",
        "UpdatedBetLimitSuccessfully": "ปรับข้อมูลวงเงินเดิมพันเรียบร้อยแล้ว"
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["vi-vn"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "Mỗi loại cược phải chọn một giá trị Giới hạn cược.",
        "Msg_10034": "Cấp trên của người dùng này chưa cấu hình cấp độ đặt cược.",
        "UpdatedBetLimitSuccessfully": "Cập nhật Giới hạn cược thành công."
    };

    angular.module("customerSetting").value("settingType7Resource", settingType7Resource);
})();
