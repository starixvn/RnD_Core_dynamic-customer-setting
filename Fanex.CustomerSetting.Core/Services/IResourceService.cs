namespace Fanex.CustomerSetting.Core.Services
{
    public interface IResourceService
    {
        string GetProductName(int productId);

        string GetProductName(int productId, string refLanguage);

        string GetSportTypeName(int sportTypeId);

        string GetSportTypeName(int sportTypeId, string refLanguage);

        string GetBetTypeName(int betTypeId);

        string GetBetTypeName(int betTypeId, string refLanguage);
    }
}
