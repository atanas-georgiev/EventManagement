namespace EventManagement.TestAutomation.TestCore.Base
{
    using System;
    using System.Globalization;
    using System.Xml;
    using AventStack.ExtentReports;
    using AventStack.ExtentReports.Gherkin.Model;
    using AventStack.ExtentReports.Reporter;
    using EventManagement.TestAutomation.Data;
    using EventManagement.TestAutomation.Paths;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;
    using OpenQA.Selenium;
    using Protractor;
    using TechTalk.SpecFlow;

    public static class BaseStepDefinitions
    {
        public static ExtentReports extent;
        public static ExtentHtmlReporter extentHTML;
        public static ExtentXReporter extentX;
        public static ExtentTest feature;
        public static ExtentTest scenario;
        public static ExtentTest steps;
        private static string browserType;
        public static string[] reportTypes;
        public static string testName;

        public delegate Type FindTypeByName(String ClassName);
        public static FindTypeByName FindTypeByNameMethod;

        public static void Authenticate(string basicAuthUser, string basicAuthPass, string authType)
        {
            switch (authType.ToLower())
            {
                case "iebasicauthentication":
                    if (basicAuthUser != null & basicAuthPass != null)
                    {
                        ElementInteractions auth = new ElementInteractions(Driver.Browser, feature);
                        auth.Authenticate(basicAuthUser, basicAuthPass);
                    }
                    break;
                case "chromebasicauthentication":
                    Driver.Browser.Navigate().GoToUrl("https://DE%5" + basicAuthUser + ":"+ basicAuthPass + "");
                    break;
                default:
                    break;
            }
        }

        //[BeforeTestRun]
        public static void BeforeTestRun(string browserTypeFromConsole, string[] reports)
        {
            reportTypes = reports;
            var brType = browserTypeFromConsole;
            
            DateTime thisDay = DateTime.Now;
            var dictionary = DictionaryInteractions.ReadFromPropertiesFile(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt");
            DictionaryInteractions.WriteInTxtFileFromDictionary(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt", dictionary, "Suffix", thisDay.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture));
            string[] tokens = thisDay.ToString("yyyy.MM.dd HH:mm:ss", CultureInfo.InvariantCulture).Split(':');
            string pathString = System.IO.Path.Combine(ReturnPath.ProjectFolderPath() + "../TestResults/" + dictionary["projectName"] + "/", brType + "_TestReport-" + tokens[0] + "h " + tokens[1] + "min " + tokens[2] + "sec");
            System.IO.Directory.CreateDirectory(pathString);
            DictionaryInteractions.WriteInTxtFileFromDictionary(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt", dictionary, "ReportPath", pathString);

            if (StringOperations.ReturnString.StringIsValueOfArray(reports, "extentX"))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ReturnPath.ProjectFolderPath() + "ExtentReport/ExtentXReportConf.xml");
                XmlNode confNode = doc.SelectSingleNode("/configuration");
                confNode.FirstChild.FirstChild.Value = dictionary["projectName"] + "-" + tokens[0] + "h " + tokens[1] + "min " + tokens[2] + "sec-" + brType;//FeatureContext.Current.FeatureInfo.Title;
                doc.Save(ReturnPath.ProjectFolderPath() + "ExtentReport/ExtentXReportConf.xml");
                extentX = new ExtentXReporter();
                extentX.LoadConfig(ReturnPath.ProjectFolderPath() + "ExtentReport/ExtentXReportConf.xml");
                
                if (StringOperations.ReturnString.StringIsValueOfArray(reports, "extent"))
                {
                    extentHTML = new ExtentHtmlReporter(pathString + "/TestReport-" + tokens[0] + "h " + tokens[1] + "min " + tokens[2] + "sec-" + brType + ".html");
                    extent = new ExtentReports();
                    extent.AttachReporter(extentHTML, extentX);
                }
                else
                {
                    extent.AttachReporter(extentX);
                }
            }
            else
            {
                if (StringOperations.ReturnString.StringIsValueOfArray(reports, "extent"))
                {
                    extentHTML = new ExtentHtmlReporter(pathString + "/TestReport-" + tokens[0] + "h " + tokens[1] + "min " + tokens[2] + "sec-" + brType + ".html");
                    extent = new ExtentReports();
                    extent.AttachReporter(extentHTML);
                }
            }
        }

        //[BeforeFeature]
        public static void BeforeFeature(string browserTypeFromConsole, string[] reports)
        {
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                feature = extent.CreateTest(FeatureContext.Current.FeatureInfo.Title + "-" + browserTypeFromConsole);
            }
        }

        //[BeforeFeature("LanguageExtent")]
        public static void LanguageExtent(string mm)
        {
            FeatureContext.Current.Add("Language", mm);
            feature = extent.CreateTest(FeatureContext.Current.FeatureInfo.Title + "-" + mm);
        }


        //[BeforeScenario("Selenium")]
        public static void BeforeWebScenarioSelenium(string browserTypeFromConsole, string urlFromConsole, string authUser, string authPass, string authType)
        {
            var dictionary = DictionaryInteractions.ReadFromPropertiesFile(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt");
            DictionaryInteractions.WriteInTxtFileFromDictionary(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt", dictionary, "tfsReportStatus", "");
            var brType = browserTypeFromConsole;
            var urlToRun = urlFromConsole;
            switch (brType.ToLower())
            {
                case "ie":
                    Driver.StartBrowser(BrowserTypes.InternetExplorer);
                    Driver.Browser.Manage().Window.Maximize();
                    UnityContainerFactory.GetContainer().RegisterInstance<IWebDriver>(Driver.Browser);
                    browserType = "IE";
                    break;
                case "chrome":
                    Driver.StartBrowser(BrowserTypes.Chrome);
                    Driver.Browser.Manage().Window.Maximize();
                    UnityContainerFactory.GetContainer().RegisterInstance<IWebDriver>(Driver.Browser);
                    browserType = "Chrome";
                    break;
                default:
                    break;
            }
            
            //Driver.Browser.Navigate().GoToUrl(urlToRun);

            //if(authUser !=null & authPass !=null & authType != null)
            //{
            //    Authenticate(authUser, authPass, authType);
            //}
        }

        //[BeforeScenario("Protractor")]
        public static void BeforeWebScenarioProtractor(string browserTypeFromConsole, string urlFromConsole, string authUser, string authPass, string authType)
        {
            var brType = browserTypeFromConsole;
            var urlToRun = urlFromConsole;
    
            switch (brType.ToLower())
            {
                case "ie":
                    Driver.StartBrowser(BrowserTypes.ProtractorIE);
                    Driver.Browser.Manage().Window.Maximize();
                    UnityContainerFactory.GetContainer().RegisterInstance<IWebDriver>(Driver.Browser);
                    UnityContainerFactory.GetContainer().RegisterInstance<NgWebDriver>(Driver.NgDriver);
                    browserType = "IE";
                    break;
                case "chrome":
                    Driver.StartBrowser(BrowserTypes.ProtractorChrome);
                    Driver.Browser.Manage().Window.Maximize();
                    UnityContainerFactory.GetContainer().RegisterInstance<IWebDriver>(Driver.Browser);
                    UnityContainerFactory.GetContainer().RegisterInstance<NgWebDriver>(Driver.NgDriver);
                    browserType = "Chrome";
                    break;
                default:
                    break;
            }

            //Driver.Browser.Navigate().GoToUrl(urlToRun);
            //if (authUser != null & authPass != null & authType != null & authUser != string.Empty & authPass != string.Empty & authType != string.Empty)
            //{
            //    Authenticate(authUser, authPass, authType);
            //}
        }

        //[BeforeScenario]
        public static void RegisterPages(string[] reports)
        {
            var scenarioTags = ScenarioContext.Current.ScenarioInfo.Tags;
            ScenarioContext.Current.Add("className", "");
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                scenario = feature.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title + "-" + browserType);
            }

            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "tfs"))
            {
                testName = ScenarioContext.Current.ScenarioInfo.Title + "-" + browserType;
            }
            //var scenarioTags1 = ScenarioContext.Current.StepContext;
            //var scenarioTags2 = ScenarioContext.Current.StepContext;

        }

        //[BeforeStep]
        public static void BeforeWebStepExtent(string[] reports)
        {
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                var type = ScenarioStepContext.Current.StepInfo.StepDefinitionType;
                switch (type.ToString())
                {
                    case "Given":
                        steps = scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    case "Then":
                        steps = scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    case "When":
                        steps = scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    case "And":
                        steps = scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                        break;
                    default:
                        steps.Info("Scenario Info");
                        break;
                }
            }
        }

        //[AfterStep]
        public static void AfterWebStepExtent(string[] reports)
        {
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                var m = steps.Status;
                var ss = scenario.Status;
                steps.Log(steps.Status, ScenarioStepContext.Current.StepInfo.Text + ". Step end with status: " + steps.Status);
            }
        }


        //[AfterScenario]
        public static void AfterScenario(string[] reports)
        {
            Driver.StopBrowser();
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                extent.Flush();
                var status = scenario.Status;

                if (status == Status.Fail || status == Status.Fatal)
                {
                    Assert.Fail();
                }
            }
            else
            {
                if (StringOperations.ReturnString.StringIsValueOfArray(reports, "tfs"))
                {
                    var dictionary = DictionaryInteractions.ReadFromPropertiesFile(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt");
                    if (dictionary["tfsReportStatus"].ToLower() == "fail")
                    {
                        Assert.Fail();
                    }
                }
            }
            
            

        }
    }
    }
