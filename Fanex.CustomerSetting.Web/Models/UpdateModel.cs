namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Web.Services;
    using Fanex.CustomerSetting.Web.Utility;

    /// <summary>
    /// UpdateModel class.
    /// </summary>
    public class UpdateModel : BaseModel
    {
        public UpdateModel()
        {
            Ip = RequestHelpers.GetClientIp();
        }

        public string UserName { get; set; }

        public bool IsMultiple
        {
            get
            {
                return false;
            }
        }

        public int SubAccountId { get; set; }

        public SettingType SettingType { get; set; }

        public string Ip { get; private set; }

        public SettingLevel StatusLevel { get; set; }

        public string StatusString { get; set; }

        public SettingLevel DiscountLevel { get; set; }

        public string DiscountString { get; set; }

        public SettingLevel BetLimitLevel { get; set; }

        public string BetLimitString { get; set; }

        public SettingLevel PositionTakingLevel { get; set; }

        public string PositionTakingString { get; set; }

        /// <summary>
        /// Creates UpdateInfo.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="modelService">The model service.</param>
        /// <returns>Returns UpdateInfo.</returns>
        public UpdateInfo CreateUpdateInfo(UserContext userContext, IModelService modelService)
        {
            return new UpdateInfo
            {
                CustId = userContext.CustId,
                RoleId = userContext.RoleId,
                Language = userContext.Language,
                ProductId = this.ProductId,
                FormType = this.FormType,
                UserName = this.UserName,
                IsMultiple = this.IsMultiple,
                SettingType = this.SettingType,
                TargetCustId = this.TargetCustId,
                TargetRoleId = this.TargetRoleId,
                SubAccountId = this.SubAccountId,
                Ip = this.Ip,
                StatusLevel = this.StatusLevel,
                Statuses = modelService.ParseStatus(StatusString, this.FormType),
                DiscountLevel = this.DiscountLevel,
                Discounts = modelService.ParseDiscount(DiscountString, this.FormType),
                BetLimitLevel = this.BetLimitLevel,
                BetLimits = modelService.ParseBetLimit(BetLimitString, this.FormType),
                PositionTakingLevel = this.PositionTakingLevel,
                PositionTakings = modelService.ParsePositionTaking(PositionTakingString, this.FormType)
            };
        }
    }
}