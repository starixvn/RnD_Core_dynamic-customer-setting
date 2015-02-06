namespace Fanex.CustomerSetting.Services
{
    /// <summary>
    /// IResourceService class.
    /// </summary>
    public interface IResourceService
    {
        string GetProductName(int productId);

        string GetProductName(int productId, string refLanguage);

        string GetSportTypeName(int sportType);

        string GetSportTypeName(int sportType, string refLanguage);

        string GetBetTypeName(int betType);

        string GetBetTypeName(int betType, string refLanguage);
    }
}