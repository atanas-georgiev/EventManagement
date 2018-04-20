namespace EventManagement.Resources.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data.Models;

    public interface IEventService
    {
        Task AddEventAsync(Event ev);

        Task DeleteEventAsync(int id);

        IQueryable<Event> GetAllEvents();
    }
}