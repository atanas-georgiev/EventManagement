using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Registration.Web.Test.Shared.Page_Objects
{
    public partial class InformationPage
    {
        public By EventNameLabel()
        {
            return By.CssSelector("dl>dt:nth-child(1)");
        }

        public By EventName()
        {
            Thread.Sleep(1000);
            return By.XPath("//dl/dd[1]");
        }

        public By EventLocationNameLabel()
        {
            return By.CssSelector("dl>dt:nth-child(3)");
        }

        public By EventLocationName()
        {
            return By.XPath("//dl/dd[3]");
            //return By.CssSelector("dl>dd:nth-child(3)");
        }

        public By DateLabel()
        {
            return By.CssSelector("dl>dt:nth-child(2)");
        }

        public By Date()
        {
            return By.XPath("//dl/dd[2]");
            //return By.CssSelector("dl>dd:nth-child(2)");
        }

        public By LectureNameLabel()
        {
            return By.CssSelector("dl>dt:nth-child(4)");
        }

        public By LectureName()
        {
            return By.XPath("//dl/dd[4]");
            //return By.CssSelector("dl>dd:nth-child(4)");
        }

        public By PriceLabel(string row)
        {
            return By.CssSelector("dl>dt:nth-child(5)");
        }

        public By Price()
        {
            return By.XPath("//dl/dd[5]");
            //return By.CssSelector("dl>dd:nth-child(5)");
        }

        public By FreeSpacesLabel()
        {
            return By.CssSelector("dl>dt:nth-child(6)");
        }
        public By FreeSpaces()
        {
            return By.XPath("//dl/dd[6]");
            //return By.CssSelector("dl>dd:nth-child(6)");
        }

        public By GobackButton()
        {
            return By.CssSelector("button.btn-secondary");
        }

        public By RegisterButton()
        {
            return By.CssSelector("ng-component button.btn-primary");
        }


    }
}
