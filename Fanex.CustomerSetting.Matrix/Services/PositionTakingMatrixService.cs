namespace Fanex.CustomerSetting.Matrix.Services
{
    using System;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Entities;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Matrix.Repositories;

    public class PositionTakingMatrixService : IPositionTakingMatrixService
    {
        private readonly IPositionTakingMatrixRepository _positionTakingMatrixRepository;

        public PositionTakingMatrixService()
            : this(null)
        {
        }

        public PositionTakingMatrixService(IPositionTakingMatrixRepository positionTakingMatrixRepository)
        {
            _positionTakingMatrixRepository = positionTakingMatrixRepository ?? new PositionTakingMatrixRepository();
        }

        public PositionTakingMatrixEntity Query(QueryInfo queryInfo)
        {
            var criteria = new PositionTakingMatrixQueryCriteria
            {
                ProductId = queryInfo.ProductId,
                UserName = queryInfo.UserName
            };

            if (!criteria.IsValid())
            {
                throw new ArgumentException("criteria is not valid");
            }

            return _positionTakingMatrixRepository.Query(criteria);
        }
    }
}