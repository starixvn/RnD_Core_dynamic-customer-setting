namespace Fanex.CustomerSetting.SettingType2.Template1.DataAccesses
{
    using Fanex.CustomerSetting.DataAccesses;
    using Fanex.CustomerSetting.Enumerations;

    public class SettingType2DataAccessFactory : ISettingDataAccessFactory
    {
        public ISettingDataAccess Create(CustomerLevel customerLevel)
        {
            return new BetLimitDataAccess();
        }
    }
}