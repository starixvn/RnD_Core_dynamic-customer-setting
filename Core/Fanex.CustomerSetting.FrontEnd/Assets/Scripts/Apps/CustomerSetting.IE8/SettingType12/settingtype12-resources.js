/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType12Resource = {};
    
    settingType12Resource["label"] = settingType12Resource["label"] || {};
    settingType12Resource["label"]["en-us"] = {
        "ClickToViewCurrentLossAmount": "Click to view Current Loss Amount",
        "CurrentLossAmount": "Current Loss Amount",
        "DailyAutoResetLimit": "Daily auto reset limit (at 12 noon GMT +8)",
        "Losing": "Losing",
        "LossLimit": "Loss Limit",
        "MaximumLoss": "Maximum Loss",
        "Of": "of",
        "OnGivenCredit": "on given credit",
        "Unlimited": "Unlimited"
    };
    
    settingType12Resource["label"] = settingType12Resource["label"] || {};
    settingType12Resource["label"]["zh-cn"] = {
        "ClickToViewCurrentLossAmount": "点击查看目前输的数额",
        "CurrentLossAmount": "目前输的数额",
        "DailyAutoResetLimit": "每日限额自动重设 (于 中午12 时正 GMT +8)",
        "Losing": "输",
        "LossLimit": "输的限额",
        "MaximumLoss": "最高输额",
        "Of": "的",
        "OnGivenCredit": "无信用额",
        "Unlimited": "无限"
    };
    
    settingType12Resource["label"] = settingType12Resource["label"] || {};
    settingType12Resource["label"]["zh-tw"] = {
        "ClickToViewCurrentLossAmount": "點擊查看目前輸的數額",
        "CurrentLossAmount": "目前輸的數額",
        "DailyAutoResetLimit": "每日限額自動重設 (于 中午12 時正 GMT +8)",
        "Losing": "輸",
        "LossLimit": "輸的限額",
        "MaximumLoss": "最高輸額",
        "Of": "的",
        "OnGivenCredit": "無信用額",
        "Unlimited": "無限"
    };
    
    settingType12Resource["label"] = settingType12Resource["label"] || {};
    settingType12Resource["label"]["th-th"] = {
        "ClickToViewCurrentLossAmount": "คลิกเพื่อดูยอดเสียปัจจุบัน",
        "CurrentLossAmount": "ยอดเสียปัจจุบัน",
        "DailyAutoResetLimit": "ตั้งวงเงินใหม่อัตโนมัติรายวัน (เวลา 12 น. GMT+8)",
        "Losing": "แพ้",
        "LossLimit": "จำกัดยอดเสีย",
        "MaximumLoss": "แพ้สูงสุด",
        "Of": "ของ",
        "OnGivenCredit": "ยอดคงเหลือที่ได้รับ",
        "Unlimited": "ไม่จำกัด"
    };
    
    settingType12Resource["label"] = settingType12Resource["label"] || {};
    settingType12Resource["label"]["vi-vn"] = {
        "ClickToViewCurrentLossAmount": "Nhấn để xem Lượng Tiền Đang Thua",
        "CurrentLossAmount": "Lượng Tiền Đang Thua",
        "DailyAutoResetLimit": "Tự động reset giới hạn chuyển khoản mỗi ngày (12 trưa GMT+8)",
        "Losing": "Thua",
        "LossLimit": "Giới Hạn Tiền Thua",
        "MaximumLoss": "Tiền thua cược lớn nhất",
        "Of": "của",
        "OnGivenCredit": "trong hạn mức tín dụng được cấp",
        "Unlimited": "Không giới hạn"
    };
    
    settingType12Resource["message"] = settingType12Resource["message"] || {};
    settingType12Resource["message"]["en-us"] = {
        "AutoResetIsViolatedWithUpline": "Daily Auto Reset is turned off for the Upline.",
        "MaximumLossValueIsRequired": "Maximum Loss value is required.",
        "MaximumLossValueShouldBeInRange": "Maximum Loss value should be in the range of ({0} - {1}).",
        "Msg_10022": "You are not allowed to change Daily auto reset limit.",
        "Msg_10033": "Maximum Loss should be less than or equal to Maximum Loss of the direct upline.",
        "UpdatedLossLimitSuccessfully": "Updated Loss Limit successfully."
    };
    
    settingType12Resource["message"] = settingType12Resource["message"] || {};
    settingType12Resource["message"]["zh-cn"] = {
        "AutoResetIsViolatedWithUpline": "关闭上线的每日限额自动重设.",
        "MaximumLossValueIsRequired": "需求最高输额.",
        "MaximumLossValueShouldBeInRange": "最高输额需介于 ({0} - {1}) 的范围.",
        "Msg_10022": "您不允许修改每日限额自动重设.",
        "Msg_10033": "最高输额应低于或相等于直系上线的最高输额.",
        "UpdatedLossLimitSuccessfully": "输的限额已更新成功。"
    };
    
    settingType12Resource["message"] = settingType12Resource["message"] || {};
    settingType12Resource["message"]["zh-tw"] = {
        "AutoResetIsViolatedWithUpline": "關閉上線的每日限額自動重設.",
        "MaximumLossValueIsRequired": "需求最高輸額.",
        "MaximumLossValueShouldBeInRange": "最高輸額需介於 ({0} - {1}) 的範圍.",
        "Msg_10022": "您不允許修改每日限額自動重設.",
        "Msg_10033": "最高輸額應低於或相等於直系上線的最高輸額.",
        "UpdatedLossLimitSuccessfully": "輸的限額已更新成功。"
    };
    
    settingType12Resource["message"] = settingType12Resource["message"] || {};
    settingType12Resource["message"]["th-th"] = {
        "AutoResetIsViolatedWithUpline": "มีการปิดการตั้งค่าใหม่อัตโนมัติรายวันสำหรับอัปไลน์",
        "MaximumLossValueIsRequired": "ต้องใช้ค่าแพ้สูงสุด",
        "MaximumLossValueShouldBeInRange": "ค่าแพ้สูงสุดควรอยู่ในช่วง ({0} - {1})",
        "Msg_10022": "ไม่อนุญาตให้เปลี่ยนแปลงวงเงินการตั้งค่าใหม่อัตโนมัติรายวัน",
        "Msg_10033": "แพ้สูงสุดควรน้อยกว่าหรือเท่ากับแพ้สูงสุดของอัปไลน์โดยตรง",
        "UpdatedLossLimitSuccessfully": "ได้ทำการปรับปรุงจำกัดยอดเสียเรียบร้อยแล้ว"
    };
    
    settingType12Resource["message"] = settingType12Resource["message"] || {};
    settingType12Resource["message"]["vi-vn"] = {
        "AutoResetIsViolatedWithUpline": "Chức năng tự động reset giới hạn chuyển khoản mỗi ngày đã được tắt cho cấp trên.",
        "MaximumLossValueIsRequired": "Tiền thua cược lớn nhất không thể trống.",
        "MaximumLossValueShouldBeInRange": "Tiền thua cược lớn nhất phải nằm trong khoảng ({0} - {1}).",
        "Msg_10022": "Bạn không được phép thay đổi chức năng Tự động reset giới hạn chuyển khoản mỗi ngày.",
        "Msg_10033": "Tiền thua cược lớn nhất phải ít hơn hoặc bằng Tiền thua cược lớn nhất của cấp trên trực tiếp.",
        "UpdatedLossLimitSuccessfully": "Cập nhật Giới Hạn Tiền Thua thành công."
    };

    angular.module("customerSetting").value("settingType12Resource", settingType12Resource);
})();
