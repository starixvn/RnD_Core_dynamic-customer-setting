/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var commissionResources = {};
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["en-us"] = {
        "Commission": "Commission",
        "CommissionForAgent": "Comm for Agent",
        "CommissionForMaster": "Comm for Master",
        "CommissionForMember": "Comm for Member"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["zh-cn"] = {
        "Commission": "佣金",
        "CommissionForAgent": "代理佣金",
        "CommissionForMaster": "总代理佣金",
        "CommissionForMember": "会员的佣金"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["zh-tw"] = {
        "Commission": "佣金",
        "CommissionForAgent": "代理佣金",
        "CommissionForMaster": "總代理佣金",
        "CommissionForMember": "會員的佣金"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["ko-kr"] = {
        "Commission": "수수료",
        "CommissionForAgent": "에이전트 수수료",
        "CommissionForMaster": "마스터 수수료",
        "CommissionForMember": "회원 수수료"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["ja-jp"] = {
        "Commission": "Commission",
        "CommissionForMember": "メンバーの手数料"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["th-th"] = {
        "Commission": "คอมมิชชั่น",
        "CommissionForAgent": "ค่าคอมมิชชั่น จาก ตัวแทน",
        "CommissionForMaster": "ค่าคอมมิชชั่น จากตัวแทนหลัก",
        "CommissionForMember": "ค่าคอมมิชชั่นสำหรับสมาชิก"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["vi-vn"] = {
        "Commission": "Hoa hồng",
        "CommissionForAgent": "Hoa hồng cho Agent",
        "CommissionForMaster": "Hoa hồng Master",
        "CommissionForMember": "Hoa hồng cho Member"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["en-us"] = {
        "CommRateCannotBeExceedMaxValue": "Commission Rate cannot be exceed Max Value.",
        "CommRateCannotBeNegative": "Commission Rate cannot be negative.",
        "Msg_1": "Invalid Commission Rate.",
        "UpdateCommissionSuccessfully": "Updated commission setting successfully."
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["zh-cn"] = {
        "Msg_1": "佣金率无效",
        "UpdateCommissionSuccessfully": "佣金已更新成功"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["zh-tw"] = {
        "Msg_1": "傭金率無效",
        "UpdateCommissionSuccessfully": "佣金已更新成功"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["ko-kr"] = {
        "UpdateCommissionSuccessfully": "수수료가 성공적으로 업데이트되었습니다."
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["ja-jp"] = {
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["th-th"] = {
        "Msg_1": "อัตราค่าคอมมิชชั่นไม่ถูกต้อง",
        "UpdateCommissionSuccessfully": "ค่าคอมมิชชั่น ได้อัพเดทเรียบร้อยแล้ว"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["vi-vn"] = {
        "UpdateCommissionSuccessfully": "Tiền hoa hồng được cập nhật thành công"
    };

    angular.module("customerSetting").value("commissionResources", commissionResources);
})();
