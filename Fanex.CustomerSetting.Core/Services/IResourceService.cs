namespace Fanex.CustomerSetting.Core.Services
{
    public interface IResourceService
    {
        string GetProductName(int productId);

        string GetSportTypeName(int sportTypeId);

        string GetBetTypeName(int betTypeId);
    }
}
