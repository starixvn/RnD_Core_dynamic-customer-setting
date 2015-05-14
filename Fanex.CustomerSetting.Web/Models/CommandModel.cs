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
        private IUserService _userService;
        private IModelService _modelService;

        public CommandModel()
            : this(null, null)
        {
        }

        public CommandModel(IUserService userService, IModelService modelService)
        {
            _userService = userService ?? new UserService();
            _modelService = modelService ?? new ModelService();
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

        public bool IsMultiple
        {
            get
            {
                return !string.IsNullOrEmpty(TargetCustIds);
            }
        }

        public CommandInfo CreateCommandInfo()
        {
            var userContext = _userService.CreateUserContext();
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
                Statuses = _modelService.ParseStatus(StatusString, this.FormType),
                DiscountLevel = this.DiscountLevel,
                Discounts = _modelService.ParseDiscount(DiscountString, this.FormType),
                BetLimitLevel = this.BetLimitLevel,
                BetLimits = _modelService.ParseBetLimit(BetLimitString, this.FormType),
                PositionTakingLevel = this.PositionTakingLevel,
                PositionTakings = _modelService.ParsePositionTaking(PositionTakingString, this.FormType)
            };
        }

        public MultipleCommandInfo CreateMultipleCommandInfo()
        {
            var userContext = _userService.CreateUserContext();
            var targetCustIds = JsonConvert.DeserializeObject<List<int>>(this.TargetCustIds);
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
                Statuses = _modelService.ParseStatus(StatusString, this.FormType),
                DiscountLevel = this.DiscountLevel,
                Discounts = _modelService.ParseDiscount(DiscountString, this.FormType),
                BetLimitLevel = this.BetLimitLevel,
                BetLimits = _modelService.ParseBetLimit(BetLimitString, this.FormType),
                PositionTakingLevel = this.PositionTakingLevel,
                PositionTakings = _modelService.ParsePositionTaking(PositionTakingString, this.FormType),
                TargetCustIds = targetCustIds,
            };
        }
    }
}