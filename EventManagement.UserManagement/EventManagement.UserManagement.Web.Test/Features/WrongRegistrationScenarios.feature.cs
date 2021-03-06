﻿// ------------------------------------------------------------------------------
//  <auto-generated>
//      This code was generated by SpecFlow (http://www.specflow.org/).
//      SpecFlow Version:2.2.0.0
//      SpecFlow Generator Version:2.2.0.0
// 
//      Changes to this file may cause incorrect behavior and will be lost if
//      the code is regenerated.
//  </auto-generated>
// ------------------------------------------------------------------------------
#region Designer generated code
#pragma warning disable
namespace EventManagement.UserManagement.Web.Test.Features
{
    using TechTalk.SpecFlow;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("TechTalk.SpecFlow", "2.2.0.0")]
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [NUnit.Framework.TestFixtureAttribute()]
    [NUnit.Framework.DescriptionAttribute("WrongRegistrationScenarios")]
    public partial class WrongRegistrationScenariosFeature
    {
        
        private TechTalk.SpecFlow.ITestRunner testRunner;
        
#line 1 "WrongRegistrationScenarios.feature"
#line hidden
        
        [NUnit.Framework.OneTimeSetUpAttribute()]
        public virtual void FeatureSetup()
        {
            testRunner = TechTalk.SpecFlow.TestRunnerManager.GetTestRunner();
            TechTalk.SpecFlow.FeatureInfo featureInfo = new TechTalk.SpecFlow.FeatureInfo(new System.Globalization.CultureInfo("en-US"), "WrongRegistrationScenarios", null, ProgrammingLanguage.CSharp, ((string[])(null)));
            testRunner.OnFeatureStart(featureInfo);
        }
        
        [NUnit.Framework.OneTimeTearDownAttribute()]
        public virtual void FeatureTearDown()
        {
            testRunner.OnFeatureEnd();
            testRunner = null;
        }
        
        [NUnit.Framework.SetUpAttribute()]
        public virtual void TestInitialize()
        {
        }
        
        [NUnit.Framework.TearDownAttribute()]
        public virtual void ScenarioTearDown()
        {
            testRunner.OnScenarioEnd();
        }
        
        public virtual void ScenarioSetup(TechTalk.SpecFlow.ScenarioInfo scenarioInfo)
        {
            testRunner.OnScenarioStart(scenarioInfo);
        }
        
        public virtual void ScenarioCleanup()
        {
            testRunner.CollectScenarioErrors();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Missing all fields")]
        public virtual void MissingAllFields()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Missing all fields", ((string[])(null)));
#line 4
this.ScenarioSetup(scenarioInfo);
#line 5
 testRunner.Given("registration page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 6
 testRunner.Then("I click on \"Register Button\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 7
 testRunner.Then("I expect to see \"The First Name field is required.\" text in \"First Name Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 8
 testRunner.Then("I expect to see \"The Last Name field is required.\" text in \"Last Name Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 9
 testRunner.Then("I expect to see \"The Email field is required.\" text in \"Email Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 10
 testRunner.Then("I expect to see \"The Password field is required.\" text in \"Password Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 11
 testRunner.Then("I expect to see \"The First Name field is required.\" text in \"First Name Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 12
 testRunner.Then("I expect to see \"The First Name field is required.\" text in \"ValidationSummaryErr" +
                    "ors\" number \"1\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 13
 testRunner.Then("I expect to see \"The Last Name field is required.\" text in \"ValidationSummaryErro" +
                    "rs\" number \"2\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 14
 testRunner.Then("I expect to see \"The Email field is required.\" text in \"ValidationSummaryErrors\" " +
                    "number \"3\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 15
 testRunner.Then("I expect to see \"The Password field is required.\" text in \"ValidationSummaryError" +
                    "s\" number \"4\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Wrong password")]
        public virtual void WrongPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Wrong password", ((string[])(null)));
#line 18
this.ScenarioSetup(scenarioInfo);
#line 19
 testRunner.Given("registration page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 20
 testRunner.Then("I enter \"Sisi First Name\" in \"First Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 21
 testRunner.Then("I enter \"Sisi Last Name\" in \"Last Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 22
 testRunner.Then("I enter \"sisi@abv.bg\" in \"Email\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 23
 testRunner.Then("I enter \"Pass\" in \"Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 24
 testRunner.Then("I enter \"Pass\" in \"Confirm Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 25
 testRunner.Then("I click on \"Register Button\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 26
 testRunner.Then("I expect to see \"The Password must be at least 6 and at max 100 characters long.\"" +
                    " text in \"Password Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 27
 testRunner.Then("I expect to see \"The Password must be at least 6 and at max 100 characters long.\"" +
                    " text in \"Validation Summary Errors\" number \"1\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Wrong confirm password")]
        public virtual void WrongConfirmPassword()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Wrong confirm password", ((string[])(null)));
#line 29
this.ScenarioSetup(scenarioInfo);
#line 30
 testRunner.Given("registration page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 31
 testRunner.Then("I enter \"Sisi First Name\" in \"First Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 32
 testRunner.Then("I enter \"Sisi Last Name\" in \"Last Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 33
 testRunner.Then("I enter \"sisi@abv.bg\" in \"Email\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 34
 testRunner.Then("I enter \"Password\" in \"Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 35
 testRunner.Then("I enter \"Passwors\" in \"Confirm Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 36
 testRunner.Then("I click on \"Register Button\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 37
 testRunner.Then("I expect to see \"The password and confirmation password do not match\" text in \"Co" +
                    "nfirm Password Error\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
        
        [NUnit.Framework.TestAttribute()]
        [NUnit.Framework.DescriptionAttribute("Wrong email")]
        public virtual void WrongEmail()
        {
            TechTalk.SpecFlow.ScenarioInfo scenarioInfo = new TechTalk.SpecFlow.ScenarioInfo("Wrong email", ((string[])(null)));
#line 39
this.ScenarioSetup(scenarioInfo);
#line 40
 testRunner.Given("registration page", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Given ");
#line 41
 testRunner.Then("I enter \"Sisi First Name\" in \"First Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 42
 testRunner.Then("I enter \"Sisi Last Name\" in \"Last Name\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 43
 testRunner.Then("I enter \"sisiabv.bg\" in \"Email\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 44
 testRunner.Then("I enter \"Password\" in \"Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 45
 testRunner.Then("I enter \"Password\" in \"Confirm Password\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 46
 testRunner.Then("I click on \"Register Button\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 47
 testRunner.Then("I expect to see \"The Email field is not a valid e-mail address.\" text in \"Email E" +
                    "rror\"", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line 48
 testRunner.Then("I expect to see \"The Email field is not a valid e-mail address.\" text in \"Validat" +
                    "ion Summary Errors\" number \"1\"|", ((string)(null)), ((TechTalk.SpecFlow.Table)(null)), "Then ");
#line hidden
            this.ScenarioCleanup();
        }
    }
}
#pragma warning restore
#endregion
