/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var betSettingResources = {};
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["en-us"] = {
        "BetSetting": "Bet Setting",
        "MaxBet": "Max Bet",
        "MaxPerMatch": "Max Per Match",
        "MinBet": "Min Bet"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["zh-cn"] = {
        "BetSetting": "投注设定",
        "MaxBet": "最高投注",
        "MaxPerMatch": "单场限额",
        "MinBet": "最低投注"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["zh-tw"] = {
        "BetSetting": "投注設定",
        "MaxBet": "最高投注",
        "MaxPerMatch": "單場限額",
        "MinBet": "最低投注"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["ko-kr"] = {
        "BetSetting": "배팅 설정",
        "MaxBet": "최대 배팅",
        "MaxPerMatch": "경기당 최대 값",
        "MinBet": "최소 배팅"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["ja-jp"] = {
        "MaxBet": "最大ベット",
        "MaxPerMatch": "1 試合毎の最大"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["th-th"] = {
        "BetSetting": "การกำหนดการวางเดิมพัน",
        "MaxBet": "ค่าเดิมพันสูงสุด",
        "MaxPerMatch": "ค่าเดิมพันสูงสุดต่อคู่การแข่งขัน",
        "MinBet": "ค่าเดิมพันต้ำสุด"
    };
    
    betSettingResources["label"] = betSettingResources["label"] || {};
    betSettingResources["label"]["vi-vn"] = {
        "BetSetting": "Tiền Cược",
        "MaxBet": "Cược lớn nhất",
        "MaxPerMatch": "Cược tối đa cho 1 trận",
        "MinBet": "Cược nhỏ nhất"
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["en-us"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "BetLimit cannot be greater than Max Value.",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Max Bet cannot be greater than Max Per Match.",
        "MaxBetCannotBeNegative": "Max Bet cannot be negative.",
        "MaxBetLessThanOrEqualMaxPerMatch": "The Max Bet should be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}.",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet.",
        "MaxPerMatchCannotBeNegative": "Max Per Match cannot be negative.",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}.",
        "MaxPerMatchMoreThanOrEqualMaxBet": "Max Per Match must be greater than or equal to Max Bet.",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet.",
        "MinBetCannotBeNegative": "Min Bet cannot be negative.",
        "MinBetLessThanOrEqualMaxBet": "The Min Bet should be less than or equal to Max",
        "MinBetMoreThanOrEqualTo": "Min Bet must be greater than or equal to {0}.",
        "Msg_10003": "Invalid Min bet/ Max bet/ Max per match.",
        "Msg_10005": "Min bet must be less than or equal to max bet and max bet must be less than or equal to max bet per match.",
        "Msg_10006": "Min bet must be greater than or equal to Agent min bet.",
        "Msg_10007": "Max bet must be less than or equal to Agent max bet.",
        "Msg_10008": "Max Per Match must be less than or equal to Agent Max Per Match.",
        "Msg_10016": "Min bet must be greater than or equal to Super Agent min bet.",
        "Msg_10017": "Max bet must be less than or equal to Super Agent max bet.",
        "Msg_10018": "Max Per Match must be less than or equal to Super Agent Max Per Match.",
        "Msg_10019": "Min bet must be greater than or equal to Master Agent min bet.",
        "Msg_10020": "Max bet must be less than or equal to Master Agent max bet.",
        "Msg_10021": "Max Per Match must be less than or equal to Master Agent Max Per Match.",
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["zh-cn"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "最高投注不可高于当场最高投注",
        "MaxBetLessThanOrEqualMaxPerMatch": "最高投注必须少于或相等于单场限额",
        "MaxBetLessThanOrEqualTo": "最高投注不得多于或相等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注额必须大于或等于最小投注额",
        "MaxPerMatchLessThanOrEqualTo": "单场最高投注需低于或相等于 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大单场比赛投注额必须大于或等于最大投注额",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高于最低投注",
        "MinBetLessThanOrEqualMaxBet": "最低投注必须少于或相等于最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注额必须大于或等于 {0}",
        "UpdateBetSettingSuccessfully": "投注设定已更新成功."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["zh-tw"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "最高投注不可高於單場最高投注",
        "MaxBetLessThanOrEqualMaxPerMatch": "最高投注必須少於或相等於單場限額",
        "MaxBetLessThanOrEqualTo": "最高投注不得多於或相等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注額必須大於或等於最小投注額",
        "MaxPerMatchLessThanOrEqualTo": "單場最高投注需底於或等於 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大單場比賽投注額必須大於或等於最大投注額",
        "MinBetCannotBeGreaterThanMaxBet": "最低投注不可高於最高投注",
        "MinBetLessThanOrEqualMaxBet": "最低投注必須少於或相等於最高投注",
        "MinBetMoreThanOrEqualTo": "最大投注額必須大於或等於 {0}",
        "UpdateBetSettingSuccessfully": "投注設定已更新成功."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["ko-kr"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "최대 배팅은 경기당최대 값보다 클수 없습니다",
        "MaxBetLessThanOrEqualMaxPerMatch": "최대베팅은 경기 당 최대치와 같거나 이하이어야합니다",
        "MaxBetLessThanOrEqualTo": "최대 배팅보다 이하이여야 합니다 {0}.",
        "MaxPerMatchLessThanOrEqualTo": "경기당 최대 값은 {0}과 같거나 작아야 합니다.",
        "MinBetCannotBeGreaterThanMaxBet": "최소 배팅은 최대 배팅보다 클수 없습니다.",
        "MinBetLessThanOrEqualMaxBet": "최소베팅은 최대 베팅과 같거나  이하이어야합니다.",
        "UpdateBetSettingSuccessfully": "베팅 설정이 성공적으로 업데이트되었습니다."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["ja-jp"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Max Bet cannot be greater than Max Per Match",
        "MaxBetLessThanOrEqualMaxPerMatch": "The Max Bet should be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "一回当たりのマックスベットポイントは表示されていますので、再確認してください {0}.",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet.",
        "MinBetLessThanOrEqualMaxBet": "The Min Bet should be less than or equal to Max Bet",
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["th-th"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "จำนวนเงินสูงสุดที่สามารถเล่นได้ต้องไม่มากกว่าจำนวนเงินสูงสุดต่อรายการที่เล่น",
        "MaxBetLessThanOrEqualMaxPerMatch": "ค่าเดิมพันสูงสุดควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุดต่อการแข่งขัน",
        "MaxBetLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุดต้องน้อยกว่า หรือเท่ากับ {0}.",
        "MaxPerMatchLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุงต่อรายการต้องน้อยกว่า หรือเท่ากับ {0}",
        "MinBetCannotBeGreaterThanMaxBet": "จำนวนเงินต่ำสุดที่สามารถเล่นได้ ต้องไม่มากกว่า จำนวนเงินสูงสุดที่สามารถเล่นได้",
        "MinBetLessThanOrEqualMaxBet": "ค่าเดิมพันขั้นต่ำควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุด.",
        "UpdateBetSettingSuccessfully": "การตั้งค่าเดิมพัน ได้อัพเดทเรียบร้อยแล้ว."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["vi-vn"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Tiền cược lớn nhất không thể lớn hơn Tiền cược tối đa của mỗi trận đấu",
        "MaxBetLessThanOrEqualMaxPerMatch": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng Tiền cược tối đa của mỗi trận đấu",
        "MaxBetLessThanOrEqualTo": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng {0}",
        "MaxPerMatchLessThanOrEqualTo": "Tiền cược tối đa cho mỗi trận phài ít hơn hoặc bằng {0}",
        "MinBetCannotBeGreaterThanMaxBet": "Tiền cược nhỏ nhất không thể lớn hơn Tiền cược lớn nhất",
        "MinBetLessThanOrEqualMaxBet": "Tiền cược nhỏ nhất phải nhỏ hơn hoặc bằng Tiền cược lớn nhất.",
        "UpdateBetSettingSuccessfully": "Thiết lập về tiền cược được cập nhật thành công"
    };

    angular.module("customerSetting").value("betSettingResources", betSettingResources);
})();
