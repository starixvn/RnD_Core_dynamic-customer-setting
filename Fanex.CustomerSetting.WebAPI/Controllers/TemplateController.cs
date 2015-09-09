namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.Caching;
    using System.Text.RegularExpressions;
    using System.Web.Hosting;
    using System.Web.Http;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

    public class TemplateController : ApiController
    {
        private const string CustomerSettingTemplateCachingKey = "CustomerSettingTemplate";
        private const string CustomerSettingTemplateFilePath = "~/App_Data/Templates/customer-setting.htm";
        private const string CustomerSettingTemplateIe8CachingKey = "CustomerSettingIE8Template";
        private const string CustomerSettingTemplateIe8FilePath = "~/App_Data/Templates/customer-setting-ie8.htm";
        private const string CustomerSettingMatrixTemplateCacheKey = "CustomerSettingMatrixTemplateCacheKey";
        private const string CustomerSettingMatrixTemplateFilePath = "~/App_Data/Templates/customer-setting-matrix.htm";
        private const byte CachingHours = 4;

        public HttpResponseMessage GetCustomerSettingTemplate([FromUri]QueryTemplateRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request param cannot be null.");
            }

            var customerSettingTemplatePath = CustomerSettingTemplateFilePath;
            var customerSettingTemplateCacheKey = CustomerSettingTemplateCachingKey;
            if (request.IsIe8)
            {
                customerSettingTemplatePath = CustomerSettingTemplateIe8FilePath;
                customerSettingTemplateCacheKey = CustomerSettingTemplateIe8CachingKey;
            }

            string template = GetTemplate(
                customerSettingTemplatePath,
                customerSettingTemplateCacheKey,
                request);

            return Request.CreateResponse(HttpStatusCode.OK, template);
        }

        public HttpResponseMessage GetCustomerSettingMatrixTemplate([FromUri]QueryTemplateRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request param cannot be null.");
            }

            string template = GetTemplate(
                CustomerSettingMatrixTemplateFilePath,
                CustomerSettingMatrixTemplateCacheKey,
                request);

            return Request.CreateResponse(HttpStatusCode.OK, template);
        }

        private string GetTemplate(string templateFilePath, string templateCacheKey, QueryTemplateRequest queryTemplateRequest)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedKey = templateCacheKey;

            var template = cache.Get(cachedKey) as string;

            if (string.IsNullOrEmpty(template))
            {
                template = Regex.Replace(File.ReadAllText(HostingEnvironment.MapPath(templateFilePath)), @"[\r\n\t ]+", " ");
                template = template.Replace("{{debug}}", queryTemplateRequest.IsDebug.ToString().ToLowerInvariant());
                template = template.Replace("{{version}}", queryTemplateRequest.ScriptVersion);
                template = template.Replace("{{min}}", queryTemplateRequest.IsDebug ? string.Empty : ".min");

                cache.Set(cachedKey, template, DateTime.Now.AddHours(CachingHours));
            }

            return template;
        }
    }
}