namespace Fanex.CustomerSetting.Resource.Services
{
    public interface IRemotingRef
    {
        string GetSportTypeName(string sportTypeId, string language);

        string GetBetTypeName(string betTypeId, string language);
    }
}
