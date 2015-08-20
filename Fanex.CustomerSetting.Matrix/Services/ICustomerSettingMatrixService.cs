namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Models;

    public interface ICustomerSettingMatrixService
    {
        CustomerSettingMatrix GetCustomerSettingMatrix(byte settingTypeId, byte productId, string customerName);
    }
}