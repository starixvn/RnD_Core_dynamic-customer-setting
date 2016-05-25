namespace Fanex.CustomerSetting.Models
{
    using Resource;
    using Results;

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
            get { return GetType().AssemblyQualifiedName; }
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