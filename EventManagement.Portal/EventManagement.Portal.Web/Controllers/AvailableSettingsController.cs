namespace EventManagement.Portal.Web.Controllers
{
    using System.Threading.Tasks;

    using EventManagement.Portal.Business.AvailableSettings;
    using EventManagement.Portal.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class AvailableSettingsController : Controller
    {
        private readonly IAvailableSettingsService availableSettingsService;

        public AvailableSettingsController(IAvailableSettingsService availableSettingsService)
        {
            this.availableSettingsService = availableSettingsService;
        }

        [HttpGet]
        public async Task<JsonResult> Get()
        {
            var formats = await this.availableSettingsService.GetAvailableDateFormatsAsync();
            var languages = await this.availableSettingsService.GetAvailableLanguagesAsync();

            var model = new AvailableSettingsVM(formats, languages);

            return this.Json(model);
        }
    }
}
