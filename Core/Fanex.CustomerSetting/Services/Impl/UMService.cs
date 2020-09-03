namespace Fanex.CustomerSetting.Services.Impl
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Criteria;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Constants;
    using Fanex.CustomerSetting.Models.Results;
    using Fanex.Data.Repository;
    using System.Collections.Generic;

    public class UMService : IUMService
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IResourceService _resourceService;

        public UMService() : this(null, null) { }

        public UMService(IDynamicRepository dynamicRepository = null, IResourceService resourceService = null)
        {
            _dynamicRepository = dynamicRepository ?? new DynamicRepository();
            _resourceService = resourceService ?? new ResourceService();
        }

        public SettingResult GetUMResult(int custId, byte productId, int? settingTypeId = null, string language = "en-US")
            => ComposeUMMessage(_dynamicRepository.Fetch<UnderMaintainanceInfo>(new UnderMaintainanceQueryCriteria()
            {
                CustId = custId,
                ProductId = productId,
                SettingTypeId = settingTypeId
            }), language);

        private SettingResult ComposeUMMessage(IEnumerable<UnderMaintainanceInfo> UMInfos, string language)
        {
            Check.Argument.IsNotNull(UMInfos, "UMInfos");
            var UMSubMessages = new List<string>();
            var dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
            foreach (var UMInfo in UMInfos)
            {
                if (UMInfo.ProductId == 0)
                {
                    UMSubMessages.Add(string.Format(Resource.Message.UMSubMsg, Resource.Message.AllProducts, Resource.Message.CustomerSettings, UMInfo.BeginTime?.ToString(dateTimeFormat), UMInfo.EndTime?.ToString(dateTimeFormat)));
                }
                else
                {
                    var productSportTypeNamePart = UMInfo.SportType == 0 ? _resourceService.GetProductName(UMInfo.ProductId, language) : _resourceService.GetSportTypeName(UMInfo.SportType, language);
                    var settingTypePart = UMInfo.SettingType == 0 ? Resource.Message.CustomerSettings : Resource.Label.ResourceManager.GetString($"SettingType_{UMInfo.SettingType}");
                    UMSubMessages.Add(string.Format(Resource.Message.UMSubMsg, productSportTypeNamePart, settingTypePart, UMInfo.BeginTime?.ToString(dateTimeFormat), UMInfo.EndTime?.ToString(dateTimeFormat)));
                }
            }

            return UMSubMessages.Count == 0 ? null : new SettingResult(ResultValues.UMError, Resource.Message.UMMainMsg, MessageTypes.Exception, UMSubMessages);
        }
    }
}
