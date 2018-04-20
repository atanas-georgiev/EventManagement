using EventManagement.Registration.Web.Test.Base;
using KPMG.TestAutomation.TestCore.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;

namespace EventManagement.Registration.Web.Test.StepsDefinitions
{
    [Binding]
    public sealed class GeneralStepDefinitions
    {
        private readonly CommanActionsImplementation helpers;

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralStepDefinitions"/> class.
        /// In UnityContainer we have the drivers.
        /// </summary>
        public GeneralStepDefinitions()
        {
            //this.helpers = UnityContainerFactory.GetContainer().Resolve<CommanActionsImplementation>();
            this.helpers = UnityContainerFactory.GetContainer().Resolve(typeof(CommanActionsImplementation), null) as CommanActionsImplementation;
        }

        /// <summary>
        /// Here you can enter some sql queries that prepare the database for automation testing
        /// </summary>
        /// <param name="pageName">The name of the first page that we see when we open the application. Just for information ususlly we don't use this parameter in sql scripts.</param>
        [Given(@"""(.*)"" Test page with spcific data")]
        public void GivenTestPageWithSpcificData(string pageName)
        {
            //some code here
        }

        [Given(@"registration page")]
        public void GivenRegistrationPage()
        {
            helpers.OpenURL("RegisterURL", "RegisterPage", string.Empty);
            ScenarioContext.Current["className"] = Regex.Replace("RegisterPage", @"\s+", string.Empty);
        }

        [Given(@"specific data")]
        public void GivenSpecificData()
        {
            Helpers dbUpdate = new Helpers();
            dbUpdate.ClearAndImportDB();
        }

        [Given(@"last registration is payed")]
        public void GivenLastRegistrationIsPayed()
        {
            Helpers dbUpdate = new Helpers();
            dbUpdate.PayEvent();
        }


        [Given(@"I login with ""(.*)"" user")]
        public void GivenILoginWithUser(string userType)
        {
            if (userType.ToLower() == "admin")
            {
                helpers.OpenURL("LoginURL", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("automationAdmin@event.com", "Email", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("Automation1!", "Password", "RegistrationTablePage", string.Empty);
                helpers.Click("LogIn", "RegistrationTablePage", string.Empty);
                helpers.OpenURL("RegistrationURL", "RegistrationTablePage", string.Empty);
                ScenarioContext.Current["className"] = "RegistrationTablePage";
            }

            if (userType.ToLower() == "noadmin")
            {
                helpers.OpenURL("LoginURL", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("regularUserAutomation@event.com", "Email", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("notAutomation1!", "Password", "RegistrationTablePage", string.Empty);
                helpers.Click("LogIn", "RegistrationTablePage", string.Empty);
                helpers.OpenURL("RegistrationURL", "RegistrationTablePage", string.Empty);
                ScenarioContext.Current["className"] = "RegistrationTablePage";
            }
        }

        [Then(@"I login with ""(.*)"" user")]
        public void GivenILoginWithUserThen(string userType)
        {
            if (userType.ToLower() == "admin")
            {
                helpers.OpenURL("LoginURL", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("automationAdmin@event.com", "Email", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("Automation1!", "Password", "RegistrationTablePage", string.Empty);
                helpers.Click("LogIn","RegistrationTablePage", string.Empty);
                helpers.OpenURL("RegistrationURL", "RegistrationTablePage", string.Empty);
                ScenarioContext.Current["className"] = "RegistrationTablePage";
            }

            if (userType.ToLower() == "noadmin")
            {
                helpers.OpenURL("LoginURL", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("regularUserAutomation@event.com", "Email", "RegistrationTablePage", string.Empty);
                helpers.EnterValueInField("notAutomation1!", "Password", "RegistrationTablePage", string.Empty);
                helpers.Click("LogIn", "RegistrationTablePage", string.Empty);
                helpers.OpenURL("RegistrationURL", "RegistrationTablePage", string.Empty);
                ScenarioContext.Current["className"] = "RegistrationTablePage";
            }
        }

        [Then(@"I open url ""(.*)""")]
        public void ThenIOpenUrl(string p0)
        {
            helpers.OpenURL(p0, ScenarioContext.Current["className"].ToString(), string.Empty);
        }


        /// <summary>
        /// Change the active PageObject page from where selenium will search the elements.
        /// </summary>
        /// <param name="pageObjectClassName">The name of the PageObject class. If the class is "FamilyPage" you must enter exactly yhe same string. If we enter different string we will see null pointer exception because such cs file don't exist.</param>
        [Then(@"I see page: ""(.*)""")]
        public void ThenISeePage(string pageObjectClassName)
        {
            ScenarioContext.Current["className"] = Regex.Replace(pageObjectClassName, @"\s+", string.Empty);
        }


        /// <summary>
        /// Click Element. If element is not there we see error in ExtentEx report.
        /// Example: When I click "Save Button"
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "SaveButton"</param>
        [When(@"I click ""(.*)""")]
        public void WhenIClick(string elementName)
        {
            this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
        }

        /// <summary>
        /// Run JS executor. On the place of elementLocatorString the method will replace the real locator
        /// </summary>
        /// <param name="elementName">Element name</param>
        [Then(@"I click on ""(.*)""")]
        public void ThenIClickOn(string elementName)
        {
            Thread.Sleep(1000);
            this.helpers.JSExecute(elementName, ScenarioContext.Current["className"].ToString(), string.Empty, "document.querySelector(\"elementLocatorString\").click();");
            Thread.Sleep(1000);

        }

