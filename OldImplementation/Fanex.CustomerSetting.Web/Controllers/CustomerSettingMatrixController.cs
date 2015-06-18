namespace Fanex.CustomerSetting.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.CustomerSetting.Matrix.Enumerations;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Services;

    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CustomerSettingMatrixController : Controller
    {
        private readonly IPositionTakingMatrixService _positionTakingMatrixService;
        private readonly ICommissionMatrixService _commissionMatrixService;

        public CustomerSettingMatrixController()
            : this(null, null)
        {
        }

        public CustomerSettingMatrixController(
            IPositionTakingMatrixService positionTakingMatrixService,
            ICommissionMatrixService commissionMatrixService)
        {
            _positionTakingMatrixService = positionTakingMatrixService ?? new PositionTakingMatrixService();
            _commissionMatrixService = commissionMatrixService ?? new CommissionMatrixService();
        }

        [HttpGet]
        [OutputCache(Duration = 14400)]
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
        public ActionResult Query(QueryInfo queryInfo)
        {
            switch (queryInfo.SettingType)
            {
                case SettingType.PositionTaking:
                    return QueryPositionTaking(queryInfo);

                case SettingType.Commission:
                    return QueryCommission(queryInfo);

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }

        private ActionResult QueryPositionTaking(QueryInfo queryInfo)
        {
            var positionTakingMatrix = _positionTakingMatrixService.Query(queryInfo);
            return Json(positionTakingMatrix);
        }

        private ActionResult QueryCommission(QueryInfo queryInfo)
        {
            var commissionMatrix = _commissionMatrixService.Query(queryInfo);
            return Json(commissionMatrix);
        }
    }
}