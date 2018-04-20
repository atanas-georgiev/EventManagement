namespace EventManagement.Shared.ServiceBus
{
    using System;
    using System.Data.SqlClient;
    using System.Threading.Tasks;

    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    /// <summary>
    /// Factory class, used for NServiceBus configuration
    /// </summary>
    public static class EndpointConfigurationFactory
    {
        public static int ImmediateRetries { get; set; } = 5;

        public static int DelayedRetries { get; set; } = 5;

        public static TimeSpan DelayedTimeIncrease { get; set; } = TimeSpan.FromSeconds(15);

        public static string EndpointName { get; set; }

        /// <summary>
        /// Configures NServiceBus Endpoint
        /// </summary>
        /// <param name="endpointName">Endpoint Name</param>
        /// <param name="endpointInstallersEnabled">Enbpoint installers</param>
        /// <returns>New endpoint configuration</returns>
        public static EndpointConfiguration ConfigureEndpoint(string endpointName, bool endpointInstallersEnabled)
        {
            if (string.IsNullOrEmpty(endpointName))
            {
                throw new ArgumentNullException(nameof(endpointName), "Missing endpoint name!");
            }

            EndpointName = endpointName;

            var endpointConfiguration = new EndpointConfiguration(endpointName);
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.EnableOutbox(); // Automatically lowers the transaction level to ReceiveOnly, which is sufficient
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
            endpointConfiguration.Recoverability()
                .Immediate(immediate => { immediate.NumberOfRetries(ImmediateRetries); })
                .Delayed(
                    delayed =>
                    {
                        delayed.NumberOfRetries(DelayedRetries).TimeIncrease(DelayedTimeIncrease);
                    });

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace != null && type.Namespace.EndsWith(".Commands"));
            conventions.DefiningEventsAs(type => type.Namespace != null && type.Namespace.EndsWith(".Events"));

            if (endpointInstallersEnabled)
            {
                endpointConfiguration.EnableInstallers();
            }

            return endpointConfiguration;
        }

        /// <summary>
        /// Configures NServiceBus Endpoint Persistence
        /// </summary>
        /// <param name="endpointConfiguration">Endpoint Configuration</param>
        /// <param name="persistenceDataConnection">Connection string for persistence</param>
        /// <param name="cacheTime">Cache time in minutes</param>
        /// <param name="persistenceInstallersEnabled">Persistense installers</param>
        /// <returns>Endpoint configuration with SQL persistence</returns>
        public static EndpointConfiguration ConfigureSqlPersistence(
            this EndpointConfiguration endpointConfiguration,
            string persistenceDataConnection,
            int cacheTime,
            bool persistenceInstallersEnabled)
        {
            if (persistenceDataConnection == null)
            {
                throw new ArgumentNullException(nameof(persistenceDataConnection), "Missing persistence data connection string!");
            }

            if (cacheTime < 0)
            {
                throw new ArgumentException("Cache time in minutes value cannot be negative.", nameof(cacheTime));
            }

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(() => new SqlConnection(persistenceDataConnection));
            persistence.TablePrefix($"{EndpointName}.");
            persistence.SubscriptionSettings().CacheFor(TimeSpan.FromMinutes(cacheTime));

            if (!persistenceInstallersEnabled)
            {
                persistence.DisableInstaller();
            }

            return endpointConfiguration;
        }

        /// <summary>
        /// Configures NServiceBus Endpoint Transport
        /// </summary>
        /// <param name="endpointConfiguration">Endpoint Configuration</param>
        /// <param name="transportConnectionString">Connection string for SQL transport protocol</param>
        /// <param name="routeConfigurator">Route configuration settings</param>
        /// <returns>Endpoint configuration with SQL transport</returns>
        public static EndpointConfiguration ConfigureSqlTransport(
            this EndpointConfiguration endpointConfiguration,
            string transportConnectionString,
            Action<RoutingSettings<SqlServerTransport>> routeConfigurator)
        {
            if (string.IsNullOrEmpty(transportConnectionString))
            {
                throw new ArgumentNullException(nameof(transportConnectionString), "Missing transport connection string!");
            }

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.ConnectionString(transportConnectionString);

            if (routeConfigurator != null)
            {
                routeConfigurator(transport.Routing());
            }

            return endpointConfiguration;
        }

        /// <summary>
        /// Starts an endpoint using given configuration
        /// </summary>
        /// <param name="endpointConfiguration">Endpoint configuration</param>
        /// <returns>Endpoint instance</returns>
        public static async Task<IEndpointInstance> StartEndpointAsync(this EndpointConfiguration endpointConfiguration)
        {
            return await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
        }
    }
}
