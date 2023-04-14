/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType23Resources = {};
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["en-us"] = {
        "BetSetting": "Bet Setting",
        "MaxBet": "Max Bet",
        "MaxPerBall": "Max Per Ball",
        "MaxPerMatch": "Max Per Match",
        "MinBet": "Min Bet"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["zh-cn"] = {
        "BetSetting": "投注设定",
        "MaxBet": "最高投注",
        "MaxPerBall": "单粒球最高投注",
        "MaxPerMatch": "单场限额",
        "MinBet": "最低投注"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["zh-tw"] = {
        "BetSetting": "投注設定",
        "MaxBet": "最高投注",
        "MaxPerBall": "单粒球最高投注",
        "MaxPerMatch": "單場限額",
        "MinBet": "最低投注"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["ko-kr"] = {
        "BetSetting": "배팅 설정",
        "MaxBet": "최대 배팅",
        "MaxPerBall": "각 공의 최대",
        "MaxPerMatch": "경기당 최대 값",
        "MinBet": "최소 배팅"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["ja-jp"] = {
        "MaxBet": "最大ベット",
        "MaxPerBall": "Max Per Ball",
        "MaxPerMatch": "1 試合毎の最大"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["th-th"] = {
        "BetSetting": "การกำหนดการวางเดิมพัน",
        "MaxBet": "ค่าเดิมพันสูงสุด",
        "MaxPerBall": "เดิมพันสูงสุดต่อบอล",
        "MaxPerMatch": "ค่าเดิมพันสูงสุดต่อคู่การแข่งขัน",
        "MinBet": "ค่าเดิมพันต้ำสุด"
    };
    
    settingType23Resources["label"] = settingType23Resources["label"] || {};
    settingType23Resources["label"]["vi-vn"] = {
        "BetSetting": "Tiền Cược",
        "MaxBet": "Cược lớn nhất",
        "MaxPerBall": "Cược tối đa cho 1 lượt banh",
        "MaxPerMatch": "Cược tối đa cho 1 trận",
        "MinBet": "Cược nhỏ nhất"
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["en-us"] = {
        "BetLimitCannotBeGreaterThanMaxValue": "BetLimit cannot be greater than Max Value.",
        "MaxBetCannotBeGreaterThanMaxPerBall": "Max Bet cannot be greater than Max Per Ball.",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Max Bet cannot be greater than Max Per Match.",
        "MaxBetCannotBeNegative": "Max Bet cannot be negative.",
        "MaxBetLessThanOrEqualMaxPerBall": "Max Bet must be less than or equal to Max Per Ball",
        "MaxBetLessThanOrEqualMaxPerMatch": "The Max Bet should be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}.",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet.",
        "MaxPerBallCannotBeGreaterThanMaxPerMatch": "Max Per Ball cannot be greater than Max Per Match.",
        "MaxPerBallCannotBeNegative": "Max Per Ball cannot be negative.",
        "MaxPerBallLessThanOrEqualMaxPerMatch": "Max Per Ball must be less than or equal to Max Per Match",
        "MaxPerBallLessThanOrEqualTo": "Max Per Ball must be less than or equal to {0}.",
        "MaxPerBallMoreThanOrEqualMaxBet": "Max Per Ball must be greater than or equal to Max Bet.",
        "MaxPerMatchCannotBeNegative": "Max Per Match cannot be negative.",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}.",
        "MaxPerMatchMoreThanOrEqualMaxBet": "Max Per Match must be greater than or equal to Max Bet.",
        "MaxPerMatchMoreThanOrEqualMaxPerBall": "Max Per Match must be greater than or equal to Max Per Ball.",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet.",
        "MinBetCannotBeNegative": "Min Bet cannot be negative.",
        "MinBetLessThanOrEqualMaxBet": "The Min Bet should be less than or equal to Max Bet.",
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
        "RequiredField": "This field is required.",
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["zh-cn"] = {
        "MaxBetCannotBeGreaterThanMaxPerBall": "最高投注不能大于单粒球最高投注。",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "最高投注不可高于当场最高投注",
        "MaxBetLessThanOrEqualMaxPerBall": "最高投注必须少于或等于单粒球最高投注",
        "MaxBetLessThanOrEqualMaxPerMatch": "最高投注必须少于或相等于单场限额",
        "MaxBetLessThanOrEqualTo": "最高投注不得多于或相等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投注限额必须大于或相等于最低投注限额",
        "MaxPerBallCannotBeGreaterThanMaxPerMatch": "单粒球最高投注不能大于单场限额。",
        "MaxPerBallCannotBeNegative": "单粒球最高投注不能是负数。",
        "MaxPerBallLessThanOrEqualMaxPerMatch": "單粒球最高投注必须少于或等于单场限额",
        "MaxPerBallLessThanOrEqualTo": "单粒球最高投注必须少于或相等于 {0}.",
        "MaxPerBallMoreThanOrEqualMaxBet": "单粒球最高投注必须大于或是等于最高投注。",
        "MaxPerMatchLessThanOrEqualTo": "单场最高投注需低于或相等于 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "单场投注限额必须大于或相等于最高投注限额",
        "MaxPerMatchMoreThanOrEqualMaxPerBall": "单场限额必须大于或是等于单粒球最高投注。",
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
        "RequiredField": "必填栏",
        "UpdateBetSettingSuccessfully": "投注设定已更新成功."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["zh-tw"] = {
        "MaxBetCannotBeGreaterThanMaxPerBall": "最高投注不能大於單粒球最高投注。",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "最高投注不可高於單場最高投注",
        "MaxBetLessThanOrEqualMaxPerBall": "最高投注必須少於或等於單粒球最高投注",
        "MaxBetLessThanOrEqualMaxPerMatch": "最高投注必須少於或相等於單場限額",
        "MaxBetLessThanOrEqualTo": "最高投注不得多於或相等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最高投註限額必須大於或相等於最低投註限額",
        "MaxPerBallCannotBeGreaterThanMaxPerMatch": "單粒球最高投注不能大於單場限額。",
        "MaxPerBallCannotBeNegative": "單粒球最高投注不能是負數。",
        "MaxPerBallLessThanOrEqualMaxPerMatch": "單粒球最高投注必須少於或等於單場限額",
        "MaxPerBallLessThanOrEqualTo": "單粒球最高投注必須少於或相等於 {0}.",
        "MaxPerBallMoreThanOrEqualMaxBet": "單粒球最高投注必須大於或是等於最高投注。",
        "MaxPerMatchLessThanOrEqualTo": "單場最高投注需底於或等於 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "單場投註限額必須大於或相等於最高投註限額",
        "MaxPerMatchMoreThanOrEqualMaxPerBall": "單場限額必須大於或是等於單粒球最高投注。",
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
        "RequiredField": "必填欄",
        "UpdateBetSettingSuccessfully": "投注設定已更新成功."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["ko-kr"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "최대 배팅은 경기당최대 값보다 클수 없습니다",
        "MaxBetLessThanOrEqualMaxPerMatch": "최대베팅은 경기 당 최대치와 같거나 이하이어야합니다",
        "MaxBetLessThanOrEqualTo": "최대 배팅보다 이하이여야 합니다 {0}.",
        "MaxPerBallLessThanOrEqualTo": "각 최대공은 {0}보다 작거나 같아야만 합니다.",
        "MaxPerMatchLessThanOrEqualTo": "경기당 최대 값은 {0}과 같거나 작아야 합니다.",
        "MinBetCannotBeGreaterThanMaxBet": "최소 배팅은 최대 배팅보다 클수 없습니다.",
        "MinBetLessThanOrEqualMaxBet": "최소베팅은 최대 베팅과 같거나  이하이어야합니다.",
        "UpdateBetSettingSuccessfully": "베팅 설정이 성공적으로 업데이트되었습니다."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["ja-jp"] = {
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Max Bet cannot be greater than Max Per Match",
        "MaxBetLessThanOrEqualMaxPerMatch": "The Max Bet should be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "一回当たりのマックスベットポイントは表示されていますので、再確認してください {0}.",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}.",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet.",
        "MinBetLessThanOrEqualMaxBet": "The Min Bet should be less than or equal to Max Bet",
        "UpdateBetSettingSuccessfully": "Bet Setting is updated successfully."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["th-th"] = {
        "MaxBetCannotBeGreaterThanMaxPerBall": "ค่าเดิมพันสูงสุดมากกว่าค่าเดิมพันสูงสุดต่อบอลไม่ได้",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "จำนวนเงินสูงสุดที่สามารถเล่นได้ต้องไม่มากกว่าจำนวนเงินสูงสุดต่อรายการที่เล่น",
        "MaxBetLessThanOrEqualMaxPerBall": "ค่าเดิมพันสูงสุดต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อบอล",
        "MaxBetLessThanOrEqualMaxPerMatch": "ค่าเดิมพันสูงสุดควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุดต่อการแข่งขัน",
        "MaxBetLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุดต้องน้อยกว่า หรือเท่ากับ {0}.",
        "MaxBetMoreThanOrEqualMinBet": "ค่าการเดิมพันขั้นสูงสุดต้องมากกว่าหรือเท่ากับค่าการเดิมพันขั้นต่ำ",
        "MaxPerBallCannotBeGreaterThanMaxPerMatch": "ค่าเดิมพันสูงสุดต่อบอลมากกว่าค่าเดิมพันสูงสุดต่อเกมไม่ได้",
        "MaxPerBallCannotBeNegative": "ค่าเดิมพันสูงสุดต่อบอล เป็นเลขติดลบไม่ได้",
        "MaxPerBallLessThanOrEqualMaxPerMatch": "ค่าเดิมพันสูงสุดต่อบอลต้องน้อยกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อคู่การแข่งขัน",
        "MaxPerBallLessThanOrEqualTo": "สูงสุดต่อบอล ต้อง น้อยกว่า หรือ เท่ากับ {0}.",
        "MaxPerBallMoreThanOrEqualMaxBet": "ค่าเดิมพันสูงสุดต่อบอลต้องมากกว่าหรือเท่ากับค่าเดิมพันสูงสุด",
        "MaxPerMatchLessThanOrEqualTo": "จำนวนการเดิมพันสูงสุงต่อรายการต้องน้อยกว่า หรือเท่ากับ {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "ค่าการเดิมพันสูงสุดต่อแมทช์ต้องมากกว่าหรือเท่ากับค่าการเดิมพันขั้นสูงสุด",
        "MaxPerMatchMoreThanOrEqualMaxPerBall": "ค่าเดิมพันสูงสุดต่อเกมต้องมากกว่าหรือเท่ากับค่าเดิมพันสูงสุดต่อบอล",
        "MinBetCannotBeGreaterThanMaxBet": "จำนวนเงินต่ำสุดที่สามารถเล่นได้ ต้องไม่มากกว่า จำนวนเงินสูงสุดที่สามารถเล่นได้",
        "MinBetLessThanOrEqualMaxBet": "ค่าเดิมพันขั้นต่ำควรจะน้อยกว่า หรือ เท่ากับค่าเดิมพันสูงสุด.",
        "MinBetMoreThanOrEqualTo": "จำนวนการเดิมพันต่ำสุดต้องมากกว่า หรือเท่ากับ {0}.",
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
        "RequiredField": "จำเป็นต้องใส่ข้อมูลนี้",
        "UpdateBetSettingSuccessfully": "การตั้งค่าเดิมพัน ได้อัพเดทเรียบร้อยแล้ว."
    };
    
    settingType23Resources["message"] = settingType23Resources["message"] || {};
    settingType23Resources["message"]["vi-vn"] = {
        "MaxBetCannotBeGreaterThanMaxPerBall": "Tiền Cược Lớn Nhất không được lớn hơn Tiền Cược Tối Đa Cho 1 Lượt Banh.",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Tiền cược lớn nhất không thể lớn hơn Tiền cược tối đa của mỗi trận đấu",
        "MaxBetLessThanOrEqualMaxPerBall": "Tiền Cược Lớn Nhất phải nhỏ hơn hoặc bằng Tiền Cược Tối Đa Cho 1 Lượt Banh.",
        "MaxBetLessThanOrEqualMaxPerMatch": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng Tiền cược tối đa của mỗi trận đấu",
        "MaxBetLessThanOrEqualTo": "Tiền cược lớn nhất phải nhỏ hơn hoặc bằng {0}",
        "MaxPerBallCannotBeGreaterThanMaxPerMatch": "Tiền Cược Tối Đa Cho 1 Lượt Banh không được lớn hơn Tiền Cược Tối Đa Cho 1 Trận.",
        "MaxPerBallCannotBeNegative": "Tiền Cược Tối Đa Cho 1 Lượt Banh không được là số âm.",
        "MaxPerBallLessThanOrEqualMaxPerMatch": "Tiền Cược Tối Đa Cho 1 Lượt Banh phải nhỏ hơn hoặc bằng Tiền Cược Tối Đa Cho 1 Trận.",
        "MaxPerBallLessThanOrEqualTo": "Tiền cược tối đa cho mỗi lượt banh phải ít hơn hoặc bằng {0}",
        "MaxPerBallMoreThanOrEqualMaxBet": "Tiền Cược Tối Đa Cho 1 Lượt Banh phải lớn hơn hoặc bằng Tiền Cược Lớn Nhất.",
        "MaxPerMatchLessThanOrEqualTo": "Tiền cược tối đa cho mỗi trận phài ít hơn hoặc bằng {0}",
        "MaxPerMatchMoreThanOrEqualMaxPerBall": "Tiền Cược Tối Đa Cho 1 Trận phải lớn hơn hoặc bằng Tiền Cược Tối Đa Cho 1 Lượt Banh.",
        "MinBetCannotBeGreaterThanMaxBet": "Tiền cược nhỏ nhất không thể lớn hơn Tiền cược lớn nhất",
        "MinBetLessThanOrEqualMaxBet": "Tiền cược nhỏ nhất phải nhỏ hơn hoặc bằng Tiền cược lớn nhất.",
        "MinBetMoreThanOrEqualTo": "Cược Nhỏ Nhất phải lớn hơn hay bằng {0}.",
        "RequiredField": "Thông tin này bắt buộc phải nhập.",
        "UpdateBetSettingSuccessfully": "Thiết lập về tiền cược được cập nhật thành công"
    };

    angular.module("customerSetting").value("settingType23Resources", settingType23Resources);
})();
