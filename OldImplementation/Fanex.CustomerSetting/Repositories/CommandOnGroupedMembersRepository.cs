namespace Fanex.CustomerSetting.Repositories
{
    using System;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.Data;

    public class CommandOnGroupedMembersRepository : ICommandOnGroupedMembersRepository
    {
        public void UpdatePositionTaking(PositionTakingCommandOnGroupedMembersCriteria criteria)
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