namespace EventManagement.Payment.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Payment.Services;
    using EventManagement.Payment.Web.ViewModels;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Admin")]
    [Produces("application/json")]
    [Route("api/Payment")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        
        // POST: api/Payment
        [HttpPost("Complete")]
        public async Task Complete([FromBody]PostPaymentApiModel model)
        {
            // Validate input arguments
            if (model == null)
            {
                throw new NullReferenceException();
            }

            if (!this.ModelState.IsValid)
            {
                throw new InvalidOperationException();
            }

            await this.paymentService.PayAsync(model.PaymentId);
        }

        [HttpPost("Cancel")]
        public async Task Cancel([FromBody]PostPaymentApiModel model)
        {
            // Validate input arguments
            if (model == null)
            {
                throw new NullReferenceException();
            }

            if (!this.ModelState.IsValid)
            {
                throw new InvalidOperationException();
            }

            await this.paymentService.CancelAsync(model.PaymentId);
        }
    }
}
