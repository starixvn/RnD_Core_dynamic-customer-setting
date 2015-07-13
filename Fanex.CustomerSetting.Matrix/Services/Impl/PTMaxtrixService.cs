namespace Fanex.CustomerSetting.Matrix.Services.Impl
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Fanex.CustomerSetting.Matrix.Criteria;
    using Fanex.CustomerSetting.Matrix.Dtos;
    using Fanex.CustomerSetting.Matrix.Models;
    using Fanex.CustomerSetting.Services;
    using Fanex.CustomerSetting.Services.Impl;
    using Fanex.NAP.Framework.Data;

    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "PT is abbriviation for Position Taking.")]
    public class PTMaxtrixService : IPTMaxtrixService
    {
        private const string DefaultLanguage = "en-US";
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;

        public PTMaxtrixService(
            IDynamicRepository dynamicRepository = null,
            IResourceService resourceService = null)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _resourceService = resourceService ?? new ResourceService();
        }

        public IEnumerable<PTMatrix> GetPTMatrices(string customerName, byte productId)
        {
            var pTMatrixDtos = GetPTMatrixDtos(customerName, productId);
            var pTMatrices = GetPTMatricesFromDto(pTMatrixDtos);

            return pTMatrices;
        }

        private IEnumerable<PTMatrix> GetPTMatricesFromDto(IEnumerable<PTMatrixDto> pTMatrixDtos)
        {
            var pTMatrices = pTMatrixDtos.GroupBy(ptRow => new
            {
                ptRow.SportType,
                ptRow.BetType,
                ptRow.IsLive
            }).Select(group => new PTMatrix
            {
                BetTypeId = group.Key.BetType,
                BetTypeName = _resourceService.GetBetTypeName(group.Key.BetType, DefaultLanguage),
                SportTypeId = group.Key.SportType,
                SportTypeName = _resourceService.GetSportTypeName(group.Key.BetType, DefaultLanguage),
                IsLive = group.Key.IsLive,
                PTMatrixItems = group.ToList().Select(ptRowItem => new PTMatrixItem(ptRowItem))
            });
            return pTMatrices;
        }

        private IEnumerable<PTMatrixDto> GetPTMatrixDtos(string customerName, byte productId)
        {
            var pTMatrixDtos = _dynamicRepository.Fetch<PTMatrixDto>(new GetPTMatrixCriteria
            {
                ProductId = productId,
                UserName = customerName
            });

            return pTMatrixDtos;
        }
    }
}