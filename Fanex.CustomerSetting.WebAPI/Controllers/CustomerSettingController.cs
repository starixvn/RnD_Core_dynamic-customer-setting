namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Web.Http;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.WebAPI.Models;
    using Fanex.CustomerSetting.WebAPI.Models.Request;
    using Newtonsoft.Json;

    public class CustomerSettingController : ApiController
    {
        public Setting Get([FromUri] QuerySettingRequest request)
        {
            var settingContext = new SettingContext(request);

            IQuerySettingService querySettingService = new QuerySettingService(settingContext);

            var query = new CustomerSettingQuery(
              request.CustomerId,
              request.CustomerLevel,
              request.ProductId,
              request.UplineLevel);

            var customerSetting = querySettingService.GetCustomerSetting(query);
         
            return customerSetting;
        }

        public HttpResponseMessage Post(UpdateSettingRequest request)
        {
            //// var json = JsonConvert.SerializeObject(customerSetting, Formatting.Indented);

            ////Setting s = JsonConvert.DeserializeObject<Setting>(
            ////    json,
            ////    new SettingItemConverter(),
            ////    new LevelConverter());
            var settingContext = new SettingContext(request);
            
            IUpdateSettingService service = new UpdateSettingService(settingContext);
            var result = service.Update(new UpdateCustomerSettingCommand
            {
                UplineLevel = Enumeration.FromValue<Level>(request.UplineLevel.Value),
                Setting = request.Setting
            });

            return Request.CreateResponse(result);
        }
    }
}
