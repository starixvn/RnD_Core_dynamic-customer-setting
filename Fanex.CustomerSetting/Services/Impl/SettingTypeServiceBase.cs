namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Enumerations;
    using Fanex.CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.Logging;

    public abstract class SettingTypeServiceBase : ISettingTypeService
    {
        protected SettingTypeServiceBase()
            : this(null, null)
        {
        }

        protected SettingTypeServiceBase(
            IResourceService resourceService,
            ILogger logger)
        {
            ResourceService = resourceService ?? new ResourceService();
            Logger = logger ?? Logging.Logger.Log;
        }

        protected IResourceService ResourceService { get; set; }

        protected ILogger Logger { get; set; }

        protected IMessageService MessageService { get; set; }

        /// <summary>
        /// Reads the update customer setting type message,
        /// after updated customer setting type.
        /// </summary>
        /// <param name="result">The result return from data access.</param>
        /// <param name="updateSuccessfulMessage">The update successful message for each setting type updated
        /// May be Position Taking, bet limit ... successful message.</param>
        /// <param name="settingLevel">The setting level for displaying message.</param>
        /// <param name="language">The language for globalization.</param>
        /// <returns>Result is processed.</returns>
        protected SettingResult TranslateUpdateResultMessage(
                    SettingResult result,
                    string updateSuccessfulMessage,
                    byte settingLevel,
                    string language)
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }

            if (result.IsSuccessful)
            {
                return new SettingResult(ResultValue.Success, updateSuccessfulMessage, result);
            }
            else
            {
                var messageContent = MessageService.TranslateMessage(result.MsgType, result.Message, settingLevel, language);

                return new SettingResult(result, messageContent.MainMessage, messageContent.SubMessages);
            }
        }

        /// <summary>
        /// Sets the name of the sport type bet type for setting item using sport type bet type for indentity.
        /// This function will use by post get setting task of setting type.
        /// </summary>
        /// <param name="settingType">Type of the setting, includes list of setting items.</param>
        /// <returns>Setting type with setting item have sport, bet type name.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Reviewed !")]
        protected SettingType SetSportTypeBetTypeName(SettingType settingType, string language)
        {
            if (settingType != null && settingType.SettingItems.Any())
            {
                List<SettingItemDefault> defaultSettingItems = new List<SettingItemDefault>();

                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is SettingItemDefault)
                    {
                        var defaultSettingItem = settingItem as SettingItemDefault;
                        defaultSettingItem.BetTypeName = ResourceService.GetBetTypeName(defaultSettingItem.BetTypeId, language);
                        defaultSettingItem.SportTypeName = ResourceService.GetSportTypeName(defaultSettingItem.SportTypeId, language);
                        defaultSettingItems.Add(defaultSettingItem);
                    }
                    else
                    {
                        throw new InvalidCastException("Invalid cast default setting item.");
                    }
                }

                settingType.SettingItems = defaultSettingItems;
            }

            return settingType;
        }

        protected SettingResult CreateSettingResult(IResult result, int customerId, SettingType settingType)
        {
            return new SettingResult(
                result.Code,
                result.Message,
                result.MsgType,
                customerId,
                settingType.Id,
                settingType.Name);
        }

        protected void SetSettingTypeReadOnlyWhenIndirectUplineEditMember(SettingType settingType, CustomerSettingQuery query)
        {
            if (query.CustomerLevel.IsMemberLevel()
                && query.UplineLevel.IsNotAgentLevel())
            {
                settingType.IsReadOnly = true;
            }
        }

        public abstract SettingType GetCustomerSettingType(
            ISettingContext settingContext,
            CustomerSettingQuery query);

        public abstract SettingResult UpdateCustomerSettingType(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType);

        public virtual IEnumerable<SettingResult> UpdateMultipleCustomerSettingType(
            ISettingContext settingContext,
            IEnumerable<int> customerIds,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType settingType)
        {
            if (customerIds == null)
            {
                throw new ArgumentNullException("customerIds");
            }

            var settingTypeResults = new List<SettingResult>();
            SettingResult settingTypeResult = null;

            foreach (var customerId in customerIds)
            {
                try
                {
                    settingTypeResult = UpdateCustomerSettingType(
                        settingContext,
                        customerId,
                        customerLevel,
                        uplineLevel,
                        productId,
                        settingType);
                }
                catch (Exception ex)
                {
                    settingTypeResult = new SettingResult(Result.SystemError, customerId, settingType.Id, settingType.Name);

                    WriteLog(settingContext, customerId, customerLevel, uplineLevel, productId, settingType, ex);
                }

                settingTypeResults.Add(settingTypeResult);
            }

            return settingTypeResults;
        }

        private void WriteLog(
            ISettingContext settingContext,
            int customerId,
            Level customerLevel,
            Level uplineLevel,
            byte productId,
            SettingType customerSettingType,
            Exception ex)
        {
            var customerSettingException = new CustomerSettingException(ex.Message, ex);
            customerSettingException.SetSettingContextInfo("Update Setting", settingContext);
            customerSettingException.SetCustomerInfo(customerId, customerLevel, uplineLevel);
            customerSettingException.SetProductInfo(productId, customerSettingType);

            Logger.Error(customerSettingException.Message, customerSettingException);
        }
    }
}