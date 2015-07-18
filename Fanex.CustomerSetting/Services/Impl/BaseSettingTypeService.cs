namespace Fanex.CustomerSetting.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public abstract class BaseSettingTypeService : ISettingTypeService
    {
        protected BaseSettingTypeService(IResourceService resourceService = null)
        {
            ResourceService = resourceService ?? new ResourceService();
        }

        protected IResourceService ResourceService { get; set; }

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
            return new SettingResult(result.Code, result.Message, result.MsgType, customerId, settingType.Id, settingType.Name);
        }

        public abstract SettingType GetCustomerSettingType(ISettingContext settingContext, QuerySettingParam queryParam, SettingType productSettingType);

        public abstract SettingResult UpdateCustomerSettingType(ISettingContext settingContext, int customerId, Enumerations.Level customerLevel, Enumerations.Level uplineLevel, byte productId, SettingType settingType);

        public virtual IEnumerable<SettingResult> UpdateMultipleCustomersSettingType(ISettingContext settingContext, IEnumerable<int> customerIds, Level customerLevel, Level uplineLevel, byte productId, SettingType settingType)
        {
            var settingTypeResults = new List<SettingResult>();

            foreach (var customerId in customerIds)
            {
                var updateResult = UpdateCustomerSettingType(settingContext, customerId, customerLevel, uplineLevel, productId, settingType);

                settingTypeResults.Add(updateResult);
            }

            return settingTypeResults;
        }
    }
}
