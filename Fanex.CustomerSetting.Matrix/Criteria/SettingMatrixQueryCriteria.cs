namespace Fanex.CustomerSetting.Matrix.Criteria
{
    using System;
    using Fanex.CustomerSetting.Matrix.Enumerations;
    using Fanex.Data.Repository;

    public class SettingMatrixQueryCriteria : CriteriaBase
    {
        private SettingType _settingType;

        public SettingMatrixQueryCriteria(SettingType settingType)
        {
            _settingType = settingType;
        }

        public string UserName { get; set; }

        public int ProductId { get; set; }

        public override string GetSettingKey()
        {
            switch (_settingType)
            {
                case SettingType.PositionTaking:
                    return "Acc_CustSetting_PTMatrix";

                case SettingType.Commission:
                    return "Acc_CustSetting_CommMatrix";

                default:
                    throw new NotImplementedException("Not implemented yet");
            }
        }

        public override bool IsValid()
        {
            return ProductId > 0 && !string.IsNullOrEmpty(UserName);
        }
    }
}