namespace Fanex.CustomerSetting.SettingType8.Models.Requests
{
    using System;

    public class StakeLevelGroupDetailsQuery
    {
        private int _customerId;
        private byte _productId;
        private short _sportTypeId;

        public int CustomerId
        {
            get
            {
                return _customerId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{CustomerId} cannot be negative or zero.");
                }

                _customerId = value;
            }
        }

        public byte ProductId
        {
            get
            {
                return _productId;
            }

            set
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), $"{ProductId} cannot be negative or zero.");
                }

                _productId = value;
            }
        }

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