namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Collections;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Web.Contexts;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;

    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }


    public class CustomerSettingQueryEx
    {
        public byte ProductId { get; set; }

        public int CustomerId { get; set; }

        public byte CustomerLevel { get; set; }

        public byte UplineLevel { get; set; }

        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public byte ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        public byte SystemId { get; set; }
    }


    public class CustomerSettingController : ApiController
    {
        public HttpResponseMessage Get([FromUri] CustomerSettingQueryEx request)
        {
            SettingContextManager.Init(
                request.ActorId,
                request.ActorName,
                request.ActorLevel,
                request.SystemId,
                request.SubAccountId);

            IQuerySettingService querySettingService = new QuerySettingService(SettingContextManager.SettingContext);

            CustomerSettingQuery query = new CustomerSettingQuery(
              request.CustomerId,
              request.CustomerLevel,
              request.ProductId,
              request.UplineLevel);

            var customerSetting = querySettingService.GetCustomerSetting(query);

            return Request.CreateResponse<ISetting>(HttpStatusCode.OK, customerSetting);
        }
    }
}
