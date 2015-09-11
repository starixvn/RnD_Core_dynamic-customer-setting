namespace Fanex.CustomerSetting.Client40
{
    using Fanex.CustomerSetting.Client40.Configurations;
    using RestSharp;
    using RestSharp.Authenticators;

    public static class ApiClient
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