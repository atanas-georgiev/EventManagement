namespace EventManagement.Registration.Services.Handlers
{
    using System.Threading.Tasks;

    using EventManagement.Payment.Shared.Events;
    using EventManagement.Registration.Data;
    using EventManagement.Registration.Data.Models;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;

    public class CompletePaymentHandler : IHandleMessages<CompletePayment>
    {
        public RegistrationDbContext DbContext { get; set; }

        public async Task Handle(CompletePayment message, IMessageHandlerContext context)
        {
            var registration = await this.DbContext.Registrations.FirstOrDefaultAsync(r => r.PaymentId == message.PaymentId);

            if (registration != null)
            {
                registration.PaymentStatus = PaymentStatus.Completed;
                await this.DbContext.SaveChangesAsync();
            }
        }
    }
}
