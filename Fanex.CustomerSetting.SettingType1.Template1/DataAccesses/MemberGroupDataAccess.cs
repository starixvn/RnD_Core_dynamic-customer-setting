namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.DataAccesses.Impl;
    using Fanex.CustomerSetting.Dtos.Results;
    using Fanex.CustomerSetting.Enumerations;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Dtos;
    using Fanex.Data;

    public class MemberGroupDataAccess : BaseDataAccess, IMemberGroupDataAccess
    {
        public IEnumerable<CustomerInfo> GetGroupedMembers(string memberIds)
        {
            using (ObjectDb objectDb = new ObjectDb("CustSetting_GroupedPTMemberLstSelect"))
            {
                return objectDb.Query<CustomerInfo>(new
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
                    return "CustSetting_SuperPTOnMemberMultiUpd";

                case CustomerLevel.Master:
                    return "CustSetting_MasterPTOnMemberMultiUpd";

                case CustomerLevel.Agent:
                    return "CustSetting_AgentPTOnMemberMultiUpd";

                default:
                    throw new InvalidOperationException("Not implemented for this customer level yet");
            }
        }

        private Hashtable GetSPParamsOfUpdateGroupedMembersSetting(UpdateMultipleMemberPTParam updateParam)
        {
            Hashtable param = new Hashtable();
            param.Add("userid", updateParam.UserId);
            param.Add("subaccid", updateParam.SubAccId);
            param.Add("custidlist", updateParam.CustIds);
            param.Add("pt", updateParam.PTXml);
            param.Add("ip", updateParam.IP);
            param.Add("errcode", 0);
            param.Add("errmsg", string.Empty);
            param.Add("msgtype", 0);

            return param;
        }
    }
}