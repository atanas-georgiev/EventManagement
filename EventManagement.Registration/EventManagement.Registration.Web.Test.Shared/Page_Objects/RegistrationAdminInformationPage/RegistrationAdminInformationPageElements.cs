using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Registration.Web.Test.Shared.Page_Objects
{
    public partial class RegistrationAdminInformationPage
    {
        public By GobackButton()
        {
            return By.CssSelector("button.btn-secondary");
        }

        public By EventName()
        {
            Thread.Sleep(1000);
            return By.CssSelector("ng-component h1");
        }

        public By LocationName()
        {
            //return By.XPath("//div.row//h3[1]");
            return By.CssSelector("div.row>div.col-center>h3:nth-child(2)");
        }

        public By Dates()
        {
            //return By.XPath("//div.row//h3[2]");
            return By.CssSelector("div.row>div.col-center>h3:nth-child(3)");
        }

        public By RegisteredPersonName(string row)
        {
            return By.CssSelector("ng-component ul>li:nth-child(" + row + ")");
        }
    }
}
