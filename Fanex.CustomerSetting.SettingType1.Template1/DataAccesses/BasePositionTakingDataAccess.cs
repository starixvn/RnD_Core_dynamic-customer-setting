namespace Fanex.CustomerSetting.SettingType1.Template1.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using Fanex.CustomerSetting.Core.DataAccesses;
    using Fanex.CustomerSetting.Core.DataAccesses.Impl;
    using Fanex.CustomerSetting.Core.Dtos.Params;
    using Fanex.CustomerSetting.Core.Dtos.Results;
    using Fanex.CustomerSetting.Core.Enumerations;
    using Fanex.CustomerSetting.Core.Models;
    using Fanex.CustomerSetting.SettingType1.Template1.Models;
    using Fanex.Data;

    public class BasePositionTakingDataAccess : BaseDataAccess, ISettingDataAccess
    {
        public virtual SettingType GetCustomerSettingType(GetCustomerSettingTypeParams getParams)
        {
            var settingKey = GetSettingKeyForGettingPositionTaking(getParams.SettingQuery.CustomerLevel);
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                return new SettingType
                {
                    SettingItems = QueryPositionTakingItems(objectDb, getParams)
                };
            }
        }

        private string GetSettingKeyForGettingPositionTaking(CustomerLevel customerLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperSettingSelect";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterSettingSelect";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentSettingSelect";

                case CustomerLevel.Member:
                    return "Acc_CustSetting_MemberSettingSelect";

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        private IEnumerable<BasePositionTakingItem> QueryPositionTakingItems(ObjectDb objectDb, GetCustomerSettingTypeParams getParams)
        {
            var storedProcedureParams = GetSPParamsForGettingPositionTaking(getParams);

            switch (getParams.SettingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                    return objectDb.Query<SuperPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Master:
                    return objectDb.Query<MasterPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Agent:
                    return objectDb.Query<AgentPositionTakingItem>(storedProcedureParams);

                case CustomerLevel.Member:
                    switch (getParams.SettingQuery.UplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return objectDb.Query<SuperOnMemberPositionTakingItem>(storedProcedureParams);

                        case CustomerLevel.Master:
                            return objectDb.Query<MasterOnMemberPositionTakingItem>(storedProcedureParams);

                        case CustomerLevel.Agent:
                            return objectDb.Query<AgentOnMemberPositionTakingItem>(storedProcedureParams);

                        default:
                            throw new ArgumentOutOfRangeException("This customer level is not supported");
                    }

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        private dynamic GetSPParamsForGettingPositionTaking(GetCustomerSettingTypeParams getParams)
        {
            var settingQuery = getParams.SettingQuery;
            switch (getParams.SettingQuery.CustomerLevel)
            {
                case CustomerLevel.Super:
                case CustomerLevel.Master:
                case CustomerLevel.Agent:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId
                    };

                case CustomerLevel.Member:
                    return new
                    {
                        CustId = settingQuery.CustomerId,
                        ProductId = settingQuery.ProductId,
                        RoleId = settingQuery.UplineLevel.Value
                    };

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }

        public virtual IResult UpdateCustomerSetting(UpdateCustomerSettingParams updateParams)
        {
            var storedProcedureParams = GetSPParamsForUpdatingPositionTaking(updateParams);
            var settingKey = GetSettingKeyForUpdatingPositionTaking(updateParams.Customer.Level, updateParams.UplineLevel);

            using (var objectDb = new ObjectDb(settingKey))
            {
                objectDb.ExecuteNonQuery(storedProcedureParams);
                var outputParams = objectDb.GetOuputParams();
                return ReadOutputWithoutMessageType(outputParams);
            }
        }

        private dynamic GetSPParamsForUpdatingPositionTaking(UpdateCustomerSettingParams updateParams)
        {
            return new
            {
                UserId = updateParams.SettingContext.Actor.Id,
                SubAccId = updateParams.SettingContext.SubAccount.Id,
                CustId = updateParams.Customer.Id,
                Ip = updateParams.SettingContext.IP,
                ErrCode = 0,
                ErrMsg = string.Empty,
                MsgType = 0,
                PT = updateParams.SettingType.ToXml()
            };
        }

        private string GetSettingKeyForUpdatingPositionTaking(CustomerLevel customerLevel, CustomerLevel? uplineLevel)
        {
            switch (customerLevel)
            {
                case CustomerLevel.Super:
                    return "Acc_CustSetting_SuperPTUpd";

                case CustomerLevel.Master:
                    return "Acc_CustSetting_MasterPTUpd";

                case CustomerLevel.Agent:
                    return "Acc_CustSetting_AgentPTUpd";

                case CustomerLevel.Member:
                    switch (uplineLevel.Value)
                    {
                        case CustomerLevel.Super:
                            return "Acc_CustSetting_SuperPTOnMemberUpd";

                        case CustomerLevel.Master:
                            return "Acc_CustSetting_MasterPTOnMemberUpd";

                        case CustomerLevel.Agent:
                            return "Acc_CustSetting_AgentPTOnMemberUpd";

                        default:
                            throw new ArgumentOutOfRangeException("This customer level is not supported");
                    }

                default:
                    throw new ArgumentOutOfRangeException("This customer level is not supported");
            }
        }
    }
}