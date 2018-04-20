namespace EventManagement.Portal.Data.Seed
{
    using System;
    using System.Linq;

    using EventManagement.Portal.Data;

    using KPMG.TaxRay.Portal.Core.Entities;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceScopeFactoryExtensions
    {
        public static void SeedData(this IServiceScopeFactory scopeFactory)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<PortalDbContext>();
                try
                {
                    context.Database.Migrate();
                    context.Database.EnsureCreated();

                    if (!Queryable.Any<PortalLink>(context.PortalLinks))
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

        private static void SeedData(PortalDbContext context)
        {
            var lang1 = new UserLanguage() { Description = "Deutsch" };
            var lang2 = new UserLanguage() { Description = "English" };
            context.UserLanguages.Add(lang1);
            context.UserLanguages.Add(lang2);
            context.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd.MM.yyyy" });
            context.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd/MM/yyyy" });
            context.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "dd-MM-yyyy" });
            context.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "MM/dd/yyyy" });
            context.UserDateTimeFormats.Add(new UserDateTimeFormat() { Description = "yyyy-MM-dd" });
            context.PortalLinks.Add(new PortalLink() { IsAdmin = false, Language = lang1, Title = "UserManagement", Link = "/EventManagement/UserManagement"});
            context.PortalLinks.Add(new PortalLink() { IsAdmin = false, Language = lang2, Title = "UserManagement", Link = "/EventManagement/UserManagement" });
            context.SaveChanges();
        }
    }
}