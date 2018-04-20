namespace EventManagement.Payment.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Payment.Data.Models;

    public interface IPaymentService
    {
        IQueryable<Payment> GetAllPayments();

        Task PayAsync(Guid paymentId);

        Task CancelAsync(Guid paymentId);
    }
}
