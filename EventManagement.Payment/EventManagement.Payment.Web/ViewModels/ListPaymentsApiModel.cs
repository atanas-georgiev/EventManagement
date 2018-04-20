namespace EventManagement.Payment.Web.ViewModels
{
    using System;

    public class ListPaymentsApiModel
    {
        public Guid? PaymentId { get; set; }

        public string UserName { get; set; }

        public string EventName { get; set; }

        public double Price { get; set; }
    }
}