        /// <summary>
        /// Run JS executor. On the place of elementLocatorString the method will replace the real locator
        /// </summary>
        /// <param name="valueToEnter"></param>
        /// <param name="elementName"></param>
        [Then(@"I enter ""(.*)"" value in ""(.*)""")]
        public void ThenIEnterValueIn(string valueToEnter, string elementName)
        {
            Thread.Sleep(1000);
            this.helpers.JSExecute(elementName, ScenarioContext.Current["className"].ToString(), string.Empty, "document.querySelector(\"elementLocatorString\").value = \"" + valueToEnter + "\";");
            Thread.Sleep(1000);
        }


        /// <summary>
        /// Run JS executor. On the place of elementLocatorString the method will replace the real locator
        /// </summary>
        /// <param name="elementName">Element Name</param>
        /// <param name="parametersForTheElement">Parameters for the element</param>
        [Then(@"I click on ""(.*)"" number ""(.*)""\|")]
        public void ThenIClickOnNumber(string elementName, string parametersForTheElement)
        {
            Thread.Sleep(1000);
            this.helpers.JSExecute(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement, "document.querySelector(\"elementLocatorString\").click();");
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Run JS executor. On the place of elementLocatorString the method will replace the real locator
        /// </summary>
        /// <param name="valueToEnter"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        [Then(@"I enter ""(.*)"" value in ""(.*)"" number ""(.*)""\|")]
        public void ThenIEnterValueInNumber(string valueToEnter, string elementName, string parametersForTheElement)
        {
            Thread.Sleep(1000);
            this.helpers.JSExecute(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement, "document.querySelector(\"elementLocatorString\").value = \"" + valueToEnter + "\";");
            Thread.Sleep(1000);
        }


        /// <summary>
        /// Move to element 
        /// </summary>
        /// <param name="elementName">Element name</param>
        [Then(@"I move to element ""(.*)""")]
        public void ThenIMoveToElement(string elementName)
        {
            Thread.Sleep(1000);
            this.helpers.MoveToElement(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
            Thread.Sleep(1000);

        }

        /// <summary>
        /// Move to element with parameters
        /// </summary>
        /// <param name="elementName">Element name</param>
        /// <param name="parametersForTheElement">Parameter</param>
        [Then(@"I move to element ""(.*)"" number ""(.*)""\|")]
        public void ThenIMoveToElementNumber(string elementName, string parametersForTheElement)
        {
            Thread.Sleep(1000);
            this.helpers.MoveToElement(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
            Thread.Sleep(1000);
        }

        /// <summary>
        /// Click Element. If element is not there we see error in ExtentEx report.
        /// Example: And/Then I click "Save Button"
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "SaveButton"</param>
        [Then(@"I click ""(.*)""")]
        public void ThenIClick(string elementName)
        {
            this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
        }

        [Then(@"I select to see ""(.*)"" ""(.*)""")]
        public void GivenISelectToSee(string theOption, string selectElementName)
        {
            this.helpers.Click(selectElementName, ScenarioContext.Current["className"].ToString(), theOption);
        }


        /// <summary>
        /// Click Element from specific number(p1). Like we use this number for find the elemrnt:
        /// public By ReligionNameField(string number)
        /// {
        /// return By.CssSelector("select#religionName" + (Int32.Parse(number) - 1).ToString());
        /// }
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "ReligionNameField"</param>
        /// <param name="parametersForTheElement">The parameter that we use to find the element. In our case: 1 for example.</param>
        [Then(@"I click ""(.*)"" number ""(.*)""\|")]
        public void ThenIClickNumber(string elementName, string parametersForTheElement)
        {
            if (elementName.IndexOf("RadioButton") > 0 || elementName.IndexOf("Option") > 0)
            {
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
            }
            else
            {
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
            }
        }

        /// <summary>
        /// Click element from specified PageObject page
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "SaveButton"</param>
        /// <param name="pageObject">The name of PageOpject page(class name)</param>
        [When(@"I click on ""(.*)"" from ""(.*)""")]
        public void WhenIClickOnFrom(string elementName, string pageObject)
        {
            this.helpers.Click(elementName, pageObject, string.Empty);
        }

        /// <summary>
        /// Select(Click) on element. If the element have RadioButton or Option in his name we click 2 times.
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "SaveButton"</param>
        [Then(@"I select ""(.*)""")]
        public void ThenISelect(string elementName)
        {
            if (elementName.IndexOf("RadioButton") > 0 || elementName.IndexOf("Option") > 0)
            {
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
            }
            else
            {
                this.helpers.Click(elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
            }
        }

        /// <summary>
        /// Click on select object and select the option from it.
        /// Then I click "Bank Account Prefered Name" for panel "3" and select "4:: Bank Account Prefered Name Option".
        /// </summary>
        /// <param name="selectElementName">select object name</param>
        /// <param name="numberOfPanel">number of the panel</param>
        /// <param name="optionName">option to select</param>
        [Then(@"I click ""(.*)"" for panel ""(.*)"" and select ""(.*)""\.")]
        public void ThenIClickForPanelAndSelect_(string selectElementName, string numberOfPanel, string optionName)
        {
            this.helpers.Click(selectElementName, ScenarioContext.Current["className"].ToString(), numberOfPanel);

            if (optionName.IndexOf("RadioButton") > 0 || optionName.IndexOf("Option") > 0)
            {
                if (optionName.IndexOf("::") > 0)
                {
                    var optionSecondParameter = optionName.Split(new[] { "::" }, StringSplitOptions.None);
                    this.helpers.Click(optionSecondParameter[1].Trim(), ScenarioContext.Current["className"].ToString(), numberOfPanel + ",," + optionSecondParameter[0]);
                    this.helpers.Click(optionSecondParameter[1].Trim(), ScenarioContext.Current["className"].ToString(), numberOfPanel + ",," + optionSecondParameter[0]);
                }
                else
                {
                    this.helpers.Click(optionName, ScenarioContext.Current["className"].ToString(), numberOfPanel);
                    this.helpers.Click(optionName, ScenarioContext.Current["className"].ToString(), numberOfPanel);
                }
            }
            else
            {
                this.helpers.Click(optionName, ScenarioContext.Current["className"].ToString(), numberOfPanel);
            }
        }

        /// <summary>
        /// Click on select object and select the option from it using JS.
        /// Then I click "Bank Account Prefered Name" for panel "3" and select "4:: Bank Account Prefered Name Option".
        /// </summary>
        /// <param name="selectElementName">select object name</param>
        /// <param name="numberOfPanel">number of the panel</param>
        /// <param name="OptionName">option to select</param>
        [Then(@"I click on ""(.*)"" for panel ""(.*)"" and select ""(.*)""\.")]
        public void ThenIClickOnForPanelAndSelect_(string selectElementName, string numberOfPanel, string OptionName)
        {
            Thread.Sleep(1000);
            this.helpers.JSExecute(selectElementName, ScenarioContext.Current["className"].ToString(), numberOfPanel, "document.querySelector(\"elementLocatorString\").click();");
            Thread.Sleep(1000);

            if (OptionName.IndexOf("RadioButton") > 0 || OptionName.IndexOf("Option") > 0)
            {
                if (OptionName.IndexOf("::") > 0)
                {
                    var optionSecondParameter = OptionName.Split(new[] { "::" }, StringSplitOptions.None);
                    Thread.Sleep(1000);
                    this.helpers.JSExecute(optionSecondParameter[1].Trim(), ScenarioContext.Current["className"].ToString(), numberOfPanel + ",," + optionSecondParameter[0], "document.querySelector(\"elementLocatorString\").click();");
                    this.helpers.JSExecute(optionSecondParameter[1].Trim(), ScenarioContext.Current["className"].ToString(), numberOfPanel + ",," + optionSecondParameter[0], "document.querySelector(\"elementLocatorString\").click();");
                    Thread.Sleep(1000);
                }
                else
                {
                    Thread.Sleep(1000);
                    this.helpers.JSExecute(OptionName, ScenarioContext.Current["className"].ToString(), numberOfPanel, "document.querySelector(\"elementLocatorString\").click();");
                    this.helpers.JSExecute(OptionName, ScenarioContext.Current["className"].ToString(), numberOfPanel, "document.querySelector(\"elementLocatorString\").click();");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Thread.Sleep(1000);
                this.helpers.JSExecute(OptionName, ScenarioContext.Current["className"].ToString(), numberOfPanel, "document.querySelector(\"elementLocatorString\").click();");
                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elementName"></param>
        /// <param name="projectName"></param>
        [When(@"I click ""(.*)"" from ""(.*)""\|")]
        public void WhenIClickFrom(string elementName, string projectName)
        {
            this.helpers.JSExecute(elementName, projectName, string.Empty, "document.querySelector(\"elementLocatorString\").click();");
        }

        /// <summary>
        /// Enter string in element. And save the value in FeatureContext like we use elementName as Key.
        /// </summary>
        /// <param name="stringToEnter">The string that we want to enter. Example: "sstoykova"</param>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "UserNameField"</param>
        [Then(@"I enter ""(.*)"" in ""(.*)""")]
        public void ThenIEnterIn(string stringToEnter, string elementName)
        {
            string value;
            if (FeatureContext.Current.TryGetValue(Regex.Replace(elementName, @"\s+", string.Empty), out value))
            {
                FeatureContext.Current[Regex.Replace(elementName, @"\s+", string.Empty)] = stringToEnter;
                this.helpers.EnterValueInField(stringToEnter, elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
            }
            else
            {
                FeatureContext.Current.Add(Regex.Replace(elementName, @"\s+", string.Empty), stringToEnter);
                this.helpers.EnterValueInField(stringToEnter, elementName, ScenarioContext.Current["className"].ToString(), string.Empty);
            }
        }


        /// <summary>
        /// Enter string in element. And save the value in FeatureContext like we use elementName as Key.We specify and PageObject where we must look for the element.
        /// </summary>
        /// <param name="stringToEnter">The string that we want to enter. Example: "sstoykova"</param>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "UserNameField"</param>
        /// <param name="nameOfPageObjectClass">The name of PageOpject page(class name)</param>
        [Then(@"I enter ""(.*)"" in ""(.*)"" from ""(.*)""")]
        public void ThenIEnterInFrom(string stringToEnter, string elementName, string nameOfPageObjectClass)
        {
            string value;
            if (FeatureContext.Current.TryGetValue(Regex.Replace(elementName, @"\s+", string.Empty), out value))
            {
                FeatureContext.Current[Regex.Replace(elementName, @"\s+", string.Empty)] = stringToEnter;
                this.helpers.EnterValueInField(stringToEnter, elementName, nameOfPageObjectClass, string.Empty);
            }
            else
            {
                FeatureContext.Current.Add(Regex.Replace(elementName, @"\s+", string.Empty), stringToEnter);
                this.helpers.EnterValueInField(stringToEnter, elementName, nameOfPageObjectClass, string.Empty);
            }
        }

        /// <summary>
        /// Enter string in element from specific number(p1). And save the value in FeatureContext like we use elementName as Key.
        /// We use this number for find the elemrnt:
        /// public By ReligionNameField(string number)
        /// {
        /// return By.CssSelector("select#religionName" + (Int32.Parse(number) - 1).ToString());
        /// }
        /// </summary>
        /// <param name="stringToEnter">The string that we want to enter. Example: "sstoykova"</param>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. In our example the name of the method is "ReligionNameField"</param>
        /// <param name="parametersForTheElement">The parameter that we use to find the element. In our case: 1 for example</param>
        [Then(@"I enter ""(.*)"" in ""(.*)"" number ""(.*)""\|")]
        public void ThenIEnterInNumber(string stringToEnter, string elementName, string parametersForTheElement)
        {
            string value;
            if (FeatureContext.Current.TryGetValue(Regex.Replace(elementName, @"\s+", string.Empty), out value))
            {
                FeatureContext.Current[Regex.Replace(elementName, @"\s+", string.Empty)] = stringToEnter;
                this.helpers.EnterValueInField(stringToEnter, elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
            }
            else
            {
                FeatureContext.Current.Add(Regex.Replace(elementName, @"\s+", string.Empty), stringToEnter);
                this.helpers.EnterValueInField(stringToEnter, elementName, ScenarioContext.Current["className"].ToString(), parametersForTheElement);
            }
        }

        /// <summary>
        /// Verify if element have specific "innerText" parameter. Like the name of the element in the page object is the same as the text:
        /// public By PersonalData()
        /// {
        /// return By.CssSelector("div#bladePersonal>div>div.panel-heading");
        /// }
        /// </summary>
        /// <param name="textOnThePageThatWeWantToCheck">The text that we want to check. In our example is "Personal Data"</param>
        [Then(@"I expect to see ""(.*)"" text")]
        public void ThenIExpectToSeeText(string textOnThePageThatWeWantToCheck)
        {
            this.helpers.CheckProperty(textOnThePageThatWeWantToCheck, ScenarioContext.Current["className"].ToString(), textOnThePageThatWeWantToCheck, string.Empty, "innerText");
        }

        /// <summary>
        /// Verify if element have specific "innerText" parameter.
        /// </summary>
        /// <param name="stringToSeeInInnerText">String that we expect to see.</param>
        /// <param name="nameOfPageOrElement">
        /// 1) If the name contains "Page" This parameter is the name of the PageObject(the name of the class).The name of the element(name of the method) in this case is like the text that we search.
        /// 2) The name of the element(name of the method) from PageObject. When there is no "Page" in the string.</param>
        [Then(@"I expect to see ""(.*)"" text in ""(.*)""")]
        public void ThenIExpectToSeeIn(string stringToSeeInInnerText, string nameOfPageOrElement)
        {
            if (nameOfPageOrElement.IndexOf("Page") > 0)
            {
                this.helpers.CheckProperty(stringToSeeInInnerText, nameOfPageOrElement, stringToSeeInInnerText, string.Empty, "innerText");
            }
            else
            {
                this.helpers.CheckProperty(stringToSeeInInnerText, ScenarioContext.Current["className"].ToString(), nameOfPageOrElement, string.Empty, "innerText");
            }
        }

        /// <summary>
        /// Verify if element have specific "value" parameter.
        /// </summary>
        /// <param name="stringToSeeInValue">String that we expect to see.</param>
        /// <param name="nameOfPageOrElement">
        /// 1) If the name contains "Page" This parameter is the name of the PageObject(the name of the class).The name of the element(name of the method) in this case is like the text that we search.
        /// 2) The name of the element(name of the method) from PageObject. When there is no "Page" in the string.</param>
        [Then(@"I expect to see ""(.*)"" value in ""(.*)""")]
        public void ThenIExpectToSeeValueIn(string stringToSeeInValue, string nameOfPageOrElement)
        {
            if (nameOfPageOrElement.IndexOf("Page") > 0)
            {
                this.helpers.CheckProperty(stringToSeeInValue, nameOfPageOrElement, stringToSeeInValue, string.Empty, "value");
            }
            else
            {
                this.helpers.CheckProperty(stringToSeeInValue, ScenarioContext.Current["className"].ToString(), nameOfPageOrElement, string.Empty, "value");
            }
        }

        /// <summary>
        /// Verify if element from specific number(p2) have specific "innerText" parameter. Like we use this number for find the element:
        /// public By ReligionNameField(string number)
        /// {
        /// return By.CssSelector("select#religionName" + (Int32.Parse(number) - 1).ToString());
        /// }
        /// </summary>
        /// <param name="stringToSeeInInnerText">String that we expect to see.</param>
        /// <param name="nameOfPageOrElement">The name of the element(name of the method) from PageObject. In our example the name of the method is "ReligionNameField"
        /// 1) If the name contains "Page" This parameter is the name of the PageObject(the name of the class).The name of the element(name of the method) in this case is like the text that we search.
        /// 2) The name of the element(name of the method) from PageObject. When there is no "Page" in the string.</param>
        /// <param name="parametersForTheElement">The parameter that we use to find the element. In our case: 1 for example</param>
        [Then(@"I expect to see ""(.*)"" text in ""(.*)"" number ""(.*)""\|")]
        public void ThenIExpectToSeeTextInNumber(string stringToSeeInInnerText, string nameOfPageOrElement, string parametersForTheElement)
        {
            if (nameOfPageOrElement.IndexOf("Page") > 0)
            {
                this.helpers.CheckProperty(stringToSeeInInnerText, nameOfPageOrElement, stringToSeeInInnerText, parametersForTheElement, "innerText");
            }
            else
            {
                this.helpers.CheckProperty(stringToSeeInInnerText, ScenarioContext.Current["className"].ToString(), nameOfPageOrElement, parametersForTheElement, "innerText");
            }
        }

        /// <summary>
        /// Verify if element from specific number(p2) have specific "value" parameter. Like we use this number for find the element:
        /// public By ReligionNameField(string number)
        /// {
        /// return By.CssSelector("select#religionName" + (Int32.Parse(number) - 1).ToString());
        /// }
        /// </summary>
        /// <param name="stringToSeeInValue">String that we expect to see.</param>
        /// <param name="nameOfPageOrElement">The name of the element(name of the method) from PageObject. In our example the name of the method is "ReligionNameField"
        /// 1) If the name contains "Page" This parameter is the name of the PageObject(the name of the class).The name of the element(name of the method) in this case is like the text that we search.
        /// 2) The name of the element(name of the method) from PageObject. When there is no "Page" in the string.</param>
        /// <param name="parametersForTheElement">The parameter that we use to find the element. In our case: 1 for example</param>
        [Then(@"I expect to see ""(.*)"" value in ""(.*)"" number ""(.*)""\|")]
        public void ThenIExpectToSeeValueInNumber(string stringToSeeInValue, string nameOfPageOrElement, string parametersForTheElement)
        {
            if (nameOfPageOrElement.IndexOf("Page") > 0)
            {
                this.helpers.CheckProperty(stringToSeeInValue, nameOfPageOrElement, stringToSeeInValue, parametersForTheElement, "value");
            }
            else
            {
                this.helpers.CheckProperty(stringToSeeInValue, ScenarioContext.Current["className"].ToString(), nameOfPageOrElement, parametersForTheElement, "value");
            }
        }

        /// <summary>
        /// Verify if element have specific "value" parameter. Like the expected value came from FeatureContext with Key the name of the element.
        /// </summary>
        /// <param name="nameOfElement">The name of the element(name of the method) from PageObject.</param>
        [Then(@"I expect to see in ""(.*)"" correct value")]
        public void ThenIExpectToSeeInCorrectValue(string nameOfElement)
        {
            this.helpers.CheckProperty(FeatureContext.Current[Regex.Replace(nameOfElement, @"\s+", string.Empty)].ToString(), ScenarioContext.Current["className"].ToString(), nameOfElement, string.Empty, "value");
        }

        /// <summary>
        /// Verify if element have specific "innerText" parameter. Like the expected value came from FeatureContext with Key the name of the element.
        /// </summary>
        /// <param name="elemetName">The name of the element(name of the method) from PageObject.</param>
        [Then(@"I expect to see in ""(.*)"" correct text")]
        public void ThenIExpectToSeeInCorrectText(string elemetName)
        {
            this.helpers.CheckProperty(FeatureContext.Current[Regex.Replace(elemetName, @"\s+", string.Empty)].ToString(), ScenarioContext.Current["className"].ToString(), elemetName, string.Empty, "innerText");
        }

        /// <summary>
        /// Verify if in drop - down we see expected value. We have 2 options here:
        /// 1. To verify "selectedIndex" property of the element. In this case you must have 2 elements in the PageObject:
        /// 1.1) First is the element(drop-down elemet) for which you must verify selected value
        /// public By Nationality()
        /// {
        /// return By.CssSelector("select#nationalityName");
        /// }
        /// 1.2) Second is the element with same name + "StringToIndex". For example if the first element is with name "Nationality" the second one will be "NationalityStringToIndex"
        ///  public String NationalityStringToIndex(string indexText)
        /// {
        ///    switch (indexText)
        ///    {
        ///        case "Bulgaria":
        ///            return "28";
        ///        case "Germany":
        ///            return "71";
        ///        case "Zambia":
        ///            return "215";
        ///        case "Not Set":
        ///            return "0";
        ///        default:
        ///            return "1000";
        ///    }
        ///  }
        ///  Example: Then/And I expect to see "for selectedIndex property: Bulgaria" in "Nationality" drop-down
        ///  2. To verify "value" property. In this situation we have only one element in PageObject: the drop-down element
        ///  Example: Then/And I expect to see "Bulgaria" in "Nationality" drop-down
        /// </summary>
        /// <param name="selectedIndexOrValue">
        /// 1.To verify "selectedIndex" property p0 must be:"for selectedIndex property: Bulgaria" like replace Bulgaria with your string that you latter will map to corresponding selectedIndex.
        /// 2.To verify "value" property p0 must be the value of the element.
        /// </param>
        /// <param name="elementName">The name of the drop-down element(name of the method) from PageObject</param>
        [Then(@"I expect to see ""(.*)"" in ""(.*)"" drop-down")]
        public void ThenIExpectToSeeInDrop_Down(string selectedIndexOrValue, string elementName)
        {
            if (Regex.Replace(selectedIndexOrValue, @"\s+", string.Empty).IndexOf("selectedIndex") > 0)
            {
                var getMappingPartFromString = selectedIndexOrValue.Split(new[] { "property:" }, StringSplitOptions.None);
                this.helpers.CheckDropDownSelectedIndex(getMappingPartFromString[1].Trim(), ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
            }
            else
            {
                this.helpers.CheckProperty(Regex.Replace(selectedIndexOrValue, @"\s+", string.Empty), ScenarioContext.Current["className"].ToString(), elementName, string.Empty, "value");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="select name"></param>
        /// <param name="option number"></param>
        /// <param name="Option Value"></param>
        [Then(@"I expect to see in ""(.*)"" selected option ""(.*)"" with value ""(.*)""")]
        public void ThenIExpectToSeeInSelectedOptionWithValue(string elementName, string optionNumber, string optionValue)
        {
            this.helpers.CheckProperty(optionNumber, ScenarioContext.Current["className"].ToString(), elementName, string.Empty, "selectedIndex");
        }


        /// <summary>
        /// Verify if in drop - down from specific number(p1), we see expected value. Like we use this number for find the elemrnt. We have 2 options here:
        /// 1. To verify "selectedIndex" property of the element. In this case you must have 2 elements in the PageObject:
        /// 1.1) First is the element(drop-down elemet) for which you must verify selected value
        /// public By Nationality(string number)
        /// {
        /// return By.CssSelector("select#nationalityName" + (Int32.Parse(number) - 1).ToString() );
        /// }
        /// 1.2) Second is the element with same name + "StringToIndex". For example if the first element is with name "Nationality" the second one will be "NationalityStringToIndex"
        ///  public String NationalityStringToIndex(string indexText)
        /// {
        ///    switch (indexText)
        ///    {
        ///        case "Bulgaria":
        ///            return "28";
        ///        case "Germany":
        ///            return "71";
        ///        case "Zambia":
        ///            return "215";
        ///        case "Not Set":
        ///            return "0";
        ///        default:
        ///            return "1000";
        ///    }
        ///  }
        ///  Example: Then/And I expect to see "for selectedIndex property: Bulgaria" in "Nationality" drop-down number "1"|
        ///  2. To verify "value" property. In this situation we have only one element in PageObject: the drop-down element
        ///  Example: Then/And I expect to see "Bulgaria" in "Nationality" drop-down number "1"|
        /// </summary>
        /// <param name="selectedIndexOrValue">
        /// 1.To verify "selectedIndex" property p0 must be:"for selectedIndex property: Bulgaria" like replace Bulgaria with your string that you latter will map to corresponding selectedIndex.
        /// 2.To verify "value" property p0 must be the value of the element.
        /// </param>
        /// <param name="elementName">The name of the drop-down element(name of the method) from PageObject in our example Nationality</param>
        /// <param name="parametersForElement">The parameter that we use to find the element. In our case: 1 for example</param>
        [Then(@"I expect to see ""(.*)"" in ""(.*)"" drop-down number ""(.*)""")]
        public void ThenIExpectToSeeInDrop_DownNumber(string selectedIndexOrValue, string elementName, string parametersForElement)
        {
            if (Regex.Replace(selectedIndexOrValue, @"\s+", string.Empty).IndexOf("selectedIndex") > 0)
            {
                var getMappingPartFromString = selectedIndexOrValue.Split(new[] { "property:" }, StringSplitOptions.None);
                this.helpers.CheckDropDownSelectedIndex(getMappingPartFromString[1].Trim(), ScenarioContext.Current["className"].ToString(), elementName, parametersForElement);
            }
            else
            {
                this.helpers.CheckProperty(Regex.Replace(selectedIndexOrValue, @"\s+", string.Empty), ScenarioContext.Current["className"].ToString(), elementName, parametersForElement, "value");
            }
        }

        /// <summary>
        /// Verify checkbox condition.
        /// </summary>
        /// <param name="checkboxName">The name of the element checkbox(name of the method) from PageObject.</param>
        /// <param name="checkedOrUnchecked">put for p1 - "checked" if you want to verify that the checkox is checked and "unchecked" for unchecked</param>
        [Then(@"I expect ""(.*)"" to be ""(.*)""")]
        public void ThenIExpectToBe(string checkboxName, string checkedOrUnchecked)
        {
            this.helpers.VerifyCheckbox(checkboxName, ScenarioContext.Current["className"].ToString(), checkedOrUnchecked, string.Empty);
        }

        /// <summary>
        ///  Verify checkbox condition with parameter.
        ///  Example: And I expect "Preferred Phone Radio Button" to be "checked" for "phone" number "1"|
        /// </summary>
        /// <param name="checkBoxName">The name of the element checkbox(name of the method) from PageObject.</param>
        /// <param name="checkedOrUnchecked">put for p1 - "checked" if you want to verify that the checkox is checked and "unchecked" for unchecked</param>
        /// <param name="numberOfThePanel">Information for panel where the radio button is. For example Phone</param>
        /// <param name="parameterToFindElement">Parameter to find the element</param>
        [Then(@"I expect ""(.*)"" to be ""(.*)"" for ""(.*)"" number ""(.*)""\|")]
        public void ThenIExpectToBeForNumber(string checkBoxName, string checkedOrUnchecked, string numberOfThePanel, string parameterToFindElement)
        {
            this.helpers.VerifyCheckbox(checkBoxName, ScenarioContext.Current["className"].ToString(), checkedOrUnchecked, parameterToFindElement);
        }

        /// <summary>
        /// Check if element is readonly or NOT. We have 3 possibilities
        /// 1.)One option is to check element.Displayed property.
        /// 2.)One option is to check element.Enabled property.
        /// 3.)Tho check some element attribute. "disabled" is one that I use the most
        /// </summary>
        /// <param name="propertyToCheck"> The property that we want to use for checking read-only status of the element
        /// 1.) For element.Displayed property enter for p0: "displayed"
        /// 2.)For element.Enabled property enter for p0: "enabled"
        /// 3.) For some element attribute enter the name of the the attribute. Must use for read only is: "disabled"
        /// </param>
        /// <param name="elementName">The name of the element(name of the method) from PageObject</param>
        /// <param name="ReadOnlyStatus">Here we specify if we want to check that element is read-only or not read-only
        /// 1) READ-ONLY: leave empty this string
        /// 2) NOT READ-ONLY: enter "NOT"
        /// </param>
        [Then(@"I check ""(.*)"" property of element ""(.*)"" to verify that element is ""(.*)"" read-only")]
        public void ThenICheckPropertyOfElementToVerifyThatElementIsRead_Only(string propertyToCheck, string elementName, string ReadOnlyStatus)
        {
            if (Regex.Replace(propertyToCheck, @"\s+", string.Empty).ToLower() == "displayed")
            {
                if (ReadOnlyStatus == string.Empty)
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "false", ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
                }
                else
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
                }
            }
            else if (Regex.Replace(propertyToCheck, @"\s+", string.Empty).ToLower() == "enabled")
            {
                if (ReadOnlyStatus == string.Empty)
                {
                    this.helpers.VerifyElementDisplayedEnabled("false", "true", ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
                }
                else
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
                }
            }
            else
            {
                if (ReadOnlyStatus == string.Empty)
                {
                    this.helpers.CheckProperty("true", ScenarioContext.Current["className"].ToString(), elementName, string.Empty, propertyToCheck);
                }
                else
                {
                    this.helpers.CheckProperty("false", ScenarioContext.Current["className"].ToString(), elementName, string.Empty, propertyToCheck);
                }
            }
        }

        /// <summary>
        /// Check if element is readonly or NOT. We have 3 possibilities
        /// 1.)One option is to check element.Displayed property.
        /// 2.)One option is to check element.Enabled property.
        /// 3.)Tho check some element attribute. "disabled" is one that I use the most
        /// </summary>
        /// <param name="propertyName"> The property that we want to use for checking read-only status of the element
        /// 1.) For element.Displayed property enter for p0: "displayed"
        /// 2.)For element.Enabled property enter for p0: "enabled"
        /// 3.) For some element attribute enter the name of the the attribute. Must use for read only is: "disabled"
        /// </param>
        /// <param name="elementName">The name of the element(name of the method) from PageObject</param>
        /// <param name="parameterToFindTheElement">Parameter to find the element</param>
        /// <param name="readOnlyStatus">Here we specify if we want to check that element is read-only or not read-only
        /// 1) READ-ONLY: leave empty this string
        /// 2) NOT READ-ONLY: enter "NOT"
        /// </param>
        [Then(@"I check ""(.*)"" property of element ""(.*)"" to verify that element for number ""(.*)"" is ""(.*)"" read-only")]
        public void ThenICheckPropertyOfElementToVerifyThatElementForNumberIsRead_Only(string propertyName, string elementName, string parameterToFindTheElement, string readOnlyStatus)
        {
            if (Regex.Replace(propertyName, @"\s+", string.Empty).ToLower() == "displayed")
            {
                if (readOnlyStatus == string.Empty)
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "false", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement);
                }
                else
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement);
                }
            }
            else if (Regex.Replace(propertyName, @"\s+", string.Empty).ToLower() == "enabled")
            {
                if (readOnlyStatus == string.Empty)
                {
                    this.helpers.VerifyElementDisplayedEnabled("false", "true", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement);
                }
                else
                {
                    this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement);
                }
            }
            else
            {
                if (readOnlyStatus == string.Empty)
                {
                    this.helpers.CheckProperty("true", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement, propertyName);
                }
                else
                {
                    this.helpers.CheckProperty("false", ScenarioContext.Current["className"].ToString(), elementName, parameterToFindTheElement, propertyName);
                }
            }
        }

        /// <summary>
        /// If element is on page. Check Enabled selenium property.
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject</param>
        /// <param name="currentNameOrSpecifiedClass">If empty get current page. If not look in specified class.</param>
        [Then(@"I expect ""(.*)"" to be on page ""(.*)""")]
        public void ThenIExpectToBeOnPage(string elementName, string currentNameOrSpecifiedClass)
        {
            if (currentNameOrSpecifiedClass == string.Empty)
            {
                this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
            }
            else
            {
                this.helpers.VerifyElementDisplayedEnabled("true", "true", currentNameOrSpecifiedClass, elementName, string.Empty);
            }
        }

        /// <summary>
        /// If element from specific number(p1) is on page. Check Enabled selenium property. Like we use this number for find the elemrnt.
        /// public By Nationality(string number)
        /// {
        /// return By.CssSelector("select#nationalityName" + (Int32.Parse(number) - 1).ToString() );
        /// }
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. Nationality in our example</param>
        /// <param name="paremeterForTheElement">The parameter that we use to find the element. In our case: 1 for example</param>
        /// <param name="currentpageOrSpecifiedClass">If empty get current page. If not look in specified class.</param>
        [Then(@"I expect ""(.*)"" number ""(.*)""\| to be on page ""(.*)""")]
        public void ThenIExpectNumberToBeOnPage(string elementName, string paremeterForTheElement, string currentpageOrSpecifiedClass)
        {
            if (currentpageOrSpecifiedClass == string.Empty)
            {
                this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), elementName, paremeterForTheElement);
            }
            else
            {
                this.helpers.VerifyElementDisplayedEnabled("true", "true", currentpageOrSpecifiedClass, elementName, paremeterForTheElement);
            }
        }

        /// <summary>
        /// If element is NOT on page. Check Enabled selenium property.
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject</param>
        [Then(@"I expect ""(.*)"" to missing from page")]
        public void ThenIExpectToMissingFromPage(string elementName)
        {
            this.helpers.VerifyThatElementIsNOTInDOM(ScenarioContext.Current["className"].ToString(), elementName, string.Empty);
        }

        /// <summary>
        /// If element from specific number(p1) is NOT on page. Check Enabled selenium property. Like we use this number for find the elemrnt.
        /// public By Nationality(string number)
        /// {
        /// return By.CssSelector("select#nationalityName" + (Int32.Parse(number) - 1).ToString() );
        /// }
        /// </summary>
        /// <param name="elementName">The name of the element(name of the method) from PageObject. Nationality in our example</param>
        /// <param name="parameterForTheElement">The parameter that we use to find the element. In our case: 1 for example</param>
        [Then(@"I expect ""(.*)"" number ""(.*)""\| to missing from page")]
        public void ThenIExpectNumberToMissingFromPage(string elementName, string parameterForTheElement)
        {
            this.helpers.VerifyThatElementIsNOTInDOM(ScenarioContext.Current["className"].ToString(), elementName, parameterForTheElement);
        }

        /// <summary>
        /// Check Labels strings on specific Page from PageObject(the name of the class)
        /// </summary>
        /// <param name="pageObjectClassName">The name of PageOpject page(class name)</param>
        [Then(@"I check labels on ""(.*)""")]
        public void GivenICheckLabelsOn(string pageObjectClassName)
        {
            this.helpers.CheckLabelsOnPage(pageObjectClassName);
        }

        /// <summary>
        /// Validate if option is part of some drop-down field.
        /// 
        /// //And I expect "Bank 1 - IBAN 1" with number "1"| to be "not" visible in "preferred TaxPayer bank account" Panel "1" for drop-down "Bank Account Prefered Name"
        /// </summary>
        /// <param name="optionName">String of the option(the name of the option)</param>
        /// <param name="numberOfElement">Number of the element(the real element of the option) in element group. We use it as parametrer to find correct option</param>
        /// <param name="visibilityOfTheOption">If empty - we expect the option to be visible. If NOT we expect the option don't be part of the drop-down</param>
        /// <param name="explainDropDownField">We use it for explanation of the drop-down field</param>
        /// <param name="panelNumber">Number of the drop-down panel</param>
        /// <param name="dropDownPanelAndOption">Name of the drop-down panel. The option must have the same name as the drop-down panel + Option
        /// Example:
        /// public By BankAccountPreferedName(string number)
        /// {
        ///  return By.CssSelector("select#bankAccountPreferedName" + (Int32.Parse(number) - 1).ToString());
        ///  }
        ///  
        /// public By BankAccountPreferedNameOption(string number, string bankNumber)
        /// {
        /// return By.CssSelector("select#bankAccountPreferedName" + (Int32.Parse(number) - 1).ToString() + ">option[ng-reflect-ng-value='" + (Int32.Parse(bankNumber) - 1).ToString() + "']");
        /// }
        /// number = p4 - panel number
        /// bankNumber = p1 - bank account with number
        /// </param>
        [Then(@"I expect ""(.*)"" with number ""(.*)""\| to be ""(.*)"" visible in ""(.*)"" Panel ""(.*)"" for drop-down ""(.*)""")]
        public void ThenIExpectWithNumberToBeVisibleInPanelForDrop_Down(string optionName, string numberOfElement, string visibilityOfTheOption, string explainDropDownField, string panelNumber, string dropDownPanelAndOption)
        {
            if (visibilityOfTheOption == string.Empty)
            {
                this.helpers.VerifyElementDisplayedEnabled("true", "true", ScenarioContext.Current["className"].ToString(), dropDownPanelAndOption + "Option", panelNumber + ",," + numberOfElement);
                this.helpers.CheckProperty(optionName, ScenarioContext.Current["className"].ToString(), dropDownPanelAndOption + "Option", panelNumber + ",," + numberOfElement, "innerText");
            }
            else
            {
                this.helpers.VerifyThatElementIsNOTInDOM(ScenarioContext.Current["className"].ToString(), dropDownPanelAndOption + "Option", panelNumber + ",," + numberOfElement);
            }
        }

        /// <summary>
        /// Verify selected value in drop-down which is with dinamic opptions
        /// </summary>
        /// <param name="nameOfTheDropDown">The name of the drop-down</param>
        /// <param name="numberOfpanel">the number of the panel in which we see this drop-down</param>
        /// <param name="valueInDropDown">The value that we expect to see in the drop-down</param>
        /// <param name="numberOfTheGroup">Expected value number from the group </param>
        /// <param name="forExplanation">For better explaining the panel and field that we verified</param>
        //And I expect in "Bank Account Prefered Name" drop-down from Panel "1" to be selected "Bank 1 - IBAN 1" which is "1" from all "bank accounts"
        [Then(@"I expect in ""(.*)"" drop-down from Panel ""(.*)"" to be selected ""(.*)"" which is ""(.*)"" from all ""(.*)""")]
        public void ThenIExpectInDrop_DownFromPanelToBeSelectedWhichIsFromAll(string nameOfTheDropDown, string numberOfpanel, string valueInDropDown, string numberOfTheGroup, string forExplanation)
        {
            this.helpers.CheckProperty((Int32.Parse(numberOfTheGroup) - 1).ToString(), ScenarioContext.Current["className"].ToString(), nameOfTheDropDown, numberOfpanel, "value");
            this.helpers.CheckProperty(valueInDropDown, ScenarioContext.Current["className"].ToString(), nameOfTheDropDown + "Option", numberOfpanel + ",," + numberOfTheGroup, "innerText");
        }

        /// <summary>
        /// Validate if element is not visible on the page by checking enable and disable value.
        /// </summary>
        /// <param name="elementName">Name of the element.
        /// In page object we must have 2 elements one when the element is visibble and second when the element is sidden with the same name + sufix Hidden
        /// public By TaxPayerTaxNumberField(string number)
        /// {
        ///   return By.CssSelector("input#taxPayerTaxNumber" + (Int32.Parse(number) - 1).ToString());
        /// }
        /// 
        ///  public By TaxPayerTaxNumberFieldHidden(string number)
        ///  {
        ///   return By.CssSelector("input#taxPayerTaxNumber" + (Int32.Parse(number) - 1).ToString() + "[heeden]");
        ///   }
        /// </param>
        /// <param name="panelNumber">numberof the panel</param>
        [Then(@"I expect that element ""(.*)"" is not visible for panel number ""(.*)""")]
        public void ThenIExpectThatElementIsNotVisibleForPanelNumber(string elementName, string panelNumber)
        {
            this.helpers.VerifyElementDisplayedEnabled("true", "false", ScenarioContext.Current["className"].ToString(), elementName + "Hidden", panelNumber);
        }

        [Then(@"I click ""(.*)"" with name ""(.*)""\|")]
        public void ThenIClickForName(string p0, string p1)
        {
            this.helpers.Click(p0, ScenarioContext.Current["className"].ToString(), p1);
        }

        [Then(@"I expect to see ""(.*)"" text in ""(.*)"" for event ""(.*)""\|")]
        public void ThenIExpectToSeeTextInForEvent(string stringToSeeInInnerText, string nameOfElement, string p2)
        {
            this.helpers.CheckProperty(stringToSeeInInnerText, ScenarioContext.Current["className"].ToString(), nameOfElement, p2, "innerText");
        }


    }
}
