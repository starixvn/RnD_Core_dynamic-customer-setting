namespace Fanex.CustomerSetting.Core.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Dtos.Results.Impl;

    public interface IValidationService
    {
        IResult ValidateSettingQuery(SettingQuery settingQuery);

        IResult ValidateSettingCommand(SettingCommand settingCommand);

        IResult ValidateMultipleSettingCommand(MultipleSettingCommand multipleSettingCommand);
    }
}
