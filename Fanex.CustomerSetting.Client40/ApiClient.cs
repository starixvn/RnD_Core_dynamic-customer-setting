namespace Fanex.CustomerSetting.Client40
{
    using System;
    using System.Linq;
    using Fanex.CustomerSetting.Client40.Configurations;
    using Fanex.CustomerSetting.Client40.Helpers;
    using RestSharp;
    using RestSharp.Authenticators;

    public static class ApiClient
    {
        public static readonly IRestClient Client;
        private static readonly IAuthenticator Authenticator = new NtlmAuthenticator();
        private static readonly object RestClientCreatingLockObject = new object();

        static ApiClient()
        {
            if (Client == null)
            {
                lock (RestClientCreatingLockObject)
                {
                    if (Client == null)
                    {
                        Client = new RestClient(RandomHelper.GetRandomUri(CustomerSettingConfiguration.Settings.ServiceUris));
                        Client.Authenticator = Authenticator;
                    }
                }
            }
        }
    }
}