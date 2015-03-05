namespace Fanex.CustomerSetting.Demo.UserLogin
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// View mode login class.
    /// </summary>
    public class LoginViewModels
    {
        public LoginViewModels()
        {
            LanguageCode = "en-US";
        }

        public int RoleId { get; set; }

        public int CustId { get; set; }

        public int SubAccountId { get; set; }

        public string LanguageCode { get; set; }

        public List<Role> Roles
        {
            get
            {
                var rolesValue = new List<Role>();
                var roles = new Dictionary<int, string>()
                    {
                                    { 1, "Member" },
                                    { 2, "Agent" },
                                    { 3, "Master" },
                                    { 4, "Super" },
                                    { 5, "Company" }
                    };

                foreach (var role in roles)
                {
                    var result = new Role
                    {
                        RoleId = role.Key,
                        RoleName = role.Value,
                    };
                    rolesValue.Add(result);
                }

                return rolesValue;
            }
        }

        public List<Language> Languages
        {
            get
            {
                var languagesValue = new List<Language>();
                var languages = new Dictionary<string, string>()
                    {
                                    { "en-US", "English" },
                                    { "zh-TW", "繁體中文" },
                                    { "zh-CN", "简体中文" },
                                    { "ja-JP", "日本語" },
                                    { "th-TH", "ภาษาไทย" },
                                     { "ko-KR", "한국어" }
                    };

                foreach (var language in languages)
                {
                    var result = new Language
                    {
                        LanguageCode = language.Key,
                        LanguageName = language.Value,
                    };
                    languagesValue.Add(result);
                }

                return languagesValue;
            }
        }
    }
}