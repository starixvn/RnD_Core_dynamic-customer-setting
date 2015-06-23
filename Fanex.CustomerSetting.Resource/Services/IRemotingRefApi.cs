namespace Fanex.CustomerSetting.Resource.Services
{
    public interface IRemotingRefApi
    {
        string GetSportTypeName(string sportTypeId, string language);

        string GetBetTypeName(string betTypeId, string language);
    }
}
