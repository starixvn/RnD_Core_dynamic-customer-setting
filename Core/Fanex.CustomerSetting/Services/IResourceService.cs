namespace Fanex.CustomerSetting.Services
{
    public interface IResourceService
    {
        string GetProductName(byte productId, string language);

        string GetSportTypeName(int sportTypeId, string language);

        string GetBetTypeName(int betTypeId, string language);

        string GetResourceData(string refId, string resourceId, string language);
    }
}