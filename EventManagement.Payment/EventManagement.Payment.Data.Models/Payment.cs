namespace EventManagement.Payment.Data.Models
{
    using System;

    public class Payment
    {
        public int Id { get; set; }

        public Guid PaymentId { get; set; }

        public string UserName { get; set; }

        public int EventId { get; set; }

        public string EventName { get; set; }

        public double Price { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
    }
}
