namespace Fanex.CustomerSetting.Demo
{
    using System.Web.Mvc;

    /// <summary>
    /// FilterConfig class.
    /// </summary>
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}