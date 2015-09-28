/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var positionTakingResources = {};
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["en-us"] = {
        "AgentMaxPT": "Agent Max PT",
        "AgentMinPT": "Agent Min PT",
        "AgentPT": "Agent PT",
        "DisableAgentAutoPt": "You have changed this agent not to take auto position taking",
        "DisableMasterAutoPt": "You have changed this master not to take auto position taking",
        "IsAuto": "Is Auto",
        "Live": "Live",
        "MasterMaxPT": "Master Max PT",
        "MasterMinPT": "Master Min PT",
        "MasterPT": "Master PT",
        "Max": "Max",
        "MaxLive": "Max Live",
        "Min": "Min",
        "MinLive": "Min Live",
        "PositionTaking": "Position Taking",
        "SuperMaxPT": "Super Max PT",
        "SuperMinPT": "Super Min PT",
        "SuperPT": "Super PT",
        "UpdateMemberPtInfo": "You have changed your position taking.",
        "UpdateMemberPtQuestion": "Do you want to update all member&apos;s position taking to:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["zh-cn"] = {
        "AgentMaxPT": "代理最高占成",
        "AgentMinPT": "代理最低占成数",
        "AgentPT": "代理占成数",
        "IsAuto": "是自动",
        "LastName": "姓",
        "Live": "现场",
        "MasterMaxPT": "总代理最高占成",
        "MasterMinPT": "总代理最低占成数",
        "MasterPT": "总代理占成数",
        "Max": "最高",
        "MaxLive": "Max Live",
        "Min": "最低",
        "MinLive": "Min Live",
        "PositionTaking": "占成数",
        "SuperMaxPT": "超级总代理最高占成",
        "SuperMinPT": "超级总代理最低占成",
        "SuperPT": "超级总代理占成数",
        "UpdateMemberPtInfo": "您已更改您的占成数",
        "UpdateMemberPtQuestion": "您是否要把所有会员的占成数更改为:",
        "UpdateMemberPtTitle": "确认更新会员的占成数"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["zh-tw"] = {
        "AgentMaxPT": "代理最高佔成",
        "AgentMinPT": "代理最低占成数",
        "AgentPT": "代理占成数",
        "IsAuto": "是自動",
        "Live": "現場",
        "MasterMaxPT": "總代理最高佔成",
        "MasterMinPT": "总代理最低占成数",
        "MasterPT": "总代理占成数",
        "Max": "最高",
        "Members": "會員",
        "Min": "最低",
        "MinLive": "Min Live",
        "PositionTaking": "佔成數",
        "SuperMaxPT": "超級總代理最高佔成",
        "SuperMinPT": "超級總代理最低佔成",
        "SuperPT": "超级总代理占成数",
        "UpdateMemberPtInfo": "您已更改您的占成数",
        "UpdateMemberPtQuestion": "您是否要把所有會員佔成數更改為:",
        "UpdateMemberPtTitle": "確認更新會員的佔成數"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["ko-kr"] = {
        "AgentMaxPT": "에이전트 최대 PT",
        "AgentMinPT": "에이전트 최소 PT",
        "AgentPT": "에이전트 PT",
        "IsAuto": "자동",
        "Live": "라이브",
        "MasterMaxPT": "마스터 최대 PT",
        "MasterMinPT": "마스터 최소PT",
        "MasterPT": "마스터 PT",
        "Max": "최대",
        "Min": "최소",
        "PositionTaking": "거래 조건",
        "SuperMaxPT": "슈퍼 최대 PT",
        "SuperMinPT": "슈퍼 최소PT",
        "SuperPT": "슈퍼 PT",
        "UpdateMemberPtInfo": "포지션테이킹(PT)이 변경되었습니다",
        "UpdateMemberPtQuestion": "모든회원의 거래조건을 바꾸기를 원하십니까:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["ja-jp"] = {
        "Live": "ライブ"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["th-th"] = {
        "AgentMaxPT": "แบ่งสู้สูงสุดของเอ",
        "AgentMinPT": "ค่าแบ่งสู้ต่ำสุดของ",
        "AgentPT": "ค่าแบ่งสู้ของเอเย่น",
        "IsAuto": "คืออัตโนมัติ",
        "Live": "สด",
        "MasterMaxPT": "แบ่งสู้สูงสุดของ",
        "MasterMinPT": "แบ่งสู้ขั้นต่ำของมาสเตอร์",
        "MasterPT": "ค่าแบ่งสู้ของมาสเตอร์",
        "Max": "สูงสุด",
        "Min": "ต่ำสุด",
        "PositionTaking": "ค่าความเสี่ยง",
        "SuperMaxPT": "แบ่งสู้สูงสุดของซุปเปรอ์",
        "SuperMinPT": "แบ่งสู้ต่ำสุดของซุปเปรอ์",
        "SuperPT": "ค่าแบ่งสู้ของซุปเปอร์",
        "UpdateMemberPtInfo": "คุณมีการเปลี่ยนค่าแบ่งสู้ของคุณ",
        "UpdateMemberPtQuestion": "คุณต้องการจะอัพเดทแบ่งสู้บนสมาชิกทั้งหมกหรือไม่:",
        "UpdateMemberPtTitle": "ยืนยันการอัพเดทการตั้งค่าความเสี่ยง"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["vi-vn"] = {
        "AgentMaxPT": "PT lớn nhất của Agent",
        "AgentMinPT": "PT nhỏ nhất của Agent",
        "AgentPT": "PT của Agent",
        "IsAuto": "Auto PT",
        "Live": "Trực Tuyến",
        "MasterMaxPT": "PT lớn nhất của Master",
        "MasterMinPT": "PT nhỏ nhất của Master",
        "MasterPT": "PT của Master",
        "Max": "Max",
        "MaxLive": "Max Live",
        "Min": "Nhỏ nhất",
        "MinLive": "Min Live",
        "PositionTaking": "PT",
        "SuperMaxPT": "PT lớn nhất của Super",
        "SuperMinPT": "PT nhỏ nhất của Super",
        "SuperPT": "PT của Super",
        "UpdateMemberPtInfo": "Bạn đã  thay đổi giá trị PT của mình",
        "UpdateMemberPtQuestion": "Bạn có muốn cập nhật giá trị PTcủa tất cả các thành viên:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["en-us"] = {
        "AgentMaxPTCannotBeGreaterThan": "Agent Max PT cannot be greater than {0}",
        "AgentMaxPTCannotBeGreaterThanMasterMaxPT": "Agent Max PT cannot be greater than Master Max PT",
        "AgentMaxPTCannotBeLessThan": "Agent Max PT cannot be less than {0}",
        "AgentMinPTCannotBeGreaterThanAgentMaxPT": "Agent Min PT cannot be greater than Agent Max PT",
        "AgentPTCannotBeGreaterThanAgentMaxPT": "Agent PT cannot be greater than Agent Max PT",
        "AgentPTCannotBeLessThanAgentMinPT": "Agent PT cannot be less than Agent Min PT",
        "MasterAutoPTCannotBeGreaterThanMasterMaxPT": "Master Auto PT cannot be greater than Master Max PT",
        "MasterAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet": "Master Auto PT cannot be greater than zero when IsAuto is not set",
        "MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT": "Master Auto PT On Member cannot be greater than Master Max PT",
        "MasterAutoPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsNotSet": "Master Auto PT On Member cannot be greater than zero when IsAuto is not set",
        "MasterAutoPTOnMemberCannotBeLessThan": "Master Auto PT On Member cannot be less than {0}",
        "MasterMaxPTCannotBeGreaterThan": "Master Max PT cannot be greater than {0}",
        "MasterMaxPTCannotBeGreaterThanSuperMaxPT": "Master Max PT cannot be greater than Super Max PT",
        "MasterMaxPTCannotBeLessThan": "Master Max PT cannot be less than {0}",
        "MasterMinPTCannotBeGreaterThanMasterMaxPT": "Master Min PT cannot be greater than Master Max PT",
        "MasterPTCannotBeGreaterThanMasterMaxPT": "Master PT cannot be greater than Master Max PT",
        "MasterPTCannotBeGreaterThanZeroWhenIsAutoIsSet": "Master PT cannot be greater than zero when IsAuto is set",
        "MasterPTCannotBeLessThan": "Master PT cannot be less than {0}",
        "MasterPTOnMemberCannotBeGreaterThan": "Master PT On Member cannot be greater than {0}",
        "MasterPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsSet": "Master PT On Member cannot be greater than zero when IsAuto is set",
        "MasterPTOnMemberCannotBeLessThan": "Master PT On Member cannot be less than {0}",
        "Msg_20001": "Wrong position taking",
        "Msg_20019": "Existing masters exceeded new Super Max PT",
        "Msg_20020": "Existing members exceeded new Super Max PT",
        "Msg_20021": "Existing masters exceeded new Master Max PT",
        "Msg_20022": "Existing members exceeded new Master Max PT",
        "Msg_20023": "Existing agents exceeded new Agent Max PT",
        "Msg_20024": "Existing members exceeded new Agent Max PT",
        "Msg_20025": "The position taking violates Upline Min/Max PT",
        "Msg_20026": "Existing agents exceeded new Master Max PT",
        "Msg_20027": "The position taking violates Master Max PT set by company",
        "Msg_20028": "The position taking violates Super Max PT",
        "Msg_20029": "The position taking violates Super Min PT",
        "Msg_20030": "The Agent Min PT violates Master Max PT",
        "Msg_20031": "The position taking violates Master Max PT",
        "Msg_20032": "The position taking violates Master Min PT",
        "Msg_20033": "The position taking violates Agent Min/Max PT",
        "Msg_20034": "The position taking violates Agent Max PT",
        "PositionTakingCannotBeExceedMaxValue": "Position Taking cannot be exceed Max Value",
        "SuperAutoPTCannotBeGreaterThanSuperMaxPT": "Super Auto PT cannot be greater than Super Max PT",
        "SuperAutoPTCannotBeGreaterThanZeroWhenIsAutoIsNotSet": "Super Auto PT cannot be greater than zero when IsAuto is not set",
        "SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT": "Super Auto PT On Member cannot be greater than Super Max PT",
        "SuperAutoPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsNotSet": "Super Auto PT On Member cannot be greater than zero when IsAuto is not set",
        "SuperAutoPTOnMemberCannotBeLessThan": "Super Auto PT On Member cannot be less than {0}",
        "SuperMaxPTCannotBeGreaterThan": "Super Max PT cannot be greater than {0}",
        "SuperMinPTCannotBeGreaterThanSuperMaxPT": "Super Min PT cannot be greater than Super Max PT",
        "SuperPTCannotBeGreaterThanSuperMaxPT": "Super PT cannot be greater than Super Max PT",
        "SuperPTCannotBeGreaterThanZeroWhenIsAutoIsSet": "Super PT cannot be greater than zero when IsAuto is set",
        "SuperPTCannotBeLessThan": "Super PT cannot be less than {0}",
        "SuperPTOnMemberCannotBeGreaterThan": "Super PT On Member cannot be greater than {0}",
        "SuperPTOnMemberCannotBeGreaterThanZeroWhenIsAutoIsSet": "Super PT On Member cannot be greater than zero when IsAuto is set",
        "SuperPTOnMemberCannotBeLessThan": "Super PT On Member cannot be less than {0}",
        "UpdatePositionTakingSuccessfully": "Updated position taking successfully."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["zh-cn"] = {
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["zh-tw"] = {
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["ko-kr"] = {
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["ja-jp"] = {
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["th-th"] = {
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("positionTakingResources", positionTakingResources);
})();
