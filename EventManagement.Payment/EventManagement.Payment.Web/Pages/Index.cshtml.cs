namespace EventManagement.Payment.Web.Pages
{
    using System.Collections.Generic;
    using System.Linq;

    using EventManagement.Payment.Services;
    using EventManagement.Payment.Web.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IPaymentService paymentService;

        public ICollection<ListPaymentsApiModel> Payments;

        public IndexModel(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public void OnGet()
        {
            var res = this.paymentService.GetAllPayments().Select(
                p => new ListPaymentsApiModel
                         {
                             PaymentId = p.PaymentId,
                             EventName = p.EventName,
                             Price = p.Price,
                             UserName = p.UserName
                         }).ToList();

            this.Payments = res;
        }
    }
}