namespace Fanex.CustomerSetting.IntegrationTests
{
    using System;
    using Fanex.CustomerSetting.IntegrationTests.Products.Product255;

    public class Program
    {
        private static void Main()
        {
            var masterEditsAgentTests = new Product255_MasterEditsAgent_IntegrationTests();

            masterEditsAgentTests.Setup();

            Console.WriteLine("Begin Tests.");

            masterEditsAgentTests.Product255_CompanyViewsAgentSetting();
            masterEditsAgentTests.Product255_CompanyUpdatesAgentSetting();
            masterEditsAgentTests.Product255_MasterViewsAgentSetting();
            masterEditsAgentTests.Product255_MasterUpdatesAgentSetting();
            masterEditsAgentTests.Product255_MasterUpdatesMultipleAgentsSetting();

            Console.WriteLine("End Tests.");
        }
    }
}