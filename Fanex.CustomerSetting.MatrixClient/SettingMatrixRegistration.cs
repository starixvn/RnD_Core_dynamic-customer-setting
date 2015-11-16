namespace Fanex.CustomerSetting.MatrixClient
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Fanex.CustomerSetting.MatrixClient.Exceptions;
    using Fanex.CustomerSetting.MatrixClient.Helpers;

    public abstract class SettingMatrixRegistration
    {
        private static SettingMatrixRegistration client;

        public static void Init()
        {
            List<Type> customerSettingClients = TypeDetection.FilterTypesInAssemblies(IsCustomerSettingClient).ToList();

            if (customerSettingClients.Count == 0)
            {
                throw new SettingMatrixException("A class extending from CustomerSettingMatrixRegistration is required");
            }
            else if (customerSettingClients.Count == 1)
            {
                Type clientType = customerSettingClients[0];

                client = Activator.CreateInstance(clientType) as SettingMatrixRegistration;
                client.RegisterRouteConfig(RouteTable.Routes);
            }
            else
            {
                throw new SettingMatrixException("Only one class extending from CustomerSettingMatrixRegistration is allowed");
            }
        }

        internal static bool CheckPermission()
        {
            if (client == null)
            {
                throw new SettingMatrixException("The CustomerSettingMatrixRegistration.Init() method must be called once the web application is started.");
            }

            return client.VerifyPermission();
        }

        public abstract bool VerifyPermission();

        public virtual void RegisterRouteConfig(RouteCollection routes)
        {
            routes.MapRoute(
                name: "DCSMatrix_Routing",
                url: "DCSMatrix/ViewCustomerSettingMatrix/{action}/{id}",
                defaults: new { controller = "ViewCustomerSettingMatrix", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Fanex.CustomerSetting.MatrixClient.Controllers" });
        }

        private static bool IsCustomerSettingClient(Type type)
        {
            return typeof(SettingMatrixRegistration).IsAssignableFrom(type) && type.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}