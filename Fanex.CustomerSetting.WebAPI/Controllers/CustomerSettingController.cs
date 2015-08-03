namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.WebAPI.Models;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;


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

    public class UpdateCustomerSettingCommandEx
    {
        public byte ProductId { get; set; }

        public int CustomerId { get; set; }

        public byte CustomerLevel { get; set; }

        public byte? UplineLevel { get; set; }

        public int ActorId { get; set; }

        public string ActorName { get; set; }

        public byte ActorLevel { get; set; }

        public int SubAccountId { get; set; }

        public byte SystemId { get; set; }

        public ISetting Setting { get; set; }
    }

    public class CustomerSettingController : ApiController
    {
        public HttpResponseMessage Get([FromUri] CustomerSettingQueryEx request)
        {
            var settingContext = new SettingContext();
            var actor = new User
            {
                Id = request.ActorId,
                Name = request.ActorName,
                Level = Enumeration.FromValue<Level>(request.ActorLevel),
                SubAccountId = request.SubAccountId
            };

            settingContext.Actor = actor;
            settingContext.System = request.SystemId;

            IQuerySettingService querySettingService = new QuerySettingService(settingContext);

            var query = new CustomerSettingQuery(
              request.CustomerId,
              request.CustomerLevel,
              request.ProductId,
              null);

            var customerSetting = querySettingService.GetCustomerSetting(query);

            var json = JsonConvert.SerializeObject(customerSetting);

           

            //var b = (Setting)setting;

            return Request.CreateResponse(HttpStatusCode.OK, json, "text/json");

        }

        public HttpResponseMessage Post(UpdateCustomerSettingCommandEx command)
        {
            var settingContext = new SettingContext();
            var actor = new User
            {
                Id = command.ActorId,
                Name = command.ActorName,
                Level = Enumeration.FromValue<Level>(command.ActorLevel),
                SubAccountId = command.SubAccountId
            };

            settingContext.Actor = actor;
            settingContext.System = command.SystemId;

            IUpdateSettingService service = new UpdateSettingService(settingContext);
            var result = service.Update(new UpdateCustomerSettingCommand { 
                UplineLevel = Enumeration.FromValue<Level>( command.UplineLevel.Value), 
                Setting = command.Setting });

            return Request.CreateResponse(result);
        }
    }
}
