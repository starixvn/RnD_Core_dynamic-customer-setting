namespace Fanex.CustomerSetting.Matrix.Services
{
    using Models;

    public interface IPTMatrixService
    {
        PTMatrix GetPTMatrix(SettingMatrixQuery settingMatrixQuery, string masterName = null);
    }
}