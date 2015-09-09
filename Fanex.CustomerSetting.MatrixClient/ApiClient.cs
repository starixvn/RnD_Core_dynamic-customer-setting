namespace Fanex.CustomerSetting.MatrixClient
{
    using Fanex.CustomerSetting.MatrixClient.Configurations;
    using RestSharp;

    internal class ApiClient
    {
        public static readonly IRestClient Client = new RestClient(SettingMatrixConfiguration.Settings.ServiceUri);
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