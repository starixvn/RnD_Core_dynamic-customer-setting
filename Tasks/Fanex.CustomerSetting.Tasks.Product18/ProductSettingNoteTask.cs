namespace Fanex.CustomerSetting.Tasks.Product18
{
    using Common.Helper;
    using Fanex.CustomerSetting.Models;
    using Fanex.CustomerSetting.Models.Requests;
    using Services;

    public class ProductSettingNoteTask : IPostQueryCustomerSettingTask
    {
        private readonly TasksProduct18Service _tasksService;

        public ProductSettingNoteTask()
        {
            _tasksService = new TasksProduct18Service();
        }

        public void Invoke(ISettingContext settingContext, CustomerSettingQuery query, ISetting setting)
        {
            Check.Argument.IsNotNull(settingContext, nameof(settingContext));
            Check.Argument.IsNotNull(query, nameof(query));
            Check.Argument.IsNotNull(setting, nameof(setting));
            
            var productsName = _tasksService.GetProductsName(settingContext, query, setting);
            if (productsName == "") return;
            
            var messageNote = string.Format(Resource.ProductNote_18, productsName);
            setting.AddWarningMessages(messageNote);
        }
    }
}