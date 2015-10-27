namespace Fanex.CustomerSetting.Client40
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Helpers;
    using RestSharp;
    using RestSharp.Authenticators;

    public static class ApiClient
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly IRestClient Client;
        private static readonly object RestClientCreatingLockObject = new object();

        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "Reviewed!")]
        static ApiClient()
        {
            if (Client == null)
            {
                lock (RestClientCreatingLockObject)
                {
                    if (Client == null)
                    {
                        Client = new RestClient(RandomHelper.GetRandomUri(CustomerSettingConfiguration.Settings.ServiceUris));
                        Client.Authenticator = new HmacAuthenticator();
                    }
                }
            }
        }
    }
}