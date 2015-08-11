namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.WebAPI.JsonHelpers;
    using Fanex.CustomerSetting.WebAPI.Models;
    using Fanex.CustomerSetting.WebAPI.Models.Request;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class CustomerSettingController : ApiController
    {
        public HttpResponseMessage Get([FromUri]QuerySettingRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request param cannot be null.");
            }

            if (ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Precondition request is failed.");
            }

            var querySettingService = new QuerySettingService(request.GetSettingContext());
            var customerSetting = querySettingService.GetCustomerSetting(request.GetCustomerSettingQuery());

            return Request.CreateResponse(HttpStatusCode.OK, customerSetting);
        }

        public HttpResponseMessage Post(HttpRequestMessage request)
        {
            string jsonContent = request.Content.ReadAsStringAsync().Result;

            var updateSettingRequest = JsonConvert.DeserializeObject<UpdateSettingRequest>(
                jsonContent,
                new SettingItemConverter(),
                new LevelConverter());

            IUpdateSettingService service = new UpdateSettingService(updateSettingRequest.GetSettingContext());

            var updateSettingResult = service.Update(new UpdateCustomerSettingCommand
            {
                Setting = updateSettingRequest.Setting,
                UplineLevel = (updateSettingRequest.UplineLevel == null)
                ? null
                : Enumeration.FromValue<Level>(updateSettingRequest.UplineLevel.Value)
            });

            return Request.CreateResponse(updateSettingResult);
        }
    }
}
