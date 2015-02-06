namespace Fanex.CustomerSetting.Demo.Controllers
{
    using System.Web.Mvc;
    using Fanex.CustomerSetting.Demo.Configurations;
    using Fanex.CustomerSetting.Entities;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;

    /// <summary>
    /// CustomerSettingController class.
    /// </summary>
    public class CustomerSettingController : Controller
    {
        #region Variables

        private ICustomerSettingService _service;
        private UserContext _userContext;

        #endregion Variables

        #region Constructors

        public CustomerSettingController()
            : this(null)
        {
        }

        public CustomerSettingController(ICustomerSettingService service)
        {
            _userContext = new UserContext
            {
                CustId = AppSettings.CustId,
                SubAccountId = AppSettings.SubAccountId,
                RoleType = AppSettings.RoleType,
                Language = AppSettings.Language
            };

            _service = service ?? new CustomerSettingService(_userContext.Language);
        }

        #endregion Constructors

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// Select customer setting.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns select JSON result.</returns>
        [HttpPost]
        public JsonResult Select(SelectModel model)
        {
            model.CustId = _userContext.CustId;
            model.RoleId = _userContext.RoleType;
            model.Language = _userContext.Language;
            if (ValidateModel(model))
            {
                var result = _service.CompanySelectSuper(model);
                if (result.IsSuccess && ValidateCustomerInformation(result.CustomerSetting.Information, model))
                {
                    return Json(result);
                }
            }

            var invalidResult = new Result(Result.FailureValue, "No permission");
            return Json(invalidResult);
        }

        [HttpPost]
        public JsonResult Update(UpdateModel model)
        {
            return null;
        }

        /// <summary>
        /// Validates the select model.
        /// </summary>
        /// <param name="model">The select model.</param>
        /// <returns>Returns model validation.</returns>
        private bool ValidateModel(SelectModel model)
        {
            switch (model.FormType)
            {
                case FormType.CompanyEditSuper:
                    if (model.RoleId != RoleType.Company || model.TargetRoleId != RoleType.Super)
                    {
                        return false;
                    }

                    break;

                case FormType.SuperEditMaster:
                    if (model.TargetRoleId != RoleType.Master)
                    {
                        return false;
                    }

                    break;

                case FormType.MasterEditAgent:
                    if (model.TargetRoleId != RoleType.Agent)
                    {
                        return false;
                    }

                    break;

                case FormType.SuperEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return false;
                    }

                    break;

                case FormType.MasterEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return false;
                    }

                    break;

                case FormType.AgentEditMember:
                    if (model.TargetRoleId != RoleType.Member)
                    {
                        return false;
                    }

                    break;

                default:
                    return false;
            }

            return true;
        }

        private bool ValidateCustomerInformation(CustomerInformationEntity customerInformation, SelectModel model)
        {
            if (customerInformation != null)
            {
                return true;
            }

            return false;
        }
    }
}