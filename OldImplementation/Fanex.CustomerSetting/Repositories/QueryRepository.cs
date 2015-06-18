namespace Fanex.CustomerSetting.Repositories
{
    using System.Linq;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Entities;
    using Fanex.Data;

    public class QueryRepository : IQueryRepository
    {
        public CompanyQuerySuperCustomerSettingEntity CompanyQuerySuper(CompanyQuerySuperCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<CompanyQuerySuperPositionTakingEntity>();
                    var betLimits = multi.Read<CompanyQuerySuperBetLimitEntity>();
                    var discounts = multi.Read<CompanyQuerySuperDiscountEntity>();
                    var statuses = multi.Read<CompanyQuerySuperProductStatusEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new CompanyQuerySuperCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        Statuses = statuses,
                        SettingContext = setting
                    };
                }
            }
        }

        public SuperQueryMasterCustomerSettingEntity SuperQueryMaster(SuperQueryMasterCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<SuperQueryMasterPositionTakingEntity>();
                    var betLimits = multi.Read<SuperQueryMasterBetLimitEntity>();
                    var discounts = multi.Read<SuperQueryMasterDiscountEntity>();
                    var statuses = multi.Read<SuperQueryMasterProductStatusEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new SuperQueryMasterCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        Statuses = statuses,
                        SettingContext = setting
                    };
                }
            }
        }

        public MasterQueryAgentCustomerSettingEntity MasterQueryAgent(MasterQueryAgentCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<MasterQueryAgentPositionTakingEntity>();
                    var betLimits = multi.Read<MasterQueryAgentBetLimitEntity>();
                    var discounts = multi.Read<MasterQueryAgentDiscountEntity>();
                    var statuses = multi.Read<MasterQueryAgentProductStatusEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new MasterQueryAgentCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        Statuses = statuses,
                        SettingContext = setting
                    };
                }
            }
        }

        public SuperQueryMemberCustomerSettingEntity SuperQueryMember(QueryMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<SuperQueryMemberPositionTakingEntity>();
                    var betLimits = multi.Read<SuperQueryMemberBetLimitEntity>();
                    var discounts = multi.Read<SuperQueryMemberDiscountEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new SuperQueryMemberCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        SettingContext = setting
                    };
                }
            }
        }

        public MasterQueryMemberCustomerSettingEntity MasterQueryMember(QueryMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<MasterQueryMemberPositionTakingEntity>();
                    var betLimits = multi.Read<MasterQueryMemberBetLimitEntity>();
                    var discounts = multi.Read<MasterQueryMemberDiscountEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new MasterQueryMemberCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        SettingContext = setting
                    };
                }
            }
        }

        public AgentQueryMemberCustomerSettingEntity AgentQueryMember(QueryMemberCriteria criteria)
        {
            using (var objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var multi = objectDb.QueryMultiple(criteria.GetSpParams()))
                {
                    var customerInformation = multi.Read<CustomerInformationEntity>().First();
                    var positionTakings = multi.Read<AgentQueryMemberPositionTakingEntity>();
                    var betLimits = multi.Read<AgentQueryMemberBetLimitEntity>();
                    var discounts = multi.Read<AgentQueryMemberDiscountEntity>();
                    var statuses = multi.Read<AgentQueryMemberProductStatusEntity>();
                    var setting = multi.Read<SettingContext>().First();
                    return new AgentQueryMemberCustomerSettingEntity
                    {
                        Information = customerInformation,
                        PositionTakings = positionTakings,
                        BetLimits = betLimits,
                        Discounts = discounts,
                        Statuses = statuses,
                        SettingContext = setting
                    };
                }
            }
        }
    }
}