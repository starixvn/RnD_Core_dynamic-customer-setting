namespace Fanex.CustomerSetting.Services
{
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Enumerations;

    /// <summary>
    /// XmlGenerator class.
    /// </summary>
    public class XmlGenerator : IXmlGenerator
    {
        private FormType _formType;
        private SettingType _settingType;

        public XmlGenerator(FormType formType, SettingType settingType)
        {
            _formType = formType;
            _settingType = settingType;
        }

        public string Generate(IEnumerable<object> data)
        {
            return null;
        }
    }
}