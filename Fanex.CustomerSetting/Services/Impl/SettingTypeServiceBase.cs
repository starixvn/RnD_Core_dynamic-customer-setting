namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Exceptions;
    using Fanex.CustomerSetting.Models;
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
                List<DefaultSettingItem> defaultSettingItems = new List<DefaultSettingItem>();

                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is DefaultSettingItem)
                    {
                        var defaultSettingItem = settingItem as DefaultSettingItem;
                        defaultSettingItem.BetTypeName = GetBetTypeName(defaultSettingItem.BetTypeId, language);
                        defaultSettingItem.SportTypeName = GetSportTypeName(defaultSettingItem.SportTypeId, language);
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

        private string GetBetTypeName(short betTypeId, string language)
        {
            if (betTypeId > 0)
            {
                return ResourceService.GetBetTypeName(betTypeId, language);
            }

            return string.Empty;
        }

        private string GetSportTypeName(short sportTypeId, string language)
        {
            if (sportTypeId > 0)
            {
                return ResourceService.GetSportTypeName(sportTypeId, language);
            }

            return string.Empty;
        }

        protected SettingResult CreateSettingResult(IResult result, int customerId, SettingType settingType)
        {
            return new SettingResult(result.Code, result.Message, result.MsgType, customerId, settingType.Id, settingType.Name);
        }

        public abstract SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType);

        public abstract SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Enumerations.Level customerLevel, Enumerations.Level uplineLevel, byte productId, SettingType settingType);

        public virtual IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(ISettingContext settingContext, IEnumerable<int> customerIds, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            var settingTypeResults = new List<SettingResult>();
            SettingResult settingTypeResult = null;

            foreach (var customerId in customerIds)
            {
                try
                {
                    //// Faced with duplication code here, i'm trying to findout solution.
                    settingTypeResult = UpdateCustomerSettingType(settingContext, customerId, customerLevel, uplineLevel, productId, settingType);
                }
                catch (Exception ex)
                {
                    settingTypeResult = new SettingResult(Result.SystemError.Code, Result.SystemError.Message, 0, customerId, settingType.Id, settingType.Name);

                    WriteUpdateSettingTypeLog(settingContext, customerId, customerLevel, uplineLevel, productId, settingType, ex);
                }

                settingTypeResults.Add(settingTypeResult);
            }

            return settingTypeResults;
        }

        private void WriteUpdateSettingTypeLog(
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