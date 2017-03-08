/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType13Resources = {};
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["en-us"] = {
        "Enable": "Enable",
        "EnableDownline": "Enable Downline",
        "ProductStatus": "Product Status"
    };
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["zh-cn"] = {
        "Enable": "启用",
        "EnableDownline": "启动下线",
        "ProductStatus": "产品状态"
    };
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["zh-tw"] = {
        "Enable": "啟用",
        "EnableDownline": "啟動下線",
        "ProductStatus": "產品狀態"
    };
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["ko-kr"] = {
        "Enable": "활성화",
        "EnableDownline": "다운라인 활성화하기",
        "ProductStatus": "Product Status"
    };
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["th-th"] = {
        "Enable": "เปิดใช้งาน",
        "EnableDownline": "เปิดการใช้งาน ดาวน์ไลน์",
        "ProductStatus": "สถานะผลิตภันฑ์"
    };
    
    settingType13Resources["label"] = settingType13Resources["label"] || {};
    settingType13Resources["label"]["vi-vn"] = {
        "Enable": "Cho phép",
        "EnableDownline": "Cho phép thành viên cấp dưới",
        "ProductStatus": "Trạng thái trò chơi"
    };
    
    settingType13Resources["message"] = settingType13Resources["message"] || {};
    settingType13Resources["message"]["en-us"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "Downline cannot be enabled when Upline is disabled.",
        "DownlineProductStatusCannotBeNull": "Downline Product Status cannot be null.",
        "DownlineStatusCannotBeChanged": "Downline's Product Status must be unchanged when the Upline has not been changed.",
        "DownlineStatusIsInvalid": "The product is not offered for this customer line.",
        "Msg_60033": "Unable to edit downline&apos;s Product Status because the account disabled for this product.",
        "Msg_60034": "Unable to synchronize this product because the upline account setting is not synchronized.",
        "Msg_60035": "You are not allowed to disable downline&apos;s product.",
        "Msg_90012": "You are not allowed to perform this action.",
        "UpdateProductStatusSuccessfully": "Product Status is updated successfully."
    };
    
    settingType13Resources["message"] = settingType13Resources["message"] || {};
    settingType13Resources["message"]["zh-cn"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "当上线被禁用下线同时也禁用。",
        "DownlineStatusCannotBeChanged": "当上线状态没有更改，下线状态也不会被更改。",
        "DownlineStatusIsInvalid": "此产品没有提供给此用户",
        "Msg_60033": "由于账号的此产品已关闭导致无法更改下线产品状态。",
        "Msg_60034": "无法连接此产品由于上线的账号设置不同步。",
        "Msg_60035": "您不能禁用下线的产品。",
        "Msg_90012": "您不能执行此操作。",
        "UpdateProductStatusSuccessfully": "产品状态更新成功"
    };
    
    settingType13Resources["message"] = settingType13Resources["message"] || {};
    settingType13Resources["message"]["zh-tw"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "當上線被禁用下線同時也禁用。",
        "DownlineStatusCannotBeChanged": "當上線狀態沒有更改，下線狀態也不會被更改。",
        "DownlineStatusIsInvalid": "此產品沒有提供給此用戶",
        "Msg_60033": "由於賬號的此產品已關閉導致無法更改下線產品狀態。",
        "Msg_60034": "無法連接此產品由於上線的賬號設置不同步。",
        "Msg_60035": "您不能禁用下線的產品。",
        "Msg_90012": "您不能執行此操作。",
        "UpdateProductStatusSuccessfully": "產品狀態更新成功"
    };
    
    settingType13Resources["message"] = settingType13Resources["message"] || {};
    settingType13Resources["message"]["th-th"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "เมื่อต้นสายมีการปิดการใช้งาน ดาวน์ไลนก็จะถูกปิดการใช้งานด้วย",
        "DownlineStatusCannotBeChanged": "เมื่อต้นสายไม่มีการเปลี่ยนแปลงสถานะของผลิตภันฑ์ใดๆ ทางดาวน์ไลน์ก็จะไม่มีการเปลี่ยนแปลงด้วย",
        "DownlineStatusIsInvalid": "ผลิตภันฑ์นี้ ไม่เปิดใช้ ให้กับลูกค้าในสายนี้",
        "Msg_60033": "ไม่สามารถแก้ไขสถานะผลิตภัณฑ์ของดาวน์ไลน์ได้ เนื่องจากไม่ได้เปิดการใช้งานผลิตภัณฑ์นี้",
        "Msg_60034": "ไม่สามารถเชื่อมต่อ(Synchronize) ผลิตภันฑ์นี้เนื่องจากบัญชีต้นสายไม่เชื่อมต่อ(Synchronize) การตั้งค่า",
        "Msg_60035": "คุณไม่สามารถปิดการใช้งานสินค้าให้กับดาวไลน์",
        "Msg_90012": "คุณไม่สามารถดำเนินการนี้",
        "UpdateProductStatusSuccessfully": "สถานะผลิตภันฑ์ อัพเดทสำเร็จแล้ว"
    };
    
    settingType13Resources["message"] = settingType13Resources["message"] || {};
    settingType13Resources["message"]["vi-vn"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "Thành viên cấp dưới sẽ không được phép chơi trong trường hợp thành viên cấp trên bị cấm chơi.",
        "DownlineStatusCannotBeChanged": "Trạng thái trò chơi của thành viên cấp dưới không được phép thay đổi nếu thành viên cấp trên không có sự thay đổi.",
        "DownlineStatusIsInvalid": "Trò chơi không được cung cấp cho những khách hàng thuộc nhóm này.",
        "Msg_60033": "Không thể tùy chỉnh trạng thái trò chơi của thành viên cấp dưới bởi vì tài khoản hiện tại không được phép chơi trò này.",
        "Msg_60034": "Không thể đồng bộ hóa trò chơi này bởi vì thiết lập của tài khoản cấp trên hiện chưa được đồng bộ hóa.",
        "Msg_60035": "Bạn không được phép vô hiệu hoá sản phẩm cược của cấp dưới.",
        "Msg_90012": "Bạn không thể thực thi hành động này.",
        "UpdateProductStatusSuccessfully": "Trạng thái trò chơi được cập nhật thành công."
    };

    angular.module("customerSetting").value("settingType13Resources", settingType13Resources);
})();
