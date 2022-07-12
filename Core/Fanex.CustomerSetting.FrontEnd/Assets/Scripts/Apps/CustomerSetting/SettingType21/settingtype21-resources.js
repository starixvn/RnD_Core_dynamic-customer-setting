/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var settingType21Resources = {};
    
    settingType21Resources["label"] = settingType21Resources["label"] || {};
    settingType21Resources["label"]["en-us"] = {
        "ProductStatus": "Product Status"
    };
    
    settingType21Resources["label"] = settingType21Resources["label"] || {};
    settingType21Resources["label"]["zh-cn"] = {
        "ProductStatus": "产品状态"
    };
    
    settingType21Resources["label"] = settingType21Resources["label"] || {};
    settingType21Resources["label"]["zh-tw"] = {
        "ProductStatus": "產品狀態"
    };
    
    settingType21Resources["label"] = settingType21Resources["label"] || {};
    settingType21Resources["label"]["th-th"] = {
        "ProductStatus": "สถานะผลิตภันฑ์"
    };
    
    settingType21Resources["label"] = settingType21Resources["label"] || {};
    settingType21Resources["label"]["vi-vn"] = {
        "ProductStatus": "Trạng thái trò chơi"
    };
    
    settingType21Resources["message"] = settingType21Resources["message"] || {};
    settingType21Resources["message"]["en-us"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "Downline cannot be enabled when Upline is disabled.",
        "Msg_60033": "Unable to edit downline&apos;s Product Status because the account disabled for this product.",
        "Msg_60035": "You are not allowed to disable downline&apos;s product.",
        "Msg_90012": "You are not allowed to perform this action.",
        "UpdateProductStatusSuccessfully": "Product Status is updated successfully."
    };
    
    settingType21Resources["message"] = settingType21Resources["message"] || {};
    settingType21Resources["message"]["zh-cn"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "当上线被禁用下线同时也禁用。",
        "Msg_60033": "由于账号的此产品已关闭导致无法更改下线产品状态。",
        "Msg_60035": "您不能禁用下线的产品。",
        "Msg_90012": "您不能执行此操作。",
        "UpdateProductStatusSuccessfully": "产品状态更新成功"
    };
    
    settingType21Resources["message"] = settingType21Resources["message"] || {};
    settingType21Resources["message"]["zh-tw"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "當上線被禁用下線同時也禁用。",
        "Msg_60033": "由於賬號的此產品已關閉導致無法更改下線產品狀態。",
        "Msg_60035": "您不能禁用下線的產品。",
        "Msg_90012": "您不能執行此操作。",
        "UpdateProductStatusSuccessfully": "產品狀態更新成功"
    };
    
    settingType21Resources["message"] = settingType21Resources["message"] || {};
    settingType21Resources["message"]["th-th"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "เมื่อต้นสายมีการปิดการใช้งาน ดาวน์ไลนก็จะถูกปิดการใช้งานด้วย",
        "Msg_60033": "ไม่สามารถแก้ไขสถานะผลิตภัณฑ์ของดาวน์ไลน์ได้ เนื่องจากไม่ได้เปิดการใช้งานผลิตภัณฑ์นี้",
        "Msg_60035": "คุณไม่สามารถปิดการใช้งานสินค้าให้กับดาวไลน์",
        "Msg_90012": "คุณไม่สามารถดำเนินการนี้",
        "UpdateProductStatusSuccessfully": "สถานะผลิตภันฑ์ อัพเดทสำเร็จแล้ว"
    };
    
    settingType21Resources["message"] = settingType21Resources["message"] || {};
    settingType21Resources["message"]["vi-vn"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "Thành viên cấp dưới sẽ không được phép chơi trong trường hợp thành viên cấp trên bị cấm chơi.",
        "Msg_60033": "Không thể tùy chỉnh trạng thái trò chơi của thành viên cấp dưới bởi vì tài khoản hiện tại không được phép chơi trò này.",
        "Msg_60035": "Bạn không được phép vô hiệu hoá sản phẩm cược của cấp dưới.",
        "Msg_90012": "Bạn không thể thực thi hành động này.",
        "UpdateProductStatusSuccessfully": "Trạng thái trò chơi được cập nhật thành công."
    };

    angular.module("customerSetting").value("settingType21Resources", settingType21Resources);
})();
