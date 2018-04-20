namespace EventManagement.Resources.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data.Models;

    public interface IResourceService
    {
        Task AddResourceAsync(Resource resource);

        Task DeleteResourceAsync(int id);

        IQueryable<Resource> GetAllResources();
    }
}