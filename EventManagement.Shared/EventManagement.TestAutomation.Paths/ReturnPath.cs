namespace EventManagement.TestAutomation.Paths
{
    using System;
    using System.IO;
    using System.Reflection;

    public class ReturnPath
    {
        //GetProjectFolderPath
        /// <summary>
        ///  To find the project folder path.
        /// </summary>
        /// <returns>The path of the project in string format.</returns>
        public static String ProjectFolderPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../../";
        }

        public static String SolutionFolderPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/../../../";
        }

        //GetScreenshotPath
        /// <summary>
        ///  The screenshots folder and prefix for all screenshtots: screenshot.
        /// </summary>
        /// <returns>Project path + "ScreenShots/screenshot".</returns>
        public static String ScreenshotPath()
        {
            return ProjectFolderPath() + "ScreenShots/screenshot";
        }

        //ScreenshotFilePath
        /// <summary>
        ///  Method that create the name of the screenshot with time stamp
        /// </summary>
        /// <returns>All File path for a screenshot.</returns>
        public static String ScreenshotFilePath(String dateFormatFileName)

        {
            string allpath = "file:/" + "/" + "/" + ScreenshotPath() + dateFormatFileName + ".png";
            return allpath;
        }

        //TFSPropertiesFilePath
        /// <summary>
        ///  Path to the file that contains TFS information
        /// </summary>
        /// <returns>Project path + TFSProperties.txt</returns>
        public static String TFSPropertiesFilePath()
        {
            return ProjectFolderPath() + "TFSProperties.txt";
        }

        public static String ReportPropertiesFilePath()
        {
            return ProjectFolderPath() + "ExtentReport/ReportProperties.txt";
        }
    }
}
