namespace Fanex.CustomerSetting.MatrixClient
{
    using System;
    using System.Linq;
    using Fanex.CustomerSetting.MatrixClient.Configurations;
    using Fanex.CustomerSetting.MatrixClient.Helpers;
    using RestSharp;
    using RestSharp.Authenticators;

    public class ApiClient
    {
        public static readonly IRestClient Client;
        private static readonly IAuthenticator Authenticator = new NtlmAuthenticator();
        private static readonly object RestClientCreatingLockObject = new object();
        private static Random _randomHandler = new Random();

        static ApiClient()
        {
            if (Client == null)
            {
                lock (RestClientCreatingLockObject)
                {
                    if (Client == null)
                    {
                        Client = new RestClient(RandomHelper.GetRandomUri(SettingMatrixConfiguration.Settings.ServiceUris));
                        Client.Authenticator = Authenticator;
                    }
                }
            }
        }
    }
}