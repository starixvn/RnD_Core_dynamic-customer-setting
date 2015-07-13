namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    public class CommissionMatrixService : ICommissionMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private const byte PTSettingTypeId = 2;
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;

        public CommissionMatrixService(
            IResourceService resourceService,
            IDynamicRepository dynamicRepository)
        {
            _resourceService = resourceService ?? new ResourceService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public CommissionMatrix GetCommissionMatrix(byte productId, string customerName)
        {
            var commissionItemMaxtrixDtos = GetCommissionItemMaxtrixDtos(productId, customerName);
            var commissionItemMatrices = GetCommissionItemMatricesFromDto(commissionItemMaxtrixDtos);

            return new CommissionMatrix
            {
                CommissionItemMatrices = commissionItemMatrices
            };
        }

        private IEnumerable<CommissionItemMatrix> GetCommissionItemMatricesFromDto(IEnumerable<CommissionItemMatrixDto> commissionItemMaxtrixDtos)
        {
            var commissionItemMatrices = commissionItemMaxtrixDtos.GroupBy(dto => new
            {
                dto.BetType,
                dto.SportType
            }).Select(groupedDto => new CommissionItemMatrix
            {
                BetTypeId = groupedDto.Key.BetType,
                BetTypeName = _resourceService.GetBetTypeName(groupedDto.Key.BetType, DefaultLanguage),
                SportTypeId = groupedDto.Key.SportType,
                SportTypeName = _resourceService.GetSportTypeName(groupedDto.Key.SportType, DefaultLanguage),
                CommissionItemMatrixRows = groupedDto.ToList().Select(commissionItemMatrixDto => new CommissionItemMatrixRow(commissionItemMatrixDto))
            });

            return commissionItemMatrices;
        }

        private IEnumerable<CommissionItemMatrixDto> GetCommissionItemMaxtrixDtos(byte productId, string customerName)
        {
            var commissionItemMaxtrixDtos = _dynamicRepository.Fetch<CommissionItemMatrixDto>(
                new CommissionMatrixQueryCriteria
                {
                    ProductId = productId,
                    UserName = customerName
                });

            return commissionItemMaxtrixDtos;
        }
    }
}