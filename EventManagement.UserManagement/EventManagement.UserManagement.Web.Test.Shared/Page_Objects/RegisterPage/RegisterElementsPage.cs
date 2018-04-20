
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventManagement.UserManagement.Web.Test.Shared.Page_Objects
{
    public partial class RegisterPage
    {
        public string RegisterURL()
        {
            return TestContext.Parameters["URLRegistration"];
        }
        
        public By RegisterButton()
        {
            return By.CssSelector("button.btn.btn-primary[type='submit']");
        }

        public By FirstName()
        {
            return By.CssSelector("input#Input_FirstName");
        }

        public By LastName()
        {
            return By.CssSelector("input#Input_LastName");
        }

        public By Email()
        {
            return By.CssSelector("input#Input_Email");
        }

        public By Password()
        {
            return By.CssSelector("input#Input_Password");
        }

        public By ConfirmPassword()
        {
            return By.CssSelector("input#Input_ConfirmPassword");
        }

        public By ValidationSummaryErrors(string row)
        {
            return By.CssSelector("div.validation-summary-errors>ul>li:nth-child("+row+")");
        }

        public By FirstNameError()
        {
            return By.CssSelector("span#Input_FirstName-error");
        }

        public By LastNameError()
        {
            return By.CssSelector("span#Input_LastName-error");
        }

        public By EmailError()
        {
            return By.CssSelector("span#Input_Email-error");
        }

        public By PasswordError()
        {
            return By.CssSelector("span#Input_Password-error");
        }

        public By ConfirmPasswordError()
        {
            return By.CssSelector("span#Input_ConfirmPassword-error");
        }
        
    }
}
