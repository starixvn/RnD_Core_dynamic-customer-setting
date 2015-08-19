namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using Fanex.CustomerSetting.Client.Configurations;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class CustomerSettingService : ICustomerSettingService
    {
        private static readonly IRestClient Client = new RestClient(CustomerSettingConfiguration.Settings.ServiceUri);
        private static readonly IAuthenticator Authenticator = new NtlmAuthenticator();
        private static readonly object AuthenticatorCreatingLockObject = new object();

        public CustomerSettingService()
        {
            SetAuthenticator();
        }

        public IRestResponse QueryCustomerSetting(QuerySettingRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersetting/", Method.GET);

            request.AddObject(query);

            return Client.Execute(request);
        }

        public IRestResponse UpdateCustomerSetting(UpdateSettingRequest update)
        {
            if (update == null)
            {
                throw new ArgumentNullException("update", "UpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/post/", Method.POST);

            request.AddJsonBody(update);

            return Client.Execute(request);
        }

        public IRestResponse MultipleUpdateCustomerSetting(MultipleUpdateSettingRequest multipleUpdate)
        {
            if (multipleUpdate == null)
            {
                throw new ArgumentNullException("multipleUpdate", "MultipleUpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/postmultiple/", Method.POST);

            request.AddJsonBody(multipleUpdate);

            return Client.Execute(request);
        }

        private void SetAuthenticator()
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