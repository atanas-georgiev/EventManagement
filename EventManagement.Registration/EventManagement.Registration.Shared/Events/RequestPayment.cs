namespace EventManagement.Registration.Shared.Events
{
    using System;

    public class RequestPayment
    {
        public Guid PaymentId { get; set; }

        public string UserName { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public double Price { get; set; }
    }
}
