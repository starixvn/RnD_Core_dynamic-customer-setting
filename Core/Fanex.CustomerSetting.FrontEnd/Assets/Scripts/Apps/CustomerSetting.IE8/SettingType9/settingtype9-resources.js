/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType9Resources = {};
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["en-us"] = {
        "BetSetting": "Bet Setting",
        "MaxBet": "Max Bet",
        "MaxPerNumber": "Max Per Number",
        "MinBet": "Min Bet"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["zh-cn"] = {
        "BetSetting": "投注设定",
        "MaxBet": "最高投注",
        "MinBet": "最低投注"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["zh-tw"] = {
        "BetSetting": "投注設定",
        "MaxBet": "最高投注",
        "MinBet": "最低投注"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["ko-kr"] = {
        "BetSetting": "배팅 설정",
        "MaxBet": "최대 배팅",
        "MinBet": "최소 배팅"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["ja-jp"] = {
        "MaxBet": "最大ベット"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["th-th"] = {
        "BetSetting": "การกำหนดการวางเดิมพัน",
        "MaxBet": "ค่าเดิมพันสูงสุด",
        "MinBet": "ค่าเดิมพันต้ำสุด"
    };
    
    settingType9Resources["label"] = settingType9Resources["label"] || {};
    settingType9Resources["label"]["vi-vn"] = {
        "BetSetting": "Tiền Cược",
        "MaxBet": "Cược lớn nhất",
        "MinBet": "Cược nhỏ nhất"
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["en-us"] = {
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
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["zh-cn"] = {
        "MaxBetLessThanOrEqualTo": "最高投注不得多于或相等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投注限额必须大于或相等于最低投注限额",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高于最低投注",
        "MinBetLessThanOrEqualMaxBet": "最低投注必须少于或相等于最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注额必须大于或等于 {0}",
        "Msg_10003": "最低投注/最高投注/单场限额无效",
        "Msg_10005": "最低投注必须少于或相等于最高投注以及最高投注必须小于或相等于单场投注限额",
        "Msg_10006": "最低投注必须大于或相等于代理最低投注限额",
        "Msg_10007": "最高投注必须少于或相等于代理最高投注限额",
        "Msg_10008": "单场限额必须少于或相等于代理单场投注限额",
        "Msg_10016": "最低投注大于或相等于超级总代理最低投注限额",
        "Msg_10017": "最高投注必须少于或相等于超级总代理最高投注限额",
        "Msg_10018": "单场限额必须少于或相等于超级总代理单场投注限额",
        "Msg_10019": "最低投注大于或相等于总代理最低投注限额",
        "Msg_10020": "最高投注必须少于或相等于总代理最高投注限额",
        "Msg_10021": "单场限额必须少于或相等于总代理单场投注限额",
        "UpdateBetSettingSuccessfully": "投注设定已更新成功."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["zh-tw"] = {
        "MaxBetLessThanOrEqualTo": "最高投注不得多於或相等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投註限額必須大於或相等於最低投註限額",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高於最高投注",
        "MinBetLessThanOrEqualMaxBet": "最低投注必須少於或相等於最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注額必須大於或等於 {0}",
        "Msg_10003": "最低投註/最高投註/單場限額無效",
        "Msg_10005": "最低投註必須少於或相等於最高投註以及最高投註必須小於或相等於單場投註限額",
        "Msg_10006": "最低投註必須大於或相等於代理最低投註限額",
        "Msg_10007": "最高投註必須少於或相等於代理最高投註限額",
        "Msg_10008": "單場限額必須少於或相等於代理單場投註限額",
        "Msg_10016": "最低投註大於或相等於超級總代理最低投註限額",
        "Msg_10017": "最高投註必須少於或相等於超級總代理最高投註限額",
        "Msg_10018": "單場限額必須少於或相等於超級總代理單場投註限額",
        "Msg_10019": "最低投註大於或相等於總代理最低投註限額",
        "Msg_10020": "最高投註必須少於或相等於總代理最高投註限額",
        "Msg_10021": "單場限額必須少於或相等於總代理單場投註限額",
        "UpdateBetSettingSuccessfully": "投注設定已更新成功."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["ko-kr"] = {
        "MaxBetLessThanOrEqualTo": "최대 배팅보다 이하이여야 합니다 {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "최소 배팅은 최대 배팅보다 클수 없습니다.",
        "MinBetLessThanOrEqualMaxBet": "최소베팅은 최대 베팅과 같거나  이하이어야합니다.",
        "UpdateBetSettingSuccessfully": "베팅 설정이 성공적으로 업데이트되었습니다."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["ja-jp"] = {
        "MaxBetLessThanOrEqualTo": "一回当たりのマックスベットポイントは表示されていますので、再確認してください {0}."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["th-th"] = {
        "MaxBetLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุดต้องน้อยกว่า หรือเท่ากับ {0}.",
        "MaxBetMoreThanOrEqualMinBet": "ค่าการเดิมพันขั้นสูงสุดต้องมากกว่าหรือเท่ากับค่าการเดิมพันขั้นต่ำ",
        "MinBetCannotBeGreaterThanMaxBet": "จำนวนเงินต่ำสุดที่สามารถเล่นได้ ต้องไม่มากกว่า จำนวนเงินสูงสุดที่สามารถเล่นได้",
        "MinBetLessThanOrEqualMaxBet": "ค่าเดิมพันขั้นต่ำควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุด.",
        "Msg_10003": "ค่า เดิมพันขั้นต่ำ/ เดิมพันขั้นสูดสุด/ เดิมพันต่อเกมสูงสุด ไม่ถูกต้อง",
        "Msg_10005": "ค่าการเดิมพันขั้นต่ำต้องน้อยกว่าหรือเท่ากับค่าเดิมพันขั้นสูงสุด และ ค่าเดิมพันสูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าเดิมพันสูงสุดต่อแมทซ์",
        "Msg_10006": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันต่ำสุดของเอเจนท์",
        "Msg_10007": "ค่าวางเดิมพันขั้นสุงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันสูงสุดของเอเจนท์",
        "Msg_10008": "ค่าวางเดิมพันต่อแมทซ์สูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันสูงสุดต่อแมทซ์ของเอเจนท์",
        "Msg_10016": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันขั้นต่ำของซูเปอร์เอเจนท์",
        "Msg_10017": "ค่าวางเดิมพันขั้นสูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดของซูเปอร์เอเจนท์",
        "Msg_10018": "ค่าวางเดิมพันขั้นสูงสุดต่อแมทซ์ต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดของซูเปอร์เอเจนท์ต่อแมทซ์",
        "Msg_10019": "ค่าวางเดิมพันขั้นต่ำต้องมากกว่าหรือเท่ากับ ค่าการเดิมพันขั้นต่ำของมาสเตอร์เอเจนท์",
        "Msg_10020": "ค่าวางเดิมพันขั้นสูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดของมาสเตอร์เอเจนท์",
        "Msg_10021": "ค่าวางเดิมพันต่อแมทซ์สูงสุดต้องน้อยกว่าหรือเท่ากับ ค่าการเดิมพันขั้นสูงสุดต่อแมทซ์ของมาสเตอร์เอเจนท์",
        "UpdateBetSettingSuccessfully": "การตั้งค่าเดิมพัน ได้อัพเดทเรียบร้อยแล้ว."
    };
    
    settingType9Resources["message"] = settingType9Resources["message"] || {};
    settingType9Resources["message"]["vi-vn"] = {
        "MaxBetLessThanOrEqualTo": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng {0}",
        "MinBetCannotBeGreaterThanMaxBet": "Tiền cược nhỏ nhất không thể lớn hơn Tiền cược lớn nhất",
        "MinBetLessThanOrEqualMaxBet": "Tiền cược nhỏ nhất phải nhỏ hơn hoặc bằng Tiền cược lớn nhất.",
        "UpdateBetSettingSuccessfully": "Thiết lập về tiền cược được cập nhật thành công"
    };

    angular.module("customerSetting").value("settingType9Resources", settingType9Resources);
})();
