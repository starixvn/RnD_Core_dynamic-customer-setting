namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using Fanex.CustomerSetting.Client.Configurations;
    using Fanex.CustomerSetting.Client.Requests;
    using RestSharp;

    public class CustomerSettingService : ICustomerSettingService
    {
        private static readonly RestClient Client =
            new RestClient(CustomerSettingConfiguration.Settings.ServiceUri);

        public IRestResponse QueryCustomerSetting(QuerySettingRequest query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var request = new RestRequest("/customersetting/", Method.GET);

            request.AddParameter("ProductId", query.ProductId);
            request.AddParameter("CustomerId", query.CustomerId);
            request.AddParameter("CustomerLevel", query.CustomerLevel);
            request.AddParameter("ActorId", query.ActorId);
            request.AddParameter("ActorName", query.ActorName);
            request.AddParameter("ActorLevel", query.ActorLevel);
            request.AddParameter("SubAccountId", query.SubAccountId);
            request.AddParameter("System", CustomerSettingConfiguration.Settings.System);
            request.AddParameter("Language", query.Language);
            request.AddParameter("Domain", query.Domain);
            request.AddParameter("ClientIP", query.ClientIP);

            return Client.Execute(request);
        }

        public IRestResponse UpdateCustomerSetting(UpdateSettingRequest update)
        {
            if (update == null)
            {
                throw new ArgumentNullException("update", "UpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/post/", Method.POST);

            request.AddParameter(
                "application/json", 
                "{" +
                    "\"Setting\":" + update.JsonSetting +
                    ",\"UplineLevel\":" + update.UplineLevel + 

                    ",\"ActorId\":" + update.ActorId + 
                    ",\"ActorName\":\"" + update.ActorName + "\"" + 
                    ",\"ActorLevel\":" + update.ActorLevel + 
                    ",\"SubAccountId\":" + update.SubAccountId + 
                    ",\"System\":" + CustomerSettingConfiguration.Settings.System + 
                    ",\"Language\":\"" + update.Language + "\"" + 
                    ",\"Domain\":\"" + update.Domain + "\"" +
                    ",\"ClientIP\":\"" + update.ClientIP + "\"" + 
                "}",
                ParameterType.RequestBody);

            return Client.Execute(request);
        }

        public IRestResponse MultipleUpdateCustomerSetting(MultipleUpdateSettingRequest multipleUpdate)
        {
            if (multipleUpdate == null)
            {
                throw new ArgumentNullException("multipleUpdate", "MultipleUpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/postmultiple/", Method.POST);

            request.AddParameter(
                "application/json",
                "{"
                + "\"Setting\":" + multipleUpdate.JsonSetting
                + ",\"UplineLevel\":" + multipleUpdate.UplineLevel
                + ",\"CustomerIds\":\"" + multipleUpdate.CustomerIds + "\""
                + ",\"CustomerNames\":\"" + multipleUpdate.CustomerNames + "\""

                + ",\"ActorId\":" + multipleUpdate.ActorId
                + ",\"ActorName\":\"" + multipleUpdate.ActorName + "\""
                + ",\"ActorLevel\":" + multipleUpdate.ActorLevel
                + ",\"SubAccountId\":" + multipleUpdate.SubAccountId
                + ",\"System\":" + CustomerSettingConfiguration.Settings.System
                + ",\"Language\":\"" + multipleUpdate.Language + "\""
                + ",\"Domain\":\"" + multipleUpdate.Domain + "\""
                + ",\"ClientIP\":\"" + multipleUpdate.ClientIP + "\""
                + "}",
                ParameterType.RequestBody);

            return Client.Execute(request);
        }
    }
}