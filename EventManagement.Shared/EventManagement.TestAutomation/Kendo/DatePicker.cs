using AventStack.ExtentReports;
using OpenQA.Selenium;
using System;
using EventManagement.TestAutomation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Kendo
{
    public class DatePicker
    {
        private readonly IWebDriver driver;
        private readonly ExtentTest extest;
        Reports.Reports reportsLibrary;
        private string[] reportTypes;

        /// <summary>
        /// Initialize asserts class with Driver and ExtentX reporting
        /// </summary>
        /// <param name="driver">Selenium IwebDriver</param>
        /// <param name="extest">ExtentTest from ExtentX reporting</param>
        public DatePicker(IWebDriver driver, ExtentTest extest, string[] reports)
        {
            this.driver = driver;
            this.extest = extest;
            this.reportsLibrary = new Reports.Reports(driver, extest);
            this.reportTypes = reports;
        }

        /// <summary>
        /// Initialize asserts class with ExtentX reporting
        /// </summary>
        /// <param name="extest"></param>
        public DatePicker(ExtentTest extest)
        {
            this.extest = extest;
        }

        public DatePicker(IWebDriver driver, string testName, string[] reports)
        {
            this.driver = driver;
            this.reportsLibrary = new Reports.Reports(driver, testName);
            this.reportTypes = reports;
        }

        /// <summary>
        /// Enter text in input(edit) field.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to mouse over.</param>
        /// <param name="text">The text that we want to enter in the element.</param>
        /// <param name="fullElementName">The name(method name) of the element that we want to enter text in.</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is fatal - the test will stop on missing element error. If this parametter is failed - the test will conntinue on missing element error. By default is set to fatal.</param>
        public void SendKeysDatePicker(Func<IWebElement> elementFactory, string text, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            Asserts asserts = new Asserts(driver);
            ElementInteractions elInteraction = new ElementInteractions(driver, extest);
            fullElementName = asserts.DefineFullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                elInteraction.WaitForElement(secondsToWait, element);
                element.SendKeys(Keys.ArrowLeft);
                element.SendKeys(Keys.ArrowLeft);
                element.Clear();
                element.SendKeys(text);
            }
            catch (Exception ex)
            {
                elInteraction.MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }
    }
}
