namespace Fanex.CustomerSetting.MatrixClient.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class SettingMatrixException : Exception, ISerializable
    {
        public SettingMatrixException()
            : base()
        {
        }

        public SettingMatrixException(string message)
            : base(message)
        {
        }

        public SettingMatrixException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected SettingMatrixException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Add implementation.
        }
    }
}