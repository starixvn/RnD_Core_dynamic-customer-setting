/*jshint -W101,-W106,-W044,-W098*/

(function () {
    "use strict";

    var productStatusResources = {};
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["en-us"] = {
        "Enable": "Enable",
        "EnableDownline": "Enable Downline",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["zh-cn"] = {
        "Enable": "启用",
        "EnableDownline": "启用下线",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["zh-tw"] = {
        "Enable": "啟用",
        "EnableDownline": "啟用下線",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["ko-kr"] = {
        "Enable": "활성화",
        "EnableDownline": "다운라인 활성화하기",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["ja-jp"] = {
        "Enable": "Enable"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["th-th"] = {
        "Enable": "เปิดใช้งาน",
        "EnableDownline": "เปิดใช้งาน ดาวไลน์",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["label"] = productStatusResources["label"] || {};
    productStatusResources["label"]["vi-vn"] = {
        "Enable": "Cho phép",
        "EnableDownline": "Enable Downline",
        "ProductStatus": "Product Status"
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["en-us"] = {
        "DownlineCannotBeEnabledWhenUplineIsDisabled": "Downline cannnot be enabled when Upline is disabled.",
        "DownlineProductStatusCannotBeNull": "Downline Product Status cannot be null.",
        "DownlineStatusCannotBeChanged": "Downline&apos;s Product Status must be unchanged when the Upline has not been changed.",
        "DownlineStatusIsInvalid": "The product is not offered for this line of customers",
        "Msg_60031": "Unable to edit downline&apos;s Product Status because the account disabled for this product.",
        "Msg_60032": "Unable to synchronize this product because the upline account setting is not synchronized.",
        "UpdateProductStatusSuccessfully": "Updated product status successfully."
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["zh-cn"] = {
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["zh-tw"] = {
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["ko-kr"] = {
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["ja-jp"] = {
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["th-th"] = {
    };
    
    productStatusResources["message"] = productStatusResources["message"] || {};
    productStatusResources["message"]["vi-vn"] = {
    };

    angular.module("customerSetting").value("productStatusResources", productStatusResources);
})();
