namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Constants;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class UpdatingSettingParam
    {
        private UserLevel _uplineLevel;
        private ISetting _setting;

        public UpdatingSettingParam(
            ISetting setting,
            UserLevel uplineLevel = null)
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

        public UserLevel UplineLevel
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
                    if (uplineLevelValue < Level.Agent || uplineLevelValue > Level.Super)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline Level is out of value range.");
                    }

                    if (Setting.CustomerLevel != UserLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("value", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (Setting.CustomerLevel == UserLevel.Member)
                {
                    throw new ArgumentNullException("value", "Upline cannot be null when updating member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}