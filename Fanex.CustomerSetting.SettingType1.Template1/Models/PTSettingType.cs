namespace Fanex.CustomerSetting.SettingType1.Template1.Models
{
    using Fanex.CustomerSetting.Models;

    public class PTSettingType : SettingType
    {
        public PTSettingType()
        {
        }

        public PTSettingType(SettingType settingType)
        {
            if (settingType != null)
            {
                Id = settingType.Id;
                Name = settingType.Name;
                SettingLevel = settingType.SettingLevel;
                SettingTemplate = settingType.SettingTemplate;
                SettingItems = settingType.SettingItems;
            }
        }

        public bool IsDisableMasterAutoPT { get; set; }
    }
}