namespace Fanex.CustomerSetting.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ILoggingService
    {
        void LogQueryCustomerSettingTypeError(int customerId, UserLevel uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogUpdatingCustomerSettingTypeError(int customerId, UserLevel uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogUpdatingMultipleCustomersSettingTypeError(IEnumerable<int> customerIds, UserLevel uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogValidatingUpdatingError(int customerId, UserLevel uplineLevel, byte productId, Exception exception);

        void LogValidatingUpdatingMultipleError(IEnumerable<int> customerIds, UserLevel uplineLevel, byte productId, Exception exception);
    }
}