using EventManagement.TestAutomation;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using EventManagement.TestAutomation.Kendo;
using EventManagement.TestAutomation.StringOperations;

namespace EventManagement.TestAutomation.TestCore.Base
{
    public abstract class BasePage<T> : ICommanActions where T : IWebDriver
    {
        
        protected T driver;
        ElementInteractions actionSe;
        Asserts assertSe;
        DatePicker kendoSe;
        Reports.Reports reports;


        public BasePage(T driver)
        {
            this.driver = driver;
            var test = BaseStepDefinitions.reportTypes;
            var testName = BaseStepDefinitions.testName;
            if(StringOperations.ReturnString.StringIsPartOfArray(BaseStepDefinitions.reportTypes, "extent"))
            {
                this.actionSe = new ElementInteractions(driver, BaseStepDefinitions.steps, BaseStepDefinitions.reportTypes);
                this.assertSe = new Asserts(driver, BaseStepDefinitions.steps, BaseStepDefinitions.reportTypes);
                this.kendoSe = new DatePicker(driver, BaseStepDefinitions.steps, BaseStepDefinitions.reportTypes);
                this.reports = new Reports.Reports(driver, BaseStepDefinitions.steps);
            }
            else
            {
                this.actionSe = new ElementInteractions(driver, BaseStepDefinitions.testName, BaseStepDefinitions.reportTypes);
                this.assertSe = new Asserts(driver, BaseStepDefinitions.testName, BaseStepDefinitions.reportTypes);
                this.kendoSe = new DatePicker(driver, BaseStepDefinitions.testName, BaseStepDefinitions.reportTypes);
                this.reports = new Reports.Reports(driver, BaseStepDefinitions.testName);
            }
            
        }

        public virtual void Open(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("The main URL cannot be null or empty.");
            }
            this.driver.Navigate().GoToUrl(url);
        }

        /// <summary>
        /// Click element
        /// </summary>
        /// <param name="byElement">the By object that we will use to find our element</param>
        /// <param name="elementName">Element Name</param>
        public virtual void Click(By byElement, string elementName)
        {
            actionSe.ClickElement(() => this.driver.FindElement(byElement), elementName);
        }

        /// <summary>
        /// Check element property
        /// </summary>
        /// <param name="byEl">The element that we want to check as By object</param>
        /// <param name="propertyName">The name of the property that we want to check</param>
        /// <param name="expectedValue">Expected value of the property</param>
        /// <param name="elementName">The name of the element - string format.</param>
        public void CheckProperty(By byEl, string propertyName, string expectedValue, string elementName)
        {
            assertSe.ValidateProperty(() => this.driver.FindElement(byEl), propertyName, expectedValue, elementName);
        }


        public void EnterValueInField(By byElement, string valueToEnter, string elementName)
        {
            actionSe.SendKeysElement(() => this.driver.FindElement(byElement), valueToEnter, elementName);
        }

        public void EnterValueInDatePicker(By byElement, string valueToEnter, string elementName)
        {
            kendoSe.SendKeysDatePicker(() => this.driver.FindElement(byElement), valueToEnter, elementName);
        }

        public void CheckLabelsOnPage(Dictionary<string, string> dict, string className, object classObject, Type classType)
        {
            var labelsOnPageCount = dict.Keys.Count;
            var keyName1 = dict.Values;
            var tt = dict.Keys.ToString();
            foreach (var item in dict)
            {
                var q = item.Key;
                var zz = item.Value;

                var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == Regex.Replace(item.Key, @"\s+", "")).Invoke(classObject, null);
                assertSe.ValidateProperty(() => this.driver.FindElement(methodObject), "innerText", item.Value, string.Empty);
            }

        }

        public void VerifyCheckbox(By byElement, string checkedStatus, string elementName)
        {
            if (checkedStatus == "checked")
            {
                assertSe.IsElementChecked(() => this.driver.FindElement(byElement), elementName);
            }
            else
            {
                assertSe.IsElementNotChecked(() => this.driver.FindElement(byElement), elementName);
            }
            
        }

        /// <summary>
        /// Verify Displayed and Enabled statuses of the element and record the result in ExtentEx report. If you leave empty enabledStatus or displayedStatus property the method will not check this status.
        /// </summary>
        /// <param name="byElement">The element that we want to check as By object</param>
        /// <param name="enabledStatus">Expected condition of element.Enabled status. String value "true" or "false"</param>
        /// <param name="displayedStatus">Expected condition of element.Displayed status. String value "true" or "false"</param>
        /// <param name="elementName">The name of the element - string format</param>
        public void VerifyElementDisplayedEnabled(By byElement, string enabledStatus, string displayedStatus, string elementName)
        {
                assertSe.VerifyElementDisplayedEnabled(() => this.driver.FindElement(byElement), enabledStatus, displayedStatus, elementName);
        }

        /// <summary>
        /// Verify that element is not in page DOM
        /// </summary>
        /// <param name="byElement">The element that we want to check as By object</param>
        /// <param name="elementName">The name of the element - string format.</param>
        public void VerifyThatElementIsNOTInDOM(By byElement, string elementName)
        {
            assertSe.VerifyThatElementIsNOTInDOM(() => this.driver.FindElement(byElement), elementName);
        }

        /// <summary>
        /// Move to element like scroll to element
        /// </summary>
        /// <param name="byElement"></param>
        /// <param name="elementName"></param>
        public void MoveToElement(By byElement, string elementName)
        {
            actionSe.MoveToElement(() => this.driver.FindElement(byElement), elementName);
        }

        /// <summary>
        /// Execute JS
        /// </summary>
        /// <param name="scriptToExecute">Execute JS</param>
        public void JSExecute(String scriptToExecute)
        {
            actionSe.JSExecute(scriptToExecute);
        }

        public void OpenURL(string url)
        {
            actionSe.OpenURL(url);
        }
    }
}
