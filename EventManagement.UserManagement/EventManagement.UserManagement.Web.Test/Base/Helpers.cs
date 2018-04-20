using KPMG.TestAutomation.Paths;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventManagement.UserManagement.Web.Test.Base
{
    public class Helpers
    {
        public void CreateUpdateTC()
        {
            var allFiles = Directory.GetFiles(ReturnPath.ProjectFolderPath() + "Features");
            Assembly a = typeof(BaseStepDefinitionsReInitialization).Assembly;
            var ab = a.GetTypes();
            foreach (string an in allFiles)
            {
                Debug.WriteLine("FileName: " + an);

                if (an.Contains(".feature") && an.Contains(".cs"))
                {
                    //var filePath = System.IO.File.ReadAllText(an);
                    //var scenarios = filePath.Split(new string[] { "[NUnit.Framework.TestAttribute()]" }, StringSplitOptions.None);
                    //var featureName = scenarios[0].Split(new string[] { "[NUnit.Framework.TestFixtureAttribute()]" }, StringSplitOptions.None)[1].Split(new string[] { "[NUnit.Framework.DescriptionAttribute(\"" }, StringSplitOptions.None)[1].Split(new string[] { "\")" }, StringSplitOptions.None)[0].Trim();
                    KPMG.TestAutomation.Reports.TFS.Helper.TFSConnection(an);

                }
            }
        }

        public void ClearDB()
        {
            var connectionstring = TestContext.Parameters["DBConnectionString"];
            SqlConnection conn = new SqlConnection(TestContext.Parameters["DBConnectionString"]);

            //SqlConnection conn = new SqlConnection(dictionary["DBConnectionString"]);
            conn.Open();

            SqlCommand a = new SqlCommand("select Id from AspNetUsers where Email = 'test11@ss.com'", conn);
            var bb = a.ExecuteReader().HasRows;
            if (bb)
            {
                //Remove Projects
                SqlCommand myCommand_0_1 = new SqlCommand("delete from [EventManagement.UserManagement.Dev.].[dbo].[AspNetUserRoles] where UserId = (select Id from AspNetUsers where Email = 'test11@ss.com')", conn);
                myCommand_0_1.ExecuteNonQuery();

                //Remove Users for Projects
                SqlCommand myCommand_0_2 = new SqlCommand("delete from [EventManagement.UserManagement.Dev.].[dbo].[AspNetUsers]  where Email = 'test11@ss.com'", conn);
                myCommand_0_2.ExecuteNonQuery();
            }
        }
    }
}
