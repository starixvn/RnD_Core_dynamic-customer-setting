namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using CustomerSetting.Services;
    using CustomerSetting.Services.Impl;
    using Data.Repository;
    using Dtos;
    using Models;
    using Resources;

    internal class CommissionMatrixService : ICommissionMatrixService
    {
        private const string DefaultLanguage = "en-US";
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;

        public CommissionMatrixService(
            IResourceService resourceService = null,
            IDynamicRepository dynamicRepository = null)
        {
            _resourceService = resourceService ?? new ResourceService();
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public CommissionMatrix GetCommissionMatrix(byte productId, string customerName, byte settingStatus)
        {
            var commissionItemMaxtrixDtos = GetCommissionItemMaxtrixDtos(productId, customerName, settingStatus);
            var commissionItemMatrices = GetCommissionItemMatricesFromDto(commissionItemMaxtrixDtos);

            return new CommissionMatrix
            {
                CommissionItemMatrices = commissionItemMatrices
            };
        }

        private IEnumerable<CommissionItemMatrix> GetCommissionItemMatricesFromDto(
            IEnumerable<CommissionItemMatrixDto> commissionItemMaxtrixDtos)
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
                CommissionItemMatrixRows = groupedDto.Select(commissionItemMatrixDto => new CommissionItemMatrixRow(commissionItemMatrixDto))
            });

            return commissionItemMatrices;
        }

        private IEnumerable<CommissionItemMatrixDto> GetCommissionItemMaxtrixDtos(
            byte productId,
            string customerName,
            byte settingStatus)
        {
            var commissionItemMaxtrixDtos = _dynamicRepository.Fetch<CommissionItemMatrixDto>(
                new CommissionMatrixQueryCriteria
                {
                    ProductId = productId,
                    UserName = customerName,
                    SettingStatus = settingStatus
                });

            if (!commissionItemMaxtrixDtos.Any())
            {
                throw new InvalidOperationException(Message.ThereIsNoData);
            }

            return commissionItemMaxtrixDtos;
        }
    }
}