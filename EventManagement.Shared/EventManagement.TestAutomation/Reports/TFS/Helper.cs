using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Reports.TFS
{
    public class Helper
    {
        public static void TFSConnection(string fileContent)
        {
            TfsTeamProjectCollection projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri("TFS Name"));
            ITestManagementTeamProject teamProject = projectCollection.GetService<ITestManagementService>().GetTeamProject("EventManagement");
            //Connect to Plan by ID
            ITestPlan foundPlan = teamProject.TestPlans.Find(5);


            var filePath = System.IO.File.ReadAllText(fileContent);
            var scenarios = filePath.Split(new string[] { "[NUnit.Framework.TestAttribute()]" }, StringSplitOptions.None);
            var featureName = scenarios[0].Split(new string[] { "[NUnit.Framework.TestFixtureAttribute()]" }, StringSplitOptions.None)[1].Split(new string[] { "[NUnit.Framework.DescriptionAttribute(\"" }, StringSplitOptions.None)[1].Split(new string[] { "\")" }, StringSplitOptions.None)[0].Trim();

            var sd = teamProject.TestSuites;
            IStaticTestSuite newSuite = teamProject.TestSuites.CreateStatic();
            //newSuite.Title = 

            foreach (string singleScenario in scenarios)
            {
                var scenarioElements = singleScenario.Split(new string[] { "\n\t" }, StringSplitOptions.None);

            }
        }    
    }
}
