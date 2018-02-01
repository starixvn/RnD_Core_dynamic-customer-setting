namespace Fanex.CustomerSetting.SettingType8.Models.Requests
{
    using System;

    public class StakeLevelGroupDetailsQuery
    {
        private short _sportTypeId;

        public short SportTypeId
        {
            get
            {
                return _sportTypeId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{SportTypeId} cannot be negative or zero.");
                }

                _sportTypeId = value;
            }
        }

        public byte SettingLevel { get; set; }
    }
}