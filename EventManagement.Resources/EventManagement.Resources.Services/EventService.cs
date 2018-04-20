namespace EventManagement.Resources.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data;
    using EventManagement.Resources.Data.Models;
    using EventManagement.Resources.Shared.Events;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;

    public class EventService : IEventService
    {
        private readonly ResourcesDbContext context;

        private readonly IMessageSession messageSession;

        public EventService(ResourcesDbContext context, IMessageSession messageSession)
        {
            this.context = context;
            this.messageSession = messageSession;
        }

        public async Task AddEventAsync(Event ev)
        {
            if (ev == null)
            {
                throw new NullReferenceException(nameof(ev));
            }

            await this.context.Events.AddAsync(ev);
            await this.context.SaveChangesAsync();

            await this.messageSession.Publish(new AddEvent()
            {
                EventId = ev.Id,
                Start = ev.Start,
                End = ev.End,
                EventName = ev.Name,
                ResourceName = ev.Resource.Name,
                LecturerName = ev.LecturerName,
                Location = ev.Resource.Location,
                AdditionalInfo = ev.AdditionalInfo,
                Price = ev.Resource.Rent / ev.Resource.PlacesCount,
                ResourcePlacesCount = ev.Resource.PlacesCount
            });
        }

        public async Task DeleteEventAsync(int id)
        {
            var ev = this.context.Events.FirstOrDefault(r => r.Id == id);

            if (ev == null)
            {
                throw new InvalidOperationException($"Event with id {id} not found!");
            }

            await this.messageSession.Publish(new DeleteEvent { EventId = id });
            this.context.Events.Remove(ev);
            await this.context.SaveChangesAsync();
        }

        public IQueryable<Event> GetAllEvents()
        {
            return this.context.Events.Include(path => path.Resource);
        }
    }
}