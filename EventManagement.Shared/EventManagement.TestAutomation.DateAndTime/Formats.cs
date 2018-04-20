namespace EventManagement.TestAutomation.DateAndTime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Methods for put the date in different formats.
    /// </summary>
    public class Formats
    {
        // DateFormatDDMMYYYY
        /// <summary>
        /// Get today date in format 28.12.2016
        /// </summary>
        /// <returns>String with today date in format 28.12.2016</returns>
        public static String DateFormatDDMMYYYY()
        {
            return DateTime.Now.ToString("dd.MM.YYYY");
        }

        // DateFormatYYYYMMDDHhMMminSSsec
        /// <summary>
        /// Get today date in format 2016-12-28 10h 36min 03sec.
        /// </summary>
        /// <returns>String with today date in format 2016-12-28 10h 36min 03sec.</returns>
        public static String DateFormatYYYYMMDDHhMMminSSsec()
        {
            var dateFormat = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            string[] tokens = dateFormat.Split(':');
            string dateFormatFileName = tokens[0] + "h " + tokens[1] + "min " + tokens[2] + "sec";
            return dateFormatFileName;
        }

        // DateFormatYYYYMMDDHhMMminSSsec
        /// <summary>
        /// Get today date in format 2016-12-28 10h 36min 03sec.
        /// </summary>
        /// <returns>String with today date in format 2016-12-28 10h 36min 03sec.</returns>
        public static String DateFormatYYYYMMDDHhMMminSSsecShort()
        {
            var dateFormat = DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
            string[] tokens = dateFormat.Split(':');
            string dateFormatFileName = tokens[0] + "." + tokens[1] + "." + tokens[2];
            return dateFormatFileName;
        }

        // DateFormatYYYYMMDDHMMSS
        /// <summary>
        /// Get today date in format 2016-12-28 10:36:03.
        /// </summary>
        /// <returns>String with today date in format 2016-12-28 10:36:03.</returns>
        public static String DateFormatYYYYMMDDHMMSS()
        {
            return DateTime.Now.ToString("yyyy-MM-dd h:mm:ss");
        }

        // AddMothsToTodayDateDDMMYYYY
        /// <summary>
        /// Get today date in format 28.12.2016
        /// </summary>
        /// <returns>String with today date in format 2016-12-28 10:36:03.</returns>
        public static String AddMothsToTodayDateDDMMYYYY(int months)
        {
            return DateTime.Now.AddMonths(months).ToString("dd.MM.yyyy");
        }
    }
}

