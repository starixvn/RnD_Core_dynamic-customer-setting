namespace Fanex.CustomerSetting.Matrix.Services
{
    using Fanex.CustomerSetting.Models.Enumerations;
    using Models;

    public interface IPTMatrixService
    {
        PTMatrix GetPTMatrix(SettingMatrixQuery settingMatrixQuery, string masterName = null, DirectMember directMemberEnum = DirectMember.None);
    }
}