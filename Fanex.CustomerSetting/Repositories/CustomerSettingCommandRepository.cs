namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    /// <summary>
    /// CustomerSettingCommandRepository class.
    /// </summary>
    public class CustomerSettingCommandRepository : ICustomerSettingCommandRepository
    {
        public void UpdateDisabledStatus(CommandDisabledStatusCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateDiscount(CommandDiscountCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateBetLimit(CommandBetLimitCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdatePositionTaking(CommandPositionTakingCriteria criteria)
        {
            Update(criteria);
        }

        private void Update(CommandCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = objectDb.GetOuputParams();
                criteria.ErrCode = Convert.ToInt32(outputParams["errcode"]);
                criteria.ErrMsg = Convert.ToString(outputParams["errmsg"]);
                criteria.MsgType = Convert.ToInt32(outputParams["msgtype"]);
            }
        }
    }
}