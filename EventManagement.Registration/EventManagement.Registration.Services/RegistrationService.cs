namespace EventManagement.Registration.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;
    using EventManagement.Registration.Shared.Events;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;

    public class RegistrationService : IRegistrationService
    {
        private readonly RegistrationDbContext context;

        private readonly IMessageSession messageSession;

        public RegistrationService(RegistrationDbContext context, IMessageSession messageSession)
        {
            this.context = context;
            this.messageSession = messageSession;
        }

        public async Task<bool> RegisterForEvent(int eventId, string userName)
        {
            var ev = await this.context.Events.FirstOrDefaultAsync(e => e.EventId == eventId);

            if (ev == null)
            {
                return false;
            }

            if (ev.ResourcePlacesCount <= 0)
            {
                return false;
            }

            Registration reg;

            if (ev.Price <= 0)
            {
                // Free event
                reg = new Registration
                            {
                                PaymentId = Guid.NewGuid(),
                                UserName = userName,
                                PaymentStatus = PaymentStatus.Completed,
                                Event = ev
                            };
            }
            else
            {
                // Paid event
                reg = new Registration
                            {
                                PaymentId = Guid.NewGuid(),
                                UserName = userName,
                                PaymentStatus = PaymentStatus.New,
                                Event = ev
                            };

                await this.messageSession.Publish(new RequestPayment
                                                      {
                                                          PaymentId = reg.PaymentId,
                                                          EventId = ev.Id,
                                                          EventName = ev.EventName,
                                                          Price = ev.Price,
                                                          UserName = reg.UserName
                                                      });
            }

            await this.context.Registrations.AddAsync(reg);
            await this.context.SaveChangesAsync();

            return true;
        }

        public IQueryable<Registration> GetRegistrationsForEvent(int eventId)
        {
            return this.context.Registrations.Include(i => i.Event).Where(r => r.Event.EventId == eventId && r.PaymentStatus == PaymentStatus.Completed);
        }
    }
}
