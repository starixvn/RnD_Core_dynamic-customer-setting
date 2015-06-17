namespace Fanex.CustomerSetting.Core.Exceptions
{
    using System;
    using System.Collections;
    using System.Runtime.Serialization;
    using System.Text;
    using Fanex.Data.Configuration;
    using Fanex.NAP.Framework.Data;

    [Serializable]
    public class StoredProcedureException : Exception
    {
        private Hashtable _params;
        private string _settingKey;

        public StoredProcedureException(string settingKey, Hashtable parameters)
        {
            _settingKey = settingKey;
            _params = parameters;
        }

        public StoredProcedureException(ICriteria criteria)
        {
            if (criteria != null)
            {
                _settingKey = criteria.GetSettingKey();
                _params = criteria.GetSpParams();
            }
            else
            {
                throw new ArgumentNullException("criteria");
            }
        }

        public StoredProcedureException(
            string settingKey,
            Hashtable parameters,
            string message)
            : base(message)
        {
            _settingKey = settingKey;
            _params = parameters;
        }

        public StoredProcedureException(
            string settingKey,
            Hashtable parameters,
            string message,
            Exception innerException)
            : base(message, innerException)
        {
            _settingKey = settingKey;
            _params = parameters;
        }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder(base.Message);
                var setting = DbSettingProviderManager.Current.GetSetting(_settingKey);
                stringBuilder.AppendFormat("\nSp Name: {0}", setting.SpName);
                foreach (var key in _params.Keys)
                {
                    var value = _params[key];
                    stringBuilder.AppendFormat("\n{0} = {1}", key, value);
                }

                return stringBuilder.ToString();
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}