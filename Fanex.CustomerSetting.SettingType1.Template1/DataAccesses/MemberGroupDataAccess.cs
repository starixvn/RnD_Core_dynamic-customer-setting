namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;
    using Fanex.Data;

    public class MemberGroupDataAccess : BaseDataAccess, IMemberGroupDataAccess
    {
        public IEnumerable<Customer> GetGroupedMembers(string memberIds)
        {
            using (ObjectDb objectDb = new ObjectDb("Acc_CustSetting_CustomerSettingModeSelect"))
            {
                return objectDb.Query<Customer>(new
                {
                    CustIDList = memberIds
                });
            }
        }

        public IResult UpdateGroupedMembersSetting(UpdateMultipleMemberPositionTakingParams updateParams)
        {
            var settingKey = GetSettingKeyOfUpdateGroupedMembersSetting(updateParams.UplineLevel);
            var storedProcedureParams = GetSPParamsOfUpdateGroupedMembersSetting(updateParams);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private string GetSettingKeyOfUpdateGroupedMembersSetting(CustomerLevel uplineLevel)
        {
            switch (uplineLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperPTOnMemberMultiUpd";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterPTOnMemberMultiUpd";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentPTOnMemberMultiUpd";

                default:
                    throw new InvalidOperationException("Not implemented for this customer level yet");
            }
        }

        private dynamic GetSPParamsOfUpdateGroupedMembersSetting(UpdateMultipleMemberPositionTakingParams updateParams)
        {
            return new
            {
                UserId = updateParams.UserId,
                SubAccId = updateParams.SubAccId,
                CustIDList = updateParams.CustIds,
                Ip = updateParams.IP,
                ErrCode = 0,
                ErrMsg = string.Empty,
                MsgType = 0,
                PT = updateParams.PositionTakingXml
            };
        }
    }
}