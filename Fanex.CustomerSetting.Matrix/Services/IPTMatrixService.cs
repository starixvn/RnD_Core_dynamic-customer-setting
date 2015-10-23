namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Matrix.Models;

    public interface IPTMatrixService
    {
        PTMatrix GetPTMatrix(SettingMatrixQuery settingMatrixQuery);
    }
}