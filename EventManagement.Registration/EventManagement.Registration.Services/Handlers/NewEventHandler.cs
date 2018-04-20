namespace EventManagement.Registration.Services.Handlers
{
    using System.Threading.Tasks;

    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;
    using EventManagement.Resources.Shared.Events;

    using NServiceBus;

    public class NewEventHandler : IHandleMessages<AddEvent>
    {
        public RegistrationDbContext DbContext { get; set; }

        public async Task Handle(AddEvent message, IMessageHandlerContext context)
        {
            var ev = new Event
                         {
                             EventId = message.EventId,
                             Start = message.Start,
                             End = message.End,
                             AdditionalInfo = message.AdditionalInfo,
                             EventName = message.EventName,
                             LecturerName = message.LecturerName,
                             Location = message.Location,
                             Price = message.Price,
                             ResourceName = message.ResourceName,
                             ResourcePlacesCount = message.ResourcePlacesCount
                         };

            await this.DbContext.AddAsync(ev);
            await this.DbContext.SaveChangesAsync();
        }
    }
}
