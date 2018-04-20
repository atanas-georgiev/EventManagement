namespace EventManagement.Registration.Services
{
    using System.Linq;

    using EventManagement.Registration.Data.Models;

    public interface IEventService
    {
        IQueryable<Event> GetAllEvents();
    }
}
