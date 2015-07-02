namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;

    public class SettingPostTasks
    {
        public SettingPostTasks(IResourceService resourceService = null)
        {
            ResourceService = resourceService ?? new ResourceService();
        }

        protected IResourceService ResourceService { get; set; }

        protected IMessageService MessageService { get; set; }

        public virtual SettingType PostGetCustomerSettingType(
            ISettingContext settingContext,
            QuerySettingParam queryParam,
            SettingType settingType)
        {
            throw new NotImplementedException("Post-Get Customer Setting Type is not implemented");
        }

        public virtual SettingResult PostUpdateCustomerSettingType(
                    ISettingContext settingContext,
                    Customer customer,
                    SettingType settingType,
                    CustomerLevel? uplineLevel,
                    SettingResult result)
        {
            throw new NotImplementedException("Post-Update Customer Setting Type is not implemented");
        }

        public virtual UpdatingSettingResult PostUpdateMultipleCustomersSettingType(
                    ISettingContext settingContext,
                    IEnumerable<Customer> customers,
                    SettingType settingType,
                    CustomerLevel? uplineLevel,
                    UpdatingSettingResult updatintSettingResult)
        {
            throw new NotImplementedException("Post-Update Multiple Customers Setting is not implemented");
        }

        /// <summary>
        /// Reads the updating customer setting type message,
        /// after updated customer setting type.
        /// </summary>
        /// <param name="result">The result return from data access.</param>
        /// <param name="updateSuccessfulMessage">The update successful message for each setting type updated
        /// May be Position Taking, bet limit ... successful message.</param>
        /// <param name="settingLevel">The setting level for displaying message.</param>
        /// <param name="language">The language for globalization.</param>
        /// <returns>Result is processed.</returns>
        protected SettingResult ReadUpdatingCustomerSettingTypeMessage(
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
                foreach (ISettingItem settingItem in settingType.SettingItems)
                {
                    if (settingItem is DefaultSettingItem)
                    {
                        var concreteSettingItem = settingItem as DefaultSettingItem;
                        concreteSettingItem.BetTypeName = ResourceService.GetBetTypeName(concreteSettingItem.BetTypeId, language);
                        concreteSettingItem.SportTypeName = ResourceService.GetSportTypeName(concreteSettingItem.SportTypeId, language);
                    }
                }
            }

            return settingType;
        }
    }
}