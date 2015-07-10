namespace Fanex.CustomerSetting.Dtos.Params
{
    using System;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;

    public class UpdatingSettingParam
    {
        private CustomerLevel? _uplineLevel;

        public UpdatingSettingParam(
            ISetting setting, 
            CustomerLevel? uplineLevel = null)
        {
            Setting = setting;
            UplineLevel = uplineLevel;
        }

        public ISetting Setting { get; set; }

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
                    if (uplineLevelValue <= 0 || uplineLevelValue > (byte)CustomerLevel.Super)
                    {
                        throw new ArgumentOutOfRangeException("UplineLevel", "Upline Level is out of value range.");
                    }

                    if (Setting != null && Setting.CustomerLevel != CustomerLevel.Member)
                    {
                        throw new ArgumentOutOfRangeException("UplineLevel", "Upline should be null when getting customer setting is not member");
                    }
                }
                else if (Setting != null && Setting.CustomerLevel == CustomerLevel.Member)
                {
                    throw new ArgumentOutOfRangeException("UplineLevel", "Upline cannot be null when updating member setting.");
                }

                _uplineLevel = value;
            }
        }
    }
}