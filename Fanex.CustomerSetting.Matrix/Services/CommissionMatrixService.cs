namespace Fanex.CustomerSetting.Matrix.Services
{
    using System;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.CustomerSetting.Matrix.Enumerations;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Repositories;

    public class CommissionMatrixService : ICommissionMatrixService
    {
        private readonly ICommissionMatrixRepository _commissionMatrixRepository;
        private readonly IResourceService _resourceService;

        public CommissionMatrixService()
            : this(null, null)
        {
        }

        public CommissionMatrixService(
            ICommissionMatrixRepository commissionMatrixRepository,
            IResourceService resourceService)
        {
            _commissionMatrixRepository = commissionMatrixRepository ?? new CommissionMatrixRepository();
            _resourceService = resourceService ?? new ResourceService();
        }

        public CommissionMatrixEntity Query(QueryInfo queryInfo)
        {
            var criteria = new SettingMatrixQueryCriteria(SettingType.Commission)
            {
                ProductId = queryInfo.ProductId,
                UserName = queryInfo.UserName
            };

            if (!criteria.IsValid())
            {
                throw new ArgumentException("criteria is not valid");
            }

            var result = _commissionMatrixRepository.Query(criteria);
            foreach (var setting in result.Settings)
            {
                setting.SportTypeName = _resourceService.GetSportTypeName(setting.SportType);
                setting.BetTypeName = _resourceService.GetBetTypeName(setting.BetType);
            }

            return result;
        }
    }
}