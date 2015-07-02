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
            using (ObjectDb objectDb = new ObjectDb("CustSetting_CustomerSettingModeSelect_1_1"))
            {
                return objectDb.Query<Customer>(new
                {
                    CustIDList = memberIds
                });
            }
        }

        public IResult UpdateGroupedMembersSetting(UpdateMultipleMemberPTParam updateParam)
        {
            if (updateParam == null)
            {
                throw new ArgumentNullException("updateParam");
            }

            var settingKey = GetSettingKeyOfUpdateGroupedMembersSetting(updateParam.UplineLevel);
            var storedProcedureParams = GetSPParamsOfUpdateGroupedMembersSetting(updateParam);

            return ExecuteNonQuery(settingKey, storedProcedureParams);
        }

        private string GetSettingKeyOfUpdateGroupedMembersSetting(CustomerLevel uplineLevel)
        {
            switch (uplineLevel)
            {
                case CustomerLevel.Super:
                    return "CustSetting_SuperPTOnMemberMultiUpd_1_1";

                case CustomerLevel.Master:
                    return "CustSetting_MasterPTOnMemberMultiUpd_1_1";

                case CustomerLevel.Agent:
                    return "CustSetting_AgentPTOnMemberMultiUpd_1_1";

                default:
                    throw new InvalidOperationException("Not implemented for this customer level yet");
            }
        }

        private dynamic GetSPParamsOfUpdateGroupedMembersSetting(UpdateMultipleMemberPTParam updateParam)
        {
            return new
            {
                UserId = updateParam.UserId,
                SubAccId = updateParam.SubAccId,
                CustIDList = updateParam.CustIds,
                Ip = updateParam.IP,
                ErrCode = 0,
                ErrMsg = string.Empty,
                MsgType = 0,
                PT = updateParam.PositionTakingXml
            };
        }
    }
}