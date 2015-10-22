namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICustomerSettingMatrixService
    {
        CustomerSettingMatrix GetCustomerSettingMatrix(SettingMatrixQuery settingMatrixQuery);
    }
}