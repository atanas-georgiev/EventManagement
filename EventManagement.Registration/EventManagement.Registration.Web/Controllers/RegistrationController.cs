namespace EventManagement.Registration.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Registration.Data.Models;
    using EventManagement.Registration.Services;
    using EventManagement.Registration.Web.ViewModels.Registration;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    [Route("api/[Controller]")]
    public class RegistrationController : Controller
    {
        private readonly IRegistrationService registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            this.registrationService = registrationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddRegistrationApiModel model)
        {            
            // Validate input arguments
            if (model == null || !this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            var user = this.User?.Identity?.Name;
            var result = await this.registrationService.RegisterForEvent(model.EventId, user);

            if (result)
            {
                return this.Json(string.Empty);
            }
            
            return this.BadRequest();            
        }

        [HttpGet("{id}")]
        public IQueryable<DetailsRegistrationViewModel> Get(int id)
        {
            var res = this.registrationService.GetRegistrationsForEvent(id).Select(r => new DetailsRegistrationViewModel { UserName = r.UserName });
            return res;
        }
    }
}
