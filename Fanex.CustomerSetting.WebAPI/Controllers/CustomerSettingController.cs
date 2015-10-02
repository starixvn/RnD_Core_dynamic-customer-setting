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
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.CustomerSetting.WebApi.Filters;
    using Fanex.CustomerSetting.WebApi.Helpers;
    using Fanex.CustomerSetting.WebApi.Models.Request;

    [SetLanguage]
    public class CustomerSettingController : ApiController
    {
        private const string CustomerSettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string CustomerSettingTemplateFilePath = "~/App_Data/Templates/customer-setting.htm";
        private const string CustomerSettingTemplateIe8CachingKey = "CustomerSettingIE8Template";
        private const string CustomerSettingTemplateIe8FilePath = "~/App_Data/Templates/customer-setting-ie8.htm";
        private const byte CachingHours = 4;

        public HttpResponseMessage Get([FromUri]QuerySettingRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request parameters cannot be null.");
            }

            var querySettingService = new QuerySettingService(request.GetSettingContext());

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

            var updateSettingService = new UpdateSettingService(request.GetSettingContext());
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

            var updateSettingService = new UpdateSettingService(request.GetSettingContext());
            var result = updateSettingService.Update(request.GetMultipleUpdateCustomerSettingCommand());

            return Request.CreateResponse(result);
        }

        public HttpResponseMessage GetTemplate([FromUri]bool isIe8)
        {
            ObjectCache cache = MemoryCache.Default;
            string template = string.Empty;
            string customerSettingTemplatePath = CustomerSettingTemplateFilePath;
            string customerSettingTemplateCacheKey = CustomerSettingTemplateCachingKey;

            if (isIe8)
            {
                customerSettingTemplatePath = CustomerSettingTemplateIe8FilePath;
                customerSettingTemplateCacheKey = CustomerSettingTemplateIe8CachingKey;
            }

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