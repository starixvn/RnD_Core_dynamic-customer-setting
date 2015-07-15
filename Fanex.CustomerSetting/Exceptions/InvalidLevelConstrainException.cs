namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidLevelConstrainException : CustomerSettingException
    {
        public InvalidLevelConstrainException()
            : base()
        {
        }

        public InvalidLevelConstrainException(string message)
            : base(message)
        {
        }

        protected InvalidLevelConstrainException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
