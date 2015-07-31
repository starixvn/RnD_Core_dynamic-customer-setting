"use strict";
/*jshint -W101,-W106,-W044,-W098*/

(function () {
    var resources = {};
    
    resources["label"] = resources["label"] || {};
    resources["label"]["en-us"] = {
        "AgentMaxPT": "Agent Max PT",
        "AgentMinPT": "Agent Min PT",
        "AgentPT": "Agent PT",
        "Agents": "Agents",
        "BetSetting": "Bet Setting",
        "Commission": "Commission",
        "CommissionForAgent": "Agent Comm",
        "CommissionForMaster": "Master Comm",
        "CommissionForMember": "Member Comm",
        "CopyTooltip": "Click to duplicate the values",
        "CustomerSetting": "Customer Setting",
        "Enable": "Enable",
        "EnableDownline": "Enable Downline",
        "Fax": "Fax",
        "FirstName": "First Name",
        "GeneralInformation": "General Information",
        "IsAuto": "Is Auto",
        "LastName": "Last Name",
        "Live": "Live",
        "MasterMaxPT": "Master Max PT",
        "MasterMinPT": "Master Min PT",
        "MasterPT": "Master PT",
        "Masters": "Masters",
        "Max": "Max",
        "MaxBet": "Max Bet",
        "MaxLive": "Max Live",
        "MaxPerMatch": "Max Per Match",
        "Members": "Members",
        "Min": "Min",
        "MinBet": "Min Bet",
        "MinLive": "Min Live",
        "MobilePhone": "Mobile Phone",
        "Nickname": "Nickname",
        "Phone": "Phone",
        "PositionTaking": "Position Taking",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "Super Max PT",
        "SuperMinPT": "Super Min PT",
        "SuperPT": "Super PT",
        "Update": "Update",
        "UserName": "UserName",
        "UserNames": "UserNames"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-cn"] = {
        "AgentMaxPT": "代理最高占成",
        "AgentMinPT": "代理最低占成数",
        "AgentPT": "代理占成数",
        "Agents": "代理",
        "BetSetting": "投注设定",
        "Commission": "佣金",
        "CommissionForAgent": "代理佣金",
        "CommissionForMaster": "总代理佣金",
        "CommissionForMember": "会员佣金",
        "Enable": "启用",
        "EnableDownline": "启用下线",
        "Fax": "传真",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "IsAuto": "是自动",
        "LastName": "姓",
        "Live": "现场",
        "MasterMaxPT": "总代理最高占成",
        "MasterMinPT": "总代理最低占成数",
        "MasterPT": "总代理占成数",
        "Masters": "会员",
        "Max": "最高",
        "MaxBet": "最高投注",
        "MaxLive": "Max Live",
        "MaxPerMatch": "单场限额",
        "Members": "会员",
        "Min": "最低",
        "MinBet": "最低投注",
        "MinLive": "Min Live",
        "MobilePhone": "手机",
        "Nickname": "昵称",
        "Phone": "电话",
        "PositionTaking": "占成数",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "超级总代理最高占成",
        "SuperMinPT": "超级总代理最低占成",
        "SuperPT": "超级总代理占成数",
        "Update": "更改",
        "UserName": "用户名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-tw"] = {
        "AgentMaxPT": "代理最高佔成",
        "AgentMinPT": "代理最低占成数",
        "AgentPT": "代理占成数",
        "Agents": "代理",
        "BetSetting": "投注設定",
        "Commission": "佣金",
        "CommissionForAgent": "代理佣金",
        "CommissionForMaster": "總代理佣金",
        "CommissionForMember": "會員佣金",
        "Enable": "啟用",
        "EnableDownline": "啟用下線",
        "Fax": "傳真",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "IsAuto": "是自動",
        "LastName": "姓",
        "Live": "現場",
        "MasterMaxPT": "總代理最高佔成",
        "MasterMinPT": "总代理最低占成数",
        "MasterPT": "总代理占成数",
        "Masters": "會員",
        "Max": "最高",
        "MaxBet": "最高投注",
        "MaxPerMatch": "單場限額",
        "Members": "會員",
        "Min": "最低",
        "MinBet": "最低投注",
        "MinLive": "Min Live",
        "MobilePhone": "手機",
        "Nickname": "暱稱",
        "Phone": "電話",
        "PositionTaking": "佔成數",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "超級總代理最高佔成",
        "SuperMinPT": "超級總代理最低佔成",
        "SuperPT": "超级总代理占成数",
        "Update": "更改",
        "UserName": "用戶名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ko-kr"] = {
        "AgentMaxPT": "에이전트 최대 PT",
        "AgentMinPT": "에이전트 최소 PT",
        "AgentPT": "에이전트 PT",
        "Agents": "에이전트",
        "BetSetting": "배팅 설정",
        "Commission": "수수료",
        "CommissionForAgent": "에이전트 수수료",
        "CommissionForMaster": "마스터 수수료",
        "CommissionForMember": "회원 수수료",
        "Enable": "활성화",
        "EnableDownline": "다운라인 활성화하기",
        "Fax": "팩스",
        "FirstName": "이름",
        "IsAuto": "자동",
        "LastName": "성",
        "Live": "라이브",
        "MasterMaxPT": "마스터 최대 PT",
        "MasterMinPT": "마스터 최소PT",
        "MasterPT": "마스터 PT",
        "Masters": "마스터",
        "Max": "최대",
        "MaxBet": "최대 배팅",
        "MaxPerMatch": "경기당 최대 값",
        "Members": "회원",
        "Min": "최소",
        "MinBet": "최소 배팅",
        "MobilePhone": "핸드폰",
        "Nickname": "닉네임",
        "Phone": "전화",
        "PositionTaking": "거래 조건",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "슈퍼 최대 PT",
        "SuperMinPT": "슈퍼 최소PT",
        "SuperPT": "슈퍼 PT",
        "Update": "업데이트",
        "UserName": "사용자 이름"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ja-jp"] = {
        "Fax": "ファックス",
        "FirstName": "名",
        "LastName": "氏",
        "Live": "ライブ",
        "MaxBet": "最大ベット",
        "MaxPerMatch": "1 試合毎の最大",
        "MobilePhone": "携帯電話番号",
        "Nickname": "ニックネーム",
        "Phone": "電話番号",
        "Update": "更新",
        "UserName": "ユーザー名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["th-th"] = {
        "AgentMaxPT": "แบ่งสู้สูงสุดของเอ",
        "AgentMinPT": "ค่าแบ่งสู้ต่ำสุดของ",
        "AgentPT": "ค่าแบ่งสู้ของเอเย่น",
        "Agents": "ตัวแทน",
        "BetSetting": "การกำหนดการวางเดิมพัน",
        "Commission": "คอมมิชชั่น",
        "CommissionForAgent": "คอมมิชชั่นของ Agent",
        "CommissionForMaster": "คอมมิชชั่นของ Master",
        "CommissionForMember": "คอมมิชชั่นของสมาชิก",
        "Enable": "เปิดใช้งาน",
        "EnableDownline": "เปิดใช้งาน ดาวไลน์",
        "Fax": "แฟกซ์",
        "FirstName": "ชื่อ",
        "GeneralInformation": "ข้อมูลทั่วไป",
        "IsAuto": "คืออัตโนมัติ",
        "LastName": "นามสกุล",
        "Live": "สด",
        "MasterMaxPT": "แบ่งสู้สูงสุดของ",
        "MasterMinPT": "แบ่งสู้ขั้นต่ำของมาสเตอร์",
        "MasterPT": "ค่าแบ่งสู้ของมาสเตอร์",
        "Masters": "ตัวแทนหลัก",
        "Max": "สูงสุด",
        "MaxBet": "ค่าเดิมพันสูงสุด",
        "MaxPerMatch": "ค่าเดิมพันสูงสุดต่อคู่การแข่งขัน",
        "Members": "สมาชิก",
        "Min": "ต่ำสุด",
        "MinBet": "ค่าเดิมพันต้ำสุด",
        "MobilePhone": "โทรศัพท์มือถือ",
        "Nickname": "ชื่อเล่น",
        "Phone": "โทรศัพท์",
        "PositionTaking": "ค่าความเสี่ยง",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "แบ่งสู้สูงสุดของซุปเปรอ์",
        "SuperMinPT": "แบ่งสู้ต่ำสุดของซุปเปรอ์",
        "SuperPT": "ค่าแบ่งสู้ของซุปเปอร์",
        "Update": "ล่าสุด",
        "UserName": "ชื่อผู้ใช้ระบบ"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["vi-vn"] = {
        "AgentMaxPT": "PT lớn nhất của Agent",
        "AgentMinPT": "PT nhỏ nhất của Agent",
        "AgentPT": "PT của Agent",
        "Agents": "Agents",
        "BetSetting": "Tiền Cược",
        "Commission": "Hoa hồng",
        "CommissionForAgent": "Agent Comm",
        "CommissionForMaster": "Master Comm",
        "CommissionForMember": "Member Comm",
        "Enable": "Cho phép",
        "EnableDownline": "Enable Downline",
        "Fax": "Fax",
        "FirstName": "Tên",
        "GeneralInformation": "Thông tin Chung",
        "IsAuto": "Auto PT",
        "LastName": "Họ",
        "Live": "Trực Tuyến",
        "MasterMaxPT": "PT lớn nhất của Master",
        "MasterMinPT": "PT nhỏ nhất của Master",
        "MasterPT": "PT của Master",
        "Masters": "Masters",
        "Max": "Max",
        "MaxBet": "Cược lớn nhất",
        "MaxLive": "Max Live",
        "MaxPerMatch": "Cược tối đa cho 1 trận",
        "Members": "Members",
        "Min": "Nhỏ nhất",
        "MinBet": "Cược nhỏ nhất",
        "MinLive": "Min Live",
        "MobilePhone": "Điện thoại di động",
        "Nickname": "Biệt danh",
        "PositionTaking": "PT",
        "ProductStatus": "Product Status",
        "SuperMaxPT": "PT lớn nhất của Super",
        "SuperMinPT": "PT nhỏ nhất của Super",
        "SuperPT": "PT của Super",
        "Update": "Cập nhật",
        "UserName": "Tài khoản"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["en-us"] = {
        "AccessDeniedMsg": "You do not have permission to access this page.",
        "AccountWasDisabled": "Account {0} was disabled.",
        "CustomersAreNotSameLine": "Customers are not in the same line!",
        "DataIsNotValid": "Data is not valid.",
        "DownlineStatusIsInvalid": "The product is not offered for this line of customers",
        "DuplicateValuesAlertMsg": "Out of range for highlighted values.",
        "MaxBetLessThanOrEqualMaxPerMatch": "Max Bet must be less than or equal to Max Per Match",
        "MaxBetLessThanOrEqualTo": "Max Bet must be less than or equal to {0}",
        "MaxBetMoreThanOrEqualMinBet": "Max Bet must be greater than or equal to Min Bet",
        "MaxPerMatchLessThanOrEqualTo": "Max Per Match must be less than or equal to {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "Max Per Match must be greater than or equal to Max Bet",
        "MinBetLessThanOrEqualMaxBet": "Min Bet must be less than or equal to Max Bet",
        "MinBetMoreThanOrEqualTo": "Min Bet must be greater than or equal to {0}",
        "Msg_90001": "Update process is unsuccessful !",
        "NoChangeToUpdate": "There is no change to update!",
        "ProductNameMissing": "Product Id {0} does not have name in {1}",
        "SettingTypeError": "This setting type is error, please notify your administrator.",
        "SportTypeOrBetTypeIsInvalid": "Sport Type or Bet Type is invalid!",
        "SubAccountIdShouldBeZeroWhenSystemIsNotAgency": "Sub Account Id Should Be Zero When System Is Not Agency.",
        "SystemError": "Oops... you are here because of a system error, please notify your administrator.",
        "UpdateBetSettingSuccessfully": "Updated bet setting successfully.",
        "UpdateCommissionSuccessfully": "Updated commission setting successfully.",
        "UpdateMemberPtInfo": "You have changed your position taking.",
        "UpdateMemberPtQuestion": "Do you want to update all member&apos;s position taking to:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT",
        "UpdatePositionTakingSuccessfully": "Updated position taking successfully.",
        "UpdateProductStatusSuccessfully": "Updated product status successfully.",
        "UplineLevelCannotBeNullIncaseOfEdittingMember": "Upline Level cannot be null incase of editting member.",
        "UplineLevelIsNotValid": "Upline Level is not valid.",
        "UplineLevelShouldBeNull": "Upline Level should be null."
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-cn"] = {
        "MaxBetLessThanOrEqualMaxPerMatch": "最大投注额必须小于或等于最大单场比赛投注额",
        "MaxBetLessThanOrEqualTo": "最大投注额必须小于或等于 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注额必须大于或等于最小投注额",
        "MaxPerMatchLessThanOrEqualTo": "最大单场比赛投注额必须小于或等于 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大单场比赛投注额必须大于或等于最大投注额",
        "MinBetLessThanOrEqualMaxBet": "最小投注额必须小于或等于最大投注额",
        "MinBetMoreThanOrEqualTo": "最大投注额必须大于或等于 {0}",
        "UpdateBetSettingSuccessfully": "成功更新投注设置。",
        "UpdateCommissionSuccessfully": "成功更新佣金设置",
        "UpdateMemberPtInfo": "您已更改您的占成数",
        "UpdateMemberPtQuestion": "您是否要把所有会员的占成数更改为:",
        "UpdateMemberPtTitle": "确认更新会员的占成数"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-tw"] = {
        "MaxBetLessThanOrEqualMaxPerMatch": "最大投注額必須小於或等於最大單場比賽投注額",
        "MaxBetLessThanOrEqualTo": "最大投注額必須小於或等於 {0}",
        "MaxBetMoreThanOrEqualMinBet": "最大投注額必須大於或等於最小投注額",
        "MaxPerMatchLessThanOrEqualTo": "最大單場比賽投注額必須小於或等於 {0}",
        "MaxPerMatchMoreThanOrEqualMaxBet": "最大單場比賽投注額必須大於或等於最大投注額",
        "MinBetLessThanOrEqualMaxBet": "最小投注額必須小於或等於最大投注額",
        "MinBetMoreThanOrEqualTo": "最大投注額必須大於或等於 {0}",
        "UpdateBetSettingSuccessfully": "成功更新投注設置。",
        "UpdateCommissionSuccessfully": "成功更新傭金設置",
        "UpdateMemberPtInfo": "您已更改您的占成数",
        "UpdateMemberPtQuestion": "您是否要把所有會員佔成數更改為:",
        "UpdateMemberPtTitle": "確認更新會員的佔成數"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ko-kr"] = {
        "UpdateMemberPtInfo": "포지션테이킹(PT)이 변경되었습니다",
        "UpdateMemberPtQuestion": "모든회원의 거래조건을 바꾸기를 원하십니까:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ja-jp"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["th-th"] = {
        "UpdateMemberPtInfo": "คุณมีการเปลี่ยนค่าแบ่งสู้ของคุณ",
        "UpdateMemberPtQuestion": "คุณต้องการจะอัพเดทแบ่งสู้บนสมาชิกทั้งหมกหรือไม่:",
        "UpdateMemberPtTitle": "ยืนยันการอัพเดทการตั้งค่าความเสี่ยง"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["vi-vn"] = {
        "UpdateMemberPtInfo": "Bạn đã  thay đổi giá trị PT của mình",
        "UpdateMemberPtQuestion": "Bạn có muốn cập nhật giá trị PTcủa tất cả các thành viên:",
        "UpdateMemberPtTitle": "Confirm Update Members&apos;s PT"
    };

    angular.module("customerSetting").value("customerSettingResource", resources);
})();
