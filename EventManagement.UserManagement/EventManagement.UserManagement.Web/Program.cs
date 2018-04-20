namespace EventManagement.UserManagement.Web
{
    using System;
    using System.Diagnostics;
    using System.Fabric;
    using System.IO;
    using System.Threading;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
    using Microsoft.ServiceFabric.Services.Runtime;

    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            var serviceFabricHostMarker = Environment.GetEnvironmentVariable("ServiceFabricHostMarker");
            if (serviceFabricHostMarker != null)
            {
                try
                {
                    // The ServiceManifest.XML file defines one or more service type names.
                    // Registering a service maps a service type name to a .NET type.
                    // When Service Fabric creates an instance of this service type,
                    // an instance of the class is created in this host process.
                    ServiceRuntime.RegisterServiceAsync(
                        "EventManagement.UserManagement.WebType",
                        context => new Web(context)).GetAwaiter().GetResult();

                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(Web).Name);

                    // Prevents this host process from terminating so services keeps running. 
                    Thread.Sleep(Timeout.Infinite);
                }
                catch (Exception e)
                {
                    ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                    throw;
                }
            }
            else
            {
                IWebHost host = null;

                try
                {
                    host = new WebHostBuilder()
                        .UseHttpSys()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .UseUrls("http://*:5002")
                        .Build();
                    host.Run();
                }
                finally
                {
                    host?.Dispose();
                }
            }
        }
    }
}