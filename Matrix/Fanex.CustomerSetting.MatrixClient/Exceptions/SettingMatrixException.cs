namespace Fanex.CustomerSetting.MatrixClient.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class SettingMatrixException : Exception
    {
        public SettingMatrixException()
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