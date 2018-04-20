using EventManagement.TestAutomation.DateAndTime;
using EventManagement.TestAutomation.StringOperations;
using NUnit.Framework;
using OpenQA.Selenium;
using Protractor;
using AventStack.ExtentReports;
using System;
using System.Linq.Expressions;

namespace EventManagement.TestAutomation
{
    /// <summary>
    /// Asserts for testing
    /// </summary>
    public class Asserts
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
        public Asserts(IWebDriver driver, ExtentTest extest, string[] reports)
        {
            this.driver = driver;
            this.extest = extest;
            this.reportsLibrary = new Reports.Reports(driver, extest);
            this.reportTypes = reports;
        }

        public Asserts(IWebDriver driver)
        {
            this.driver = driver;
        }

        /// <summary>
        /// Initialize asserts class with ExtentX reporting
        /// </summary>
        /// <param name="extest"></param>
        public Asserts(ExtentTest extest)
        {
            this.extest = extest;
        }

        public Asserts(IWebDriver driver, string testName, string[] reports)
        {
            this.driver = driver;
            this.reportsLibrary = new Reports.Reports(driver, testName);
            this.reportTypes = reports;
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

        /// <summary>
        /// Put Failed result with error messages for: AssemblyNotinitialized,CantResolveClassType, MissingClassType, CantResolveInerface;
        /// </summary>
        /// <param name="className"></param>
        /// <param name="errorType"></param>
        public void RecordCantFindErrors(string className, string errorType)
        {
            string errormessage = "don't have template for this error mesage";
            switch (errorType)
            {
                case "AssemblyNotinitialized":
                    errormessage = "Missing delegate. Page object assembly is missing";
                    break;
                case "MissingClassType":
                    errormessage = "Missing delegate. Missing ClassType: " + className;
                    break;
                case "CantResolveClassType":
                    errormessage = "Missing delegate. Can't resolve ClassType: " + className;
                    break;
                case "CantResolveInerface":
                    errormessage = "Missing delegate. Can't resolve interface: " + className;
                    break;
                default:
                    break;
            }
            reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fatal", errormessage);
            Assert.Fail();
        }

        /// <summary>
        /// Remove next row from real value and replace it with " " 
        /// </summary>
        /// <param name="realValue">real value</param>
        /// <returns></returns>
        public string FormattRealValue(string realValue)
        {
            if (realValue.IndexOf("\r\n") > 0)
            {
                return realValue.Replace("\r\n", " ");
            }
            else
            {
                return realValue;
            }  
        }

        ///// <summary>
        ///// Put Failed result in ExtentEx report with picture and specific error message;
        ///// </summary>
        ///// <param name="errorMessage">The error message.</param>
        ///// <param name="driver">Instance of the driver.</param>
        ///// <returns>Put Failed result in ExtentEx report that element is missing.</returns>
        //public void WriteErrorResult(string errorMessage, Status fatalORFailed)
        //{
        //    string dateFormatFileNameStr = Formats.DateFormatYYYYMMDDHhMMminSSsecShort();
        //    string testName = extest.GetModel().Parent.Name.Replace(" ","").Split('-')[0];
            
        //    GetScreenshotInterface().GetScreenshot().SaveAsFile(Data.DictionaryInteractions.ReportPropertiesDictionary["ReportPath"] + "/"+testName + dateFormatFileNameStr + ".png", ScreenshotImageFormat.Png);
        //    extest.Log(fatalORFailed, errorMessage + "<br><b>StackTrace: </b><br>" + ReturnString.FormatingStackTrace(Environment.StackTrace), MediaEntityBuilder.CreateScreenCaptureFromPath(Data.DictionaryInteractions.ReportPropertiesDictionary["ReportPath"] + "/screenshot" + dateFormatFileNameStr + ".png").Build());
        //    extest.Info("The screenshot for ExtentX reports: ", MediaEntityBuilder.CreateScreenCaptureFromPath("uploads/screenshot" + dateFormatFileNameStr + ".png").Build());
        //    Console.Write(fatalORFailed.ToString() + errorMessage + System.Environment.NewLine + "Screenshot: "+ testName + dateFormatFileNameStr + ".png" + System.Environment.NewLine + "StackTrace:" + System.Environment.NewLine + ReturnString.FormatingStackTrace(Environment.StackTrace));
        //    var test2 = extest.GetModel().Parent.Name;

        //    if (fatalORFailed == Status.Fatal)
        //    {
                
        //        Assert.Fail();
        //    }
        //}

        //public void WritePassResult(string passMessage)
        //{
        //    Console.Write("PASS: " + passMessage);
        //    extest.Log(Status.Pass, passMessage);
        //}

        public string DefineFullElementName(Func<IWebElement> elementFactory, string fullElementName)
        {
            if (fullElementName == string.Empty)
            {
                var element1 = elementFactory();
                Expression<Func<IWebElement>> kk = () => element1;
                var methodName = ((MethodCallExpression)kk.Body).Method.ReflectedType.FullName.Split('.');
               return methodName[methodName.Length - 2] + "." + methodName[methodName.Length - 1] + "." + ((MethodCallExpression)kk.Body).Method.Name;
            }
            else
            {
                return fullElementName;
            }
        }

        /// <summary>
        /// Validate if property of the element have specific value and record the result in ExtentEx report. First wait X seconds element to become clickable.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to validate.</param>
        /// <param name="propertyName">The name of the property that we want to check.</param>
        /// <param name="expectedValue">Value that we expect the property to have.</param>
        /// <param name="fullElementName">Element Name as string.
        /// 1.)Put the name of the element as string. If the name of the element is different than method name which is in elementFactory. We use this with speckflow tests.
        /// 2.)Leave empty if the name of the method in elementFactory is the name of the element</param>
        /// <param name="secondsToWait">How many seconds to wait element to become clickable</param>
        public void ValidateProperty(Func<IWebElement> elementFactory, String propertyName, String expectedValue, string fullElementName, int secondsToWait = 30)

        {
            ElementInteractions elInt = new ElementInteractions(driver);
            IWebElement element;
            fullElementName = DefineFullElementName(elementFactory, fullElementName);

            try
            {

                element = elementFactory();
                elInt.WaitForElement(secondsToWait, element);
                var pp = FormattRealValue(element.GetAttribute(propertyName).Trim());
                if (!pp.Equals(expectedValue.Trim()))
                {
                    reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "Element is: " + fullElementName + ". Property Name is " + propertyName + ". Expected value is |" + expectedValue.Trim() + "|, but actuall we see |" + pp + "| ");
                }
                else
                {
                    reportsLibrary.WritePassResult("Element is: " + fullElementName + " and Property Name is " + propertyName + " and expected value is |" + expectedValue.Trim() + "|,actuall value is |" + pp + "| ", reportTypes);
                }
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fatal", "Element " + fullElementName + " is missing. <br><b>ErrorMessage: </b><br>" + ex.Message);
                return;
            }
        }

