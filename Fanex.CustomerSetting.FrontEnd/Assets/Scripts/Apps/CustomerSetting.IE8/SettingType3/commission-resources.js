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
        "CommissionForMember": "会员佣金"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["zh-tw"] = {
        "Commission": "佣金",
        "CommissionForAgent": "代理佣金",
        "CommissionForMaster": "總代理佣金",
        "CommissionForMember": "會員佣金"
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
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["th-th"] = {
        "Commission": "คอมมิชชั่น",
        "CommissionForAgent": "คอมมิชชั่นของ Agent",
        "CommissionForMaster": "คอมมิชชั่นของ Master",
        "CommissionForMember": "คอมมิชชั่นของสมาชิก"
    };
    
    commissionResources["label"] = commissionResources["label"] || {};
    commissionResources["label"]["vi-vn"] = {
        "Commission": "Hoa hồng",
        "CommissionForAgent": "Agent Comm",
        "CommissionForMaster": "Master Comm",
        "CommissionForMember": "Member Comm"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["en-us"] = {
        "CommRateCannotBeExceedMaxValue": "Commission Rate cannot be exceed Max Value",
        "CommRateCannotBeNegative": "Commission Rate cannot be negative",
        "Msg_1": "Invalid commission rate",
        "UpdateCommissionSuccessfully": "Updated commission setting successfully."
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["zh-cn"] = {
        "UpdateCommissionSuccessfully": "成功更新佣金设置"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["zh-tw"] = {
        "UpdateCommissionSuccessfully": "成功更新傭金設置"
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["ko-kr"] = {
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["ja-jp"] = {
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["th-th"] = {
    };
    
    commissionResources["message"] = commissionResources["message"] || {};
    commissionResources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("commissionResources", commissionResources);
})();
