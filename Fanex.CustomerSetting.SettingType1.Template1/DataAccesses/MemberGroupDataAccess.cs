namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.DataAccesses.Impl;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
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
            using (var objectDb = new ObjectDb(GetSettingKeyOfUpdateGroupedMembersSetting(updateParams.UplineLevel)))
            {
                objectDb.ExecuteNonQuery(GetSPParamsOfUpdateGroupedMembersSetting(updateParams));
                var outputParams = objectDb.GetOuputParams();
                return ReadOutputWithMessageType(outputParams);
            }
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
                    throw new ArgumentOutOfRangeException("Not implemented for this customer level yet");
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