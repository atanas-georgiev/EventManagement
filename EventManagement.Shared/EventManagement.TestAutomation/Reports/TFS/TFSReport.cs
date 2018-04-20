using EventManagement.TestAutomation.Data;
using EventManagement.TestAutomation.Paths;
using EventManagement.TestAutomation.StringOperations;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.TestAutomation.Reports
{
    public partial class Reports
    {

        public void ErrorMessageTFS(string errorMessage, string fatalORFailed)
        {
            Console.Write(fatalORFailed.ToUpper() + ": " + errorMessage + System.Environment.NewLine + "Screenshot: " + testName + dateFormatFileNameStr + ".png" + System.Environment.NewLine + "StackTrace:" + System.Environment.NewLine + ReturnString.FormatingStackTrace(Environment.StackTrace));
            var dictionary = DictionaryInteractions.ReadFromPropertiesFile(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt");
            DictionaryInteractions.WriteInTxtFileFromDictionary(ReturnPath.ProjectFolderPath() + "ExtentReport/ReportProperties.txt", dictionary, "tfsReportStatus", "fail");
        }

        public void WriteTFSPassResult(string passMessage)
        {
            Console.Write("PASS: " + passMessage);
        }
    }
}
