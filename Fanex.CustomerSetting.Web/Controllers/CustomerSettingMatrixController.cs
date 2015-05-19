namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Services;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingMatrixController : Controller
    {
        private readonly IPositionTakingMatrixService _positionTakingMatrixService;

        public CustomerSettingMatrixController()
            : this(null)
        {
        }

        public CustomerSettingMatrixController(IPositionTakingMatrixService positionTakingMatrixService)
        {
            _positionTakingMatrixService = positionTakingMatrixService ?? new PositionTakingMatrixService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var rootUrl = Request.ApplicationPath;
            if (rootUrl.EndsWith("/", StringComparison.CurrentCulture))
            {
                rootUrl = rootUrl.Substring(0, rootUrl.Length - 1);
            }

            var filePath = Server.MapPath("~/app/customerSettingMatrix/customer-setting-matrix-page.htm");
            var htmlContent = System.IO.File.ReadAllText(filePath);
            htmlContent = htmlContent.Replace("{{rootUrl}}", rootUrl);
            return Content(htmlContent, "text/HTML");
        }

        [HttpPost]
        public ActionResult PositionTaking(int productId, string userName)
        {
            var queryInfo = new QueryInfo
            {
                ProductId = productId,
                UserName = userName
            };

            var positionTakingMatrix = _positionTakingMatrixService.Query(queryInfo);
            return Json(positionTakingMatrix);
        }
    }
}