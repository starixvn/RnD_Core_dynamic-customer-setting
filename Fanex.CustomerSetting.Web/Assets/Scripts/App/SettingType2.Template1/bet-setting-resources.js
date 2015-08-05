"use strict";
/*jshint -W101,-W106,-W044,-W098*/

(function () {
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
        "BetLimitCannotBeGreaterThanMaxValue": "BetLimit cannot be greater than Max Value",
        "MaxBetCannotBeGreaterThanMaxPerMatch": "Max Bet cannot be greater than Max Per Match",
        "MaxBetCannotBeNegative": "Max Bet cannot be negative",
        "MaxBetLessThanOrEqualMaxPerMatch": "Max Bet must be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet",
        "MaxPerMatchCannotBeNegative": "Max Per Match cannot be negative",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "Max Per Match must be greater than or equal to Max Bet",
        "MinBetCannotBeGreaterThanMaxBet": "Min Bet cannot be greater than Max Bet",
        "MinBetCannotBeNegative": "Min Bet cannot be negative",
        "MinBetLessThanOrEqualMaxBet": "Min Bet must be less than or equal to Max Bet",
        "MinBetMoreThanOrEqualTo": "Min Bet must be greater than or equal to {0}",
        "Msg_10003": "Wrong Min bet/ Max bet/ Max per match",
        "Msg_10005": "Min bet must be less than max bet and max bet must be less than max bet per match",
        "Msg_10006": "Min bet must be equal to or greater than Agent min bet",
        "Msg_10007": "Max bet must be equal to or less than Agent max bet",
        "Msg_10008": "Max Per Match must be equal to or less than Agent Max Per Match",
        "Msg_10016": "Min bet must be equal to or greater than Super Agent min bet",
        "Msg_10017": "Max bet must be equal to or less than Super Agent max bet",
        "Msg_10018": "Max Per Match must be equal to or less than Super Agent Max Per Match",
        "Msg_10019": "Min bet must be equal to or greater than Master Agent min bet",
        "Msg_10020": "Max bet must be equal to or less than Master Agent max bet",
        "Msg_10021": "Max Per Match must be equal to or less than Master Agent Max Per Match",
        "UpdateBetSettingSuccessfully": "Updated bet setting successfully."
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["zh-cn"] = {
        "MaxBetLessThanOrEqualMaxPerMatch": "最大投注额必须小于或等于最大单场比赛投注额",
        "MaxBetLessThanOrEqualTo": "最大投注额必须小于或等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注额必须大于或等于最小投注额",
        "MaxPerMatchLessThanOrEqualTo": "最大单场比赛投注额必须小于或等于 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大单场比赛投注额必须大于或等于最大投注额",
        "MinBetLessThanOrEqualMaxBet": "最小投注额必须小于或等于最大投注额",
        "MinBetMoreThanOrEqualTo": "最大投注额必须大于或等于 {0}",
        "UpdateBetSettingSuccessfully": "成功更新投注设置。"
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["zh-tw"] = {
        "MaxBetLessThanOrEqualMaxPerMatch": "最大投注額必須小於或等於最大單場比賽投注額",
        "MaxBetLessThanOrEqualTo": "最大投注額必須小於或等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注額必須大於或等於最小投注額",
        "MaxPerMatchLessThanOrEqualTo": "最大單場比賽投注額必須小於或等於 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大單場比賽投注額必須大於或等於最大投注額",
        "MinBetLessThanOrEqualMaxBet": "最小投注額必須小於或等於最大投注額",
        "MinBetMoreThanOrEqualTo": "最大投注額必須大於或等於 {0}",
        "UpdateBetSettingSuccessfully": "成功更新投注設置。"
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["ko-kr"] = {
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["ja-jp"] = {
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["th-th"] = {
    };
    
    betSettingResources["message"] = betSettingResources["message"] || {};
    betSettingResources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("betSettingResources", betSettingResources);
})();
