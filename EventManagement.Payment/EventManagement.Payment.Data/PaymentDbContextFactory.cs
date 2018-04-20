namespace EventManagement.Payment.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class PaymentDbContextFactory : IDesignTimeDbContextFactory<PaymentDbContext>
    {
        public PaymentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventManagement.Payment;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PaymentDbContext(optionsBuilder.Options);
        }
    }
}
