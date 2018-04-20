using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Protractor;
using AventStack.ExtentReports;
using System;
using System.Linq.Expressions;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace EventManagement.TestAutomation
{
    /// <summary>
    /// Class with methods that interact with elements like click, on mouse over...
    /// </summary>
    public class ElementInteractions
    {

        private readonly IWebDriver driver;
        private ExtentTest extest;
        Reports.Reports reportsLibrary;
        private string[] reportTypes;

        /// <summary>
        /// ElementInteractions constructor with Extent report
        /// </summary>
        /// <param name="driver">Instance of the driver.</param>
        /// <param name="extest">ExtentTest object where we add the log.</param>
        public ElementInteractions(IWebDriver driver, ExtentTest extest, string[] reports)
        {
            this.driver = driver;
            this.extest = extest;
            this.reportsLibrary = new Reports.Reports(driver, extest);
            this.reportTypes = reports;
        }

        public ElementInteractions(IWebDriver driver, ExtentTest extest)
        {
            this.driver = driver;
            this.extest = extest;
            this.reportsLibrary = new Reports.Reports(driver, extest);
        }

        public ElementInteractions(IWebDriver driver, string testName, string[] reports)
        {
            this.driver = driver;
            this.reportsLibrary = new Reports.Reports(driver, testName);
            this.reportTypes = reports;
        }

        /// <summary>
        /// Wait WebDriver for specified as parametter seconds
        /// </summary>
        /// <param name="seconds">How many seconds to wait.</param>
        /// <returns>New object from WebDriverWait(Selenium) type.</returns>
        public WebDriverWait DriverWait(int seconds)
        {
            return new WebDriverWait(driver, new TimeSpan(0, 0, seconds));
        }

        public ElementInteractions(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void MissingElement(string fullElementName, string exseptionMessage, string fatalORFailed = "Fatal")
        {
            reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, fatalORFailed.ToLower(), "Element " + fullElementName + " is missing. <br><b>ErrorMessage: </b><br>" + exseptionMessage);
        }

        public string FullElementName(Func<IWebElement> elementFactory, string fullElementName)
        {
            Asserts asserts = new Asserts(driver);
            return asserts.DefineFullElementName(elementFactory, fullElementName);

        }

        /// <summary>
        /// Wait depending on Protractor or Selenium Driver
        /// </summary>
        /// <param name="seconds">How many seconds to wait.</param>
        /// <param name="element">Element to wait.</param>
        public void WaitForElement(int secondsToWait, IWebElement element)
        {
            if (driver is NgWebDriver)
            {
                ((NgWebDriver)driver).WaitForAngular();
            }
            else
            {

                DriverWait(secondsToWait).Until(ExpectedConditions.ElementToBeClickable(element));
            }
        }

        /// <summary>
        /// Wait element to become clickable and click it when we have element Name as string.
        /// </summary>
        /// <param name="elementFactory">Factory that contains IWebElement that we want to click on.</param>
        /// <param name="fullElementName">The name of the element(method name) in page object</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="fatalORFailed">If the parametter is Status.Fatal - the test will stop on missing element error. If this parametter is Status.Failed - the test will conntinue on missing element error. By default is set to Fatal.</param>

        public void ClickElement(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            
            IWebElement element;
            IWebElement elementSecond;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                WaitForElement(secondsToWait, element);
                element.Click();
            }
            catch
            {
                try
                {
                    Thread.Sleep(1000);
                    elementSecond = elementFactory();
                    WaitForElement(secondsToWait, elementSecond);
                    elementSecond.Click();
                }
                catch (Exception exSecond)
                {
                    MissingElement(fullElementName, exSecond.Message, fatalORFailed);
                    return;
                }
            }
        }

        /// <summary>
        /// Wait element to become clickable and Mouse Over an element.
        /// </summary>
        /// <param name="elementFactory">Factory that contains IWebElement that we want to mouse over.</param>
        /// <param name="fullElementName">The name of the element(method name) in page object</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is fatal - the test will stop on missing element error. If this parametter is failed - the test will conntinue on missing element error. By default is set to fatal.</param>
        public void OnMouseOver(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                WaitForElement(secondsToWait, element);
                OpenQA.Selenium.Interactions.Actions action = new OpenQA.Selenium.Interactions.Actions(driver);
                action.MoveToElement(element).Perform();
            }
            catch (Exception ex)
            {
                MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Click on element after an attribute of other element have specific value.
        /// </summary>
        /// <param name="elementFactoryCheck">Factory that contains only the element which attribute we want to check.</param>
        /// <param name="fullNameElementToCheck">The name(method name) of the element that we want to check</param>
        /// <param name="elementFactoryClick">Factory that contains only the element that we want to click on.</param>
        /// <param name="fullNameEmentToClick">The name(method name) of the element that we want to click</param>
        /// <param name="attributToCheck">The name of attribute that we want to check</param>
        /// <param name="expectedValue">Expected value of the attribute.</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error.</param>
        public void ClickElementAfterCheckProperty(Func<IWebElement> elementFactoryCheck, string fullNameElementToCheck, Func<IWebElement> elementFactoryClick, string fullNameEmentToClick, string attributToCheck, string expectedValue, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            IWebElement elementToCheck;
            IWebElement elementToClick;
            fullNameElementToCheck = FullElementName(elementFactoryCheck, fullNameElementToCheck);
            fullNameEmentToClick = FullElementName(elementFactoryClick, fullNameEmentToClick);

            try
            {

                elementToCheck = elementFactoryCheck();
                elementToClick = elementFactoryClick();
                WaitForElement(secondsToWait, elementToCheck);
                WaitForElement(secondsToWait, elementToClick);
                if (elementToCheck.GetAttribute(attributToCheck).Trim().Equals(expectedValue.Trim()))
                {
                    elementToClick.Click();
                }

            }
            catch (Exception ex)
            {
                MissingElement(fullNameElementToCheck + " or " + fullNameEmentToClick, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Enter text in input(edit) field.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to mouse over.</param>
        /// <param name="text">The text that we want to enter in the element.</param>
        /// <param name="fullElementName">The name(method name) of the element that we want to enter text in.</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is fatal - the test will stop on missing element error. If this parametter is failed - the test will conntinue on missing element error. By default is set to fatal.</param>
        public void SendKeysElement(Func<IWebElement> elementFactory, string text, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                WaitForElement(secondsToWait, element);
                if(element.GetAttribute("innerText")!=string.Empty|| element.GetAttribute("value") != string.Empty)
                {
                    element.Clear();
                }
                element.SendKeys(text);
            }
            catch (Exception ex)
            {
                MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Click "Enter" button for specific element.
        /// </summary>
        /// <param name="elementExpression">Factory that contains only the element that we want to mouse over.</param>
        /// /// <param name="fullElementName">The name(method name) of the element that we want to click on.</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error..</param>
        public void ClickEnterButtonForElement(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                WaitForElement(secondsToWait, element);
                element.SendKeys(Keys.Return);
            }
            catch (Exception ex)
            {
                MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Switch to specific frame.
        /// </summary>
        /// <param name="elementExpression">Expression that contains only the element that we want to mouse over.</param>
        /// <param name="FatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error..</param>
        public void SwitchToFrame(Expression<Func<IWebElement>> elementExpression, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            var methodName = ((MethodCallExpression)elementExpression.Body).Method.ReflectedType.FullName.Split('.');
            var fullElementName = methodName[methodName.Length - 2] + "." + methodName[methodName.Length - 1] + "." + ((MethodCallExpression)elementExpression.Body).Method.Name;

            try
            {
                var elementFactory = elementExpression.Compile();
                element = elementFactory();
                driver.SwitchTo().Frame(element);
            }
            catch (Exception ex)
            {
                MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Switch to specific frame.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to mouse over.</param>
        /// <param name="fullElementName">The name(method name) of the element that we want to switch to.</param>
        /// <param name="FatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error.</param>
        public void SwitchToFrame(Func<IWebElement> elementFactory, string fullElementName, string fatalORFailed = "Fatal")
        {
            IWebElement element;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                driver.SwitchTo().Frame(element);
            }
            catch (Exception ex)
            {
                MissingElement(fullElementName, ex.Message, fatalORFailed);
                return;
            }
        }

        /// <summary>
        /// Swithing to alert and accept it. Then back to main window.
        /// </summary>
        public void AcceptAlert()
        {
            try
            {
                Thread.Sleep(1000);
                IAlert alertMA = driver.SwitchTo().Alert();
                alertMA.Accept();
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "Some error while accepting alert. <br><b>ErrorMessage: </b><br>" + ex.Message);
            }
            
        }

        /// <summary>
        /// Handles the Windows Security authentication popup
        /// Supported from Selenium 3.0
        /// </summary>
        /// <param name="username">The user name e.g domain\\username</param>
        /// <param name="password">The user password</param>
        public void Authenticate(string username, string password)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                alert.SetAuthenticationCredentials(username, password);
                alert.Accept();
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "Some error while authenticate with user: " + username + ". <br><b>ErrorMessage: </b><br>" + ex.Message);
            }
        }

        /// <summary>
        /// Move to element like scroll to element
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to move to.</param>
        /// <param name="fullElementName">The name(method name) of the element that we want to switch to.</param>
        /// <param name="secondsToWait">How many second to wait the element to become clickable. By default is 30 seconds</param>
        /// <param name="FatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error..</param>
        public void MoveToElement(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30, string fatalORFailed = "Fatal")
        {

            IWebElement element;
            IWebElement elementSecond;
            fullElementName = FullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                WaitForElement(secondsToWait, element);
                Actions actions = new Actions(driver);
                actions.MoveToElement(element);
                actions.Perform();
            }
            catch
            {
                try
                {
                    Thread.Sleep(1000);
                    elementSecond = elementFactory();
                    WaitForElement(secondsToWait, elementSecond);
                    Actions actions = new Actions(driver);
                    actions.MoveToElement(elementSecond);
                    actions.Perform();
                }
                catch (Exception exSecond)
                {

                    MissingElement(fullElementName, exSecond.Message, fatalORFailed.ToLower());
                    return;
                }
            }
        }

        /// <summary>
        /// Execute JS
        /// </summary>
        /// <param name="scriptToExecute">the string of the js</param>
        /// <param name="fatalORFailed">If the parametter is Fatal - the test will stop on missing element error. If this parametter is Failed - the test will conntinue on missing element error..</param>
        public void JSExecute(String scriptToExecute, string fatalORFailed = "Fail")
        {
            try
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)this.driver;
                js.ExecuteScript(scriptToExecute);
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, fatalORFailed.ToLower(), "Error when we execute JS: |" + scriptToExecute + "|. <br><b>ErrorMessage: </b><br>" + ex.Message);
                return;
            }
        }

        public void OpenURL(string url)
        {
            driver.Navigate().GoToUrl(url);
        }

    }
}
