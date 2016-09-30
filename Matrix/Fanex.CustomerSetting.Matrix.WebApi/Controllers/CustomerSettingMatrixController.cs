namespace Fanex.CustomerSetting.Matrix.WebApi.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Web.Hosting;
    using System.Web.Http;
    using CustomerSetting.Models.Results;
    using Models;
    using Services;
    using Services.Impl;

    public class CustomerSettingMatrixController : ApiController
    {
        private const string CustomerSettingMatrixTemplateCacheKey = "CustomerSettingMatrixTemplateCacheKey";
        private const string CustomerSettingMatrixTemplateFilePath = "~/App_Data/Templates/customer-setting-matrix.htm";
        private const byte CachingHours = 4;

        private ICustomerSettingMatrixService _customerSettingMatrixService;

        public CustomerSettingMatrixController()
            : this(null)
        {
        }

        public CustomerSettingMatrixController(ICustomerSettingMatrixService customerSettingMatrixService)
        {
            _customerSettingMatrixService = customerSettingMatrixService ?? new CustomerSettingMatrixService();
        }

        public HttpResponseMessage Get([FromUri]SettingMatrixQuery request)
        {
            return GetSettingMatrix(request);
        }

        public HttpResponseMessage Post([FromBody]SettingMatrixQuery request)
        {
            return GetSettingMatrix(request);
        }

        public HttpResponseMessage GetTemplate()
        {
            ObjectCache cache = MemoryCache.Default;
            var template = cache.Get(CustomerSettingMatrixTemplateCacheKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = File.ReadAllText(HostingEnvironment.MapPath(CustomerSettingMatrixTemplateFilePath));
                cache.Set(CustomerSettingMatrixTemplateCacheKey, template, DateTime.Now.AddHours(CachingHours));
            }

            return Request.CreateResponse(HttpStatusCode.OK, template);
        }

        public HttpResponseMessage GetSettingMatrix(SettingMatrixQuery request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request parameter cannot be null.");
            }

            try
            {
                var customerSettingMatrix = _customerSettingMatrixService.GetCustomerSettingMatrix(request);

                return Request.CreateResponse(HttpStatusCode.OK, customerSettingMatrix);
            }
            catch (InvalidOperationException invalidException)
            {
                return Request.CreateResponse(
                    HttpStatusCode.InternalServerError,
                    new Result(Result.DataError.Code, invalidException.Message));
            }
        }
    }
}