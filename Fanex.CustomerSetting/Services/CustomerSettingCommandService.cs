namespace Fanex.CustomerSetting.Services
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    /// <summary>
    /// UpdateService class.
    /// </summary>
    public class CustomerSettingCommandService : ICustomerSettingCommandService
    {
        #region Variables

        private readonly ICustomerSettingCommandRepository _respository;
        private readonly IXmlGenerator _xmlGenerator;
        private readonly IMessageService _messageService;

        #endregion Variables

        #region Constructors

        public CustomerSettingCommandService(string languageCode)
            : this(null, null, null, languageCode)
        {
        }

        public CustomerSettingCommandService(
            ICustomerSettingCommandRepository respository,
            IXmlGenerator xmlGenerator,
            IMessageService messageService,
            string languageCode)
        {
            _respository = respository ?? new CustomerSettingCommandRepository();
            _xmlGenerator = xmlGenerator ?? new XmlGenerator();
            _messageService = messageService ?? new MessageService(languageCode);
        }

        #endregion Constructors

        /// <summary>
        /// Updates customer setting.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns UpdateResult.</returns>
        public CommandResult Update(CommandInfo model)
        {
            Result statusResult = null;
            Result discountResult = null;
            Result betLimitResult = null;
            Result positionTakingResult = null;
            switch (model.SettingType)
            {
                case SettingType.All:
                    statusResult = UpdateStatus(model);
                    discountResult = UpdateDiscount(model);
                    betLimitResult = UpdateBetLimit(model);
                    positionTakingResult = UpdatePositionTaking(model);
                    break;

                case SettingType.Status:
                    statusResult = UpdateStatus(model);
                    break;

                case SettingType.Discount:
                    discountResult = UpdateDiscount(model);
                    break;

                case SettingType.BetLimit:
                    betLimitResult = UpdateBetLimit(model);
                    break;

                case SettingType.PositionTaking:
                    positionTakingResult = UpdatePositionTaking(model);
                    break;
            }

            return new CommandResult(statusResult, discountResult, betLimitResult, positionTakingResult);
        }

        /// <summary>
        /// Updates customer setting status.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update result.</returns>
        private Result UpdateStatus(CommandInfo model)
        {
            if (model.Statuses == null || model.Statuses.Count() == 0)
            {
                return Result.NoChange;
            }

            var criteria = new CommandDisabledStatusCriteria
            {
                UserId = model.CustId,
                SubAccId = model.SubAccountId,
                CustId = model.TargetCustId,
                IsDisabled = _xmlGenerator.GenerateStatus(model.Statuses),
                Ip = model.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _respository.UpdateDisabledStatus(criteria);
            return Process(criteria, model, SettingType.Status);
        }

        /// <summary>
        /// Updates customer setting discount.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update result.</returns>
        private Result UpdateDiscount(CommandInfo model)
        {
            if (model.Discounts == null || model.Discounts.Count() == 0)
            {
                return Result.NoChange;
            }

            var criteria = new CommandDiscountCriteria
            {
                UserId = model.CustId,
                SubAccId = model.SubAccountId,
                CustId = model.TargetCustId,
                Discount = _xmlGenerator.GenerateDiscount(model.Discounts),
                Ip = model.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _respository.UpdateDiscount(criteria);
            return Process(criteria, model, SettingType.Discount);
        }

        /// <summary>
        /// Updates customer setting bet limit.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update result.</returns>
        private Result UpdateBetLimit(CommandInfo model)
        {
            if (model.BetLimits == null || model.BetLimits.Count() == 0)
            {
                return Result.NoChange;
            }

            var criteria = new CommandBetLimitCriteria
            {
                UserId = model.CustId,
                SubAccId = model.SubAccountId,
                CustId = model.TargetCustId,
                BetLimit = _xmlGenerator.GenerateBetLimit(model.BetLimits),
                Ip = model.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _respository.UpdateBetLimit(criteria);
            return Process(criteria, model, SettingType.BetLimit);
        }

        /// <summary>
        /// Updates customer setting position taking.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update result.</returns>
        private Result UpdatePositionTaking(CommandInfo model)
        {
            if (model.PositionTakings == null || model.PositionTakings.Count() == 0)
            {
                return Result.NoChange;
            }

            var criteria = new CommandPositionTakingCriteria(model.FormType)
            {
                UserId = model.CustId,
                SubAccId = model.SubAccountId,
                CustId = model.TargetCustId,
                PositionTaking = _xmlGenerator.GeneratePositionTaking(model.PositionTakings),
                Ip = model.Ip
            };
            if (!criteria.IsValid())
            {
                throw new CriteriaException(criteria);
            }

            _respository.UpdatePositionTaking(criteria);
            return Process(criteria, model, SettingType.PositionTaking);
        }

        /// <summary>
        /// Processes the specified criteria.
        /// </summary>
        /// <param name="criteria">The update criteria.</param>
        /// <param name="model">The update model.</param>
        /// <param name="settingType">Type of the setting.</param>
        /// <returns>Returns update result.</returns>
        private CommandResult Process(CommandCriteria criteria, CommandInfo model, SettingType settingType)
        {
            var settingLevel = SettingLevel.None;
            switch (settingType)
            {
                case SettingType.Status:
                    settingLevel = model.StatusLevel;
                    break;

                case SettingType.Discount:
                    settingLevel = model.DiscountLevel;
                    break;

                case SettingType.BetLimit:
                    settingLevel = model.BetLimitLevel;
                    break;

                case SettingType.PositionTaking:
                    settingLevel = model.PositionTakingLevel;
                    break;
            }

            var messageInfo = new MessageInfo(criteria)
            {
                IsMultiple = model.IsMultiple,
                UserName = model.UserName,
                TargetRoleId = model.TargetRoleId,
                SettingLevel = settingLevel,
                SettingType = settingType
            };
            var message = _messageService.GetTranslatedMessage(messageInfo);
            return new CommandResult(messageInfo.ErrCode, message);
        }
    }
}