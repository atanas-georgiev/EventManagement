namespace EventManagement.Payment.Data
{
    using EventManagement.Payment.Data.Models;

    using Microsoft.EntityFrameworkCore;

    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().HasKey(k => k.Id);
            base.OnModelCreating(modelBuilder);
        }
    }
}
