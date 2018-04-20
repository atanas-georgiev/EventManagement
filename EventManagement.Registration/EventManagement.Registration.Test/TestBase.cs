namespace EventManagement.Registration.Test
{
    using System.Net.Http;
    using EventManagement.Registration.Web;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;

    public class TestBase
    {
        private TestServer server;

        protected HttpClient Client { get; private set; }

        public void Init()
        {
            this.server = new TestServer(new WebHostBuilder().UseEnvironment("IntegrationTest").UseStartup<Startup>());
            this.Client = this.server.CreateClient();
        }
    }
}
