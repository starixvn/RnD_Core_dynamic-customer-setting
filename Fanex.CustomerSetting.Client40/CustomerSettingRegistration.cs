namespace Fanex.CustomerSetting.Client40
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Routing;
    using Fanex.CustomerSetting.Client40.Helpers;
    using Fanex.CustomerSetting.Client40.Models;

    public abstract class CustomerSettingRegistration
    {
        private static CustomerSettingRegistration client;

        public static SettingContext SettingContext
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

            if (customerSettingClients.Count > 0)
            {
                Type clientType = customerSettingClients[0];

                client = Activator.CreateInstance(clientType) as CustomerSettingRegistration;
                client.RegisterRouteConfig(RouteTable.Routes);
            }
        }

        public static bool HasPermission(CheckingPermissionContext checkingPermissionContext)
        {
            if (client != null)
            {
                return client.CheckPermission(checkingPermissionContext);
            }

            return true;
        }

        public abstract bool CheckPermission(CheckingPermissionContext checkingPermissionContext);

        public abstract SettingContext FetchSettingContext();

        public abstract void RegisterRouteConfig(RouteCollection routes);

        private static bool IsCustomerSettingClient(Type type)
        {
            return typeof(CustomerSettingRegistration).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}