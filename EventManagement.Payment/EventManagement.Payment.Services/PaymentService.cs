namespace EventManagement.Payment.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Payment.Data;
    using EventManagement.Payment.Data.Models;
    using EventManagement.Payment.Shared.Events;
    
    using NServiceBus;

    public class PaymentService : IPaymentService
    {
        private readonly PaymentDbContext context;

        private readonly IMessageSession messageSession;

        public PaymentService(PaymentDbContext context, IMessageSession messageSession)
        {
            this.context = context;
            this.messageSession = messageSession;
        }

        public IQueryable<Payment> GetAllPayments()
        {
            return this.context.Payments.Where(p => p.PaymentStatus == PaymentStatus.New);
        }

        public async Task PayAsync(Guid paymentId)
        {
            await this.messageSession.Publish(new CompletePayment { PaymentId = paymentId });
        }

        public async Task CancelAsync(Guid paymentId)
        {
            await this.messageSession.Publish(new CancelPayment { PaymentId = paymentId });
        }
    }
}
