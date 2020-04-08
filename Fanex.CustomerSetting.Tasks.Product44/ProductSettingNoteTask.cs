namespace Fanex.CustomerSetting.Tasks.Product41
{
    using Fanex.Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;

    public class ProductSettingNoteTask : IPostQueryCustomerSettingTask
    {
        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            Fanex.Logging.LogService.Info("ProductSettingNoteTask.Invoke was called");
            setting.AddWarningMessages(Resource.BBinSettingNote);
        }
    }
}
