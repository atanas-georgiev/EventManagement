namespace EventManagement.Registration.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;
    using EventManagement.Registration.Services.Handlers;
    using EventManagement.Registration.Web;
    using EventManagement.Resources.Shared.Events;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;

    [TestFixture]
    public class DataStoredByMessageTest : TestBase
    {
        private NewEventHandler newEventHandler;

        private DeleteEventHandler deleteEventHandler;

        [SetUp]
        public void SetUp()
        {
            this.Init();
            this.newEventHandler = new NewEventHandler();
            this.deleteEventHandler = new DeleteEventHandler();
        }

        [Test]
        [Category("IntegrationTest")]
        public void AddEventMessageShouldAddDatabaseEntry()
        {
            var message = new AddEvent
            {
                EventId = 1,
                EventName = "TestEvent",
                ResourcePlacesCount = 10,
                Price = 100,
                Location = "TestLocation",
                Start = DateTime.Parse("2018-07-15 08:00:00.0000000"),
                End = DateTime.Parse("2018 -07-15 10:00:00.0000000"),
                LecturerName = "TestName",
                ResourceName = "TestResource",
                AdditionalInfo = "TestInfo"
            };

            this.newEventHandler.DbContext =
                Startup.ServiceProvider.GetService(typeof(RegistrationDbContext)) as RegistrationDbContext;
            NServiceBus.Testing.Test.Handler(this.newEventHandler).OnMessage(message);

            var getEvents = new HttpRequestMessage(HttpMethod.Get, "api/Event");
            var getEventsResponse = this.Client.SendAsync(getEvents).GetAwaiter().GetResult();
            var content = getEventsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //JObject getAvailableSettingsJson = JObject.Parse(content);
            var apiResult = JsonConvert.DeserializeObject<List<Event>>(content);

            Assert.AreEqual(apiResult.Count(), 1);
            var check = apiResult.First();
            Assert.AreEqual(check.EventId, message.EventId);
            Assert.AreEqual(check.EventName, message.EventName);
            Assert.AreEqual(check.ResourcePlacesCount, message.ResourcePlacesCount);
            Assert.AreEqual(check.Price, message.Price);
            Assert.AreEqual(check.Location, message.Location);
            Assert.AreEqual(check.Start, message.Start);
            Assert.AreEqual(check.End, message.End);
            Assert.AreEqual(check.LecturerName, message.LecturerName);
            Assert.AreEqual(check.ResourceName, message.ResourceName);
            Assert.AreEqual(check.AdditionalInfo, message.AdditionalInfo);

            var getEventDetails = new HttpRequestMessage(HttpMethod.Get, "api/Event/1");
            var getEventDetailsResponse = this.Client.SendAsync(getEventDetails).GetAwaiter().GetResult();
            var contentDetails = JsonConvert.DeserializeObject<Event>(getEventDetailsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            Assert.IsNotNull(contentDetails);

            var getEventDetailsNotFound = new HttpRequestMessage(HttpMethod.Get, "api/Event/100");
            var getEventDetailsNotFoundResponse = this.Client.SendAsync(getEventDetailsNotFound).GetAwaiter().GetResult();
            Assert.AreEqual(getEventDetailsNotFoundResponse.StatusCode, HttpStatusCode.NotFound);
        }

        [Test]
        [Category("IntegrationTest")]
        public void DeleteEventMessageShouldAddDatabaseEntry()
        {
            var message = new AddEvent
            {
                EventId = 1,
                EventName = "TestEvent",
                ResourcePlacesCount = 10,
                Price = 100,
                Location = "TestLocation",
                Start = DateTime.Parse("2018-07-15 08:00:00.0000000"),
                End = DateTime.Parse("2018 -07-15 10:00:00.0000000"),
                LecturerName = "TestName",
                ResourceName = "TestResource",
                AdditionalInfo = "TestInfo"
            };

            this.newEventHandler.DbContext =
                Startup.ServiceProvider.GetService(typeof(RegistrationDbContext)) as RegistrationDbContext;
            NServiceBus.Testing.Test.Handler(this.newEventHandler).OnMessage(message);
            var getEvents = new HttpRequestMessage(HttpMethod.Get, "api/Event");
            var getEventsResponse = this.Client.SendAsync(getEvents).GetAwaiter().GetResult();
            var content = getEventsResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var apiResult = JsonConvert.DeserializeObject<List<Event>>(content);

            var beforeDeletion = apiResult.Count();

            var eventId = apiResult.First().EventId;

            this.deleteEventHandler.DbContext = Startup.ServiceProvider.GetService(typeof(RegistrationDbContext)) as RegistrationDbContext;
            NServiceBus.Testing.Test.Handler(this.deleteEventHandler).OnMessage(new DeleteEvent { EventId = eventId });

            var getEventsDeleted = new HttpRequestMessage(HttpMethod.Get, "api/Event");
            var getEventsDeletedResponse = this.Client.SendAsync(getEventsDeleted).GetAwaiter().GetResult();
            var contentDeleted = getEventsDeletedResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var apiResultDeleted = JsonConvert.DeserializeObject<List<Event>>(contentDeleted);

            Assert.AreEqual(apiResultDeleted.Count(), (beforeDeletion - 1));
        }
    }
}
