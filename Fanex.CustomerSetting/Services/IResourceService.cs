namespace Fanex.CustomerSetting.Services
{
    public interface IResourceService
    {
        string GetProductName(byte productId);

        string GetSportTypeName(int sportTypeId);

        string GetBetTypeName(int betTypeId);
    }
}
