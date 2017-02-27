namespace Fanex.CustomerSetting.Tasks.SettingType13
{
    using System.Linq;
    using Models;
    using Models.Enumerations;

    public abstract class AddSettingType13WarningTask
    {
        internal const short SettingType13Id = 13;

        protected SettingType FilterSettingType13OnExternalSites(ISettingContext settingContext, ISetting setting)
        {
            if (settingContext.System == (byte)SystemType.Nap)
            {
                return null;
            }

            return setting.SettingTypes.FirstOrDefault(settingType => settingType.Id == SettingType13Id);
        }
    }
}