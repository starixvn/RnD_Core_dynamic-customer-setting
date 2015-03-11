namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    /// <summary>
    /// CustomerSettingUpdateRepository class.
    /// </summary>
    public class CustomerSettingUpdateRepository : ICustomerSettingUpdateRepository
    {
        public void UpdateDisabledStatus(UpdateDisabledStatusCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateDiscount(UpdateDiscountCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateBetLimit(UpdateBetLimitCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdatePositionTaking(UpdatePositionTakingCriteria criteria)
        {
            Update(criteria);
        }

        private void Update(UpdateCriteria criteria)
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