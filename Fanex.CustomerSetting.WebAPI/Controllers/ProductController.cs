namespace Fanex.CustomerSetting.WebAPI.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;

    public class ProductController : ApiController
    {
        private IProductService _productService;

        public ProductController()
            : this(null)
        {
        }

        public ProductController(IProductService productService)
        {
            _productService = productService ?? new ProductService();
        }

        public HttpResponseMessage GetAllProductNames()
        {
            var productNames = _productService.GetAllProductNames();

            return Request.CreateResponse(HttpStatusCode.OK, productNames);
        }
    }
}