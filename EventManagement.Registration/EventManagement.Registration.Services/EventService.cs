namespace EventManagement.Registration.Services
{
    using System.Linq;

    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;

    public class EventService : IEventService
    {
        private readonly RegistrationDbContext context;

        public EventService(RegistrationDbContext context)
        {
            this.context = context;
        }

        public IQueryable<Event> GetAllEvents()
        {
            var events = this.context.Events.Include(e => e.Registrations).Select(
                e => new Event()
                         {
                             Start = e.Start,
                             End = e.End,
                             AdditionalInfo = e.AdditionalInfo,
                             EventId = e.EventId,
                             EventName = e.EventName,
                             Id = e.Id,
                             LecturerName = e.LecturerName,
                             Location = e.Location,
                             Price = e.Price,
                             Registrations = e.Registrations,
                             ResourceName = e.ResourceName,
                             ResourcePlacesCount = e.ResourcePlacesCount - e.Registrations.Count(
                                                       r => r.PaymentStatus == PaymentStatus.New
                                                            || r.PaymentStatus == PaymentStatus.Completed)
                         });

            return events;
        }
    }
}