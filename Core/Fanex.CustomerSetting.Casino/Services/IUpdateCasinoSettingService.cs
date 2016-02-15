namespace Fanex.CustomerSetting.Casino.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CustomerSetting.Models.Requests;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.CustomerSetting.Services;

    public interface IUpdateCasinoSettingService : IUpdateSettingService
    {
        IResult Update(UpdateCustomerSettingCommand command);
    }
}
