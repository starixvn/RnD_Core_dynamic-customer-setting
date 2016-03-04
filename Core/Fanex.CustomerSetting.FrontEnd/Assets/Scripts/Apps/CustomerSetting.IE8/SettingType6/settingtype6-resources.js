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
        "Of": "of",
        "OnGivenCredit": "on given credit",
        "Unlimited": "Unlimited",
        "WinLossLimit": "Win Loss Limit",
        "Winning": "Winning"
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["zh-cn"] = {
        "ClickToViewCurrentWinLossLimit": "点击查看目前的赢/输金额",
        "CurrentWinLossAmount": "目前的赢/输金额",
        "DailyAutoResetLimit": "每日自动重置限度（中午12点GMT+8)",
        "Losing": "输",
        "MaximumLoss": "最大损失",
        "MaximumWin": "最大收益",
        "Of": "的",
        "OnGivenCredit": "在给定信用额上",
        "Unlimited": "无限",
        "WinLossLimit": "赢输限度",
        "Winning": "赢"
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["zh-tw"] = {
        "ClickToViewCurrentWinLossLimit": "按一下查看當前的盈虧金額",
        "CurrentWinLossAmount": "當前的盈虧金額",
        "DailyAutoResetLimit": "每天自動重設限制 (中午 12 點 GMT +8)",
        "Losing": "輸盤",
        "MaximumLoss": "最大損失",
        "MaximumWin": "最大收入",
        "Of": "的",
        "OnGivenCredit": "關於給定積分",
        "Unlimited": "無限制的",
        "WinLossLimit": "盈虧限值",
        "Winning": "贏取"
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["th-th"] = {
        "ClickToViewCurrentWinLossLimit": "คลิกเพื่อดูจำนวนแพ้/ชนะปัจจุบัน",
        "CurrentWinLossAmount": "จำนวนแพ้/ชนะปัจจุบัน",
        "DailyAutoResetLimit": "ตั้งวงเงินใหม่อัตโนมัติรายวัน (เวลา 12 น. GMT+8)",
        "Losing": "แพ้",
        "MaximumLoss": "แพ้สูงสุด",
        "MaximumWin": "ชนะสูงสุด",
        "Of": "ของ",
        "OnGivenCredit": "ยอดคงเหลือที่ได้รับ",
        "Unlimited": "ไม่จำกัด",
        "WinLossLimit": "วงเงินแพ้ชนะ",
        "Winning": "ชนะ"
    };
    
    settingType6Resource["label"] = settingType6Resource["label"] || {};
    settingType6Resource["label"]["vi-vn"] = {
        "ClickToViewCurrentWinLossLimit": "Nhấn để xem giá trị thắng thua hiện tại",
        "CurrentWinLossAmount": "Giá trị thắng thua hiện tại",
        "DailyAutoResetLimit": "Tự động reset giới hạn chuyển khoản mỗi ngày (12 trưa GMT+8)",
        "Losing": "Thua",
        "MaximumLoss": "Tiền thua cược lớn nhất",
        "MaximumWin": "Tiền thắng cược lớn nhất",
        "Of": "của",
        "OnGivenCredit": "trong hạn mức tín dụng được cấp",
        "Unlimited": "Không giới hạn",
        "WinLossLimit": "Giới hạn thắng thua",
        "Winning": "Thắng"
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["en-us"] = {
        "AutoResetIsViolatedWithUpline": "Daily Auto Reset is turned off for the Upline.",
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
        "AutoResetIsViolatedWithUpline": "上线每日自动重置关闭。",
        "MaximumLossValueIsRequired": "最大损失值是必须的。",
        "MaximumLossValueShouldBeInRange": "最大损失值应在({0} - {1})范围内。",
        "MaximumWinShouldBotExceedThanUplineMaximumWin": "直接上线。",
        "MaximumWinShouldNotBeLessThanZero": "最大收益不能小于0。",
        "Msg_10022": "您不能改变每日自动重置限度。",
        "Msg_10024": "直接上线。",
        "Msg_10033": "直接上线。",
        "UpdatedWinLossLimitSuccessfully": "成功更新赢输限度。"
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["zh-tw"] = {
        "AutoResetIsViolatedWithUpline": "上線的每日自動重設已經關閉。",
        "MaximumLossValueIsRequired": "規定了最大損失數值。",
        "MaximumLossValueShouldBeInRange": "最大損失數值的範圍應該是({0} - {1})。",
        "MaximumWinShouldBotExceedThanUplineMaximumWin": "最大收入應該少於或等於直接上線的最大收入。",
        "MaximumWinShouldNotBeLessThanZero": "最大收入應該不少於 0。",
        "Msg_10022": "您無法更改每日自動重設限值。",
        "Msg_10024": "最大收入應該少於或等於直接上線的最大收入。",
        "Msg_10033": "最大損失應該少於或等於直接上線的最大損失。",
        "UpdatedWinLossLimitSuccessfully": "已經成功更新盈虧限值。"
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["th-th"] = {
        "AutoResetIsViolatedWithUpline": "มีการปิดการตั้งค่าใหม่อัตโนมัติรายวันสำหรับอัปไลน์",
        "MaximumLossValueIsRequired": "ต้องใช้ค่าแพ้สูงสุด",
        "MaximumLossValueShouldBeInRange": "ค่าแพ้สูงสุดควรอยู่ในช่วง ({0} - {1})",
        "MaximumWinShouldBotExceedThanUplineMaximumWin": "ชนะสูงสุดควรน้อยกว่าหรือเท่ากับชนะสูงสุดของอัปไลน์โดยตรง",
        "MaximumWinShouldNotBeLessThanZero": "ชนะสูงสุดไม่ควรน้อยกว่า 0",
        "Msg_10022": "ไม่อนุญาตให้เปลี่ยนแปลงวงเงินการตั้งค่าใหม่อัตโนมัติรายวัน",
        "Msg_10024": "ชนะสูงสุดควรน้อยกว่าหรือเท่ากับชนะสูงสุดของอัปไลน์โดยตรง",
        "Msg_10033": "แพ้สูงสุดควรน้อยกว่าหรือเท่ากับแพ้สูงสุดของอัปไลน์โดยตรง",
        "UpdatedWinLossLimitSuccessfully": "ปรับข้อมูลวงเงินแพ้ชนะเรียบร้อยแล้ว"
    };
    
    settingType6Resource["message"] = settingType6Resource["message"] || {};
    settingType6Resource["message"]["vi-vn"] = {
        "AutoResetIsViolatedWithUpline": "Chức năng tự động reset giới hạn chuyển khoản mỗi ngày đã được tắt cho cấp trên.",
        "MaximumLossValueIsRequired": "Tiền thua cược lớn nhất không thể trống.",
        "MaximumLossValueShouldBeInRange": "Tiền thua cược lớn nhất phải nằm trong khoảng ({0} - {1}).",
        "MaximumWinShouldBotExceedThanUplineMaximumWin": "Tiền thắng cược lớn nhất phải ít hơn hoặc bằng Tiền thắng cược lớn nhất của cấp trên trực tiếp.",
        "MaximumWinShouldNotBeLessThanZero": "Tiền thắng cược lớn nhất phải lớn hơn 0.",
        "Msg_10022": "Bạn không được phép thay đổi chức năng Tự động reset giới hạn chuyển khoản mỗi ngày.",
        "Msg_10024": "Tiền thắng cược lớn nhất phải ít hơn hoặc bằng Tiền thắng cược lớn nhất của cấp trên trực tiếp.",
        "Msg_10033": "Tiền thua cược lớn nhất phải ít hơn hoặc bằng Tiền thua cược lớn nhất của cấp trên trực tiếp.",
        "UpdatedWinLossLimitSuccessfully": "Cập nhật Giới hạn thắng thua thành công."
    };

    angular.module("customerSetting").value("settingType6Resource", settingType6Resource);
})();
