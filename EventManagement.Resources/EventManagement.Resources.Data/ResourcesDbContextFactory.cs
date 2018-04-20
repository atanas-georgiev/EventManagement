namespace EventManagement.Resources.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class ResourcesDbContextFactory: IDesignTimeDbContextFactory<ResourcesDbContext>
    {
        public ResourcesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ResourcesDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventManagement.Resources;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new ResourcesDbContext(optionsBuilder.Options);
        }
    }
}
