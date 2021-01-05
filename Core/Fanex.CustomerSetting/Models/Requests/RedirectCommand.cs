namespace Fanex.CustomerSetting.Models.Requests
{
    using System;
    using Enumerations;

    public class RedirectCommand : CustomerSettingQuery
    {
        private short _settingTypeId;

        public RedirectCommand(
            int customerId,
            Level customerLevel,
            byte productId,
            Level uplineLevel,
            short settingTypeId,
            byte actionId,
            ActionType actionType,
            byte settingStatus,
            string jsonParameters)
            : base(customerId, customerLevel, productId, settingStatus, uplineLevel)
        {
            SettingTypeId = settingTypeId;
            ActionId = actionId;
            ActionType = actionType;
            JsonParameters = jsonParameters;
        }

        public RedirectCommand(
            int customerId,
            byte customerLevel,
            byte productId,
            byte? uplineLevel,
            short settingTypeId,
            byte actionId,
            ActionType actionType,
            byte settingStatus,
            string jsonParameters)
            : base(customerId, customerLevel, productId, settingStatus, uplineLevel)
        {
            SettingTypeId = settingTypeId;
            ActionId = actionId;
            ActionType = actionType;
            JsonParameters = jsonParameters;
        }

        public short SettingTypeId
        {
            get
            {
                return _settingTypeId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(SettingTypeId)} cannot be negative or zero.");
                }

                _settingTypeId = value;
            }
        }

        public byte ActionId { get; set; }

        public ActionType ActionType { get; set; }

        public string JsonParameters { get; set; }
    }
}