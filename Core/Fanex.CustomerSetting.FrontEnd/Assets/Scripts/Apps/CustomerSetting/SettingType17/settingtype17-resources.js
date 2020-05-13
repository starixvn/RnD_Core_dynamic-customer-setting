/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType17Resource = {};
    
    settingType17Resource["label"] = settingType17Resource["label"] || {};
    settingType17Resource["label"]["en-us"] = {
        "BetLimit": "Bet Limit"
    };
    
    settingType17Resource["label"] = settingType17Resource["label"] || {};
    settingType17Resource["label"]["zh-cn"] = {
        "BetLimit": "投注限额"
    };
    
    settingType17Resource["label"] = settingType17Resource["label"] || {};
    settingType17Resource["label"]["zh-tw"] = {
        "BetLimit": "投注限額"
    };
    
    settingType17Resource["label"] = settingType17Resource["label"] || {};
    settingType17Resource["label"]["th-th"] = {
        "BetLimit": "วงเงินเดิมพัน"
    };
    
    settingType17Resource["label"] = settingType17Resource["label"] || {};
    settingType17Resource["label"]["vi-vn"] = {
        "BetLimit": "Giới hạn cược"
    };
    
    settingType17Resource["message"] = settingType17Resource["message"] || {};
    settingType17Resource["message"]["en-us"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "Each bet type must have one Bet Limit value to be selected.",
        "Msg_10034": "This user&apos;s upline does not have Stake Level.",
        "UpdatedBetLimitSuccessfully": "Updated Bet Limit successfully.",
        "Y7StakeLevelRequired": "Each bet type must have at least one Bet Limit value to be selected."
    };
    
    settingType17Resource["message"] = settingType17Resource["message"] || {};
    settingType17Resource["message"]["zh-cn"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每项投注类别需至少选择一个投注限额.",
        "Msg_10034": "这帐号的上线无投注本金级别.",
        "UpdatedBetLimitSuccessfully": "投注限额已成功更新.",
        "Y7StakeLevelRequired": "每种投注类型至少需要选择一个投注限额。"
    };
    
    settingType17Resource["message"] = settingType17Resource["message"] || {};
    settingType17Resource["message"]["zh-tw"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "每項投注類別需至少選擇一個投注限額.",
        "Msg_10034": "這帳號的上線無投註本金級別.",
        "UpdatedBetLimitSuccessfully": "投注限額已成功更新.",
        "Y7StakeLevelRequired": "每種投注類型至少需要選擇一個投注限額。"
    };
    
    settingType17Resource["message"] = settingType17Resource["message"] || {};
    settingType17Resource["message"]["th-th"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "การเดิมพันแต่ละประเภทต้องมีค่าวงเงินเดิมพันหนึ่งค่าให้เลือก",
        "Msg_10034": "อัปไลน์ของผู้ใช้รายนี้ไม่มีระดับเงินเดิมพัน",
        "UpdatedBetLimitSuccessfully": "ปรับข้อมูลวงเงินเดิมพันเรียบร้อยแล้ว",
        "Y7StakeLevelRequired": "การเดิมพันแต่ละประเภทจะต้องเลือกอย่างน้อยหนึ่งวงเงินเดิมพัน"
    };
    
    settingType17Resource["message"] = settingType17Resource["message"] || {};
    settingType17Resource["message"]["vi-vn"] = {
        "MemberStakeLevelMustBeSelectedForBetType": "Mỗi loại cược phải chọn một giá trị Giới hạn cược.",
        "Msg_10034": "Cấp trên của người dùng này chưa cấu hình cấp độ đặt cược.",
        "UpdatedBetLimitSuccessfully": "Cập nhật Giới hạn cược thành công.",
        "Y7StakeLevelRequired": "Mỗi loại cược phải có ít nhất một Giới Hạn Cược."
    };
    angular.module("customerSetting").value("settingType17Resource", settingType17Resource);
})();
