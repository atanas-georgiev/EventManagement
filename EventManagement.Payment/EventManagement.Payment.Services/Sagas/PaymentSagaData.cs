namespace EventManagement.Payment.Services.Sagas
{
    using System;

    using NServiceBus;

    public class PaymentSagaData : ContainSagaData
    {
        public Guid PaymentId { get; set; }

        public bool Completed { get; set; }
    }
}
