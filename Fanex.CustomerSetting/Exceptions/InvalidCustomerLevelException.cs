namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidCustomerLevelException : CustomerSettingException
    {
        public InvalidCustomerLevelException() : base()
        {
        }

        public InvalidCustomerLevelException(string message) : base(message) 
        { 
        }

        protected InvalidCustomerLevelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
