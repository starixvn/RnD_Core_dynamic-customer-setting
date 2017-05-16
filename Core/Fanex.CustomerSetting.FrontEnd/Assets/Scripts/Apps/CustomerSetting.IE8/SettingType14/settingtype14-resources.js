/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType14Resources = {};
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["en-us"] = {
        "AgentCommission": "Agent Commission",
        "AgentGivenCommission": "Agent Given Commission",
        "CommForAgent": "Comm For Agent",
        "CommForMaster": "Comm For Master",
        "MasterCommission": "Master Commission",
        "MasterGivenCommission": "Master Given Commission",
        "MasterRemainCommission": "Master Remain Commission",
        "MaxGivenCommission": "Max Given Commission",
        "MemberCommission": "Member Commission",
        "SuperCommission": "Super Commission",
        "SuperGivenCommission": "Super Given Commission",
        "SuperRemainCommission": "Super Remain Commission",
        "TicketCommission": "Ticket Commission"
    };
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["zh-cn"] = {
        "AgentCommission": "代理佣金",
        "AgentGivenCommission": "代理可设置佣金",
        "CommForAgent": "代理佣金",
        "CommForMaster": "总代理佣金",
        "MasterCommission": "总代理佣金",
        "MasterGivenCommission": "总代理可设置佣金",
        "MasterRemainCommission": "总代理保留佣金",
        "MaxGivenCommission": "最高可设置佣金",
        "MemberCommission": "会员的佣金",
        "SuperCommission": "超级总代理佣金",
        "SuperGivenCommission": "超级总代可设置佣金",
        "SuperRemainCommission": "超级总代保留佣金",
        "TicketCommission": "注单佣金"
    };
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["zh-tw"] = {
        "AgentCommission": "代理佣金",
        "AgentGivenCommission": "代理可設置佣金",
        "CommForAgent": "代理佣金",
        "CommForMaster": "總代理佣金",
        "MasterCommission": "總代理佣金",
        "MasterGivenCommission": "總代理可設置佣金",
        "MasterRemainCommission": "總代理保留佣金",
        "MaxGivenCommission": "最高可設置佣金",
        "MemberCommission": "會員的佣金",
        "SuperCommission": "超級總代理佣金",
        "SuperGivenCommission": "超級總代可設置佣金",
        "SuperRemainCommission": "超級總代保留佣金",
        "TicketCommission": "注單佣金"
    };
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["th-th"] = {
        "AgentCommission": "คอมมิชั่นของเอเย่น",
        "AgentGivenCommission": "เอเจ่นต์สามารถตั้งค่าคอมมิชชั่น",
        "CommForAgent": "ค่าคอมมิชชั่นจากตัวแทน",
        "CommForMaster": "ค่าคอมมิชชั่นจากตัวแทนหลัก",
        "MasterCommission": "มาสเตอร์คอมมิชั่น",
        "MasterGivenCommission": "มาสเตอร์สามารถตั้งค่าคอมมิชชั่น",
        "MasterRemainCommission": "มาสเตอร์คงไว้ค่าคอมมิชชั่น",
        "MaxGivenCommission": "ค่าคอมมิชชั่นสูงสุดที่กำหนด",
        "MemberCommission": "ค่าคอมมิชชั่นสำหรับสมาชิก",
        "SuperCommission": "คอมมิชั่นของซุปเปอร์",
        "SuperGivenCommission": "ซุปเปอร์สามารถตั้งค่าคอมมิชชั่น",
        "SuperRemainCommission": "ซุปเปอร์คงไว้ค่าคอมมิชชั่น",
        "TicketCommission": "ค่าคอมมิชชั่นของตั๋ว"
    };
    
    settingType14Resources["label"] = settingType14Resources["label"] || {};
    settingType14Resources["label"]["vi-vn"] = {
        "AgentCommission": "Hoa hồng cho Agent",
        "AgentGivenCommission": "Hoa hồng Agent được cấp",
        "CommForAgent": "Hoa hồng cho Agent",
        "CommForMaster": "Hoa hồng cho Master",
        "MasterCommission": "Hoa hồng cho Master",
        "MasterGivenCommission": "Hoa hồng Master được cấp",
        "MasterRemainCommission": "Hoa hồng Master còn",
        "MaxGivenCommission": "Hoa hồng tối đa được cấp",
        "MemberCommission": "Hoa hồng cho Member",
        "SuperCommission": "Hoa hồng cho Super",
        "SuperGivenCommission": "Hoa hồng Super được cấp",
        "SuperRemainCommission": "Hoa hồng Super còn",
        "TicketCommission": "Hoa hồng trên vé cược"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["en-us"] = {
        "CommissionCannotBeExceed100Percent": "The Commission cannot exceed 100%.",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "The Commission must be less than or equal to the Given Commission.",
        "CommissionMustBePositive": "The Commission must be a positive number.",
        "Msg_1": "Invalid Commission Rate.",
        "UpdatedTicketCommissionSuccessfully": "Updated Ticket Commission successfully."
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["zh-cn"] = {
        "CommissionCannotBeExceed100Percent": "佣金不能超出 100%",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "佣金必须少于或等于给予的佣金",
        "CommissionMustBePositive": "佣金必须是正数",
        "Msg_1": "佣金率无效",
        "UpdatedTicketCommissionSuccessfully": "成功更新注单的佣金"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["zh-tw"] = {
        "CommissionCannotBeExceed100Percent": "佣金不能超出 100%",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "佣金必須少於或等於給予的佣金",
        "CommissionMustBePositive": "佣金必須是正數",
        "Msg_1": "佣金率無效",
        "UpdatedTicketCommissionSuccessfully": "成功更新注單的佣金"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["th-th"] = {
        "CommissionCannotBeExceed100Percent": "ค่าคอมมิชชั่นจะต้องไม่เกิน 100%",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "ค่าคอมมิชชั่นต้องน้อยกว่าหรือเท่ากับที่กำหนด",
        "CommissionMustBePositive": "ค่าคอมมิชชั่นต้องเป็นจำนวนบวก",
        "Msg_1": "ค่าคอมมิชชั่นโมฆะ",
        "UpdatedTicketCommissionSuccessfully": "อัปเดตค่าคอมมิชชั่นของตั๋วเรียบร้อยแล้ว"
    };
    
    settingType14Resources["message"] = settingType14Resources["message"] || {};
    settingType14Resources["message"]["vi-vn"] = {
        "CommissionCannotBeExceed100Percent": "Hoa hồng không được vượt 100%.",
        "CommissionMustBeLessThanOrEqualToGivenCommission": "Hoa hồng cho cấp dưới không lớn hơn mức đã nhận.",
        "CommissionMustBePositive": "Hoa hồng phải là số nguyên dương",
        "Msg_1": "Hoa hồng không phù hợp",
        "UpdatedTicketCommissionSuccessfully": "Cập nhật Hoa hồng trên vé cược thành công."
    };

    angular.module("customerSetting").value("settingType14Resources", settingType14Resources);
})();
