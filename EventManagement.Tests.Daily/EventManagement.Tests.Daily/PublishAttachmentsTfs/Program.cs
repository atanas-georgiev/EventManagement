using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishAttachmentsTfs
{
    class Program
    {
        static void Main(string[] args)
        {
            KPMG.TestAutomation.Reports.TFS.AddAttachmentsToReport.AddScreenshots("https://kpmg-cerberus.visualstudio.com", "lsayy6dfuexhweq6k23473vef24rwesozibhh7y3fqddenvhgjea", "5bcf4592-0335-46e5-a8ef-3d93156b34c5", "EventManagement.Tests", "EventManagement", "EventManagement.Tests.Daily.EndToEnd");
        }
    }
}
