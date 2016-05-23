namespace Fanex.CustomerSetting.Models.Requests
{
    using System;
    using Enumerations;

    public class RedirectCommand
    {
        private short _settingTypeId;
        private byte _actionId;

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

        public byte ActionId
        {
            get
            {
                return _actionId;
            }

            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{nameof(ActionId)} cannot be negative.");
                }

                _actionId = value;
            }
        }

        public ActionType ActionType { get; set; }

        public string JsonParameters { get; set; }
    }
}