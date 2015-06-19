namespace Fanex.CustomerSetting.Core.Handlers
{
    using RemotingRef;

    public class RemotingRefApi : IRemotingRefApi
    {
        public string GetSportTypeName(string sportTypeId, string language)
        {
            return RefData.GetSportName(sportTypeId, language);
        }

        public string GetBetTypeName(string betTypeId, string language)
        {
            return RefData.GetBetTypeName(betTypeId, language);
        }
    }
}