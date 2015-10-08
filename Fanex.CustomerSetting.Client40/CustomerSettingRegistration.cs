namespace Fanex.CustomerSetting.Client40
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.Client40.Helpers;
    using Fanex.CustomerSetting.Client40.Models;

    public abstract class CustomerSettingRegistration
    {
        private static CustomerSettingRegistration client;

        internal static SettingContext SettingContext
        {
            get
            {
                if (client != null)
                {
                    return client.FetchSettingContext();
                }

                return new SettingContext();
            }
        }

        public static void Init()
        {
            List<Type> customerSettingClients = TypeDetection.FilterTypesInAssemblies(IsCustomerSettingClient).ToList();

            if (customerSettingClients.Count == 0)
            {
                throw new InvalidOperationException("A class extending from CustomerSettingRegistration is required");
            }
            else if (customerSettingClients.Count == 1)
            {
                Type clientType = customerSettingClients[0];

                client = Activator.CreateInstance(clientType) as CustomerSettingRegistration;
                client.RegisterRouteConfig(RouteTable.Routes);
            }
            else
            {
                throw new InvalidOperationException("Only one class extending from CustomerSettingRegistration is allowed");
            }
        }

        internal static bool CheckQueryPermission(PermissionContext checkingPermissionContext)
        {
            CheckInitializationState();

            return client.VerifyQueryPermission(checkingPermissionContext);
        }

        internal static bool CheckUpdatePermission(PermissionContext checkingPermissionContext)
        {
            CheckInitializationState();

            return client.VerifyUpdatePermission(checkingPermissionContext);
        }

        internal static bool CheckMultipleUpdatePermission(PermissionContext checkingPermissionContext)
        {
            CheckInitializationState();

            return client.VerifyMultipleUpdatePermission(checkingPermissionContext);
        }

        public abstract bool VerifyQueryPermission(PermissionContext permissionContext);

        public abstract bool VerifyUpdatePermission(PermissionContext permissionContext);

        public abstract bool VerifyMultipleUpdatePermission(PermissionContext permissionContext);

        public abstract SettingContext FetchSettingContext();

        public virtual void RegisterRouteConfig(RouteCollection routes)
        {
            routes.MapRoute(
                name: "DCS_default",
                url: "DCS/{controller}/{action}/{id}",
                defaults: new { controller = "ViewCustomerSetting", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.CustomerSetting.Client40.Controllers" });
        }

        private static bool IsCustomerSettingClient(Type type)
        {
            return typeof(CustomerSettingRegistration).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null;
        }

        private static void CheckInitializationState()
        {
            if (client == null)
            {
                throw new InvalidOperationException("The CustomerSettingRegistration.Init() method must be called once the web application is started.");
            }
        }

        internal static PermissionContext GetPermissionContext()
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