        /// <summary>
        /// Verify Displayed and Enabled statuses of the element and record the result in ExtentEx report. If you leave empty enabledStatus or displayedStatus property the method will not check this status.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to validate.</param>
        /// <param name="fullElementName">Element Name as string.
        /// 1.)Put the name of the element as string. If the name of the element is different than method name which is in elementFactory. We use this with speckflow tests.
        /// 2.)Leave empty if the name of the method in elementFactory is the name of the element</param>
        /// <param name="enabledStatus">Expected condition of element.Enabled status. String value "true" or "false"</param>
        /// <param name="displayedStatus">Expected condition of element.Displayed status. String value "true" or "false"</param>
        /// <param name="secondsToWait">If the driver is not protractor driver. How much time to wait element to become clickable.</param>
        public void VerifyElementDisplayedEnabled(Func<IWebElement> elementFactory, string enabledStatus, string displayedStatus, string fullElementName, int secondsToWait = 30)
        {
            ElementInteractions elInt = new ElementInteractions(driver);
            IWebElement element;
            fullElementName = DefineFullElementName(elementFactory, fullElementName);
           
            try
            {
                element = elementFactory();
                elInt.WaitForElement(secondsToWait, element);
                if(!(enabledStatus == string.Empty))
                {
                    if(element.Enabled.ToString().ToLower() == enabledStatus)
                    {
                        reportsLibrary.WritePassResult("For element" + fullElementName + " we expect Enabled status to be: |" + enabledStatus + "|, and it actual is |" + element.Enabled.ToString().ToLower() + "| ", reportTypes);
                    }
                    else
                    {
                        reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "For element" + fullElementName + " we expect Enabled status to be: |" + enabledStatus + "|, but actual status is |" + element.Enabled.ToString().ToLower() + "| ");
                    }
                }
                else
                {
                    reportsLibrary.WritePassResult("For element" + fullElementName + " we don't want to check Enabled status", reportTypes);
                }

                if (!(displayedStatus == string.Empty))
                {
                    if (element.Displayed.ToString().ToLower() == displayedStatus)
                    {
                        reportsLibrary.WritePassResult("For element" + fullElementName + " we expect Displayed status to be: |" + displayedStatus + "|, and it actual is |" + element.Displayed.ToString().ToLower() + "|", reportTypes);
                    }
                    else
                    {
                        reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "For element" + fullElementName + " we expect Displayed status to be: |" + displayedStatus + "|, but actual status is |" + element.Displayed.ToString().ToLower() + "|");
                    }
                }
                else
                {
                    reportsLibrary.WritePassResult("For element" + fullElementName + " we don't want to check Displayed status", reportTypes);
                }
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fatal", "Element " + fullElementName + " is missing. <br><b>ErrorMessage: </b><br>" + ex.Message);
                return;
            }
        }

        // IsElementNotThere
        /// <summary>
        /// Validate if element is NOT on page and record the result in ExtentEx report.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to validate.</param>
        /// <param name="fullElementName">Element Name.
        /// 1.)Put the name of the element as string. If the name of the element is different than method name which is in elementFactory. We use this with speckflow tests.
        /// 2.)Leave empty if the name of the method in elementFactory is the name of the element</param>
        public void VerifyThatElementIsNOTInDOM(Func<IWebElement> elementFactory, string fullElementName)
        {
            IWebElement element;
            fullElementName = DefineFullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", "Element: " + fullElementName + " is there but it must not be.");
            }
            catch
            {
                reportsLibrary.WritePassResult("Element is NOT in the page. ", reportTypes);
                return;
            }
        }

        // IsElementChecked
        /// <summary>
        /// Validate if element is NOT on page and record the result in ExtentEx report.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to validate.</param>
        /// <param name="fullElementName">Element Name.</param>
        /// <param name="secondsToWait">If the driver is not protractor driver. How much time to wait element to become clickable.</param>
        public void IsElementChecked(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30)
        {
            ElementInteractions elInt = new ElementInteractions(driver);
            IWebElement element;
            fullElementName = DefineFullElementName(elementFactory, fullElementName);

            try
            {

                element = elementFactory();
                elInt.WaitForElement(secondsToWait, element);
                
                if ((element.GetAttribute("checked"))=="true")
                {
                    reportsLibrary.WritePassResult(fullElementName + " is checked", reportTypes);
                }
                else
                {
                    reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", fullElementName + "must be checked but it is not.");
                   
                }
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fatal", "Element " + fullElementName + " is missing. <br><b>ErrorMessage: </b><br>" + ex.Message);
                return;
            }
        }

        // IsElementNotChecked
        /// <summary>
        /// Validate if element is NOT on page and record the result in ExtentEx report.
        /// </summary>
        /// <param name="elementFactory">Factory that contains only the element that we want to validate.</param>
        /// <param name="fullElementName">Element Name.</param>
        /// <param name="secondsToWait">If the driver is not protractor driver. How much time to wait element to become clickable.</param>
        public void IsElementNotChecked(Func<IWebElement> elementFactory, string fullElementName, int secondsToWait = 30)
        {
            ElementInteractions elInt = new ElementInteractions(driver);
            IWebElement element;
            fullElementName = DefineFullElementName(elementFactory, fullElementName);

            try
            {
                element = elementFactory();
                elInt.WaitForElement(secondsToWait, element);
                var zz = element.GetAttribute("checked");
                if ((element.GetAttribute("checked")) != "true")
                {
                    reportsLibrary.WritePassResult(fullElementName + " is NOT checked", reportTypes);
                }
                else
                {
                    reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fail", fullElementName + "must be NOT checked but it is checked.");
                }
            }
            catch (Exception ex)
            {
                reportsLibrary.WriteErrorMessageWithScreenshot(reportTypes, "fatal", "Element " + fullElementName + " is missing. <br><b>ErrorMessage: </b><br>" + ex.Message);
                return;
            }
        }

        // SetTestStatus
        /// <summary>
        /// Will fail the test in VS if we have recorded Failed step in ExtentExReport. Call this method at the end of every test.
        /// </summary>
        /// <param name="test">Object where we collect the results from the validateion.</param>
        /// <returns>Put the Failed Pass status in VS.</returns>
        public void SetTestStatus()
        {
            var status = extest.Status;
            if (status == Status.Fail || status == Status.Fatal)
            {
                Assert.Fail();
            }
        }
    }
}
