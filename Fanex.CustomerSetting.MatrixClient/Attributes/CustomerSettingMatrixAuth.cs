namespace Fanex.CustomerSetting.MatrixClient.Attributes
{
    using System;
    using System.Web;
    using System.Web.Mvc;

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public sealed class CustomerSettingMatrixAuth : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return SettingMatrixRegistration.CheckPermission();
        }
    }
}