namespace Fanex.CustomerSetting.Matrix.Services
{
    using System;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.CustomerSetting.Matrix.Enumerations;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Repositories;

    public class PositionTakingMatrixService : IPositionTakingMatrixService
    {
        private readonly IPositionTakingMatrixRepository _positionTakingMatrixRepository;
        private readonly IResourceService _resourceService;

        public PositionTakingMatrixService()
            : this(null, null)
        {
        }

        public PositionTakingMatrixService(
            IPositionTakingMatrixRepository positionTakingMatrixRepository,
            IResourceService resourceService)
        {
            _positionTakingMatrixRepository = positionTakingMatrixRepository ?? new PositionTakingMatrixRepository();
            _resourceService = resourceService ?? new ResourceService();
        }

        public PositionTakingMatrixEntity Query(QueryInfo queryInfo)
        {
            var criteria = new SettingMatrixQueryCriteria(SettingType.PositionTaking)
            {
                ProductId = queryInfo.ProductId,
                UserName = queryInfo.UserName
            };

            if (!criteria.IsValid())
            {
                throw new ArgumentException("criteria is not valid");
            }

            var result = _positionTakingMatrixRepository.Query(criteria);
            foreach (var setting in result.Settings)
            {
                setting.SportTypeName = _resourceService.GetSportTypeName(setting.SportType);
                setting.BetTypeName = _resourceService.GetBetTypeName(setting.BetType);
            }

            return result;
        }
    }
}