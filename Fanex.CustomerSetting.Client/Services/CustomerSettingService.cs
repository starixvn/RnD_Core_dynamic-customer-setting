namespace Fanex.CustomerSetting.Client.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Web;
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
            request.AddParameter("ClientIp", query.ClientIp);
            
            return Client.Execute(request);
        }


        public IRestResponse UpdateCustomerSetting(UpdateSettingRequest update)
        {
            if(update == null)
            {
                throw new ArgumentNullException("update", "UpdateSettingRequest cannot be null");
            }

            var request = new RestRequest("/customersetting/", Method.POST);

            request.AddParameter("Application/Json", 
                "{" 
                + "\"Setting\":" + update.JsonSetting
                + ",\"ProductId\":" + update.ProductId
                + ",\"CustomerLevel\":" + update.CustomerLevel
                + ",\"ActorId\":" + update.ActorId
                + ",\"ActorName\":\"" + update.ActorName + "\""
                + ",\"ActorLevel\":" + update.ActorLevel
                + ",\"SubAccountId\":" + update.SubAccountId
                + ",\"System\":" + CustomerSettingConfiguration.Settings.System
                + ",\"Language\":\"" + update.Language + "\""
                + ",\"Domain\":\"" + update.Domain + "\""
                + ",\"ClientIp\":\"" + update.ClientIp + "\""
                +"}", 
                ParameterType.RequestBody);
            
            return Client.Execute(request);
        }
    }
}
