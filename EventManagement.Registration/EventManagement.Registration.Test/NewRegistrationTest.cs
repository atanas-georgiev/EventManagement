namespace EventManagement.Registration.Test
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;

    using EventManagement.Payment.Shared.Events;
    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;
    using EventManagement.Registration.Data.Seed;
    using EventManagement.Registration.Services.Handlers;
    using EventManagement.Registration.Web;

    using Microsoft.EntityFrameworkCore;

    using Newtonsoft.Json;

    using NUnit.Framework;

    [TestFixture]
    public class NewRegistrationTest : TestBase
    {
        private CompletePaymentHandler completePaymentHandler;

        private CancelPaymentHandler cancelPaymentHandler;

        private RegistrationDbContext registrationDbContext;

        [SetUp]
        public void SetUp()
        {
            this.completePaymentHandler = new CompletePaymentHandler();
            this.cancelPaymentHandler = new CancelPaymentHandler();

            IntegrationSeed.Method = this.Seed;
            this.Init();

            this.registrationDbContext = Startup.ServiceProvider.GetService(typeof(RegistrationDbContext)) as RegistrationDbContext;
        }

        [Test]
        [Category("IntegrationTest")]
        public void NewRegistrationReuqestNoAuthenticationShouldReturnUnauthorized1()
        {
            var postNewRegistration = "{\"EventId\":\"1\"}";
            var postNewRegistrationRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Registration")
                {
                    Content = new StringContent(
                            postNewRegistration,
                            Encoding.UTF8,
                            "application/json")
                };

            var postNewRegistrationResponse = this.Client.SendAsync(postNewRegistrationRequest).GetAwaiter().GetResult();

            Assert.AreEqual(postNewRegistrationResponse.StatusCode, HttpStatusCode.Unauthorized);
        }

        [Test]
        [Category("IntegrationTest")]
        public void NewRegistrationReuqestShouldAddDatabaseEntry1()
        {
            var ev = JsonConvert.DeserializeObject<Event>(this.Client.GetStringAsync("api/Event/1").GetAwaiter().GetResult());

            var postNewRegistration = "{\"EventId\":\"1\"}";
            var postNewRegistrationRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Registration")
                {
                    Content = new StringContent(
                            postNewRegistration,
                            Encoding.UTF8,
                            "application/json")
                };

            postNewRegistrationRequest.Headers.Add("TestEmail", "test@email.com");
            postNewRegistrationRequest.Headers.Add("TestName", "Test Test");
            postNewRegistrationRequest.Headers.Add("TestAdmin", "true");
            var postNewRegistrationResponse = this.Client.SendAsync(postNewRegistrationRequest).GetAwaiter().GetResult();

            Assert.AreEqual(postNewRegistrationResponse.StatusCode, HttpStatusCode.OK);

            var ev2 = JsonConvert.DeserializeObject<Event>(this.Client.GetStringAsync("api/Event/1").GetAwaiter().GetResult());

            Assert.IsTrue(ev2.ResourcePlacesCount < ev.ResourcePlacesCount);
        }

        [Test]
        [Category("IntegrationTest")]
        public void NewRegistrationInvalidEventShouldReturnCorrectResult1()
        {
            var postNewRegistration = "{\"EventId\":\"100\"}";
            var postNewRegistrationRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Registration")
                {
                    Content = new StringContent(
                            postNewRegistration,
                            Encoding.UTF8,
                            "application/json")
                };

            postNewRegistrationRequest.Headers.Add("TestEmail", "test@email.com");
            postNewRegistrationRequest.Headers.Add("TestName", "Test Test");
            postNewRegistrationRequest.Headers.Add("TestAdmin", "true");
            var postNewRegistrationResponse = this.Client.SendAsync(postNewRegistrationRequest).GetAwaiter().GetResult();

            Assert.AreEqual(postNewRegistrationResponse.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        [Category("IntegrationTest")]
        public void CompletePaymentMessageShouldModifyRegistration1()
        {
            var postNewRegistration = "{\"EventId\":\"1\"}";
            var postNewRegistrationRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Registration")
                {
                    Content = new StringContent(
                            postNewRegistration,
                            Encoding.UTF8,
                            "application/json")
                };

            postNewRegistrationRequest.Headers.Add("TestEmail", "test@email.com");
            postNewRegistrationRequest.Headers.Add("TestName", "Test Test");
            postNewRegistrationRequest.Headers.Add("TestAdmin", "true");
            var postNewRegistrationResponse = this.Client.SendAsync(postNewRegistrationRequest).GetAwaiter().GetResult();
            Assert.AreEqual(postNewRegistrationResponse.StatusCode, HttpStatusCode.OK);

            var reg = this.registrationDbContext.Registrations.Last();
            Assert.AreEqual(reg.PaymentStatus, PaymentStatus.New);

            this.completePaymentHandler.DbContext = this.registrationDbContext;
            NServiceBus.Testing.Test.Handler(this.completePaymentHandler).OnMessage(new CompletePayment { PaymentId = reg.PaymentId });

            Assert.AreEqual(reg.PaymentStatus, PaymentStatus.Completed);
        }

        [Test]
        [Category("IntegrationTest")]
        public void CancelPaymentMessageShouldModifyRegistration1()
        {
            var postNewRegistration = "{\"EventId\":\"1\"}";
            var postNewRegistrationRequest =
                new HttpRequestMessage(HttpMethod.Post, "/api/Registration")
                {
                    Content = new StringContent(
                            postNewRegistration,
                            Encoding.UTF8,
                            "application/json")
                };

            postNewRegistrationRequest.Headers.Add("TestEmail", "test@email.com");
            postNewRegistrationRequest.Headers.Add("TestName", "Test Test");
            postNewRegistrationRequest.Headers.Add("TestAdmin", "true");
            var postNewRegistrationResponse = this.Client.SendAsync(postNewRegistrationRequest).GetAwaiter().GetResult();
            Assert.AreEqual(postNewRegistrationResponse.StatusCode, HttpStatusCode.OK);

            var reg = this.registrationDbContext.Registrations.Last();
            Assert.AreEqual(reg.PaymentStatus, PaymentStatus.New);

            this.cancelPaymentHandler.DbContext = this.registrationDbContext;
            NServiceBus.Testing.Test.Handler(this.cancelPaymentHandler).OnMessage(new CancelPayment { PaymentId = reg.PaymentId });

            Assert.AreEqual(reg.PaymentStatus, PaymentStatus.Canceled);
        }

        private void Seed(DbContext context)
        {
            var registrationContext = context as RegistrationDbContext;

            if (registrationContext.Events.Any())
            {
                return;
            }

            registrationContext?.Add(
                new Event
                {
                    EventId = 1,
                    EventName = "TestEvent",
                    ResourcePlacesCount = 5,
                    Price = 100,
                    Location = "TestLocation",
                    Start = DateTime.Now,
                    End = DateTime.Now,
                    LecturerName = "TestName",
                    ResourceName = "TestResource",
                    AdditionalInfo = "TestInfo"
                });
            registrationContext?.SaveChanges();
        }
    }
}
