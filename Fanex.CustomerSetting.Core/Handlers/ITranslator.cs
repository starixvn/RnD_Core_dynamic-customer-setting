namespace Fanex.CustomerSetting.Core.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Models;

    public interface ITranslator
    {
        void Translate(SettingType settingType);
    }
}
