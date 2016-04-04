namespace Fanex.CustomerSetting.RemotingRefApi
{
    public interface IRemotingRefService
    {
        string GetSportTypeName(string sportTypeId, string language);

        string GetBetTypeName(string betTypeId, string language);

        string GetResourceData(string refId, string resourceId, string language);
    }
}