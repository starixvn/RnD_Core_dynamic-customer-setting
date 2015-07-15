namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ActorStatusDisabledException : CustomerSettingException
    {
        public ActorStatusDisabledException() : base()
        {

        }

        public ActorStatusDisabledException(string message) : base (message)
        {

        }
    }
}
