namespace Fanex.CustomerSetting.Matrix.WebApi.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;

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

        public HttpResponseMessage GetAll()
        {
            var productNames = _productService.GetAllProducts();

            return Request.CreateResponse(HttpStatusCode.OK, productNames);
        }
    }
}