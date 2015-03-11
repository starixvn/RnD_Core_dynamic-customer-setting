namespace Fanex.CustomerSetting.Services
{
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Repositories;

    /// <summary>
    /// UpdateService class.
    /// </summary>
    public class CustomerSettingUpdateService : ICustomerSettingUpdateService
    {
        #region Variables

        private readonly ICustomerSettingUpdateRepository _respository;
        private readonly IXmlGenerator _xmlGenerator;
        private readonly IMessageService _messageService;

        #endregion Variables

        #region Constructors

        public CustomerSettingUpdateService(string languageCode)
            : this(null, null, null, languageCode)
        {
        }

        public CustomerSettingUpdateService(
            ICustomerSettingUpdateRepository respository,
            IXmlGenerator xmlGenerator,
            IMessageService messageService,
            string languageCode)
        {
            _respository = respository ?? new CustomerSettingUpdateRepository();
            _xmlGenerator = xmlGenerator ?? new XmlGenerator();
            _messageService = messageService ?? new MessageService(languageCode);
        }

        #endregion Constructors

        /// <summary>
        /// Updates customer setting.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns UpdateResult.</returns>
        public UpdateResult Update(UpdateInfo model)
        {
            switch (model.SettingType)
            {
                case SettingType.All:
                    return null;

                case SettingType.Status:
                    return UpdateStatus(model);

                case SettingType.Discount:
                    return UpdateDiscount(model);

                case SettingType.BetLimit:
                    return UpdateBetLimit(model);

                case SettingType.PositionTaking:
                    return UpdatePositionTaking(model);
            }

            return null;
        }

        /// <summary>
        /// Updates customer setting status.
        /// </summary>
        /// <param name="model">The update model.</param>
        /// <returns>Returns update result.</returns>
        private UpdateResult UpdateStatus(UpdateInfo model)
        {
            var criteria = new UpdateDisabledStatusCriteria
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
        private UpdateResult UpdateDiscount(UpdateInfo model)
        {
            var criteria = new UpdateDiscountCriteria
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
        private UpdateResult UpdateBetLimit(UpdateInfo model)
        {
            var criteria = new UpdateBetLimitCriteria
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
        private UpdateResult UpdatePositionTaking(UpdateInfo model)
        {
            var criteria = new UpdatePositionTakingCriteria(model.FormType)
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
        private UpdateResult Process(UpdateCriteria criteria, UpdateInfo model, SettingType settingType)
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
                SettingLevel = settingLevel,
                SettingType = settingType
            };
            var message = _messageService.GetTranslatedMessage(messageInfo);
            return new UpdateResult(messageInfo.ErrCode, message);
        }
    }
}