namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Fanex.CustomerSetting.Matrix.Services;
    using Fanex.CustomerSetting.Matrix.Services.Impl;
    using Fanex.CustomerSetting.WebAPI.Models.Request;

    public class CustomerSettingMatrixController : ApiController
    {
        private ICustomerSettingMatrixService _customerSettingMatrixService;

        public CustomerSettingMatrixController() : this(null)
        {
        }

        public CustomerSettingMatrixController(ICustomerSettingMatrixService customerSettingMatrixService)
        {
            _customerSettingMatrixService = customerSettingMatrixService ?? new CustomerSettingMatrixService();
        }

        public HttpResponseMessage Get([FromUri]QuerySettingMatrixRequest request)
        {
            if (request == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Request param cannot be null.");
            }

            var customerSettingMatrix = _customerSettingMatrixService.GetCustomerSettingMatrix(
                request.SettingTypeId,
                request.ProductId,
                request.CustomerName);

            return Request.CreateResponse(HttpStatusCode.OK, customerSettingMatrix);
        }
    }
}
