/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var resources = {};
    
    resources["label"] = resources["label"] || {};
    resources["label"]["en-us"] = {
        "Agents": "Agents",
        "CopyTooltip": "Click to duplicate the values",
        "CustomerSetting": "Customer Setting",
        "Fax": "Fax",
        "FirstName": "First Name",
        "GeneralInformation": "General Information",
        "LastName": "Last Name",
        "Live": "Live",
        "Masters": "Masters",
        "Members": "Members",
        "MobilePhone": "Mobile Phone",
        "Nickname": "Nickname",
        "No": "No",
        "Phone": "Phone",
        "Update": "Update",
        "UserName": "Username",
        "UserNames": "Usernames",
        "Yes": "Yes"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-cn"] = {
        "Agents": "代理",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "LastName": "姓",
        "Live": "现场",
        "Masters": "会员",
        "Members": "会员",
        "MobilePhone": "手机",
        "Nickname": "昵称",
        "Phone": "电话",
        "Update": "更改",
        "UserName": "用户名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-tw"] = {
        "Agents": "代理",
        "Fax": "傳真",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "LastName": "姓",
        "Live": "現場",
        "Masters": "會員",
        "Members": "會員",
        "MobilePhone": "手機",
        "Nickname": "暱稱",
        "Phone": "電話",
        "Update": "更改",
        "UserName": "用戶名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ko-kr"] = {
        "Agents": "에이전트",
        "Fax": "팩스",
        "FirstName": "이름",
        "LastName": "성",
        "Live": "라이브",
        "Masters": "마스터",
        "Members": "회원",
        "MobilePhone": "핸드폰",
        "Nickname": "닉네임",
        "Phone": "전화",
        "Update": "업데이트",
        "UserName": "사용자 이름"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ja-jp"] = {
        "Fax": "ファックス",
        "FirstName": "名",
        "LastName": "氏",
        "Live": "ライブ",
        "MobilePhone": "携帯電話番号",
        "Nickname": "ニックネーム",
        "Phone": "電話番号",
        "Update": "更新",
        "UserName": "ユーザー名"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["th-th"] = {
        "Agents": "ตัวแทน",
        "Fax": "แฟกซ์",
        "FirstName": "ชื่อ",
        "GeneralInformation": "ข้อมูลทั่วไป",
        "LastName": "นามสกุล",
        "Live": "สด",
        "Masters": "ตัวแทนหลัก",
        "Members": "สมาชิก",
        "MobilePhone": "โทรศัพท์มือถือ",
        "Nickname": "ชื่อเล่น",
        "Phone": "โทรศัพท์",
        "Update": "ล่าสุด",
        "UserName": "ชื่อผู้ใช้ระบบ"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["vi-vn"] = {
        "Agents": "Agents",
        "Fax": "Fax",
        "FirstName": "Tên",
        "GeneralInformation": "Thông tin Chung",
        "LastName": "Họ",
        "Live": "Trực Tuyến",
        "Masters": "Masters",
        "Members": "Members",
        "MobilePhone": "Điện thoại di động",
        "Nickname": "Biệt danh",
        "Update": "Cập nhật",
        "UserName": "Tài khoản"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["en-us"] = {
        "AccessDeniedMsg": "You do not have permission to access this page.",
        "AccountWasDisabled": "Account {0} was disabled.",
        "CopyingFailed": "Unable to copy value to the hi-lighted field(s).",
        "CustomersAreNotSameLine": "Customers are not in the same line!",
        "DataIsNotValid": "Data is not valid.",
        "DuplicateValuesAlertMsg": "Out of range for highlighted values.",
        "Msg_90001": "Update process is unsuccessful !",
        "NoChangeToUpdate": "There is no change to update!",
        "ProductNameMissing": "Product Id {0} does not have name in {1}",
        "SettingTypeError": "Unable to load the setting type due to a technical problem. Please try again later or notify system administrator!",
        "SportTypeOrBetTypeIsInvalid": "Sport Type or Bet Type is invalid!",
        "SubAccountIdShouldBeZeroWhenSystemIsNotAgency": "Sub Account Id Should Be Zero When System Is Not Agency.",
        "SystemError": "Oops... you are here because of a system error, please notify system administrator.",
        "UplineLevelCannotBeNullIncaseOfEdittingMember": "Upline Level cannot be null incase of editting member.",
        "UplineLevelIsNotValid": "Upline Level is not valid.",
        "UplineLevelShouldBeNull": "Upline Level should be null.",
        "YouAreNotAuthorized": "You are not authorized on this function!"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-cn"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-tw"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ko-kr"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ja-jp"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["th-th"] = {
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("customerSettingResource", resources);
})();
