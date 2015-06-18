namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    public class CommandRepository : ICommandRepository
    {
        public void UpdateDisabledStatus(DisabledStatusCommandCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateDiscount(DiscountCommandCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdateBetLimit(BetLimitCommandCriteria criteria)
        {
            Update(criteria);
        }

        public void UpdatePositionTaking(PositionTakingCommandCriteria criteria)
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