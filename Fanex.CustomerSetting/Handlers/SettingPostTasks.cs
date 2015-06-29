namespace Fanex.CustomerSetting.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Dtos.Params;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Resource.Services;
    using Fanex.CustomerSetting.Resource.Services.Impl;

    public class SettingPostTasks
    {
        protected readonly IResourceService ResourceService;

        public SettingPostTasks(IResourceService resourceService = null)
        {
            ResourceService = resourceService ?? new ResourceService();
        }

        protected IMessageService MessageService { get; set; }

        public virtual SettingType PostGetCustomerSettingType(
            ISettingContext settingContext,
            QuerySettingParams queryParams,
            SettingType settingType)
        {
            throw new NotImplementedException("Post-Get Customer Setting Type is not implemented");
        }

        /// <summary>
        /// Sets the name of the sport type bet type for setting item using sport type bet type for indentity.
        /// This function will use by post get setting task of setting type.
        /// </summary>
        /// <param name="settingType">Type of the setting, includes list of setting items.</param>
        /// <returns>Setting type with setting item have sport, bet type name.</returns>
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

        public virtual IResult PostUpdateCustomerSettingType(
            ISettingContext settingContext, 
            Customer customer,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Customer Setting Type is not implemented");
        }

        protected IResult ReadUpdateingCustomerSettingTypeMessage(
            IResult result, 
            string updateSuccessfulMessage,
            byte settingLevel,
            string language)
        {
            Check.Argument.IsNotNull(result, "result");

            if (result.IsSuccessful)
            {
                return new Result(Result.SuccessValue, updateSuccessfulMessage);
            }
            else
            {
                var messageContent = MessageService.TranslateMessage(result.MsgType, result.Message, settingLevel, language);
                return new Result(result.Code, messageContent.MainMessage, messageContent.SubMessages);
            }
        }

        public virtual IResult PostUpdateMultipleCustomersSettingType(
            ISettingContext settingContext, 
            IEnumerable<Customer> customers,
            SettingType settingType,
            CustomerLevel? uplineLevel,
            IResult result)
        {
            throw new NotImplementedException("Post-Update Multiple Customers Setting is not implemented");
        }
    }
}