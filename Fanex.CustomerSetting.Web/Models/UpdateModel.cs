namespace Fanex.CustomerSetting.Web.Models
{
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Web.Services;

    /// <summary>
    /// UpdateModel class.
    /// </summary>
    public class UpdateModel : BaseModel
    {
        public int SubAccountId { get; set; }

        public SettingType SettingType { get; set; }

        public string Ip { get; set; }

        public string StatusString { get; set; }

        public string DiscountString { get; set; }

        public string BetLimitString { get; set; }

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
                SettingType = this.SettingType,
                TargetCustId = this.TargetCustId,
                TargetRoleId = this.TargetRoleId,
                SubAccountId = this.SubAccountId,
                Ip = this.Ip,
                Statuses = modelService.ParseStatus(StatusString, this.FormType),
                Discounts = modelService.ParseDiscount(DiscountString, this.FormType),
                BetLimits = modelService.ParseBetLimit(BetLimitString, this.FormType),
                PositionTakings = modelService.ParsePositionTaking(PositionTakingString, this.FormType)
            };
        }
    }
}