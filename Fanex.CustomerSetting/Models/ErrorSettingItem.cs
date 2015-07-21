namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Resource;

    public class ErrorSettingItem : ISettingItem
    {
        public string ErrorMessage
        {
            get
            {
                return Message.SettingTypeError;
            }
        }

        public string TypeName
        {
            get { return this.GetType().AssemblyQualifiedName; }
        }

        public IResult Validate()
        {
            throw new NotImplementedException();
        }

        public string ToXml()
        {
            throw new NotImplementedException();
        }
    }
}