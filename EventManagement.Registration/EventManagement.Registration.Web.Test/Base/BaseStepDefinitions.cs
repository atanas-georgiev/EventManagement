using KPMG.TestAutomation.StringOperations;
using KPMG.TestAutomation.TestCore.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace EventManagement.Registration.Web.Test.Base
{
    [Binding]
    public static class BaseStepDefinitionsReInitialization
    {
        /// <summary>
        /// Serach specific class type in Shared Project
        /// </summary>
        /// <param name="className">The name of the page(class) from Page_Object</param>
        /// <returns>return the type of the page(class)</returns>
        public static Type FindTypeByName(string className)
        {
            Assembly a = typeof(BaseStepDefinitionsReInitialization).Assembly;
            foreach (AssemblyName an in a.GetReferencedAssemblies())
            {
                Debug.WriteLine("AssemblyName: " + an.Name);
                if (an.Name.Contains("KPMG.") || an.Name.Contains(".Shared"))
                {
                    Assembly b = Assembly.Load(an.FullName);
                    foreach (Type t in b.GetExportedTypes())
                    {
                        if (t.Name == className)
                        {
                            return t;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Run before all fetures. ExtentEx reports connected.
        /// </summary>
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            //Helpers hh = new Helpers();
            //hh.CreateUpdateTC();
            //var filett = System.IO.File.ReadAllText(ReturnPath.ProjectFolderPath() + "Features/AbvSecond.feature");

            // Initialize page Specflow_POC.Shared.Page_Objects
            BaseStepDefinitions.FindTypeByNameMethod = BaseStepDefinitionsReInitialization.FindTypeByName;
            EventManagement.Registration.Web.Test.Shared.RegisterClass.RegisterAssembly();

            //Console parametters don't allow ; in the parameter. So when we need DB connection string as parametter we use DB_0 parameter to show us how many parts have DB Connection string
            BaseStepDefinitions.BeforeTestRun(TestContext.Parameters["BR"], Reports());
        }

        /// <summary>
        /// Run before every Feature. Create extent object with name of the Feature.
        /// </summary>
        [BeforeFeature]
        public static void BeforeFeature()
        {
            BaseStepDefinitions.BeforeFeature(TestContext.Parameters["BR"], Reports());
        }

        /// <summary>
        /// Run before every Scenario. Use it if in this scenario you will use selenium drivers.
        /// Open the url specified in RunTimeSettings.runsettings
        /// </summary>
        [BeforeScenario]
        public static void BeforeWebScenario()
        {
            var allTags = ScenarioContext.Current.ScenarioInfo.Tags;
            if (ReturnString.StringIsValueOfArray(allTags, "Protractor"))
            {
                BaseStepDefinitions.BeforeWebScenarioProtractor(TestContext.Parameters["BR"], TestContext.Parameters["URL"], TestContext.Parameters["AuthUser"], TestContext.Parameters["AuthPass"], TestContext.Parameters["AuthType"]);
            }
            else
            {
                BaseStepDefinitions.BeforeWebScenarioSelenium(TestContext.Parameters["BR"], TestContext.Parameters["URL"], TestContext.Parameters["AuthUser"], TestContext.Parameters["AuthPass"], TestContext.Parameters["AuthType"]); //iebasicauthentication; chromebasicauthentication
            }
        }

        /// <summary>
        /// Run before every Scenario. Add Scenario to Extend Report
        /// </summary>
        [BeforeScenario]
        public static void RegisterPages()
        {
            BaseStepDefinitions.RegisterPages(Reports());
        }

        /// <summary>
        /// Run before every Step. Add the step to the Scenario in ExtentX report.
        /// </summary>
        [BeforeStep]
        public static void BeforeWebStepExtent()
        {
            BaseStepDefinitions.BeforeWebStepExtent(Reports());
        }

        /// <summary>
        /// Run after every Step. Put status to the step in ExtentX report.
        /// </summary>
        [AfterStep]
        public static void AfterWebStepExtent()
        {
            BaseStepDefinitions.AfterWebStepExtent(Reports());
        }

        /// <summary>
        /// Run after every Scenario. Close the Driver and record the ExtentX report
        /// </summary>
        [AfterScenario]
        public static void AfterScenario()
        {
            BaseStepDefinitions.AfterScenario(Reports());
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            //BaseStepDefinitions.AfterTestRun();
        }

        private static string[] Reports()
        {
            return TestContext.Parameters["Report"].Split(',');
        }

    }
}
