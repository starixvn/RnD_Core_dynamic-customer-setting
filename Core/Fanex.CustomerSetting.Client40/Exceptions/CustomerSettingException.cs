namespace Fanex.CustomerSetting.Client40.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class CustomerSettingException : Exception
    {
        public CustomerSettingException()
        {
        }

        public CustomerSettingException(string message)
            : base(message)
        {
        }

        public CustomerSettingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected CustomerSettingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}