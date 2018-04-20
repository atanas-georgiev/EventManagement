using AventStack.ExtentReports;
using EventManagement.TestAutomation.DateAndTime;
using EventManagement.TestAutomation.StringOperations;
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

        public void WriteExtentPassResult(string passMessage)
        {
            extest.Log(Status.Pass, passMessage);
        }

        public void ErrorMessageExtent(string extentReportType, string errorMessage, string fatalORFailed)
        {
            Status fatalOrFailedStatus;
            if(fatalORFailed.ToLower() == "fatal")
            {
                fatalOrFailedStatus = Status.Fatal;
            }
            else
            {
                fatalOrFailedStatus = Status.Fail;
            }

            if (extentErrorCalledAlready)
            {
                if (extentReportType == "extentx")
                {
                    GetScreenshotInterface().GetScreenshot().SaveAsFile(Data.DictionaryInteractions.ReportPropertiesDictionary["ExtentXLocation"] + "/screenshot" + dateFormatFileNameStr + ".png", ScreenshotImageFormat.Png);
                    extest.Info("The screenshot for ExtentX reports: ", MediaEntityBuilder.CreateScreenCaptureFromPath("uploads/screenshot" + dateFormatFileNameStr + ".png").Build());
                }
            }
            else
            {
                extest.Log(fatalOrFailedStatus, errorMessage + "<br><b>StackTrace: </b><br>" + ReturnString.FormatingStackTrace(Environment.StackTrace), MediaEntityBuilder.CreateScreenCaptureFromPath(Data.DictionaryInteractions.ReportPropertiesDictionary["ReportPath"] + "/" + testName + dateFormatFileNameStr + ".png").Build());
                if (extentReportType == "extentx")
                {
                    GetScreenshotInterface().GetScreenshot().SaveAsFile(Data.DictionaryInteractions.ReportPropertiesDictionary["ExtentXLocation"] + "/screenshot" + dateFormatFileNameStr + ".png", ScreenshotImageFormat.Png);
                    extest.Info("The screenshot for ExtentX reports: ", MediaEntityBuilder.CreateScreenCaptureFromPath("uploads/screenshot" + dateFormatFileNameStr + ".png").Build());
                }
                extentErrorCalledAlready = true;
            }
        }
    }
}
