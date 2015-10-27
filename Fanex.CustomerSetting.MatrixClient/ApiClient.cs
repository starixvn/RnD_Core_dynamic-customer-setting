namespace Fanex.CustomerSetting.MatrixClient
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.MatrixClient.Configurations;
    using Fanex.CustomerSetting.MatrixClient.Helpers;
    using RestSharp;
    using RestSharp.Authenticators;

    public class ApiClient
    {
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "Reviewed!")]
        public static readonly IRestClient Client;
        private static readonly object RestClientCreatingLockObject = new object();

        static ApiClient()
        {
            if (Client == null)
            {
                lock (RestClientCreatingLockObject)
                {
                    if (Client == null)
                    {
                        Client = new RestClient(RandomHelper.GetRandomUri(SettingMatrixConfiguration.Settings.ServiceUris));
                        Client.Authenticator = new HmacAuthenticator();
                    }
                }
            }
        }
    }
}