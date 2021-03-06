﻿namespace EventManagement.Registration.Data.Seed
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceScopeFactoryExtensions
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<RegistrationDbContext>();
                try
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();

                    //if (!context.Events.Any())
                    //{
                    //    SeedData(context);
                    //}
                }
                catch (Exception e)
                {
                    // N/A
                }
            }
        }

        private static void SeedData(RegistrationDbContext context)
        {
        }
    }
}
