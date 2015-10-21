namespace Fanex.CustomerSetting.Experiments
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CustomerSetting.Experiments.Concurrent;
    using Fanex.CustomerSetting.Experiments.WebApi;

    internal class Program
    {
        private static void Main(string[] args)
        {
            ////CustomerSettingClient.TestUpdate();
            ////CustomerSettingClient.TestMultipleUpdate();
            ////RunConcurrentExperiment();
        }

        private static void RunConcurrentExperiment()
        {
            var factory = new SettingTypeServiceFactory();

            Task.Factory.StartNew(() =>
            {
                var settingTypeService = factory.GetSettingTypeService();
                settingTypeService.UpdateSettingType("Setting Type A");
            });

            Task.Factory.StartNew(() =>
            {
                var settingTypeService = factory.GetSettingTypeService();
                settingTypeService.UpdateSettingType("Setting Type  B");
            });

            Task.Factory.StartNew(() =>
            {
                var settingTypeService = factory.GetSettingTypeService();
                settingTypeService.UpdateSettingType("Setting Type  C");
            });

            Console.ReadLine();
        }
    }
}