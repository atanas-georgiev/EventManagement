namespace EventManagement.TestAutomation.StringOperations
{
    using System;
    using System.Configuration;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public static class ReturnString
    {
        // FormatingStackTrace
        /// <summary>
        /// Format the stack trace information to look good in ExtentEx report
        /// </summary>
        /// <param name="stTrace">String from stack trace.</param>
        /// <returns>Formated string from stack trace.</returns>
        public static String FormatingStackTrace(string stTrace)
        {
            var stacktrace = string.IsNullOrEmpty(stTrace)
                    ? ""
                    : string.Format("<pre>{0}</pre>", stTrace);
            return stacktrace;
        }

        // QuoteCheck
        /// <summary>
        /// Check string for '
        /// </summary>
        /// <param name="stringToBeChecked">String that will be checked.</param>
        /// <returns>????</returns>
        public static string QuoteCheck(string stringToBeChecked)
        {
            int indexOfTheQuote = stringToBeChecked.IndexOf("'");
            int skillLength = stringToBeChecked.Length - indexOfTheQuote - 1;
            if (stringToBeChecked.Contains("'"))
            {
                stringToBeChecked = stringToBeChecked.Substring(0, indexOfTheQuote) + "') and contains(text(),'" + stringToBeChecked.Substring(indexOfTheQuote + 1, skillLength) + "";
            }
            return stringToBeChecked;
        }

        // GetCurrentMethod
        /// <summary>
        /// Get current method Name
        /// </summary>
        /// <returns>The name of the method in which we call GetCurrentMethod()</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }

        // GetCurrentClass
        /// <summary>
        /// Get current class Name
        /// </summary>
        /// <returns>The name of the class in which we call GetCurrentClass()</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetCurrentClass()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return sf.GetMethod().ReflectedType.Name;
        }

        // GetTestName
        /// <summary>
        /// Extract Test name from string: "TestName(\"Browser\")". We get this string TestContext.CurrentContext.Test.Name
        /// </summary>
        /// <param name="nonFormatName">string: "TestName(\"Browser\")"</param>
        /// <returns>The Test name</returns>
        public static string GetTestName(string nonFormatName)
        {
            var namearray = nonFormatName.Split('"');
            var name_1 = namearray[0].Split('(');
            return name_1[0];
        }

        // GetBrowserName
        /// <summary>
        /// Extract Browser name from string: "TestName(\"Browser\")". We get this string TestContext.CurrentContext.Test.Name
        /// </summary>
        /// <param name="nonFormatName">string: "TestName(\"Browser\")"</param>
        /// <returns>The Browser name</returns>
        public static string GetBrowserName(string nonFormatName)
        {
            var namearray = nonFormatName.Split('"');
            var browsserName = namearray[1].Split('\\');
            return browsserName[0];
        }

        // GetURLFromConfigStr
        /// <summary>
        /// Extract URL from app.config file
        /// </summary>
        /// <returns>Application URL</returns>
        public static string GetURLFromConfigStr()
        {
            return ConfigurationManager.AppSettings["AppURL"];
        }

        /// <summary>
        /// Extract value from app.config file
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>The method return Desired value</returns>
        public static string[] GetValueFromConfig(string key)
        {
            var keyValueString = ConfigurationManager.AppSettings[key];
            string[] keyValues = keyValueString.Split(',');
            return keyValues;
        }

        public static bool StringIsPartOfArray(string[] arrayToSearchIn, string valueToSearch)
        {
            foreach (string x in arrayToSearchIn)
            {
                if (x.ToLower().Contains(valueToSearch.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool StringIsValueOfArray(string[] arrayToSearchIn, string valueToSearch)
        {
            foreach (string x in arrayToSearchIn)
            {
                if (x.ToLower().Equals(valueToSearch.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
