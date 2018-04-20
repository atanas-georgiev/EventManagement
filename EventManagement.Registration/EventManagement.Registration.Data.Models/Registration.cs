namespace EventManagement.Registration.Data.Models
{
    using System;

    public class Registration
    {
        public int Id { get; set; }

        public virtual Event Event { get; set; }

        public string UserName { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public Guid PaymentId { get; set; }
    }
}
