using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Portal.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class PortalDbContextFactory : IDesignTimeDbContextFactory<PortalDbContext>
    {
        public PortalDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PortalDbContext>();
            optionsBuilder.UseSqlServer("Server=.;Database=EventManagement.Portal;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new PortalDbContext(optionsBuilder.Options);
        }
    }
}
