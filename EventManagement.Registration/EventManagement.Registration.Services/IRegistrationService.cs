namespace EventManagement.Registration.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Registration.Data.Models;

    public interface IRegistrationService
    {
        Task<bool> RegisterForEvent(int eventId, string userName);

        IQueryable<Registration> GetRegistrationsForEvent(int eventId);
    }
}
