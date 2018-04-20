namespace EventManagement.Payment.Services.Sagas
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Payment.Data;
    using EventManagement.Payment.Data.Models;
    using EventManagement.Payment.Shared.Events;
    using EventManagement.Registration.Shared.Events;

    using Microsoft.EntityFrameworkCore;

    using NServiceBus;
    using NServiceBus.Persistence.Sql;

    public class PaymentSaga : SqlSaga<PaymentSagaData>,
                               IAmStartedByMessages<RequestPayment>,
                               IHandleMessages<CancelPayment>,
                               IHandleMessages<CompletePayment>,
                               IHandleTimeouts<PaymentSagaTimeout>
    {
        public PaymentDbContext DbContext { get; set; }

        protected override string CorrelationPropertyName => nameof(PaymentSagaData.PaymentId);

        public async Task Handle(RequestPayment message, IMessageHandlerContext context)
        {
            var payment = new Payment
                              {
                                  EventId = message.EventId,
                                  EventName = message.EventName,
                                  PaymentId = message.PaymentId,
                                  Price = message.Price,
                                  UserName = message.UserName,
                                  PaymentStatus = PaymentStatus.New
                              };

            await this.DbContext.Payments.AddAsync(payment);
            await this.DbContext.SaveChangesAsync();
            
            this.Data.PaymentId = payment.PaymentId;
            this.Data.Completed = false;
            await this.RequestTimeout<PaymentSagaTimeout>(context, TimeSpan.FromDays(1));
        }

        public async Task Handle(CancelPayment message, IMessageHandlerContext context)
        {
            var payment = await this.DbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == message.PaymentId);

            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Canceled;
                await this.DbContext.SaveChangesAsync();
                this.Data.Completed = false;
            }
        }

        public async Task Handle(CompletePayment message, IMessageHandlerContext context)
        {
            var payment = await this.DbContext.Payments.FirstOrDefaultAsync(p => p.PaymentId == message.PaymentId);

            if (payment != null)
            {
                payment.PaymentStatus = PaymentStatus.Completed;
                await this.DbContext.SaveChangesAsync();
                this.Data.Completed = true;
            }
        }

        public Task Timeout(PaymentSagaTimeout state, IMessageHandlerContext context)
        {
            var payment = this.DbContext.Payments.FirstOrDefault(p => p.PaymentId == this.Data.PaymentId);

            if (payment != null)
            {
                payment.PaymentStatus = this.Data.Completed ? PaymentStatus.Completed : PaymentStatus.Canceled;
                this.DbContext.SaveChanges();
            }

            context.Publish(new CancelPayment { PaymentId = this.Data.PaymentId });

            return Task.CompletedTask;
        }

        protected override void ConfigureHowToFindSaga(IConfigureHowToFindSagaWithMessage mapper)
        {
            mapper.ConfigureMapping<PaymentSagaData, RequestPayment>(message => message.PaymentId, payment => payment.PaymentId);
            mapper.ConfigureMapping<PaymentSagaData, CompletePayment>(message => message.PaymentId, payment => payment.PaymentId);
            mapper.ConfigureMapping<PaymentSagaData, CancelPayment>(message => message.PaymentId, payment => payment.PaymentId);
        }

        protected override void ConfigureMapping(IMessagePropertyMapper mapper)
        {
        }
    }
}