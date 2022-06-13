/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var resources = {};
    
    resources["label"] = resources["label"] || {};
    resources["label"]["en-us"] = {
        "Agent": "Agent",
        "Agents": "Agents",
        "CollapseAllToolTip": "Click to Expand All",
        "CollapseToolTip": "Click to Expand",
        "CopyTooltip": "Click to duplicate the values",
        "CustomerSetting": "Customer Setting",
        "ExpandAllToolTip": "Click to Collapse All",
        "ExpandToolTip": "Click to Collapse",
        "Fax": "Fax",
        "FirstName": "First Name",
        "GeneralInformation": "General Information",
        "LastName": "Last Name",
        "Live": "Live",
        "Master": "Master",
        "Masters": "Masters",
        "Member": "Member",
        "Members": "Members",
        "MobilePhone": "Mobile Phone",
        "Nickname": "Nickname",
        "No": "No",
        "Phone": "Phone",
        "SettingType_1": "Position Taking",
        "SettingType_11": "Product Status",
        "SettingType_12": "Loss Limit",
        "SettingType_13": "Product Status",
        "SettingType_14": "Ticket Commission",
        "SettingType_15": "Winning Share Commission",
        "SettingType_16": "Bet Setting",
        "SettingType_17": "Bet Limit",
        "SettingType_18": "Position Taking",
        "SettingType_19": "Commission",
        "SettingType_2": "Bet Setting",
        "SettingType_3": "Commission",
        "SettingType_4": "Product Status",
        "SettingType_5": "Charge Commission",
        "SettingType_6": "Win Loss Limit",
        "SettingType_7": "Bet Limit",
        "SettingType_8": "Bet Limit",
        "SettingType_9": "Bet Setting",
        "Super": "Super",
        "Update": "Update",
        "UserName": "Username",
        "UserNames": "Usernames",
        "Yes": "Yes"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-cn"] = {
        "Agent": "代理",
        "Agents": "代理",
        "CollapseAllToolTip": "点击全部扩张",
        "CollapseToolTip": "点击扩张",
        "CopyTooltip": "点击以复制数值",
        "CustomerSetting": "会员设定",
        "ExpandAllToolTip": "点击全部收起",
        "ExpandToolTip": "点击收起",
        "Fax": "传真",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "LastName": "姓",
        "Live": "现场",
        "Master": "总代理",
        "Masters": "总代理",
        "Member": "会员",
        "Members": "会员",
        "MobilePhone": "手机",
        "Nickname": "昵称",
        "No": "无",
        "Phone": "电话",
        "SettingType_1": "占成数",
        "SettingType_11": "产品状态",
        "SettingType_12": "输的限额",
        "SettingType_13": "产品状态",
        "SettingType_14": "注单佣金",
        "SettingType_15": "获奖赢额佣金",
        "SettingType_16": "投注设定",
        "SettingType_17": "投注限额",
        "SettingType_18": "占成数",
        "SettingType_19": "佣金",
        "SettingType_2": "投注设定",
        "SettingType_3": "佣金",
        "SettingType_4": "产品状态",
        "SettingType_5": "抽取佣金",
        "SettingType_6": "输赢限额",
        "SettingType_7": "投注限额",
        "SettingType_8": "投注限额",
        "SettingType_9": "投注设定",
        "Super": "超级总代理",
        "Update": "更改",
        "UserName": "用户名",
        "UserNames": "用户名",
        "Yes": "是"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["zh-tw"] = {
        "Agent": "代理",
        "Agents": "代理",
        "CollapseAllToolTip": "點擊全部擴張",
        "CollapseToolTip": "點擊擴張",
        "CopyTooltip": "點擊以復制數值",
        "CustomerSetting": "會員設定",
        "ExpandAllToolTip": "點擊全部收起",
        "ExpandToolTip": "點擊收起",
        "Fax": "傳真",
        "FirstName": "名",
        "GeneralInformation": "基本信息",
        "LastName": "姓",
        "Live": "現場",
        "Master": "總代理",
        "Masters": "總代理",
        "Member": "會員",
        "Members": "會員",
        "MobilePhone": "手機",
        "Nickname": "暱稱",
        "No": "無",
        "Phone": "電話",
        "SettingType_1": "佔成數",
        "SettingType_11": "產品狀態",
        "SettingType_12": "輸的限額",
        "SettingType_13": "產品狀態",
        "SettingType_14": "注單佣金",
        "SettingType_15": "獲獎贏額佣",
        "SettingType_16": "投注設定",
        "SettingType_17": "投注限額",
        "SettingType_18": "佔成數",
        "SettingType_19": "佣金",
        "SettingType_2": "投注設定",
        "SettingType_3": "佣金",
        "SettingType_4": "產品狀態",
        "SettingType_5": "抽取傭金",
        "SettingType_6": "輸贏限額",
        "SettingType_7": "投注限額",
        "SettingType_8": "投注限額",
        "SettingType_9": "投注設定",
        "Super": "超級總代理",
        "Update": "更改",
        "UserName": "用戶名",
        "UserNames": "用戶名",
        "Yes": "是"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ko-kr"] = {
        "Agents": "에이전트",
        "CustomerSetting": "Customer Setting",
        "Fax": "팩스",
        "FirstName": "이름",
        "GeneralInformation": "일반 정보",
        "LastName": "성",
        "Live": "라이브",
        "Masters": "마스터",
        "Members": "회원",
        "MobilePhone": "핸드폰",
        "Nickname": "닉네임",
        "No": "아니오",
        "Phone": "전화",
        "SettingType_1": "거래 조건",
        "SettingType_16": "배팅 설정",
        "SettingType_18": "거래 조건",
        "SettingType_19": "수수료",
        "SettingType_2": "배팅 설정",
        "SettingType_3": "수수료",
        "SettingType_9": "배팅 설정",
        "Update": "업데이트",
        "UserName": "사용자 이름",
        "Yes": "Yes"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["ja-jp"] = {
        "Fax": "ファックス",
        "FirstName": "名",
        "GeneralInformation": "General Information",
        "LastName": "氏",
        "Live": "ライブ",
        "MobilePhone": "携帯電話番号",
        "Nickname": "ニックネーム",
        "No": "No",
        "Phone": "電話番号",
        "Update": "更新",
        "UserName": "ユーザー名",
        "Yes": "Yes"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["th-th"] = {
        "Agent": "เอเย่นต์",
        "Agents": "เอเจนท์",
        "CollapseAllToolTip": "คลิกเพื่อขยายทั้งหมด",
        "CollapseToolTip": "คลิกเพื่อขยาย",
        "CopyTooltip": "คลิ๊กค่าที่จะซ้ำกัน",
        "CustomerSetting": "ตั้งค่าสมาชิก",
        "ExpandAllToolTip": "คลิกเพื่อปิดส่วนขยายทั้งหมด",
        "ExpandToolTip": "คลิกเพื่อปิดส่วนขยาย",
        "Fax": "แฟกซ์",
        "FirstName": "ชื่อ",
        "GeneralInformation": "ข้อมูลทั่วไป",
        "LastName": "นามสกุล",
        "Live": "สด",
        "Master": "มาสเตอร์",
        "Masters": "มาสเตอร์ต่าง ๆ",
        "Member": "สมาชิก",
        "Members": "เมมเบอร์ต่าง ๆ",
        "MobilePhone": "โทรศัพท์มือถือ",
        "Nickname": "ชื่อเล่น",
        "No": "ไม่",
        "Phone": "โทรศัพท์",
        "SettingType_1": "ค่าความเสี่ยง",
        "SettingType_11": "สถานะผลิตภันฑ์",
        "SettingType_12": "จำกัดยอดเสีย",
        "SettingType_13": "สถานะผลิตภันฑ์",
        "SettingType_14": "ค่าคอมมิชชั่นของตั๋ว",
        "SettingType_15": "ค่าคอมมิชชั่นที่ชนะ",
        "SettingType_16": "การกำหนดการวางเดิมพัน",
        "SettingType_17": "วงเงินเดิมพัน",
        "SettingType_18": "ค่าความเสี่ยง",
        "SettingType_19": "คอมมิชชั่น",
        "SettingType_2": "การกำหนดการวางเดิมพัน",
        "SettingType_3": "คอมมิชชั่น",
        "SettingType_4": "สถานะผลิตภันฑ์",
        "SettingType_5": "ค่าธรรมเนียมคอมมิชชั่น",
        "SettingType_6": "วงเงินแพ้ชนะ",
        "SettingType_7": "วงเงินเดิมพัน",
        "SettingType_8": "วงเงินเดิมพัน",
        "SettingType_9": "การกำหนดการวางเดิมพัน",
        "Super": "ซุปเปอร์",
        "Update": "ล่าสุด",
        "UserName": "ชื่อผู้ใช้ระบบ",
        "UserNames": "ยูเซอร์เนม",
        "Yes": "ตกลง"
    };
    
    resources["label"] = resources["label"] || {};
    resources["label"]["vi-vn"] = {
        "Agents": "Agents",
        "CustomerSetting": "Thiết lập",
        "Fax": "Fax",
        "FirstName": "Tên",
        "GeneralInformation": "Thông tin Chung",
        "LastName": "Họ",
        "Live": "Trực Tuyến",
        "Masters": "Masters",
        "Member": "Người chơi",
        "Members": "Members",
        "MobilePhone": "Điện thoại di động",
        "Nickname": "Biệt danh",
        "No": "Không",
        "Phone": "Điện thoại",
        "SettingType_1": "PT",
        "SettingType_11": "Trạng thái trò chơi",
        "SettingType_12": "Giới Hạn Tiền Thua",
        "SettingType_13": "Trạng thái trò chơi",
        "SettingType_14": "Hoa hồng trên vé cược",
        "SettingType_15": "Phần chia thắng cược",
        "SettingType_16": "Tiền Cược",
        "SettingType_17": "Giới hạn cược",
        "SettingType_18": "PT",
        "SettingType_19": "Hoa hồng",
        "SettingType_2": "Tiền Cược",
        "SettingType_3": "Hoa hồng",
        "SettingType_4": "Trạng thái trò chơi",
        "SettingType_6": "Giới hạn thắng thua",
        "SettingType_7": "Giới hạn cược",
        "SettingType_8": "Giới hạn cược",
        "SettingType_9": "Tiền Cược",
        "Update": "Cập nhật",
        "UserName": "Tài khoản",
        "Yes": "Có"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["en-us"] = {
        "AccessDeniedMsg": "Sorry, You don&apos;t have permission to access this page.",
        "AccountWasDisabled": "This account was disabled.",
        "AllProducts": "All Products",
        "CopiedSuccessfully": "All fields in green are copied.",
        "CopyingFailed": "Unable to copy to the fields in red.",
        "CustomersAreNotSameLine": "Customers are not in the same line!",
        "CustomerSettings": "Customer Settings",
        "DataIsNotValid": "Data is not valid.",
        "DuplicateValuesAlertMsg": "Out of range for highlighted values.",
        "Msg_90001": "Update process is unsuccessful !",
        "NoChangeToUpdate": "There is no change to update.",
        "ProductNameMissing": "Product Id {0} does not have name in {1}",
        "SettingTypeError": "Unable to load the setting type due to a technical problem. Please try again later or notify system administrator!",
        "SettingTypeHasNoSettingItem": "Setting Type {0} does not have any setting item.",
        "SportTypeOrBetTypeIsInvalid": "Sport Type or Bet Type is invalid!",
        "SubAccountIdShouldBeZeroWhenSystemIsNotAgency": "Sub Account Id Should Be Zero When System Is Not Agency.",
        "SystemError": "Oops... you are here because of a system error, please notify system administrator.",
        "UMMainMsg": "Unable to Update because of the maintenance of the settings below (GMT+8):",
        "UMSubMsg": "{0}: {1} ({2} to {3})",
        "UnderMaintainanceMainMsg": "Here is the maintenance schedule (GMT +8). During the maintenance period, these settings cannot be changed. We are sorry for any inconvenience caused.",
        "UplineLevelCannotBeNullIncaseOfEdittingMember": "Upline Level cannot be null incase of editting member.",
        "UplineLevelIsNotValid": "Upline Level is not valid.",
        "UplineLevelShouldBeNull": "Upline Level should be null.",
        "YouAreNotAuthorized": "You are not authorized on this function!"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-cn"] = {
        "AccessDeniedMsg": "对不起,您不被允许进入此网页.",
        "AccountWasDisabled": "这个帐号禁用",
        "AllProducts": "所有产品",
        "CopiedSuccessfully": "绿色的所有字段已被复制。",
        "CopyingFailed": "红色字段无法复制。",
        "CustomersAreNotSameLine": "会员不同线！",
        "CustomerSettings": "客户设定",
        "DuplicateValuesAlertMsg": "突显值已超出范围。",
        "Msg_90001": "更新程序失败！",
        "NoChangeToUpdate": "没有任何的变化更新。",
        "SettingTypeError": "由于技术故障导致加载设置类型失败。请稍后再试或联系系统管理员！",
        "SystemError": "哎哟..因为系统故障所以您会身在此处，请向管理层联系吧",
        "UMMainMsg": "由於以下設定正在維護中，因此無法更新 (GMT+8):",
        "UMSubMsg": "{0}: {1} ({2} 至 {3})",
        "YouAreNotAuthorized": "您没有使用此功能的权限"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["zh-tw"] = {
        "AccessDeniedMsg": "對不起, 您不被允許進入此網頁.",
        "AccountWasDisabled": "這個帳號禁用",
        "AllProducts": "所有產品",
        "CopiedSuccessfully": "綠色的所有字段已被複製。",
        "CopyingFailed": "紅色字段無法複製。",
        "CustomersAreNotSameLine": "會員不同線！",
        "CustomerSettings": "客戶設定",
        "DuplicateValuesAlertMsg": "突顯值已超出範圍。",
        "Msg_90001": "更新程序失敗！",
        "NoChangeToUpdate": "沒有任何的變化更新。",
        "SettingTypeError": "由於技術故障導致加載設置類型失敗。請稍後再試或聯系系統管理員！",
        "SystemError": "哎喲..因為系統故障所以您會身在此處，請向管理層聯繫吧",
        "UMMainMsg": "由于以下设定正在维护中，因此无法更新 (GMT+8):",
        "UMSubMsg": "{0}: {1} ({2} 至 {3})",
        "YouAreNotAuthorized": "您沒有使用此功能的權限"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ko-kr"] = {
        "AccessDeniedMsg": "죄송합니다. 이페이지를 엑서스 할 수 있는 권한이 없습니다.",
        "AccountWasDisabled": "이 계정은 실행하실수없는 계정입니다.",
        "SystemError": "죄송합니다. 시스템에러가 발생했습니다. 관리자에게 문의 바랍니다.",
        "YouAreNotAuthorized": "해당 기능에 대한 권한이 없습니다."
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["ja-jp"] = {
        "AccessDeniedMsg": "Sorry, You don&apos;t have permission to access this page.",
        "AccountWasDisabled": "This account was disabled.",
        "SystemError": "Oops... you are here because of a system error, please notify the administrators"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["th-th"] = {
        "AccessDeniedMsg": "ขออภัยคุณไม่ได้รับอนุญาติให้เข้าหน้านี้",
        "AccountWasDisabled": "บัญชีนี้ได้ถูกปิดไปแล้ว",
        "AllProducts": "โปรดักส์ทั้งหมด",
        "CopiedSuccessfully": "ช่องสีเขียวทั้งหมดจะถูกคัดลอก",
        "CopyingFailed": "ไม่สามารถคัดลอกไปยังช่องสีแดง",
        "CustomersAreNotSameLine": "ลูกเค้าไม่ได้อยู่ในสายเดียวกัน",
        "CustomerSettings": "การตั้งค่าของลูกค้า",
        "DuplicateValuesAlertMsg": "ตัวเลขที่ถูกไฮไลท์ไม่ได้อยู่ในขอบเขต",
        "Msg_90001": "กระบวนการอัพเดทสำเร็จแล้ว",
        "NoChangeToUpdate": "ไม่มีการเปลี่ยนแปลงการอัพเดท",
        "SettingTypeError": "ไม่สามารถโหลดการตั้งค่าต่าง ๆ เนื่องจากปัญหาทางด้านเทคนิค รบกวนลองใหม่อีกครั้งหรือจนกว่าจะได้รับคำประกาศจากทางผู้ดูแลระบบ",
        "SystemError": "อุ้ย...คุณอยู่ที่นี่เนื่องจากระบบผิดพลาด กรุณาแจ้งผู้ให้บริการ",
        "UMMainMsg": "เนื่องจากอยู่ในระหว่างการปรับปรุง ดังนั้นในช่วงเวลาดังกล่าวจึงไม่สามารถอัพเดทได้ (GMT+8):",
        "UMSubMsg": "{0}: {1} ({2} ถึง {3})",
        "YouAreNotAuthorized": "คุณไม่มีอำนาจในการเข้าใช้งานฟังก์ชันนี้"
    };
    
    resources["message"] = resources["message"] || {};
    resources["message"]["vi-vn"] = {
        "AccessDeniedMsg": "Xin lỗi, bạn không được phép truy cập vào trang này",
        "AccountWasDisabled": "Tài khoản bị vô hiệu hóa",
        "AllProducts": "Tất cả sản phẩm",
        "CopiedSuccessfully": "Tất cả những ô được gán giá trị sao chép sẽ được đánh dấu xanh.",
        "CopyingFailed": "Không thể gán giá trị được sao chép vào những ô được đánh dấu đỏ.",
        "CustomerSettings": "Thiết lập khách hàng",
        "NoChangeToUpdate": "Không có thay đổi mới được cập nhật.",
        "SystemError": "Oh …hệ thống đang bi lỗi,Vui lòng báo với người quản lý",
        "UMMainMsg": "Không thể cập nhật do hệ thống đang bảo trì các thiết lập sau (GMT+8):",
        "UMSubMsg": "{0}: {1} ({2} đến {3})",
        "YouAreNotAuthorized": "Bạn không được cho phép sử dụng chức năng này"
    };

    angular.module("customerSetting").value("customerSettingResource", resources);
})();
