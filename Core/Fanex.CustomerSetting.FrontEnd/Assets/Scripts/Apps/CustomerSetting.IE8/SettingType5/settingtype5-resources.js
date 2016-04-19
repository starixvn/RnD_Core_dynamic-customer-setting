/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType5Resources = {};
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["en-us"] = {
        "AgentsShare": "Agent&apos;s Share",
        "ChargeCommission": "Charge Commission",
        "ChargeOnMemberNetWin": "Charge on Member Net Win",
        "CommForAgent": "Comm for Agent",
        "CommForMaster": "Comm for Master",
        "Given": "Given",
        "MastersShare": "Master&apos;s Share",
        "Remain": "Remain",
        "SupersShare": "Super&apos;s Share",
        "TotalCharge": "Total Charge"
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["zh-cn"] = {
        "AgentsShare": "代理占成",
        "ChargeCommission": "抽取佣金",
        "ChargeOnMemberNetWin": "会员净赢抽取的金额",
        "CommForAgent": "代理佣金",
        "CommForMaster": "总代理佣金",
        "Given": "给予",
        "MastersShare": "总代理占成",
        "Remain": "剩余",
        "SupersShare": "超级总代理占成",
        "TotalCharge": "总抽取金额"
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["zh-tw"] = {
        "AgentsShare": "代理占成",
        "ChargeCommission": "抽取傭金",
        "ChargeOnMemberNetWin": "會員凈贏抽取的金額",
        "CommForAgent": "代理傭金",
        "CommForMaster": "總代理傭金",
        "Given": "給予",
        "MastersShare": "總代理占成",
        "Remain": "剩余",
        "SupersShare": "超級總代理占成",
        "TotalCharge": "總抽取金額"
    };
    
    settingType5Resources["label"] = settingType5Resources["label"] || {};
    settingType5Resources["label"]["th-th"] = {
        "AgentsShare": "ส่วนแบ่งของเอเจนต์",
        "ChargeCommission": "ค่าธรรมเนียมคอมมิชชั่น",
        "ChargeOnMemberNetWin": "คิดเงินจากเงินได้สุทธิ",
        "CommForAgent": "ค่าคอมมิชชั่นสำหรับเอเจนต์",
        "CommForMaster": "ค่าคอมมิชชั่นสำหรับมาสเตอร์",
        "Given": "ได้รับ",
        "MastersShare": "ส่วนแบ่งของมาสเตอร์",
        "Remain": "คงเหลือ",
        "SupersShare": "ส่วนแบ่งของซูเปอร์",
        "TotalCharge": "ค่าธรรมเนียมรวม"
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["en-us"] = {
        "Msg_1": "Invalid Charge Commission Rate.",
        "ShareCannotBeExceed100Percent": "The Share cannot be exceed 100%.",
        "ShareMustBeLessThanOrEqualToGivenCommissionError": "The Share must be less than or equal to the Given Commission.",
        "ShareMustBePositive": "The Share must be a positive number.",
        "UpdatedChargeCommissionSuccessfully": "Charge Commission is updated successfully."
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["zh-cn"] = {
        "Msg_1": "无效的抽取佣金率。",
        "ShareCannotBeExceed100Percent": "占成不能超过 100%。",
        "ShareMustBeLessThanOrEqualToGivenCommissionError": "占成必须小于或相等予佣金。",
        "ShareMustBePositive": "占成必须是正数。",
        "UpdatedChargeCommissionSuccessfully": "抽取佣金已成功更新。"
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["zh-tw"] = {
        "Msg_1": "無效的抽取傭金率。",
        "ShareCannotBeExceed100Percent": "占成不能超過 100%。",
        "ShareMustBeLessThanOrEqualToGivenCommissionError": "占成必须小于或相等予佣金。",
        "ShareMustBePositive": "占成必須是正數。",
        "UpdatedChargeCommissionSuccessfully": "抽取傭金已成功更新。"
    };
    
    settingType5Resources["message"] = settingType5Resources["message"] || {};
    settingType5Resources["message"]["th-th"] = {
        "Msg_1": "อัตราค่าธรรมเนียมคอมมิชชั่นไม่ถูกต้อง",
        "ShareCannotBeExceed100Percent": "ส่วนแบ่งต้องไม่เกิน 100%",
        "ShareMustBeLessThanOrEqualToGivenCommissionError": "ส่วนแบ่งต้องน้อยกว่าหรือเท่ากับค่าคอมมิชชั่นที่ได้รับ",
        "ShareMustBePositive": "ส่วนแบ่งต้องเป็นจำนวนบวก",
        "UpdatedChargeCommissionSuccessfully": "อัพเดทค่าธรรมเนียมคอมมิชชั่นเรียบร้อยแล้ว"
    };

    angular.module("customerSetting").value("settingType5Resources", settingType5Resources);
})();
