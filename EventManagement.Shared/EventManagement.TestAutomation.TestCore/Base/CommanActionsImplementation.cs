namespace EventManagement.TestAutomation.TestCore.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    using OpenQA.Selenium;

    using TechTalk.SpecFlow;

    public class CommanActionsImplementation
    {
        Asserts exAssert = new Asserts(BaseStepDefinitions.steps);


        /// <summary>
        /// Create object of all parameters nedded for element.
        /// </summary>
        /// <param name="elementParameter">All parameters needed separeated with ",".</param>
        /// <returns>Return object[].</returns>
        public object[] ParametersObject(string elementParameter)
        { //Create object of all parameters nedded for element
           
            if (elementParameter == "")
            {
               return null;
            }
            else
            {
                if (elementParameter.IndexOf(",,") > 0)
                {
                    //1,,2
                    return elementParameter.Split(new[] { ",," }, StringSplitOptions.None).Select(p => (Object)p).ToArray();
                }
                else
                {
                    return new object[] { elementParameter };// "2" 
                }
            }
        }

        public Type FindClassTypeFromAssemblyByString(string className)
        {
            if (BaseStepDefinitions.FindTypeByNameMethod == null)
            {
                exAssert.RecordCantFindErrors(className, "AssemblyNotinitialized");
                throw new Exception("AssemblyNotinitialized");
            }
            var classType = BaseStepDefinitions.FindTypeByNameMethod(Regex.Replace(className, @"\s+", ""));
            if (classType == null)
            {
                exAssert.RecordCantFindErrors(className, "MissingClassType");
            }
            return classType;
        }

        public object ResolveClassTypeFromAssemblyByString(Type classType, string className)
        {
            var classObject = UnityContainerFactory.GetContainer().Resolve(classType, className);
            if (classObject == null)
            {
                exAssert.RecordCantFindErrors(className, "CantResolveClassType");
            }
            return classObject;
        }

        public ICommanActions ResolveClassTypeToICommanActions(Type classType, string className)
        {
            var classInterface = (ICommanActions)UnityContainerFactory.GetContainer().Resolve(classType, className);
            if (classInterface == null)
            {
                exAssert.RecordCantFindErrors(className, "CantResolveInerface");
            }
            return classInterface;
        }

        /// <summary>
        /// Click element from specific page.
        /// </summary>
        /// <param name="buttonName">The name of the element = method name.</param>
        /// <param name="_className">The name of the page = class name where your element is declared.</param>
        public void Click(string _buttonName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_buttonName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);
           

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            classInterface.Click(methodObject, _buttonName);
        }

        /// <summary>
        /// Click element from specific page.
        /// </summary>
        /// <param name="buttonName">The name of the element = method name.</param>
        /// <param name="_className">The name of the page = class name where your element is declared.</param>
        public void MoveToElement(string _buttonName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_buttonName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);


            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            classInterface.MoveToElement(methodObject, _buttonName);
        }

        /// <summary>
        /// Execute JS
        /// </summary>
        /// <param name="_elementName">element name</param>
        /// <param name="_className">page object</param>
        /// <param name="elementParameter">parameters for the element</param>
        /// <param name="_jsString">js string to execute</param>
        public void JSExecute(string _elementName, string _className, string elementParameter, string _jsString)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);


            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            // Get element locator from By object 
            var methodObjectToString = methodObject.ToString();
            var indForLocator = methodObjectToString.IndexOf(": ");
            var locator = methodObjectToString.Substring(indForLocator + 2);

            // Replace "elementLocatorString" parameter with real locator.
            var executableString = _jsString.Replace("elementLocatorString", locator);
            classInterface.JSExecute(executableString);
        }

        public void OpenURL(string _elementName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);


            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var urlToOpen = (String)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);

            classInterface.OpenURL(urlToOpen);
        }


        /// <summary>
        /// Check that text Exist on page 
        /// </summary>
        /// <param name="text">Text that we expect to see in the page.</param>
        /// <param name="_className">The name of the page = class name where your element is declared.</param>
        /// /// <param name="_elementName">The name of the element = method name.</param>
        public void CheckProperty(string expectedValue, string _className, string _elementName, string elementParameter, string propertyToCheck)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var withoutslash = buttonName.Replace("/", "");
            var clearName = withoutslash;
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == clearName).Invoke(classObject, parametersForElement);
            classInterface.CheckProperty(methodObject, propertyToCheck, expectedValue, _elementName);

        }

       
        public void CheckDropDownSelectedIndex(string indexText, string _className, string _elementName, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");
            var stringToIndex = Regex.Replace(_elementName + "StringToIndex", @"\s+", "");
           
            //Create object of all parameters nedded for StringToIndex
            object[] parametersStringToIndex = new object[] { indexText };
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var withoutslash = buttonName.Replace("/", "");
            var clearName = withoutslash;
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == clearName).Invoke(classObject, parametersForElement);
            var methodObjectIndex = (String)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == stringToIndex).Invoke(classObject, parametersStringToIndex);
            classInterface.CheckProperty(methodObject, "selectedIndex", methodObjectIndex, _elementName);

        }

        public void VerifyThatElementIsNOTInDOM(string _className, string _elementName, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var withoutslash = buttonName.Replace("/", "");
            var clearName = withoutslash;
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == clearName).Invoke(classObject, parametersForElement);
            classInterface.VerifyThatElementIsNOTInDOM(methodObject, _elementName);
        }

        /// <summary>
        /// Find the element from Page_Object folder. Verify Displayed and Enabled statuses of the element and record the result in ExtentEx report. If you leave empty enabledStatus or displayedStatus property the method will not check this status.
        /// </summary>
        /// <param name="enabledStatus">Expected condition of element.Enabled status. String value "true" or "false"</param>
        /// <param name="displayedStatus">Expected condition of element.Dispalyed status. String value "true" or "false"</param>
        /// <param name="_className">The name of the class where to search for the method. For Example: HomePage</param>
        /// <param name="_elementName">The name of the element which is == to method name.</param>
        /// <param name="elementParameter">Parameter of the element. If the method of the element don't have parameters leave it string.Empty</param>
        public void VerifyElementDisplayedEnabled(string enabledStatus, string displayedStatus, string _className, string _elementName, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var withoutslash = buttonName.Replace("/", "");
            var clearName = withoutslash;
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == clearName).Invoke(classObject, parametersForElement);
            classInterface.VerifyElementDisplayedEnabled(methodObject, enabledStatus, displayedStatus, _elementName);
        }

        public void ClickRadioButton(string _elementName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            classInterface.Click(methodObject, _elementName);
            classInterface.Click(methodObject, _elementName);
        }

        public void VerifyCheckbox(string _elementName, string _className, string checkedStatus, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var withoutslash = buttonName.Replace("/", "");
            var clearName = withoutslash;
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == clearName).Invoke(classObject, parametersForElement);

            classInterface.VerifyCheckbox(methodObject, checkedStatus, _elementName);

        }

        /// <summary>
        /// Enter value in Field
        /// </summary>
        /// <param name="valueToEnter">Value that we want to enter in the field.</param>
        /// <param name="_className">The name of the page = class name where your element is declared.</param>
        /// /// <param name="_elementName">The name of the element = method name.</param>
        public void EnterValueInField(string valueToEnter, string _elementName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            classInterface.EnterValueInField(methodObject, valueToEnter, _elementName);

        }

        /// <summary>
        /// Enter value in Kendo DatePicker
        /// </summary>
        /// <param name="valueToEnter">Value that we want to enter in the field.</param>
        /// <param name="_className">The name of the page = class name where your element is declared.</param>
        /// /// <param name="_elementName">The name of the element = method name.</param>
        public void EnterValueInFieldInDatePicker(string valueToEnter, string _elementName, string _className, string elementParameter)
        {
            //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");
            var buttonName = Regex.Replace(_elementName, @"\s+", "");

            //Create object of all parameters nedded for element
            object[] parametersForElement = ParametersObject(elementParameter);

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            //Get the element in By type
            var methodObject = (By)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == buttonName).Invoke(classObject, parametersForElement);
            classInterface.EnterValueInDatePicker(methodObject, valueToEnter, _elementName);

        }


        /// <summary>
        /// Check Labels which are collected in dictionary like the key in the dictionary is = to name of the element in the class
        /// </summary>
        /// <param name="_className">The name of the page = class name where the elements are declared.</param>
        public void CheckLabelsOnPage(string _className)
        { //Remove spaces from class and button name
            var className = Regex.Replace(_className, @"\s+", "");

            //Initialize classType, classObject and the Interface with comman actions
            var classType = FindClassTypeFromAssemblyByString(className);
            var classObject = ResolveClassTypeFromAssemblyByString(classType, className);
            var classInterface = ResolveClassTypeToICommanActions(classType, className);

            String dict;
            switch (FeatureContext.Current["Language"].ToString().ToLower())
            {
                case "en":
                    dict = "en_Labels";
                    break;
                case "ge":
                    dict = "ge_Labels";
                    break;
                default:
                    dict = "en_Labels";
                    break;
            }
            var methodObject = (Dictionary<string, string>)classType.GetRuntimeMethods().FirstOrDefault(z => z.Name == Regex.Replace(dict, @"\s+", "")).Invoke(classObject, null);
            classInterface.CheckLabelsOnPage(methodObject, className, classObject, classType);
        }

    }
}
