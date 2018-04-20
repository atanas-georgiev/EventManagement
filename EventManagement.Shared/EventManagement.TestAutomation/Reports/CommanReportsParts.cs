using AventStack.ExtentReports;
using EventManagement.TestAutomation.DateAndTime;
using NUnit.Framework;
using OpenQA.Selenium;
using Protractor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Reports
{
    public partial class Reports
    {
        private readonly IWebDriver driver;
        private readonly ExtentTest extest;
        private readonly string testName;
        private string dateFormatFileNameStr = Formats.DateFormatYYYYMMDDHhMMminSSsecShort();
        private bool extentErrorCalledAlready;
       

        public Reports(IWebDriver driver, ExtentTest extest)
        {
            this.driver = driver;
            this.extest = extest;
            this.testName = extest.GetModel().Parent.Name.Replace(" ", "").Split('-')[0];
            
        }

        public Reports(IWebDriver driver, string testName)
        {
            this.driver = driver;
            this.testName = testName.Replace(" ", "").Split('-')[0];
        }

        /// <summary>
        /// Take screenshot. We can use protractor or selenium driver
        /// </summary>
        /// <returns></returns>
        public ITakesScreenshot GetScreenshotInterface()
        {
            if (this.driver is NgWebDriver)
            {
                return (ITakesScreenshot)((NgWebDriver)driver).WrappedDriver;
            }
            else
            {
                return (ITakesScreenshot)driver;
            }
        }

        public void MainScreenshot(IWebDriver driver)
        {
            GetScreenshotInterface().GetScreenshot().SaveAsFile(Data.DictionaryInteractions.ReportPropertiesDictionary["ReportPath"] + "/" + testName + dateFormatFileNameStr + ".png", ScreenshotImageFormat.Png);
        }

        public void WritePassResult(string passMessage, string[] reports)
        {
            if (StringOperations.ReturnString.StringIsPartOfArray(reports, "extent"))
            {
                WriteExtentPassResult(passMessage);
            }
            if (StringOperations.ReturnString.StringIsValueOfArray(reports, "tfs"))
            {
                WriteTFSPassResult(passMessage);
            }
        }

        public void WriteErrorMessageWithScreenshot(string[] reports, string fatalORFailed, string errorMessage)
        {
            extentErrorCalledAlready = false;

            GetScreenshotInterface().GetScreenshot().SaveAsFile(Data.DictionaryInteractions.ReportPropertiesDictionary["ReportPath"] + "/" + testName + dateFormatFileNameStr + ".png", ScreenshotImageFormat.Png);
            if (StringOperations.ReturnString.StringIsValueOfArray(reports, "extent"))
            {
                ErrorMessageExtent("extent", errorMessage, fatalORFailed);
            }
            if (StringOperations.ReturnString.StringIsValueOfArray(reports, "extentx"))
            {
                ErrorMessageExtent("extentx", errorMessage, fatalORFailed);
            }
            if (StringOperations.ReturnString.StringIsValueOfArray(reports, "tfs"))
            {
                ErrorMessageTFS(errorMessage, fatalORFailed);
            }
            if (fatalORFailed == "Fatal")
            {
                Assert.Fail();
            }
        }
    }
}
