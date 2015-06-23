namespace Fanex.CustomerSetting.Core.Handlers
{
    using Fanex.CustomerSetting.Core.Handlers.Impl;

    public static class RemotingRefApiHandler
    {
        private static IRemotingRefApi _remotingRefApi = new RemotingRefApi();

        /// <summary>
        /// For unittest purpose.
        /// </summary>
        public static IRemotingRefApi RemotingRefApi
        {
            get { return _remotingRefApi; }
            set { _remotingRefApi = value; }
        }
    }
}