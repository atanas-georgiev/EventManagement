namespace EventManagement.UserManagement.Data.Seed
{
    using System;
    using System.Linq;

    using EventManagement.UserManagement.Shared.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceScopeFactoryExtensions
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory, RoleManager<IdentityRole> roleManager)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<UserManagementDbContext>();
                try
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();

                    if (!context.Roles.Any())
                    {
                        SeedData(context, roleManager);
                    }
                }
                catch (Exception e)
                {
                    // N/A
                }
            }
        }

        private static void SeedData(UserManagementDbContext context, RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole("User")).GetAwaiter().GetResult();
            context.SaveChanges();
        }
    }
}
