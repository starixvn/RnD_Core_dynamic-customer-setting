namespace Fanex.CustomerSetting.Services
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public interface ILoggingService
    {
        void LogQueryCustomerSettingTypeError(int customerId, CustomerLevel? uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogUpdatingCustomerSettingTypeError(int customerId, CustomerLevel? uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogUpdatingMultipleCustomersSettingTypeError(IEnumerable<int> customerIds, CustomerLevel? uplineLevel, byte productId, SettingType settingType, Exception exception);

        void LogValidatingUpdatingError(int customerId, CustomerLevel? uplineLevel, byte productId, Exception exception);

        void LogValidatingUpdatingMultipleError(IEnumerable<int> customerIds, CustomerLevel? uplineLevel, byte productId, Exception exception);
    }
}