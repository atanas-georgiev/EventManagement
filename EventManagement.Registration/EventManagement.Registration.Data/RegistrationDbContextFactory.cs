namespace EventManagement.Registration.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class RegistrationDbContextFactory : IDesignTimeDbContextFactory<RegistrationDbContext>
    {
        public RegistrationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RegistrationDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventManagement.Registration;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new RegistrationDbContext(optionsBuilder.Options);
        }
    }
}
