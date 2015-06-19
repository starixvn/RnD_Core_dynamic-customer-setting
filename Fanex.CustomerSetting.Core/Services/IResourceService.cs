namespace Fanex.CustomerSetting.Core.Services
{
    public interface IResourceService
    {
        string GetProductName(byte productId);

        string GetSportTypeName(int sportTypeId);

        string GetBetTypeName(int betTypeId);
    }
}
