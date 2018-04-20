namespace EventManagement.Portal.Data
{
    using KPMG.TaxRay.Portal.Core.Entities;

    using Microsoft.EntityFrameworkCore;

    public class PortalDbContext : DbContext
    {
        public PortalDbContext(DbContextOptions<PortalDbContext> options)
            : base(options)
        {
        }

        public DbSet<PortalLink> PortalLinks { get; set; }

        public DbSet<UserDateTimeFormat> UserDateTimeFormats { get; set; }

        public DbSet<UserLanguage> UserLanguages { get; set; }

        public DbSet<UserSetting> UserSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PortalLink>().HasKey(k => k.Id);
            modelBuilder.Entity<UserDateTimeFormat>().HasKey(k => k.Id);
            modelBuilder.Entity<UserLanguage>().HasKey(k => k.Id);
            modelBuilder.Entity<UserSetting>().HasKey(k => k.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}