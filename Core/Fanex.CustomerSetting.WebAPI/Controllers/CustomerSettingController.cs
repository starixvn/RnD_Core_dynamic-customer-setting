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
    using Fanex.CustomerSetting.Models.Results;
    using Filters;
    using Helpers;
    using Models.Request;
    using Services;
    using Services.Impl;
    using System.Linq;
    using Fanex.CustomerSetting.Models;
    using System.Collections.Generic;

    [SetLanguageFilter]
    public class CustomerSettingController : ApiController
    {
        private const string CustomerSettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string CustomerSettingTemplateFilePath = "~/App_Data/Templates/customer-setting.htm";
        private const byte CachingHours = 4;
        private readonly ISettingServiceFactory settingServiceFactory;
        private readonly IUMService umService;

        public CustomerSettingController(ISettingServiceFactory settingServiceFactory, IUMService umService)
        {
            this.settingServiceFactory = settingServiceFactory ?? new SettingServiceFactory();
            this.umService = umService ?? new UMService();
        }

        public CustomerSettingController() : this(null, null)
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
            if (result.GetType().Equals(typeof(UpdateSettingResult)))
            {
                result = AddUMMessage(result, request);
            }

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
            if (result.GetType().Equals(typeof(UpdateSettingResult)))
            {
                result = AddUMMessage(result, request);
            }

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

        private bool IsPartialUpdate(IEnumerable<SettingType> settingTypes) => settingTypes.Count() == 1;

        private IResult AddUMMessage(IResult result, UpdateSettingRequest request)
        {
            var updateSettingResult = (UpdateSettingResult)result;
            var umResult = this.umService.GetUMResult(request.Setting.CustomerId, request.Setting.ProductId, IsPartialUpdate(request.Setting.SettingTypes) ? request.Setting.SettingTypes.FirstOrDefault()?.Id : null, request.Language);
            if (umResult != null) updateSettingResult.AddSubResult(umResult);
            updateSettingResult.RemoveNoChangeByUMSubResults();
            return updateSettingResult;
        }
    }
}