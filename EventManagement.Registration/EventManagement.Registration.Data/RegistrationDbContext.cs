namespace EventManagement.Registration.Data
{
    using EventManagement.Registration.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class RegistrationDbContext : DbContext
    {
        public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }

        public DbSet<Registration> Registrations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Registration>().HasKey(k => k.Id);
            modelBuilder.Entity<Event>().HasKey(k => k.Id);
            modelBuilder.Entity<Registration>().HasOne(p => p.Event).WithMany(b => b.Registrations);
            base.OnModelCreating(modelBuilder);
        }
    }
}
