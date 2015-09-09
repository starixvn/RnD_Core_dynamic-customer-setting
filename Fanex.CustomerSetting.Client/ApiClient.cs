namespace Fanex.CustomerSetting.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Client.Configurations;
    using RestSharp;

    internal class ApiClient
    {
        public static readonly IRestClient Client = new RestClient(CustomerSettingConfiguration.Settings.ServiceUri);
        private static readonly IAuthenticator Authenticator = new NtlmAuthenticator();
        private static readonly object AuthenticatorCreatingLockObject = new object();

        static ApiClient()
        {
            if (Client.Authenticator == null)
            {
                lock (AuthenticatorCreatingLockObject)
                {
                    if (Client.Authenticator == null)
                    {
                        Client.Authenticator = Authenticator;
                    }
                }
            }
        }
    }
}
