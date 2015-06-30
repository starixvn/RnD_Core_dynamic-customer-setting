namespace Fanex.CustomerSetting.Resource.Services
{
    public interface IRemotingRefService
    {
        string GetSportTypeName(string sportTypeId, string language);

        string GetBetTypeName(string betTypeId, string language);
    }
}
