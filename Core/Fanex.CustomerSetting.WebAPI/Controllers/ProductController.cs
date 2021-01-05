namespace Fanex.CustomerSetting.WebApi.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Services;
    using Services.Impl;

    public class ProductController : ApiController
    {
        private readonly IProductService _productService;

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