namespace EventManagement.Portal.Web.Test
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using EventManagement.Portal.Data;
    using EventManagement.Portal.Data.Seed;

    using KPMG.TaxRay.Portal.Core.Entities;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json.Linq;

    using NUnit.Framework;

    [TestFixture]
    public class UserSettingsNoAdminUser
    {
        private TestServer server;

        private HttpClient client;

        [SetUp]
        public void SetUp()
        {
            IntegrationSeed.Method = this.DbSeed;
            this.server = new TestServer(new WebHostBuilder().UseEnvironment("IntegrationTest").UseStartup<Startup>());
            this.client = this.server.CreateClient();
        }

        [Test]
        [Category("IntegrationTest")]
        public void Authorization_NOAdminUser()
        {
            var getSettings = new HttpRequestMessage(HttpMethod.Get, "api/settings");
            var getSettingsResponse = this.client.SendAsync(getSettings).GetAwaiter().GetResult();
            Assert.AreEqual(getSettingsResponse.StatusCode, HttpStatusCode.Unauthorized);

            this.client = AuthorizationFactory.NOAdminUser(this.client);
            var getSettings2 = new HttpRequestMessage(HttpMethod.Get, "api/settings");
            var getSettingsResponse2 = this.client.SendAsync(getSettings2).GetAwaiter().GetResult();
            Assert.AreEqual(getSettingsResponse2.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        [Category("IntegrationTest")]
        public void Post_UserSettings_NOAdminUser()
        {

            var postJson = "{\"Language\":\"2\",\"DateFormat\":\"5\"}";
            var postSettingsRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson,
                            Encoding.UTF8,
                            "application/json")
                };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var postSettingsResponse = this.client.SendAsync(postSettingsRequest).GetAwaiter().GetResult();
            Assert.AreEqual(postSettingsResponse.StatusCode, HttpStatusCode.OK);
            var postContent = postSettingsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject postSettingsJson = JObject.Parse(postContent);
            Assert.AreEqual("5", postSettingsJson["dateFormat"].ToString());
            Assert.AreEqual("2", postSettingsJson["language"].ToString());

            var getSettings =
                new HttpRequestMessage(HttpMethod.Get, "/api/Settings")
                { };

            var getSettingsResponse = this.client.SendAsync(getSettings).GetAwaiter().GetResult();
            Assert.AreEqual(getSettingsResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getSettingsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject getSettingsResponceJson = JObject.Parse(getContent);
            Assert.AreEqual("5", getSettingsResponceJson["dateFormat"].ToString());
            Assert.AreEqual("2", getSettingsResponceJson["language"].ToString());
        }

        [Test]
        [Category("IntegrationTest")]
        public void Post_EditUserSettings_NOAdminUser()
        {
            var postJson = "{\"dateFormat\":5,\"language\":2}";
            var postSettingsRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson,
                            Encoding.UTF8,
                            "application/json")
                };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var postSettingsResponse = this.client.SendAsync(postSettingsRequest).GetAwaiter().GetResult();
            Assert.AreEqual(postSettingsResponse.StatusCode, HttpStatusCode.OK);
            var postContent = postSettingsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject postSettingsJson = JObject.Parse(postContent);
            Assert.AreEqual("5", postSettingsJson["dateFormat"].ToString());
            Assert.AreEqual("2", postSettingsJson["language"].ToString());

            var getSettings =
                new HttpRequestMessage(HttpMethod.Get, "/api/Settings")
                { };

            var getSettingsResponse = this.client.SendAsync(getSettings).GetAwaiter().GetResult();
            Assert.AreEqual(getSettingsResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getSettingsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject getSettingsResponceJson = JObject.Parse(getContent);
            Assert.AreEqual("5", getSettingsResponceJson["dateFormat"].ToString());
            Assert.AreEqual("2", getSettingsResponceJson["language"].ToString());

            var postJson1 = "{\"dateFormat\":3,\"language\":1}";
            var postSettingsRequest1 =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson1,
                            Encoding.UTF8,
                            "application/json")
                };

            var postSettingsResponse1 = this.client.SendAsync(postSettingsRequest1).GetAwaiter().GetResult();
            Assert.AreEqual(postSettingsResponse1.StatusCode, HttpStatusCode.OK);
            var postContent1 = postSettingsResponse1.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject postSettingsJson1 = JObject.Parse(postContent1);
            Assert.AreEqual(postSettingsJson1["dateFormat"].ToString(), "3");
            Assert.AreEqual(postSettingsJson1["language"].ToString(), "1");

            var getSettings1 =
                new HttpRequestMessage(HttpMethod.Get, "/api/Settings")
                { };

            var getSettingsResponse1 = this.client.SendAsync(getSettings1).GetAwaiter().GetResult();
            Assert.AreEqual(getSettingsResponse1.StatusCode, HttpStatusCode.OK);
            var getContent1 = getSettingsResponse1.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject getSettingsResponceJson1 = JObject.Parse(getContent1);
            Assert.AreEqual("3", getSettingsResponceJson1["dateFormat"].ToString());
            Assert.AreEqual("1", getSettingsResponceJson1["language"].ToString());
        }

        [Test]
        [Category("IntegrationTest")]
        public void Post_EnterWrongValues_0_0_NOAdminUser()
        {
            
            var postJson = "{\"dateFormat\":0,\"language\":0}";
            var postSettingsRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson,
                            Encoding.UTF8,
                            "application/json")
                };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            Assert.Throws<System.NullReferenceException>(() =>
            {
                this.client.SendAsync(postSettingsRequest).GetAwaiter().GetResult();
            });
        }

        [Test]
        [Category("IntegrationTest")]
        public void Post_EnterWrongValues_1_6_NOAdminUser()
        {
            var postJson = "{\"dateFormat\":6,\"language\":1}";
            var postSettingsRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson,
                            Encoding.UTF8,
                            "application/json")
                };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            Assert.Throws<System.NullReferenceException>(() =>
            {
                this.client.SendAsync(postSettingsRequest).GetAwaiter().GetResult();
            });
        }

        [Test]
        [Category("IntegrationTest")]
        public void Post_EnterWrongValues_3_5_NOAdminUser()
        {
            var postJson = "{\"dateFormat\":5,\"language\":3}";
            var postSettingsRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Settings")
                {
                    Content = new StringContent(
                            postJson,
                            Encoding.UTF8,
                            "application/json")
                };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            Assert.Throws<System.NullReferenceException>(() =>
            {
                this.client.SendAsync(postSettingsRequest).GetAwaiter().GetResult();
            });
        }

        [Test]
        [Category("IntegrationTest")]
        public void GET_User_NOAdminUser()
        {
            var getUser =
                new HttpRequestMessage(HttpMethod.Get, "/api/User")
                { };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var getUserResponse = this.client.SendAsync(getUser).GetAwaiter().GetResult();
            Assert.AreEqual(getUserResponse.StatusCode, HttpStatusCode.OK);
            var content = getUserResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            Assert.AreEqual("\"Test NoAdmin\"", content);
        }

        [Test]
        [Category("IntegrationTest")]
        public void GET_AvailableSettings_NOAdminUser()
        {
           var getAvailableSettingsRequest =
                new HttpRequestMessage(HttpMethod.Get, "/api/AvailableSettings")
                { };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var getAvailableSettingsResponse = this.client.SendAsync(getAvailableSettingsRequest).GetAwaiter().GetResult();
            Assert.AreEqual(getAvailableSettingsResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getAvailableSettingsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            JObject getAvailableSettingsJson = JObject.Parse(getContent);
            Assert.AreEqual("dd.MM.yyyy", getAvailableSettingsJson["dateFormats"][0]["description"].ToString());
            Assert.AreEqual("dd/MM/yyyy", getAvailableSettingsJson["dateFormats"][1]["description"].ToString());
            Assert.AreEqual("dd-MM-yyyy", getAvailableSettingsJson["dateFormats"][2]["description"].ToString());
            Assert.AreEqual("MM/dd/yyyy", getAvailableSettingsJson["dateFormats"][3]["description"].ToString());
            Assert.AreEqual("yyyy-MM-dd", getAvailableSettingsJson["dateFormats"][4]["description"].ToString());
            Assert.AreEqual("1", getAvailableSettingsJson["dateFormats"][0]["id"].ToString());
            Assert.AreEqual("2", getAvailableSettingsJson["dateFormats"][1]["id"].ToString());
            Assert.AreEqual("3", getAvailableSettingsJson["dateFormats"][2]["id"].ToString());
            Assert.AreEqual("4", getAvailableSettingsJson["dateFormats"][3]["id"].ToString());
            Assert.AreEqual("5", getAvailableSettingsJson["dateFormats"][4]["id"].ToString());
            Assert.AreEqual("1", getAvailableSettingsJson["languages"][0]["id"].ToString());
            Assert.AreEqual("Deutsch", getAvailableSettingsJson["languages"][0]["description"].ToString());
            Assert.AreEqual("2", getAvailableSettingsJson["languages"][1]["id"].ToString());
            Assert.AreEqual("English", getAvailableSettingsJson["languages"][1]["description"].ToString());
        }

        [Test]
        [Category("IntegrationTest")]
        public void GET_GermanLinks_NOAdminUser()
        {
            var getLanguageIDRequest =
                new HttpRequestMessage(HttpMethod.Get, "/api/Links?languageId=1")
                { };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var getLanguageIDResponse = this.client.SendAsync(getLanguageIDRequest).GetAwaiter().GetResult();
            Assert.AreEqual(getLanguageIDResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getLanguageIDResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult().Replace("[", "").Replace("]", "").Trim();
            JObject getAvailableSettingsJson = JObject.Parse(getContent);

            Assert.AreEqual("/EventManagement/UserManagement", getAvailableSettingsJson["link"].ToString());
            Assert.AreEqual("UserManagement", getAvailableSettingsJson["title"].ToString());
        }

        [Test]
        [Category("IntegrationTest")]
        public void GET_EnglishLinks_NOAdminUser()
        {
            var getLanguageIDRequest =
                new HttpRequestMessage(HttpMethod.Get, "/api/Links?languageId=2")
                { };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var getLanguageIDResponse = this.client.SendAsync(getLanguageIDRequest).GetAwaiter().GetResult();
            Assert.AreEqual(getLanguageIDResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getLanguageIDResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult().Replace("[", "").Replace("]", "").Trim();
            JObject getAvailableSettingsJson = JObject.Parse(getContent);

            Assert.AreEqual("/EventManagement/UserManagement", getAvailableSettingsJson["link"].ToString());
            Assert.AreEqual("UserManagement", getAvailableSettingsJson["title"].ToString());
        }

        [Test]
        [Category("IntegrationTest")]
        public void GET_LinksNoSuchLanguage_NOAdminUser()
        {
            var getLanguageIDRequest =
                new HttpRequestMessage(HttpMethod.Get, "/api/Links?languageId=5")
                { };
            this.client = AuthorizationFactory.NOAdminUser(this.client);

            var getLanguageIDResponse = this.client.SendAsync(getLanguageIDRequest).GetAwaiter().GetResult();
            Assert.AreEqual(getLanguageIDResponse.StatusCode, HttpStatusCode.OK);
            var getContent = getLanguageIDResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            Assert.AreEqual("[]", getContent);
        }

    private void DbSeed(DbContext dbContext)
        {
            var portalContext = dbContext as PortalDbContext;

            if (portalContext.UserLanguages.Count() != 0)
            {
                return;
            }

            var lang1 = new UserLanguage() { Description = "Deutsch" };
            var lang2 = new UserLanguage() { Description = "English" };
            portalContext.UserLanguages.Add(lang1);
            portalContext.UserLanguages.Add(lang2);
            portalContext.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd.MM.yyyy" });
            portalContext.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd/MM/yyyy" });
            portalContext.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd-MM-yyyy" });
            portalContext.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "MM/dd/yyyy" });
            portalContext.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "yyyy-MM-dd" });
            portalContext.PortalLinks.Add(new PortalLink() { IsAdmin = false, Language = lang1, Title = "UserManagement", Link = "/EventManagement/UserManagement" });
            portalContext.PortalLinks.Add(new PortalLink() { IsAdmin = false, Language = lang2, Title = "UserManagement", Link = "/EventManagement/UserManagement" });
            portalContext.SaveChanges();
        }
    }
}
