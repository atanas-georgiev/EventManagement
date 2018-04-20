using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace EventManagement.Portal.Web.Test
{
    public static class AuthorizationFactory
    {
        public static HttpClient AdminUser(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("TestEmail", "test@email.com");
            client.DefaultRequestHeaders.Add("TestName", "Test Test");
            client.DefaultRequestHeaders.Add("TestAdmin", "true");
            return client;
        }

        public static HttpClient NOAdminUser(HttpClient client)
        {
            client.DefaultRequestHeaders.Add("TestEmail", "test@email.com");
            client.DefaultRequestHeaders.Add("TestName", "Test NoAdmin");
            client.DefaultRequestHeaders.Add("TestAdmin", "false");
            return client;
        }
    }
}
