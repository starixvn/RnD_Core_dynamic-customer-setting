namespace Fanex.CustomerSetting.Client40.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Client40.Models;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CustomerSettingAuth : AuthorizeAttribute
    {
        public bool IsUpdate { get; set; }

        public bool IsMultipleUpdate { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            PermissionContext permissionContext = GetPermissionContext();

            permissionContext.AllowUpdate = IsUpdate;

            if (IsMultipleUpdate)
            {
                return CustomerSettingRegistration.CheckMultipleUpdatePermission(permissionContext);
            }
            else
            {
                if (IsUpdate)
                {
                    return CustomerSettingRegistration.CheckUpdatePermission(permissionContext);
                }
                else
                {
                    return CustomerSettingRegistration.CheckQueryPermission(permissionContext);
                }
            }
        }

        private PermissionContext GetPermissionContext()
        {
            SettingContext settingContext = CustomerSettingRegistration.SettingContext;
            var context = new PermissionContext();

            context.ActorId = settingContext.ActorId;
            context.ActorLevel = settingContext.ActorLevel;
            context.ActorName = settingContext.ActorName;
            context.SubAccountId = settingContext.SubAccountId;
            context.Domain = settingContext.Domain;
            context.Language = settingContext.Language;
            context.ClientIP = settingContext.IP;

            return context;
        }
    }
}