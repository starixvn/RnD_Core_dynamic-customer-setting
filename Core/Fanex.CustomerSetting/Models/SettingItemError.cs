namespace Fanex.CustomerSetting.Models
{
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Resource;

    public class SettingItemError : ISettingItem
    {
        public SettingItemError()
        {
            //// Workaround for multiple language, if private set, cannot use any other languages
            ErrorMessage = Message.SettingTypeError;
        }

        public string ErrorMessage
        {
            get;
            set;
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