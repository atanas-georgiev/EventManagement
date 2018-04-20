namespace EventManagement.UserManagement.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class ApplicationContextFactory : IDesignTimeDbContextFactory<UserManagementDbContext>
    {
        public UserManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserManagementDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventManagement.UserManagement;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new UserManagementDbContext(optionsBuilder.Options);
        }
    }
}
