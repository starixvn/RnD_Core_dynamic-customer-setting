/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var positionTakingResources = {};
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["en-us"] = {
        "AgentMaxPT": "Agent Max PT",
        "AgentMinPT": "Agent Min PT",
        "AgentPT": "Agent PT",
        "DisableAgentAutoPt": "You have changed this agent not to take auto position taking.",
        "DisableMasterAutoPt": "You have changed this master not to take auto position taking.",
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
        "AgentMinPT": "代理最低占成",
        "AgentPT": "代理占成数",
        "DisableAgentAutoPt": "您已更改于此代理不使用自动占成",
        "DisableMasterAutoPt": "您已更改此总代理不再自动占成.",
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
        "AgentMinPT": "代理最低佔成",
        "AgentPT": "代理占成数",
        "DisableAgentAutoPt": "You have changed this agent not to take auto position taking",
        "DisableMasterAutoPt": "You have changed this master not to take auto position taking.",
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
        "DisableAgentAutoPt": "해당 에이전트는 자동 거래조건을 설정 하지 못하게 변경 하였습니다",
        "DisableMasterAutoPt": "해당 마스터의 자동 거래조건을 설정 하지 못하게 변경 하였습니다.",
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
        "AgentMaxPT": "Agent Max PT",
        "AgentMinPT": "Agent Min PT",
        "AgentPT": "Agent PT",
        "DisableAgentAutoPt": "You have changed this agent not to take auto position taking",
        "DisableMasterAutoPt": "You have changed this master not to take auto position taking.",
        "IsAuto": "Is Auto",
        "Live": "ライブ",
        "MasterMaxPT": "Master Max PT",
        "MasterMinPT": "Master Min PT",
        "MasterPT": "Master PT",
        "Max": "Max",
        "Min": "Min",
        "PositionTaking": "Position Taking",
        "SuperMaxPT": "Super Max PT",
        "SuperMinPT": "Super Min PT",
        "SuperPT": "Super PT",
        "UpdateMemberPtInfo": "You have changed your position taking.",
        "UpdateMemberPtQuestion": "Do you want to update all member&apos;s position taking to:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };
    
    positionTakingResources["label"] = positionTakingResources["label"] || {};
    positionTakingResources["label"]["th-th"] = {
        "AgentMaxPT": "แบ่งสู้สูงสุดของเอเย่น",
        "AgentMinPT": "แบ่งสู้ต่ำสุดของเอเย่น",
        "AgentPT": "ค่าแบ่งสู้ของเอเย่น",
        "DisableAgentAutoPt": "คุณได้เปลี่ยนเอเย่นท่านนี้ไม่ถือแบ่งสู้อัตโนมัติ",
        "DisableMasterAutoPt": "คุณได้ทำการเปลี่ยนตัวแทนหลักและ ไม่ได้เลือกตำแหน่งอัตโนมัติ.",
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
        "DisableAgentAutoPt": "Bạn vừa mới ngăn Agent này lấy Auto PT",
        "DisableMasterAutoPt": "Bạn vừa mới ngăn Master này lấy Auto PT.",
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
        "AgentMaxPTCannotBeGreaterThan": "Agent Max PT must be less than or equal to {0}.",
        "AgentMaxPTCannotBeGreaterThanMasterMaxPT": "Agent Max PT must be less than or equal to Master Max PT.",
        "AgentMaxPTCannotBeLessThan": "Agent Max PT must be greater than or equal to {0}.",
        "AgentMinPTCannotBeGreaterThanAgentMaxPT": "Agent Min PT must be less than or equal to Agent Max PT.",
        "AgentPTCannotBeGreaterThanAgentMaxPT": "Agent PT must be less than or equal to Agent Max PT.",
        "AgentPTCannotBeLessThanAgentMinPT": "Agent PT must be greater than or equal to Agent Min PT.",
        "MasterAutoPTCannotBeGreaterThanMasterMaxPT": "Master Auto PT must be less than or equal to Master Max PT.",
        "MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT": "Master Auto PT on Member must be less than or equal to Master Max PT.",
        "MasterAutoPTOnMemberCannotBeLessThan": "Master Auto PT on Member must be greater than or equal to {0}.",
        "MasterMaxPTCannotBeGreaterThan": "Master Max PT must be less than or equal to {0}.",
        "MasterMaxPTCannotBeGreaterThanSuperMaxPT": "Master Max PT must be less than or equal to Super Max PT.",
        "MasterMaxPTCannotBeLessThan": "Master Max PT must be greater than or equal to {0}.",
        "MasterMinPTCannotBeGreaterThanMasterMaxPT": "Master Min PT must be less than or equal to Master Max PT.",
        "MasterPTCannotBeGreaterThanMasterMaxPT": "Master PT must be less than or equal to Master Max PT.",
        "MasterPTCannotBeLessThan": "Master PT must be greater than or equal to {0}.",
        "MasterPTOnMemberCannotBeGreaterThan": "Master PT on Member must be less than or equal to {0}.",
        "MasterPTOnMemberCannotBeLessThan": "Master PT on Member must be greater than or equal to {0}.",
        "Msg_20001": "Invalid position taking.",
        "Msg_20019": "Existing masters exceed new Super Max PT.",
        "Msg_20020": "Existing members exceed new Super Max PT.",
        "Msg_20021": "Existing masters exceed new Master Max PT.",
        "Msg_20022": "Existing members exceed new Master Max PT.",
        "Msg_20023": "Existing agents exceed new Agent Max PT.",
        "Msg_20024": "Existing members exceed new Agent Max PT.",
        "Msg_20025": "The position taking violates Upline Min/Max PT.",
        "Msg_20026": "Existing agents exceed new Master Max PT.",
        "Msg_20027": "The position taking violates Master Max PT set by company.",
        "Msg_20028": "The position taking violates Super Max PT.",
        "Msg_20029": "The position taking violates Super Min PT.",
        "Msg_20030": "The Agent Min PT violates Master Max PT.",
        "Msg_20031": "The position taking violates Master Max PT.",
        "Msg_20032": "The position taking violates Master Min PT.",
        "Msg_20033": "The position taking violates Agent Min/Max PT.",
        "Msg_20034": "The position taking violates Agent Max PT.",
        "SuperAutoPTCannotBeGreaterThanSuperMaxPT": "Super Auto PT must be less than or equal to Super Max PT.",
        "SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT": "Super Auto PT on Member must be less than or equal to Super Max PT.",
        "SuperAutoPTOnMemberCannotBeLessThan": "Super Auto PT on Member must be greater than or equal to {0}.",
        "SuperMaxPTCannotBeGreaterThan": "Super Max PT must be less than or equal to {0}.",
        "SuperMinPTCannotBeGreaterThanSuperMaxPT": "Super Min PT must be less than or equal to Super Max PT.",
        "SuperPTCannotBeGreaterThanSuperMaxPT": "Super PT must be less than or equal to Super Max PT.",
        "SuperPTCannotBeLessThan": "Super PT must be greater than or equal to {0}.",
        "SuperPTOnMemberCannotBeGreaterThan": "Super PT on Member must be less than or equal to {0}.",
        "SuperPTOnMemberCannotBeLessThan": "Super PT on Member must be greater than or equal to {0}.",
        "UpdatePositionTakingSuccessfully": "Position taking is updated successfully."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["zh-cn"] = {
        "AgentMaxPTCannotBeGreaterThan": "代理最高占成必须少于或相等于{0}",
        "AgentMaxPTCannotBeGreaterThanMasterMaxPT": "代理最高占成必须少于或相等于总代理最高占成",
        "AgentMaxPTCannotBeLessThan": "代理最高占成必须大于或相等于{0}",
        "AgentMinPTCannotBeGreaterThanAgentMaxPT": "代理最低占成必须少于或相等于代理最高占成",
        "AgentPTCannotBeGreaterThanAgentMaxPT": "代理占成必须少于或相等于代理最高占成",
        "AgentPTCannotBeLessThanAgentMinPT": "代理占成必须大于或相等于代理最低占成",
        "MasterAutoPTCannotBeGreaterThanMasterMaxPT": "总代理自动占成必须少于或相等于总代理最高占成",
        "MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT": "总代理自动占成在会员必须少于或相等于总代理最高占成",
        "MasterAutoPTOnMemberCannotBeLessThan": "总代理自动占成在会员必须大于或相等于{0}",
        "MasterMaxPTCannotBeGreaterThan": "总代理最高占成必须少于或相等于{0}",
        "MasterMaxPTCannotBeGreaterThanSuperMaxPT": "总代理最高占成必须少于或相等于超级总代理最高占成",
        "MasterMaxPTCannotBeLessThan": "总代理最高占成必须大于或相等于{0}",
        "MasterMinPTCannotBeGreaterThanMasterMaxPT": "总代理最低占成必须少于或相等于总代理最高占成",
        "MasterPTCannotBeGreaterThanMasterMaxPT": "总代理占成必须低于或相等于总代理最高占成。",
        "MasterPTCannotBeLessThan": "总代理占成必须大于或相等于 {0}。",
        "MasterPTOnMemberCannotBeGreaterThan": "总代理于会员的占成必须小于或相等于 {0}。",
        "MasterPTOnMemberCannotBeLessThan": "总代理于会员的占成必须大于或相等于 {0}。",
        "Msg_20001": "占成无效",
        "Msg_20019": "現有总代理超過新增超级总代理的最高占成。",
        "Msg_20020": "現有会员超過新增超级总代理的最高占成。",
        "Msg_20021": "現有总代理超過新增总代理的最高占成。",
        "Msg_20022": "現有会员超過新增总代理的最高占成。",
        "Msg_20023": "現有代理超過新增代理的最高占成。",
        "Msg_20024": "現有会员超過新增代理的最高占成。",
        "Msg_20025": "此占成违反上线的最低/最高占成.",
        "Msg_20026": "現有代理超過新增总代理的最高占成。",
        "Msg_20027": "此占成违反公司设置于总代理的最高占成。",
        "Msg_20028": "此占成违反超级总代理的最高占成。",
        "Msg_20029": "此占成违反超级总代理的最低占成。",
        "Msg_20030": "此代理最低占成违反总代理的最高占成。",
        "Msg_20031": "此占成违反总代理的最高占成。",
        "Msg_20032": "此占成违反总代理的最低占成。",
        "Msg_20033": "此占成违反代理的最低/最高占成。",
        "Msg_20034": "此占成违反代理的最高占成。",
        "SuperAutoPTCannotBeGreaterThanSuperMaxPT": "超级总代理的自动占成必须少于或相等于超级总代理的最高占成。",
        "SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT": "超级总代理于会员的自动占成必须少于或相等于超级总代理的最高占成。",
        "SuperAutoPTOnMemberCannotBeLessThan": "超级总代理于会员的自动占成必须大于或相等于 {0}。",
        "SuperMaxPTCannotBeGreaterThan": "超级总代理的最高占成必须少于或相等于 {0}。",
        "SuperMinPTCannotBeGreaterThanSuperMaxPT": "超级总代理的最低占成必须少于或相等于超级总代理的最高占成。",
        "SuperPTCannotBeGreaterThanSuperMaxPT": "超级总代理的占成必须少于或相等于超级总代理的最高占成。",
        "SuperPTOnMemberCannotBeGreaterThan": "超级总代理于会员的占成必须少于或相等于 {0}。",
        "SuperPTOnMemberCannotBeLessThan": "超级总代理于会员的占成必须大于或相等于 {0}。",
        "UpdatePositionTakingSuccessfully": "占成数设定已更新成功."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["zh-tw"] = {
        "AgentMaxPTCannotBeGreaterThan": "代理最高占成必須少於或相等於{0}",
        "AgentMaxPTCannotBeGreaterThanMasterMaxPT": "代理最高占成必須少於或相等於總代理最高占成",
        "AgentMaxPTCannotBeLessThan": "代理最高占成必須大於或相等於{0}",
        "AgentMinPTCannotBeGreaterThanAgentMaxPT": "代理最低占成必須少於或相等於代理最高占成",
        "AgentPTCannotBeGreaterThanAgentMaxPT": "代理占成必須少於或相等於代理最高占成",
        "AgentPTCannotBeLessThanAgentMinPT": "代理占成必須大於或相等於代理最低占成",
        "MasterAutoPTCannotBeGreaterThanMasterMaxPT": "總代理自動占成必須少於或相等於總代理最高占成",
        "MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT": "總代理自動占成在會員必須少於或相等於總代理最高占成",
        "MasterAutoPTOnMemberCannotBeLessThan": "總代理自動占成在會員必須大於或相等於{0}",
        "MasterMaxPTCannotBeGreaterThan": "總代理最高占成必須少於或相等於{0}",
        "MasterMaxPTCannotBeGreaterThanSuperMaxPT": "總代理最高占成必須少於或相等於超級總代理最高占成",
        "MasterMaxPTCannotBeLessThan": "總代理最高占成必須大於或相等於{0}",
        "MasterMinPTCannotBeGreaterThanMasterMaxPT": "總代理最低占成必須少於或相等於總代理最高占成",
        "MasterPTCannotBeGreaterThanMasterMaxPT": "總代理占成必須低於或相等於總代理最高占成。",
        "MasterPTCannotBeLessThan": "總代理占成必須大於或相等於 {0}。",
        "MasterPTOnMemberCannotBeGreaterThan": "總代理於會員的占成必須小於或相等於 {0}。",
        "MasterPTOnMemberCannotBeLessThan": "總代理於會員的占成必須大於或相等於 {0}。",
        "Msg_20001": "占成無效",
        "Msg_20019": "現有總代理超過新增超級總代理的最高占成",
        "Msg_20020": "現有會員超過新增超級總代理的最高占成。",
        "Msg_20021": "現有總代理超過新增總代理的最高占成。",
        "Msg_20022": "現有會員超過新增總代理的最高占成。",
        "Msg_20023": "現有代理超過新增代理的最高占成。",
        "Msg_20024": "現有會員超過新增代理的最高占成。",
        "Msg_20025": "此占成違反上線的最低/最高占成.",
        "Msg_20026": "現有代理超過新增總代理的最高占成。",
        "Msg_20027": "此占成違反公司設置於總代理的最高占成。",
        "Msg_20028": "此占成違反超級總代理的最高占成。",
        "Msg_20029": "此占成違反超級總代理的最低占成。",
        "Msg_20030": "此代理最低占成違反總代理的最高占成。",
        "Msg_20031": "此占成違反總代理的最高占成。",
        "Msg_20032": "此占成違反總代理的最低占成。",
        "Msg_20033": "此占成違反代理的最低/最高占成。",
        "Msg_20034": "此占成違反代理的最高占成。",
        "SuperAutoPTCannotBeGreaterThanSuperMaxPT": "超級總代理的自動占成必須少於或相等於超級總代理的最高占成。",
        "SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT": "超級總代理於會員的自動占成必須少於或相等於超級總代理的最高占成。",
        "SuperAutoPTOnMemberCannotBeLessThan": "超級總代理於會員的自動占成必須大於或相等於 {0}。",
        "SuperMaxPTCannotBeGreaterThan": "超級總代理的最高占成必須少於或相等於 {0}。",
        "SuperMinPTCannotBeGreaterThanSuperMaxPT": "超級總代理的最低占成必須少於或相等於超級總代理的最高占成。",
        "SuperPTCannotBeGreaterThanSuperMaxPT": "超級總代理的占成必須少於或相等於超級總代理的最高占成。",
        "SuperPTOnMemberCannotBeGreaterThan": "超級總代理於會員的占成必須少於或相等於{0}。",
        "SuperPTOnMemberCannotBeLessThan": "超級總代理於會員的占成必須大於或相等於 {0}。",
        "UpdatePositionTakingSuccessfully": "佔成數設定已更新成功."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["ko-kr"] = {
        "UpdatePositionTakingSuccessfully": "포지션 설정이 성공적으로 업데이트되었습니다."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["ja-jp"] = {
        "UpdatePositionTakingSuccessfully": "Position taking is updated successfully."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["th-th"] = {
        "AgentMaxPTCannotBeGreaterThan": "ค่าความเสี่ยงสูงสุดของเอเจนท์ ต้องน้อยกว่าหรือเท่ากับ {0}.",
        "AgentMaxPTCannotBeGreaterThanMasterMaxPT": "ค่าความเสี่ยงสูงสุดของเอเจนท์ ต้องน้อยกว่าหรือเท่ากับ ค่าความเสี่ยงสูงสุดของมาสเตอร์",
        "AgentMaxPTCannotBeLessThan": "ค่าความเสี่ยงสูงสุดของเอเจนท์ ต้องมากกว่าหรือเท่ากับ {0}.",
        "AgentMinPTCannotBeGreaterThanAgentMaxPT": "ค่าความเสี่ยงขั้นต่ำของเอเจนท์ ต้องน้อยกว่าหรือเท่ากับ ค่าความเสี่ยงสูงสุดของเอเจนท์",
        "AgentPTCannotBeGreaterThanAgentMaxPT": "ค่าความเสี่ยงของเอเจนท์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของเอเจนท์",
        "AgentPTCannotBeLessThanAgentMinPT": "ค่าความเสี่ยงของเอเจนท์ต้องมากกว่าหรือเท่ากับค่าความเสี่ยงต่ำสุดของเอเจนท์",
        "MasterAutoPTCannotBeGreaterThanMasterMaxPT": "ค่าความเสี่ยงแบบอัตโนมัติของมาสเตอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของมาสเตอร์",
        "MasterAutoPTOnMemberCannotBeGreaterThanMasterMaxPT": "ค่าความเสี่ยงแบบอัตโนมัติของมาสเตอร์ต่อเมมเบอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของมาสเตอร์",
        "MasterAutoPTOnMemberCannotBeLessThan": "ค่าความเสี่ยงแบบอัตโนมัติของมาสเตอร์ต้องมากกว่าหรือเท่ากับ {0}.",
        "MasterMaxPTCannotBeGreaterThan": "ค่าความเสี่ยงสูงสุดของมาสเตอร์ต้องน้อยกว่าหรือเท่ากับ {0}.",
        "MasterMaxPTCannotBeGreaterThanSuperMaxPT": "ค่าความเสี่ยงสูงสุดของมาสเตอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของซูเปอร์",
        "MasterMaxPTCannotBeLessThan": "ค่าความเสี่ยงสูงสุดของมาสเตอร์ต้องมากกว่าหรือเท่ากับ {0}.",
        "MasterMinPTCannotBeGreaterThanMasterMaxPT": "ค่าความเสี่ยงขั้นต่ำของมาสเตอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของมาสเตอร์",
        "MasterPTCannotBeGreaterThanMasterMaxPT": "ค่าความเสี่ยงของมาสเตอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของมาสเตอร์",
        "MasterPTCannotBeLessThan": "ค่าความเสี่ยงของมาสเตอร์ต้องมากกว่าหรือเท่ากับ {0}.",
        "MasterPTOnMemberCannotBeGreaterThan": "ค่าความเสี่ยงของมาสเตอร์ต่อเมมเบอร์ต้องน้อยกว่าหรือเท่ากับ {0}.",
        "MasterPTOnMemberCannotBeLessThan": "ค่าความเสี่ยงของมาสเตอร์ต่อเมมเบอร์ต้องมากกว่าหรือเท่ากับ {0}.",
        "Msg_20001": "ค่าความเสี่ยงไม่ถูกต้อง",
        "Msg_20019": "การตั้งค่ามาสเตอร์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของซูเปอร์",
        "Msg_20020": "การตั้งค่าเมมเบอร์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของซูเปอร์",
        "Msg_20021": "การตั้งค่ามาสเตอร์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของมาสเตอร์",
        "Msg_20022": "การตั้งค่าเมมเบอร์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของมาสเตอร์",
        "Msg_20023": "การตั้งค่าเอเจนท์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของเอเจนท์",
        "Msg_20024": "การตั้งค่าเมมเบอร์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของเอเจนท์",
        "Msg_20025": "ค่าความเสี่ยงขัดกับการตั้งค่าค่าความเสี่ยงขั้นต่ำ/ขั้นสูงสุดของทางต้นสาย",
        "Msg_20026": "การตั้งค่าเอเจนท์ต่างๆ ที่มีอยู่ก่อนแล้วเกินกว่าการตั้งค่าความเสี่ยงสูงสุดอันใหม่ของมาสเตอร์",
        "Msg_20027": "ค่าความเสี่ยงขัดกับการตั้งค่าค่าความเสี่ยงขั้นสูงสุดของมาสเตอร์ ที่ทางบริษัทตั้งค่าไว้",
        "Msg_20028": "ค่าความเสี่ยงขัดกับการตั้งค่าค่าความเสี่ยงขั้นสูงสุดของซูเปอร์",
        "Msg_20029": "ค่าความเสี่ยงขัดกับการตั้งค่าความเสี่ยงขั้นต่ำของซูเปอร์",
        "Msg_20030": "ค่าความเสี่ยงของเอเจนท์ขัดกับการตั้งค่าค่าความเสี่ยงขั้นสูงสุดของมาสเตอร์",
        "Msg_20031": "ค่าความเสี่ยงขัดกับการตั้งค่าค่าความเสี่ยงขั้นสูงสุดของมาสเตอร์",
        "Msg_20032": "ค่าความเสี่ยงขัดกับการตั้งค่าความเสี่ยงขั้นต่ำของมาสเตอร์",
        "Msg_20033": "ค่าความเสี่ยงขัดกับการตั้งค่าความเสี่ยงขั้นต่ำสุด/สูงสุดของเอเจนท์",
        "Msg_20034": "ค่าความเสี่ยงขัดกับการตั้งค่าค่าความเสี่ยงขั้นสูงสุดของเอเจนท์",
        "SuperAutoPTCannotBeGreaterThanSuperMaxPT": "ค่าความเสี่ยงแบบอัตโนมัติของซูเปอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของซูเปอร์",
        "SuperAutoPTOnMemberCannotBeGreaterThanSuperMaxPT": "ค่าความเสี่ยงแบบอัตโนมัติของซูเปอร์ต่อเมมเบอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของซูเปอร์",
        "SuperAutoPTOnMemberCannotBeLessThan": "ค่าความเสี่ยงแบบอัตโนมัติของซูเปอร์ต่อเมมเบอร์ต้องมากกว่าหรือเท่ากับ {0}.",
        "SuperMaxPTCannotBeGreaterThan": "ค่าความเสี่ยงของซูเปอร์ต้องน้อยกว่าหรือเท่ากับ {0}.",
        "SuperMinPTCannotBeGreaterThanSuperMaxPT": "ค่าความเสี่ยงขั้นต่ำของซูเปอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของซูเปอร์",
        "SuperPTCannotBeGreaterThanSuperMaxPT": "ค่าความเสี่ยงของซูเปอร์ต้องน้อยกว่าหรือเท่ากับค่าความเสี่ยงสูงสุดของซูเปอร์",
        "SuperPTOnMemberCannotBeGreaterThan": "ค่าความเสี่ยงของซูเปอร์ต่อเมมเบอร์ต้องน้อยกว่าหรือเท่ากับ {0}.",
        "SuperPTOnMemberCannotBeLessThan": "ค่าความเสี่ยงของซูเปอร์ต่อเมมเบอร์ต้องมากกว่ากว่าหรือเท่ากับ {0}.",
        "UpdatePositionTakingSuccessfully": "การตั้งค่าแบ่งสู้ ได้อัพเดทเรียบร้อยแล้ว."
    };
    
    positionTakingResources["message"] = positionTakingResources["message"] || {};
    positionTakingResources["message"]["vi-vn"] = {
        "UpdatePositionTakingSuccessfully": "PT được cập nhật thành công."
    };

    angular.module("customerSetting").value("positionTakingResources", positionTakingResources);
})();
