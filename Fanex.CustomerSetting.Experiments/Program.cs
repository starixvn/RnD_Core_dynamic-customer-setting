namespace Fanex.CustomerSetting.Experiments
{
    using System;
    using System.Threading.Tasks;
    using Fanex.CustomerSetting.Experiments.Concurrent;

    internal class Program
    {
        private static void Main(string[] args)
        {
            RunConcurrentExperiment();
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