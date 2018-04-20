using KPMG.TestAutomation.TestCore.Base;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.Registration.Web.Test.Shared.Page_Objects
{
    public partial class RegistrationAdminInformationPage : BasePage<IWebDriver>
    {
        public RegistrationAdminInformationPage(IWebDriver iWebDriver)
            : base(iWebDriver)
        {
        }
    }
}
