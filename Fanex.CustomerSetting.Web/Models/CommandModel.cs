namespace Fanex.CustomerSetting.Web.Models
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Web.Services;
    using Fanex.CustomerSetting.Web.Utility;
    using Newtonsoft.Json;

    public class CommandModel : BaseModel
    {
        public CommandModel()
        {
            Ip = RequestHelper.GetClientIp();
        }

        public string TargetCustIds { get; set; }

        public int SubAccountId { get; set; }

        public CommandType CommandType { get; set; }

        public string Ip { get; private set; }

        public SettingLevel StatusLevel { get; set; }

        public string StatusString { get; set; }

        public SettingLevel DiscountLevel { get; set; }

        public string DiscountString { get; set; }

        public SettingLevel BetLimitLevel { get; set; }

        public string BetLimitString { get; set; }

        public SettingLevel PositionTakingLevel { get; set; }

        public string PositionTakingString { get; set; }

        public CommandInfo CreateCommandInfo(UserContext userContext, IModelService modelService)
        {
            if (!string.IsNullOrEmpty(this.TargetCustIds))
            {
                var targetCustIds = JsonConvert.DeserializeObject<List<int>>(this.TargetCustIds);
                return CreateMultipleCommandInfo(userContext, modelService, targetCustIds);
            }

            return CreateSingleCommandInfo(userContext, modelService);
        }

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