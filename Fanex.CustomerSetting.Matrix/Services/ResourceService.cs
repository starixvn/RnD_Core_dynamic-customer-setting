namespace Fanex.CustomerSetting.Matrix.Services
{
    using RemotingRef;

    public class ResourceService : IResourceService
    {
        public ResourceService()
        {
            RefLanguage = GetRefLanguage("en-US");
        }

        public string RefLanguage { get; private set; }

        #region Get sport type name

        public string GetSportTypeName(int sportType)
        {
            return GetSportTypeName(sportType, RefLanguage);
        }

        public string GetSportTypeName(int sportType, string refLanguage)
        {
            var sportTypeName = RefData.GetSportName(sportType.ToString(), refLanguage);
            return string.IsNullOrEmpty(sportTypeName)
                ? string.Format("Sport Type {0}", sportType)
                : sportTypeName;
        }

        #endregion Get sport type name

        #region Get bet type name

        public string GetBetTypeName(int betType)
        {
            return GetBetTypeName(betType, RefLanguage);
        }

        public string GetBetTypeName(int betType, string refLanguage)
        {
            var betTypeName = RefData.GetBetTypeName(betType.ToString(), refLanguage);
            return string.IsNullOrEmpty(betTypeName)
                ? string.Format("Bet Type {0}", betType)
                : betTypeName;
        }

        #endregion Get bet type name

        #region Utils

        private static string GetRefLanguage(string language)
        {
            var defaultRefLanguage = "en";
            if (string.IsNullOrEmpty(language))
            {
                return defaultRefLanguage;
            }

            switch (language.ToLower())
            {
                case "en-us": return "en"; // English
                case "zh-tw": return "ch"; // Traditional
                case "zh-cn": return "cs"; // Simplified
                case "ja-jp": return "jp"; // Japanese
                case "th-th": return "th"; // Thai
                default: return defaultRefLanguage;
            }
        }

        #endregion Utils
    }
}