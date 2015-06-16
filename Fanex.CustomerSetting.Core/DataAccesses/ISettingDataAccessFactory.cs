namespace Fanex.CustomerSetting.Core.DataAccesses
{
    using Fanex.CustomerSetting.Core.Enumerations;

    public interface ISettingDataAccessFactory
    {
        ISettingDataAccess Create(CustomerLevel customerLevel);
    }
}