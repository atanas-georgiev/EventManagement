namespace EventManagement.Resources.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data;
    using EventManagement.Resources.Data.Models;

    using NServiceBus;

    public class ResourceService : IResourceService
    {
        private readonly ResourcesDbContext context;

        public ResourceService(ResourcesDbContext context)
        {
            this.context = context;
        }

        public async Task AddResourceAsync(Resource resource)
        {
            if (resource == null)
            {
                throw new NullReferenceException(nameof(resource));
            }

            await this.context.Resources.AddAsync(resource);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteResourceAsync(int id)
        {
            var resource = this.context.Resources.FirstOrDefault(r => r.Id == id);

            if (resource == null)
            {
                throw new InvalidOperationException($"Resource with id {id} not found!");
            }

            this.context.Resources.Remove(resource);
            await this.context.SaveChangesAsync();
        }

        public IQueryable<Resource> GetAllResources()
        {
            return this.context.Resources;
        }
    }
}