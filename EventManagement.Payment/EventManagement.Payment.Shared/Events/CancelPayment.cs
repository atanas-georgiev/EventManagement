namespace EventManagement.Payment.Shared.Events
{
    using System;

    public class CancelPayment
    {
        public Guid PaymentId { get; set; }
    }
}
