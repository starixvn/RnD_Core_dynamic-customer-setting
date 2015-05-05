namespace Fanex.CustomerSetting.Web.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Web.Services;
    using Fanex.CustomerSetting.Web.Utility;
    using Newtonsoft.Json;

    /// <summary>
    /// CommandModel class.
    /// </summary>
    public class CommandModel : BaseModel
    {
        public CommandModel()
        {
            Ip = RequestHelpers.GetClientIp();
        }

        public string TargetCustIds { get; set; }

        public int SubAccountId { get; set; }

        public SettingType CommandType { get; set; }

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
        /// Creates CommandInfo.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="modelService">The model service.</param>
        /// <returns>Returns CommandInfo.</returns>
        public CommandInfo CreateCommandInfo(UserContext userContext, IModelService modelService)
        {
            if (!string.IsNullOrEmpty(this.TargetCustIds))
            {
                var targetCustIds = JsonConvert.DeserializeObject<List<int>>(this.TargetCustIds);
                return CreateMultipleCommandInfo(userContext, modelService, targetCustIds);
            }

            return CreateSingleCommandInfo(userContext, modelService);
        }

        /// <summary>
        /// Creates single command information.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="modelService">The model service.</param>
        /// <returns>Returns CommandInfo object.</returns>
        private CommandInfo CreateSingleCommandInfo(UserContext userContext, IModelService modelService)
        {
            return new CommandInfo
            {
                CustId = userContext.CustId,
                RoleId = userContext.RoleId,
                Language = userContext.Language,
                ProductId = this.ProductId,
                FormType = this.FormType,
                CommandType = this.CommandType,
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

        /// <summary>
        /// Creates multiple command information.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="modelService">The model service.</param>
        /// <param name="targetCustIds">The target customer ids.</param>
        /// <returns>Returns MultipleCommandInfo object.</returns>
        private MultipleCommandInfo CreateMultipleCommandInfo(
            UserContext userContext,
            IModelService modelService,
            IEnumerable<int> targetCustIds)
        {
            return new MultipleCommandInfo
            {
                CustId = userContext.CustId,
                RoleId = userContext.RoleId,
                Language = userContext.Language,
                ProductId = this.ProductId,
                FormType = this.FormType,
                CommandType = this.CommandType,
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
                PositionTakings = modelService.ParsePositionTaking(PositionTakingString, this.FormType),
                TargetCustIds = targetCustIds,
            };
        }
    }
}