namespace Fanex.CustomerSetting.DataAccesses
{
    using Fanex.CustomerSetting.Enumerations;

    public interface ISettingDataAccessFactory
    {
        ISettingDataAccess Create(CustomerLevel customerLevel);
    }
}