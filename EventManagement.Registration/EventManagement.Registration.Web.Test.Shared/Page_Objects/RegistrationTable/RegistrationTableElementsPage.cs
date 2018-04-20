using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.Registration.Web.Test.Shared.Page_Objects
{
    public partial class RegistrationTablePage
    {
        public string LoginURL()
        {
            return TestContext.Parameters["URLLogin"];
        }

        public By Email()
        {
            return By.CssSelector("input#Input_Email");
        }

        public By Password()
        {
            return By.CssSelector("input#Input_Password");
        }

        public By LogIn()
        {
            return By.CssSelector("button[type='submit']");
        }

        public string RegistrationURL()
        {
            Thread.Sleep(3000);
            return TestContext.Parameters["URLRegistration"];
        }  

        public By EventName(string row)
        {
            return By.CssSelector("table>tbody tr:nth-child(" + row + ")>td:nth-child(2)>div");
        }

        public By LocationName(string row)
        {
            return By.CssSelector("table>tbody tr:nth-child(" + row + ")>td:nth-child(3)>div");
        }

        public By Date(string row)
        {
            return By.CssSelector("table>tbody tr:nth-child(" + row + ")>td:nth-child(4)>div");
        }

        public By FreeSpaces(string eventNameToFind)
        {
            Thread.Sleep(3000);
            var eventsCount = driver.FindElements(By.CssSelector("table>tbody tr")).Count;

            for (var i = 1; i < eventsCount + 1; i++)
            {
                var eventName = driver.FindElement(EventName(i.ToString())).GetAttribute("innerText");
                if (eventNameToFind == eventName)
                {
                    return By.CssSelector("table>tbody tr:nth-child(" + i + ")>td:nth-child(5)>div");
                }
            }
            return By.CssSelector("table>tbody tr:nth-child(0)>td:nth-child(5)>div");
        }



        public By InformationButton(string row)
        {
            return By.CssSelector("table>tbody tr:nth-child(" + row + ")>td:nth-child(6) span[title='Information']");
        }

        public By RegistrationsButton(string row)
        {
            return By.CssSelector("table>tbody tr:nth-child(" + row + ")>td:nth-child(7) span[title='Registrations']");
        }

        public By RowsPerPage()
        {
            return By.CssSelector("tfoot select");
        }

        public By PerPage(string rowsPerPage)
        {
            Thread.Sleep(500);
            return By.CssSelector("tfoot select>option[value='"+ rowsPerPage + "']");
        }

        public By LogOut()
        {
            return By.CssSelector("div.navbar-profile-details>div.navbar-links>a");
        }


    }
}
