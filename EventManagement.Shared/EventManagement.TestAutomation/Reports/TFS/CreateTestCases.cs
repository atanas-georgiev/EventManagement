using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Reports.TFS
{
    public class CreateTestCases
    {
        public void CreateTestCasesFromAutomatedTests(string tfsURL, string tfsProjectName, int generalAutomationPlanID)
        {
            //Connect to Project by name
            TfsTeamProjectCollection projectCollection = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsURL));
            ITestManagementTeamProject teamProject = projectCollection.GetService<ITestManagementService>().GetTeamProject(tfsProjectName);
            //Connect to Plan by ID
            ITestPlan foundPlan = teamProject.TestPlans.Find(generalAutomationPlanID);
            IStaticTestSuite newSuite = teamProject.TestSuites.CreateStatic();
            newSuite.Title = "My Suite";

        }
        
    }
}
