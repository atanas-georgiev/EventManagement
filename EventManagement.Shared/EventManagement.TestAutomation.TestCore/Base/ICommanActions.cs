namespace EventManagement.TestAutomation.TestCore.Base
{
    using System;
    using System.Collections.Generic;

    using OpenQA.Selenium;

    public interface ICommanActions
    {
        void Click(By byElement, string ElementName);

        void CheckProperty(By byEl, string propertyName, string expectedValue, string elementName);

        void EnterValueInField(By byElement, string valueToEnter, string elementName);

        void EnterValueInDatePicker(By byElement, string valueToEnter, string elementName);

        void CheckLabelsOnPage(Dictionary<string, string> dict, string className, object classObject, Type classType);

        void VerifyCheckbox(By byElement, string checkedStatus, string elementName);

        /* void VerifyElementExistOnPage(By byElement, bool isExist, string elementName); */

        void VerifyThatElementIsNOTInDOM(By byElement, string elementName);

        /* void VerifyElementIsDisplayed(By byElement, bool isDisplayed, string elementName); */

        void VerifyElementDisplayedEnabled(By byElement, string enabledStatus, string displayedStatus, string elementName);

        void MoveToElement(By byElement, string elementName);

        void JSExecute(string scriptToExecute);

        void OpenURL(string url);
    }
}
