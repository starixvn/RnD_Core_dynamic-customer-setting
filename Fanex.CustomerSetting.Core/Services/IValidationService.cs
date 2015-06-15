namespace Fanex.CustomerSetting.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Models.Params;
    using Fanex.CustomerSetting.Core.Results;
    using Fanex.CustomerSetting.Core.Results.Impl;

    public interface IValidationService
    {
        IResult ValidateSettingQuery(SettingQuery settingQuery);

        IResult ValidateSettingCommand(SettingCommand settingCommand);

        IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand);
    }
}
