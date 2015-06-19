namespace Fanex.CustomerSetting.Core.Handlers
{
    public static class RemotingRefApiHandler
    {
        private static IRemotingRefApi _remotingRefApi = new RemotingRefApi();

        public static IRemotingRefApi RemotingRefApi
        {
            get { return _remotingRefApi; }
            set { _remotingRefApi = value; }
        }
    }
}