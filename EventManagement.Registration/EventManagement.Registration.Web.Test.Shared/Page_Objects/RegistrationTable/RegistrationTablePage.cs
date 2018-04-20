using KPMG.TestAutomation.TestCore.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Registration.Web.Test.Shared.Page_Objects
{
    public partial class RegistrationTablePage : BasePage<IWebDriver>
    {
        public RegistrationTablePage(IWebDriver iWebDriver)
            : base(iWebDriver)
        {
        }


        public By RegistrationInformationButtonForSpecificEvent(string eventNameToFind)
        {
            Thread.Sleep(3000);
            var eventsCount = driver.FindElements(By.CssSelector("table>tbody tr")).Count;
            
            for(var i =1; i< eventsCount+1; i++)
            {
                var eventName = driver.FindElement(EventName(i.ToString())).GetAttribute("innerText");
                if(eventNameToFind== eventName)
                {
                    return RegistrationsButton(i.ToString());
                }
            }
            return RegistrationsButton("0");
        }

        public By InformationButtonForSpecificEvent(string eventNameToFind)
        {
            Thread.Sleep(3000);
            var eventsCount = driver.FindElements(By.CssSelector("table>tbody tr")).Count;

            for (var i = 1; i < eventsCount + 1; i++)
            {
                var eventName = driver.FindElement(EventName(i.ToString())).GetAttribute("innerText");
                if (eventNameToFind == eventName)
                {
                    return InformationButton(i.ToString());
                }
            }
            return InformationButton("0");
        }
    }
}
