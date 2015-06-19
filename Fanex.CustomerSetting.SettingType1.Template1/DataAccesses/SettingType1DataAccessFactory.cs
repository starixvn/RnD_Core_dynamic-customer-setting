namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.Enumerations;

    public class SettingType1DataAccessFactory : ISettingDataAccessFactory
    {
        public ISettingDataAccess Create(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Agent:
                case CustomerLevel.Member:
                    return new BasePositionTakingDataAccess();

                case CustomerLevel.Master:
                    return new MasterPositionTakingDataAccess();

                default:
                    throw new InvalidOperationException("Customer level data access does not support!");
            }
        }
    }
}