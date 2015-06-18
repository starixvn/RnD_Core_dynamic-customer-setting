namespace Fanex.CustomerSetting.Matrix.Services
{
    public interface IResourceService
    {
        string GetSportTypeName(int sportType);

        string GetSportTypeName(int sportType, string refLanguage);

        string GetBetTypeName(int betType);

        string GetBetTypeName(int betType, string refLanguage);
    }
}