namespace Fanex.CustomerSetting.Matrix.Services
{
    using Models;

    public interface ICustomerSettingMatrixService
    {
        CustomerSettingMatrix GetCustomerSettingMatrix(SettingMatrixQuery settingMatrixQuery, byte SettingStatus);
    }
}