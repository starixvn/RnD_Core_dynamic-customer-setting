namespace Fanex.CustomerSetting.Dtos.Results
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.CustomerSetting.Constants;

    public class SettingResult : Result
    {
        public static readonly SettingResult SuccessSettingResult = new SettingResult(ResultValue.Success, string.Empty, 0, 0, string.Empty, 0, string.Empty);
        public static readonly SettingResult ErrorSettingResult = new SettingResult(ResultValue.Error, string.Empty, 0, 0, string.Empty, 0, string.Empty);

        public SettingResult(
            int code,
            string message,
            int msgType,
            int customerId,
            string customerName,
            int settingTypeId,
            string settingTypeName)
            : base(code, message, msgType)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            SettingTypeId = settingTypeId;
            SettingTypeName = settingTypeName;
        }

        public SettingResult(
            int code,
            string message,
            int msgType,
            int customerId,
            string customerName,
            int settingTypeId,
            string settingTypeName,
            IEnumerable<string> subMessages)
            : this(
            code,
            message,
            msgType,
            customerId,
            customerName,
            settingTypeId,
            settingTypeName)
        {
            SubMessages = subMessages;
        }

        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "2", Justification = "Reviewed !")]
        public SettingResult(
            int code,
            string message,
            SettingResult settingResult)
            : this(
            code,
            message,
            settingResult.MsgType,
            settingResult.CustomerId,
            settingResult.CustomerName,
            settingResult.SettingTypeId,
            settingResult.SettingTypeName,
            settingResult.SubMessages)
        {
        }

        public SettingResult(
            SettingResult settingResult,
            string mainMessage,
            IEnumerable<string> subMessages)
            : this(
            settingResult.Code,
            mainMessage,
            settingResult.MsgType,
            settingResult.CustomerId,
            settingResult.CustomerName,
            settingResult.SettingTypeId,
            settingResult.SettingTypeName,
            subMessages)
        {
        }

        public int CustomerId { get; private set; }

        public string CustomerName { get; private set; }

        public int SettingTypeId { get; private set; }

        public string SettingTypeName { get; private set; }

        public IEnumerable<string> SubMessages { get; private set; }
    }
}