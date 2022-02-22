namespace Fanex.CustomerSetting.Services.Impl
{
    using RemotingRef;

    public class ResourceService : IResourceService
    {
        private const string SportTypeDefaultFormat = "Sport Type {0}";
        private const string BetTypeDefaultFormat = "Bet Type {0}";
        private const string ProductDefaultFormat = "Product {0}";
        private const string ProductRefId = "Product";
        private const string SportRefId = "DummySport";
        private const string RefResourceDefaultFormat = "Ref Resource: {0} - Key: {1}";

        private readonly IRemotingRefService _remotingRef;

        public ResourceService() : this(null)
        {
        }

        public ResourceService(IRemotingRefService remotingRef)
        {
            _remotingRef = remotingRef ?? new RemotingRefService();
        }

        public string GetProductName(byte productId, string language)
        {
            if (productId <= 0)
            {
                return string.Empty;
            }

            var resourceData = _remotingRef.GetResourceDataByRefIdAndResourceId(
                ProductRefId,
                productId.ToString(),
                language,
                string.Empty);

            return string.IsNullOrEmpty(resourceData)
                ? string.Format(ProductDefaultFormat, productId)
                : resourceData;
        }

        public string GetSportTypeName(int sportTypeId, string language)
        {
            if (sportTypeId <= 0)
            {
                return string.Empty;
            }

            var sportTypeName = _remotingRef.GetSportName(sportTypeId.ToString(), language);

            if (string.IsNullOrWhiteSpace(sportTypeName))
            {
                sportTypeName = _remotingRef.GetResourceDataByRefIdAndResourceId(
                    SportRefId,
                    sportTypeId.ToString(),
                    language,
                    string.Empty);
            }

            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format(SportTypeDefaultFormat, sportTypeId)
                : sportTypeName;
        }

        public string GetBetTypeName(int betTypeId, string language)
        {
            if (betTypeId <= 0)
            {
                return string.Empty;
            }

            var betTypeName = _remotingRef.GetBetTypeName(betTypeId.ToString(), language);

            return string.IsNullOrEmpty(betTypeName)
                ? string.Format(BetTypeDefaultFormat, betTypeId)
                : betTypeName;
        }

        public string GetResourceData(string refId, string resourceId, string language)
        {
            if (string.IsNullOrEmpty(refId) || string.IsNullOrEmpty(resourceId))
            {
                return string.Empty;
            }

            var resourceData = _remotingRef.GetResourceDataByRefIdAndResourceId(
                refId,
                resourceId,
                language,
                string.Empty);

            return string.IsNullOrEmpty(resourceData)
                ? string.Format(RefResourceDefaultFormat, refId, resourceId)
                : resourceData;
        }
    }
}