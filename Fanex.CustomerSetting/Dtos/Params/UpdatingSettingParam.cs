namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class UpdatingSettingParam
    {
        private CustomerLevel? _uplineLevel;
        private ISetting _setting;

        public UpdatingSettingParam(
            ISetting setting,
            CustomerLevel? uplineLevel = null)
        {
            Setting = setting;
            UplineLevel = uplineLevel;
        }

        public ISetting Setting
        {
            get
            {
                return _setting;
            }

            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value", "Setting cannot be null when updating customer setting.");
                }

                _setting = value;
            }
        }

        public CustomerLevel? UplineLevel
        {
            get
            {
                return _uplineLevel;
            }

            set
            {
                if (value != null)
                {
                    byte uplineLevelValue = (byte)value.Value;
                    if (uplineLevelValue < Level.Member || uplineLevelValue > Level.Super)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline Level is out of value range.");
                    }

                    if (Setting != null && Setting.CustomerLevel != CustomerLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (Setting != null && Setting.CustomerLevel == CustomerLevel.Member)
                {
                    throw new ArgumentOutOfRangeException("value", "Upline cannot be null when updating member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}