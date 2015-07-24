namespace Fanex.CustomerSetting.Models
{
    using System;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Resource;

    public class SettingItemError : ISettingItem
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
            return new Result(Constants.ResultValue.Error, Message.SettingTypeError);
        }

        public string ToXml()
        {
            return string.Format("<Error>{0}</Error>", Message.SettingTypeError);
        }
    }
}