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
        "BetLimit": "下注限度"
    };
    
    settingType7Resource["label"] = settingType7Resource["label"] || {};
    settingType7Resource["label"]["zh-tw"] = {
        "BetLimit": "投注限值"
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
        "MemberStakeLevelMustBeSelectedForBetType": "Each bet type must have one Bet Limit value be selected.",
        "Msg_10034": "This user&apos;s upline does not have Stake Level.",
        "UpdatedBetLimitSuccessfully": "Updated Bet Limit successfully."
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["zh-cn"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每种下注类型必须有一个下注限值可选。",
        "Msg_10034": "用户的上线没有赌注水平线。",
        "UpdatedBetLimitSuccessfully": "成功更新下注限度。"
    };
    
    settingType7Resource["message"] = settingType7Resource["message"] || {};
    settingType7Resource["message"]["zh-tw"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每種投注類型必須選擇一個投注極限數值。",
        "Msg_10034": "這位用戶的上線沒有賭金等級。",
        "UpdatedBetLimitSuccessfully": "已經成功更新投注限值。"
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
