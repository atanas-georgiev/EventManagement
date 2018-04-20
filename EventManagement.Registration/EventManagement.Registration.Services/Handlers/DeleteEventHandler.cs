namespace EventManagement.Registration.Services.Handlers
{
    using System.Threading.Tasks;

    using EventManagement.Registration.Data;
    using EventManagement.Resources.Shared.Events;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;

    public class DeleteEventHandler : IHandleMessages<DeleteEvent>
    {
        public RegistrationDbContext DbContext { get; set; }

        public async Task Handle(DeleteEvent message, IMessageHandlerContext context)
        {
            var ev = await this.DbContext.Events.FirstOrDefaultAsync(e => e.EventId == message.EventId);

            if (ev != null)
            {
                this.DbContext.Remove(ev);
                await this.DbContext.SaveChangesAsync();
            }
        }
    }
}
