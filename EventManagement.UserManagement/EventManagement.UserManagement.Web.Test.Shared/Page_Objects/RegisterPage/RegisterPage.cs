using OpenQA.Selenium;
using KPMG.TestAutomation.TestCore.Base;

namespace EventManagement.UserManagement.Web.Test.Shared.Page_Objects
{
    public partial class RegisterPage : BasePage<IWebDriver>
    {
        public RegisterPage(IWebDriver iWebDriver)
            : base(iWebDriver)
        {
        }
    }
}
