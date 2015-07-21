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
            Task.Factory.StartNew(() =>
            {
                //// User A
                var factory = new SettingTypeServiceFactory();

                var settingTypeService = factory.GetSettingTypeService();
                settingTypeService.UpdateSettingType("User A");
            });

            Task.Factory.StartNew(() =>
            {
                //// User B
                var factory = new SettingTypeServiceFactory();

                var settingTypeService = factory.GetSettingTypeService();
                settingTypeService.UpdateSettingType("User B");
            });

            Console.ReadLine();
        }
    }
}