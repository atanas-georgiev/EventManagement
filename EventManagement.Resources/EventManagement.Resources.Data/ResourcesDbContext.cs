namespace EventManagement.Resources.Data
{
    using EventManagement.Resources.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class ResourcesDbContext : DbContext
    {
        public ResourcesDbContext(DbContextOptions<ResourcesDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Resource> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>().HasKey(k => k.Id);
            modelBuilder.Entity<Resource>().HasKey(k => k.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
