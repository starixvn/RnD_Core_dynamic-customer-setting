/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType20Resources = {};
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["en-us"] = {
        "BetSetting": "Bet Setting",
        "MaxBet": "Max Bet",
        "MaxPerNumber": "Max Per Number",
        "MinBet": "Min Bet"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["zh-cn"] = {
        "BetSetting": "投注设定",
        "MaxBet": "最高投注",
        "MaxPerNumber": "最高数额",
        "MinBet": "最低投注"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["zh-tw"] = {
        "BetSetting": "投注設定",
        "MaxBet": "最高投注",
        "MaxPerNumber": "最高數額",
        "MinBet": "最低投注"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["ko-kr"] = {
        "BetSetting": "배팅 설정",
        "MaxBet": "최대 배팅",
        "MinBet": "최소 배팅"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["ja-jp"] = {
        "MaxBet": "最大ベット"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["th-th"] = {
        "BetSetting": "การกำหนดการวางเดิมพัน",
        "MaxBet": "ค่าเดิมพันสูงสุด",
        "MaxPerNumber": "ค่าเดิมพันสูงสุดต่อหมายเลข",
        "MinBet": "ค่าเดิมพันต้ำสุด"
    };
    
    settingType20Resources["label"] = settingType20Resources["label"] || {};
    settingType20Resources["label"]["vi-vn"] = {
        "BetSetting": "Tiền Cược",
        "MaxBet": "Cược lớn nhất",
        "MaxPerNumber": "Tiền Cược tối đa cho mỗi số",
        "MinBet": "Cược nhỏ nhất"
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["en-us"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "BetLimit cannot be greater than Max Value.",
        "MaxBetCannotBeGreaterThanMaxPerNumber": "Max Bet cannot be greater than Max Per Number.",
        "MaxBetCannotBeNegative": "Max Bet cannot be negative.",
        "MaxBetLessThanOrEqualMaxPerNumber": "The Max Bet should be less than or equal to Max Per Number.",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}.",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet.",
        "MaxPerNumberCannotBeNegative": "Max Per Number cannot be negative.",
        "MaxPerNumberGreaterThanOrEqualMaxBet": "Max Per Number must be greater than or equal to Max Bet.",
        "MaxPerNumberLessThanOrEqualTo": "Max Per Number must be less than or equal to {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet.",
        "MinBetCannotBeNegative": "Min Bet cannot be negative.",
        "MinBetLessThanOrEqualMaxBet": "The Min Bet should be less than or equal to Max Bet.",
        "MinBetMoreThanOrEqualTo": "Min Bet must be greater than or equal to {0}.",
        "Msg_10006": "Min Bet must be greater than or equal to Agent&apos;s Min Bet.",
        "Msg_10007": "Max Bet must be less than or equal to Agent&apos;s Max Bet.",
        "Msg_10016": "Min Bet must be greater than or equal to Super Agent&apos;s Min Bet.",
        "Msg_10017": "Max Bet must be less than or equal to Super Agent&apos;s Max Bet.",
        "Msg_10019": "Min Bet must be greater than or equal to Master Agent&apos;s Min Bet.",
        "Msg_10020": "Max Bet must be less than or equal to Master Agent&apos;s Max Bet.",
        "Msg_10035": "Invalid Min Bet/ Max Bet/ Max Per Number.",
        "Msg_10036": "Min Bet must be less than or equal to Max Bet and Max Bet must be less than or equal to Max Per Number.",
        "Msg_10037": "Max Per Number must be less than or equal to Super Agent&apos;s Max Per Number.",
        "Msg_10038": "Max Per Number must be less than or equal to Master Agent&apos;s Max Per Number.",
        "Msg_10039": "Max Per Number must be less than or equal to Agent Max Per Number.",
        "RequiredField": "This field is required.",
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["zh-cn"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "投注限额不能大于最高值",
        "MaxBetCannotBeGreaterThanMaxPerNumber": "最高投注不能大于最高数额。",
        "MaxBetCannotBeNegative": "最高投注不能为负数。",
        "MaxBetLessThanOrEqualMaxPerNumber": "最高投注应小于或等于每个最高数额。",
        "MaxBetLessThanOrEqualTo": "最高投注不得多于或相等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投注限额必须大于或相等于最低投注限额",
        "MaxPerNumberCannotBeNegative": "最高数额不能为负数。",
        "MaxPerNumberGreaterThanOrEqualMaxBet": "最高数额必须大于或等于最高投注。",
        "MaxPerNumberLessThanOrEqualTo": "最高数额必须小于或等于{0}。",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高于最低投注",
        "MinBetCannotBeNegative": "最低投注不能为负数。",
        "MinBetLessThanOrEqualMaxBet": "最低投注必须少于或相等于最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注额必须大于或等于 {0}",
        "Msg_10006": "最低投注必须大于或相等于代理最低投注限额",
        "Msg_10007": "最高投注必须少于或相等于代理最高投注限额",
        "Msg_10016": "最低投注大于或相等于超级总代理最低投注限额",
        "Msg_10017": "最高投注必须少于或相等于超级总代理最高投注限额",
        "Msg_10019": "最低投注大于或相等于总代理最低投注限额",
        "Msg_10020": "最高投注必须少于或相等于总代理最高投注限额",
        "Msg_10035": "无效的最低投注/最高投注/最高数额。",
        "Msg_10036": "最低投注必须小于或等于最高投注，最高投注必须小于或等于最高数额。",
        "Msg_10037": "最高数额必须小于或等于超级总代理的最高数额。",
        "Msg_10038": "最高数额必须小于或等于总代理的最高数额。",
        "Msg_10039": "最高数额必须小于或等于代理的最高数额。",
        "RequiredField": "必填栏",
        "UpdateBetSettingSuccessfully": "投注设定已更新成功."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["zh-tw"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "投注限額不能大於最高值。",
        "MaxBetCannotBeGreaterThanMaxPerNumber": "最高投注不能大於最高數額。",
        "MaxBetCannotBeNegative": "最高投注不能為負數。",
        "MaxBetLessThanOrEqualMaxPerNumber": "最高投注應小於或等於每個最高數額。",
        "MaxBetLessThanOrEqualTo": "最高投注不得多於或相等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投註限額必須大於或相等於最低投註限額",
        "MaxPerNumberCannotBeNegative": "最高數額不能為負數。",
        "MaxPerNumberGreaterThanOrEqualMaxBet": "最高數額必須大於或等於最高投注。",
        "MaxPerNumberLessThanOrEqualTo": "最高數額必須小於或等於{0}。",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高於最高投注",
        "MinBetCannotBeNegative": "最低投注不能為負數。",
        "MinBetLessThanOrEqualMaxBet": "最低投注必須少於或相等於最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注額必須大於或等於 {0}",
        "Msg_10006": "最低投註必須大於或相等於代理最低投註限額",
        "Msg_10007": "最高投註必須少於或相等於代理最高投註限額",
        "Msg_10016": "最低投註大於或相等於超級總代理最低投註限額",
        "Msg_10017": "最高投註必須少於或相等於超級總代理最高投註限額",
        "Msg_10019": "最低投註大於或相等於總代理最低投註限額",
        "Msg_10020": "最高投註必須少於或相等於總代理最高投註限額",
        "Msg_10035": "無效的最低投注/最高投注/最高數額。",
        "Msg_10036": "最低投注必須小於或等於最高投注，最高投注必須小於或等於最高數額。",
        "Msg_10037": "最高數額必須小於或等於超級總代理的最高數額。",
        "Msg_10038": "最高數額必須小於或等於總代理的最高數額。",
        "Msg_10039": "最高數額必須小於或等於代理的最高數額。",
        "RequiredField": "必填欄",
        "UpdateBetSettingSuccessfully": "投注設定已更新成功."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["ko-kr"] = {
        "MaxBetLessThanOrEqualTo": "최대 배팅보다 이하이여야 합니다 {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "최소 배팅은 최대 배팅보다 클수 없습니다.",
        "MinBetLessThanOrEqualMaxBet": "최소베팅은 최대 베팅과 같거나  이하이어야합니다.",
        "UpdateBetSettingSuccessfully": "베팅 설정이 성공적으로 업데이트되었습니다."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["ja-jp"] = {
        "MaxBetLessThanOrEqualTo": "一回当たりのマックスベットポイントは表示されていますので、再確認してください {0}."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["th-th"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "ค่าเดิมพันต้องไม่มากกว่าค่าเดิมพันสูงสุด",
        "MaxBetCannotBeGreaterThanMaxPerNumber": "ค่าเดิมพันสูงสุดต้องไม่มากว่าค่าเดิมพันสูงสุดต่อหมายเลข",
        "MaxBetCannotBeNegative": "ค่าเดิมพันสูงสุดต้องไม่ติดลบ",
        "MaxBetLessThanOrEqualMaxPerNumber": "ค่าเดิมพันสูงสุดควรจะน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อหมายเลข",
        "MaxBetLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุดต้องน้อยกว่า หรือเท่ากับ {0}.",
        "MaxBetMoreThanOrEqualMinBet": "ค่าการเดิมพันขั้นสูงสุดต้องมากกว่าหรือเท่ากับค่าการเดิมพันขั้นต่ำ",
        "MaxPerNumberCannotBeNegative": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องไม่ติดลบ",
        "MaxPerNumberGreaterThanOrEqualMaxBet": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องมากกว่าหรือเท่ากับค่าเดิมพันสูงสุด",
        "MaxPerNumberLessThanOrEqualTo": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องน้อยกว่าหรือเท่ากับ{0}",
        "MinBetCannotBeGreaterThanMaxBet": "จำนวนเงินต่ำสุดที่สามารถเล่นได้ ต้องไม่มากกว่า จำนวนเงินสูงสุดที่สามารถเล่นได้",
        "MinBetCannotBeNegative": "ค่าเดิมพันต่ำสุดต้องไม่ติดลบ",
        "MinBetLessThanOrEqualMaxBet": "ค่าเดิมพันขั้นต่ำควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุด.",
        "MinBetMoreThanOrEqualTo": "จำนวนการเดิมพันต่ำสุดต้องมากกว่า หรือเท่ากับ {0}.",
        "Msg_10006": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันต่ำสุดของเอเจนท์",
        "Msg_10007": "ค่าวางเดิมพันขั้นสุงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันสูงสุดของเอเจนท์",
        "Msg_10016": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันขั้นต่ำของซูเปอร์เอเจนท์",
        "Msg_10017": "ค่าวางเดิมพันขั้นสูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดของซูเปอร์เอเจนท์",
        "Msg_10019": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันขั้นต่ำของมาสเตอร์เอเจนท์",
        "Msg_10020": "ค่าวางเดิมพันขั้นสูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดของมาสเตอร์เอเจนท์",
        "Msg_10035": "ค่าเดิมพันต่ำสุด/ ค่าเดิมพันสูงสุด/ ค่าเดิมพันสูงสุดต่อหมายเลข ไม่ถูกต้อง",
        "Msg_10036": "ค่าเดิมพันต่ำสุดต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุด และ  ค่าเดิมพันสูงสุดต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อหมายเลข",
        "Msg_10037": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อหมายเลขของซุปเปอร์เอเจ่นต์",
        "Msg_10038": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อหมายเลขของมาสเตอร์เอเจ่นต์",
        "Msg_10039": "ค่าเดิมพันสูงสุดต่อหมายเลขต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อหมายเลขของเอเจ่นต์",
        "RequiredField": "จำเป็นต้องใส่ข้อมูลนี้",
        "UpdateBetSettingSuccessfully": "การตั้งค่าเดิมพัน ได้อัพเดทเรียบร้อยแล้ว."
    };
    
    settingType20Resources["message"] = settingType20Resources["message"] || {};
    settingType20Resources["message"]["vi-vn"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "Hạn mức đặt cược không thể lớn hơn Max Value.",
        "MaxBetCannotBeGreaterThanMaxPerNumber": "Tiền Cược lớn nhất không thể lớn hơn Tiền Cược tối đa cho mỗi số.",
        "MaxBetCannotBeNegative": "Tiền Cược lớn nhất không thể là số âm.",
        "MaxBetLessThanOrEqualMaxPerNumber": "Tiền Cược lớn nhất phải nhỏ hơn hoặc bằng Tiền Cược tối đa cho mỗi số.",
        "MaxBetLessThanOrEqualTo": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng {0}",
        "MaxBetMoreThanOrEqualMinBet": "Tiền cược lớn nhất phải lớn hơn hoặc bằng tiền cược nhỏ nhất.",
        "MaxPerNumberCannotBeNegative": "Tiền Cược tối đa cho mỗi số không thể là số âm.",
        "MaxPerNumberGreaterThanOrEqualMaxBet": "Tiền Cược tối đa cho mỗi số phải lớn hơn hoặc bằng Tiền Cược lớn nhất.",
        "MaxPerNumberLessThanOrEqualTo": "Tiền Cược tối đa cho mỗi số phải nhỏ hơn hoặc bằng {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "Tiền cược nhỏ nhất không thể lớn hơn Tiền cược lớn nhất",
        "MinBetCannotBeNegative": "Tiền Cược nhỏ nhất không thể là số âm.",
        "MinBetLessThanOrEqualMaxBet": "Tiền cược nhỏ nhất phải nhỏ hơn hoặc bằng Tiền cược lớn nhất.",
        "MinBetMoreThanOrEqualTo": "Cược Nhỏ Nhất phải lớn hơn hay bằng {0}.",
        "Msg_10006": "Tiền Cược nhỏ nhất phải lớn hơn hoặc bằng Tiền Cược nhỏ nhất của Agent.",
        "Msg_10007": "Tiền Cược lớn nhất phải nhỏ hơn hoặc bằng Tiền Cược lớn nhất của Agent.",
        "Msg_10016": "Tiền Cược nhỏ nhất phải lớn hơn hoặc bằng Tiền Cược nhỏ nhất của Super.",
        "Msg_10017": "Tiền Cược lớn nhất phải nhỏ hơn hoặc bằng Tiền Cược lớn nhất của Super.",
        "Msg_10019": "Tiền Cược nhỏ nhất phải lớn hơn hoặc bằng Tiền Cược nhỏ nhất của Master",
        "Msg_10020": "Tiền Cược lớn nhất phải nhỏ hơn hoặc bằng Tiền Cược lớn nhất của Master.",
        "Msg_10035": "Giá trị Tiền Cược nhỏ nhất / Tiền Cược lớn nhất / Tiền Cược tối đa cho mỗi số không hợp lệ.",
        "Msg_10036": "Tiền Cược nhỏ nhất phải nhỏ hơn hoặc bằng Tiền Cược lớn nhất và Tiền Cược lớn nhất phải nhỏ hơn hoặc bằng Tiền Cược tối đa cho mỗi số.",
        "Msg_10037": "Tiền Cược tối đa cho mỗi số phải nhỏ hơn hoặc bằng Tiền Cược tối đa cho mỗi số của Super.",
        "Msg_10038": "Tiền Cược tối đa cho mỗi số phải nhỏ hơn hoặc bằng Tiền Cược tối đa cho mỗi số của Master.",
        "Msg_10039": "Tiền Cược tối đa cho mỗi số phải nhỏ hơn hoặc bằng Tiền Cược tối đa cho mỗi số của Agent.",
        "RequiredField": "Thông tin này bắt buộc phải nhập.",
        "UpdateBetSettingSuccessfully": "Thiết lập về tiền cược được cập nhật thành công"
    };

    angular.module("customerSetting").value("settingType20Resources", settingType20Resources);
})();
