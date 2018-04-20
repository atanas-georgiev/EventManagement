namespace EventManagement.Payment.Data.Seed
{
    using System;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceScopeFactoryExtensions
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PaymentDbContext>();
                try
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();

                    if (!context.Payments.Any())
                    {
                        SeedData(context);
                    }
                }
                catch (Exception e)
                {
                    // N/A
                }
            }
        }

        private static void SeedData(PaymentDbContext context)
        {
        }
    }
}
