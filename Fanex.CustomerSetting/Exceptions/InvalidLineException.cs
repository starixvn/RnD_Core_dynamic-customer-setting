namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class InvalidLineException : CustomerSettingException
    {
         public InvalidLineException() : base()
        {
        }

        public InvalidLineException(string message) : base(message) 
        { 
        }

        protected InvalidLineException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
