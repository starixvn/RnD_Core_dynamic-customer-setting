namespace Fanex.CustomerSetting.WebApi.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Web.Hosting;
    using System.Web.Http;
    using System.Web.Http.ModelBinding;
    using Filters;
    using Helpers;
    using Models.Request;
    using Services;
    using Services.Impl;

    [SetLanguageFilter]
    public class CustomerSettingController : ApiController
    {
        private const string CustomerSettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string CustomerSettingTemplateFilePath = "~/App_Data/Templates/customer-setting.htm";
        private const byte CachingHours = 4;
        private readonly ISettingServiceFactory settingServiceFactory;

        public CustomerSettingController(ISettingServiceFactory settingServiceFactory)
        {
            this.settingServiceFactory = settingServiceFactory ?? new SettingServiceFactory();
        }

        public CustomerSettingController() : this(null)
        {
        }

        public HttpResponseMessage PostSettingRequest([FromBody]QuerySettingRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request parameters cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Precondition request is failed.");
            }

            var querySettingService = settingServiceFactory.CreateQueryService(request.System, request.ProductId, request.GetSettingContext());

            try
            {
                var customerSetting = querySettingService.GetCustomerSetting(request.GetCustomerSettingQuery());

                return Request.CreateResponse(HttpStatusCode.OK, customerSetting);
            }
            catch (AccessViolationException accessViolationException)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, accessViolationException.Message);
            }
        }

        public HttpResponseMessage Post(
            [ModelBinder(typeof(UpdateSettingRequestModelBinder<UpdateSettingRequest>))]UpdateSettingRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Update setting request cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Precondition request is failed.");
            }

            var updateSettingService = settingServiceFactory.CreateUpdateService(
                request.System,
                request.Setting.ProductId,
                request.GetSettingContext());

            var result = updateSettingService.Update(request.GetUpdateCustomerSettingCommand());

            return Request.CreateResponse(result);
        }

        public HttpResponseMessage PostMultiple(
            [ModelBinder(typeof(UpdateSettingRequestModelBinder<MultipleUpdateSettingRequest>))]MultipleUpdateSettingRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Multiple update setting request cannot be null.");
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed, "Precondition request is failed.");
            }

            var updateSettingService = settingServiceFactory.CreateUpdateService(
               request.System,
               request.Setting.ProductId,
               request.GetSettingContext());

            var result = updateSettingService.Update(request.GetMultipleUpdateCustomerSettingCommand());

            return Request.CreateResponse(result);
        }

        public HttpResponseMessage GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            string template = string.Empty;
            string customerSettingTemplatePath = CustomerSettingTemplateFilePath;
            string customerSettingTemplateCacheKey = CustomerSettingTemplateCachingKey;

            template = cache.Get(customerSettingTemplateCacheKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = File.ReadAllText(HostingEnvironment.MapPath(customerSettingTemplatePath));
                cache.Set(customerSettingTemplateCacheKey, template, DateTime.Now.AddHours(CachingHours));
            }

            return Request.CreateResponse(HttpStatusCode.OK, template);
        }
    }
}