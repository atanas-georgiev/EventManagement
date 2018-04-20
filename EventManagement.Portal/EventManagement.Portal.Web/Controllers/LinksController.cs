namespace EventManagement.Portal.Web.Controllers
{
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using EventManagement.Portal.Web.ViewModels;

    using KPMG.TaxRay.Portal.Business.Links;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class LinksController : Controller
    {
        private readonly ILinksService linksService;

        public LinksController(ILinksService linksService)
        {
            this.linksService = linksService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]int languageId)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var links = await this.linksService.GetLinksAsync(languageId, isAdmin);
            return this.Json(links.Select(x => new LinksVM() { Title = x.Title, Link = x.Link }));
        }
    }
}