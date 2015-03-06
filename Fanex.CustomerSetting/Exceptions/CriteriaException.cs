namespace Fanex.CustomerSetting.Exceptions
{
    using System;
    using System.Runtime.Serialization;
    using System.Text;
    using Fanex.Data.Configuration;
    using Fanex.Data.Repository;

    /// <summary>
    /// CriteriaException class.
    /// </summary>
    public class CriteriaException : Exception
    {
        public CriteriaException()
        {
        }

        public CriteriaException(ICriteria criteria)
        {
            Criteria = criteria;
        }

        public CriteriaException(ICriteria criteria, string message)
            : base(message)
        {
            Criteria = criteria;
        }

        public CriteriaException(ICriteria criteria, string message, Exception innerException)
            : base(message, innerException)
        {
            Criteria = criteria;
        }

        public ICriteria Criteria { get; private set; }

        public override string Message
        {
            get
            {
                var stringBuilder = new StringBuilder(base.Message);
                var setting = DbSettingProviderManager.Current.GetSetting(Criteria.GetSettingKey());
                stringBuilder.AppendFormat("\nSp Name: {0}", setting.SpName);
                var param = Criteria.GetSpParams();
                foreach (var key in param.Keys)
                {
                    var value = param[key];
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