using KPMG.TestAutomation.Paths;
using NUnit.Framework;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace EventManagement.Registration.Web.Test.Base
{
    public class Helpers
    {

        public void ClearAndImportDB()
        {
            var connectionstring = TestContext.Parameters["DBConnectionString"];
            SqlConnection conn = new SqlConnection(TestContext.Parameters["DBConnectionString"]);

            //SqlConnection conn = new SqlConnection(dictionary["DBConnectionString"]);
            conn.Open();

            SqlCommand paiedEventExist = new SqlCommand("select Id from Events where EventId = 33", conn);
            SqlCommand notPaiedEventExist = new SqlCommand("select Id from Events where EventId = 34", conn);
            var paiedEventExistRows = paiedEventExist.ExecuteReader().HasRows;
            var notPaiedEventExistRows = paiedEventExist.ExecuteReader().HasRows;
            if (paiedEventExistRows)
            {
                //Remove paiedEvent registrations
                SqlCommand myCommand_0_1 = new SqlCommand("delete from Registrations WHERE EventId = (select Id from Events where EventId = 33)", conn);
                myCommand_0_1.ExecuteNonQuery();

                //Remove paiedEvent
                SqlCommand myCommand_0_2 = new SqlCommand("DELETE FROM [EventManagement.Registration].[dbo].[Events] WHERE EventId = 33", conn);
                myCommand_0_2.ExecuteNonQuery();
            }

            if (notPaiedEventExistRows)
            {
                //Remove not paiedEvent registrations
                SqlCommand myCommand_0_1 = new SqlCommand("delete from Registrations WHERE EventId = (select Id from Events where EventId = 34)", conn);
                myCommand_0_1.ExecuteNonQuery();

                //Remove not paiedEvent
                SqlCommand myCommand_0_2 = new SqlCommand("DELETE FROM [EventManagement.Registration].[dbo].[Events] WHERE EventId = 34", conn);
                myCommand_0_2.ExecuteNonQuery();                
            }

            //Imort paiedEvent
            SqlCommand myCommand_0_3 = new SqlCommand("INSERT INTO [EventManagement.Registration].[dbo].[Events] (AdditionalInfo,[End],EventName,LecturerName,[Location],Price,ResourceName,[Start],EventId,ResourcePlacesCount) VALUES('Automation Paied info', '2018-07-15 10:00:00.0000000', 'Automation Paied', 'Teodor, Marina', 'KPMG', 45, 'Automation Paied resource', '2018-07-15 08:00:00.0000000', 33, 25);", conn);
            myCommand_0_3.ExecuteNonQuery();

            //Import not paiedEvent
            SqlCommand myCommand_0_4 = new SqlCommand("INSERT INTO [EventManagement.Registration].[dbo].[Events] (AdditionalInfo,[End],EventName,LecturerName,[Location],Price,ResourceName,[Start],EventId,ResourcePlacesCount) VALUES('Automation Not Paied info', '2018-07-20 10:00:00.0000000', 'Automation Not Paied', 'Margarita', 'KPMG', 0, 'Automation Not Paied resource', '2018-07-20 08:00:00.0000000', 34, 25); ", conn);
            myCommand_0_4.ExecuteNonQuery();

            conn.Close();
        }

        public void PayEvent()
        {
            var connectionstring = TestContext.Parameters["DBConnectionString"];
            SqlConnection conn = new SqlConnection(TestContext.Parameters["DBConnectionString"]);

            //SqlConnection conn = new SqlConnection(dictionary["DBConnectionString"]);
            conn.Open();

            SqlCommand paiedEventExist = new SqlCommand("select Id from Events where EventId = 33", conn);
            var paiedEventExistRows = paiedEventExist.ExecuteReader().HasRows;
            if (paiedEventExistRows)
            {
                //Update PaymentStatus to payed
                SqlCommand myCommand_0_1 = new SqlCommand("update Registrations set PaymentStatus=1 WHERE EventId = (select Id from Events where EventId = 33)", conn);
                myCommand_0_1.ExecuteNonQuery();
            }

            conn.Close();
        }

    }

}
