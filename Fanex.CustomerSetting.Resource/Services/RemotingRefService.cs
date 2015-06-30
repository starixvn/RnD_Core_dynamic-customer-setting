namespace Fanex.CustomerSetting.Resource.Services
{
    using RemotingRef;

    public class RemotingRefService : IRemotingRefService
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
