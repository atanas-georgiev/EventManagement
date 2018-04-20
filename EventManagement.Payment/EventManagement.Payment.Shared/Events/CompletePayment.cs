namespace EventManagement.Payment.Shared.Events
{
    using System;

    public class CompletePayment
    {
        public Guid PaymentId { get; set; }
    }
}
