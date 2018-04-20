namespace EventManagement.TestAutomation.Data
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;

    using AventStack.ExtentReports;

    using EventManagement.TestAutomation.Paths;

    using NUnit.Framework;

    using OpenQA.Selenium;

    /// <summary>
    /// Methods to work with IDictionary
    /// </summary>
    public class DictionaryInteractions
    {
        // ReadFromPropertiesFile
        /// <summary>
        /// Get values from txt file and save them in IDictionary.
        /// </summary>
        /// <param name="filePath">The path to the txt file.</param>
        /// <returns>Dictionary with values from txt file.</returns>
        public static IDictionary<string, string> ReadFromPropertiesFile(string filePath)
        {

            var dictionaryName = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines(filePath, Encoding.UTF8))
                dictionaryName.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
            return dictionaryName;

        }

        // WriteInTxtFileFromDictionary
        /// <summary>
        /// Set values in IDictionary and recored them in txt file.
        /// </summary>
        /// <param name="filePath">The path to the txt file.</param>
        /// <param name="dictionary">IDictionary object that we will record in the file.</param>
        /// <param name="keyName">The name of the key that must be updated.</param>
        /// <param name="keyValue">The value of the key that must be updated.</param>
        public static void WriteInTxtFileFromDictionary(string filePath, IDictionary<string, string> dictionary, string keyName, string keyValue)
        {
            dictionary[keyName] = keyValue;
            using (StreamWriter file = new StreamWriter(filePath))
                foreach (var entry in dictionary)
                {
                    file.WriteLine("{0}={1}", entry.Key, entry.Value);
                }
        }

        // TFSProp
        /// <summary>
        ///  TFSProp is a dictionary created from tfsproperies.txt file
        /// </summary>
        public static IDictionary<string, string> TFSProp
        {
            get
            {
                return ReadFromPropertiesFile(ReturnPath.TFSPropertiesFilePath());
            }
        }

        public static IDictionary<string, string> ReportPropertiesDictionary
        {
            get
            {
                return ReadFromPropertiesFile(ReturnPath.ReportPropertiesFilePath());
            }
        }
    }
}
