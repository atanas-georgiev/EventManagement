namespace EventManagement.Resources.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data.Models;
    using EventManagement.Resources.Services;

    using EventManagement.Resources.Web.ViewModels.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Http;

    [Produces("application/json")]
    [Route("api/Resource")]
    public class ResourceController : Controller
    {
        private readonly IResourceService resourceService;

        private readonly IEventService eventService;

        public ResourceController(IResourceService resourceService, IEventService eventService)
        {
            this.resourceService = resourceService;
            this.eventService = eventService;
        }

        // GET: api/Resource
        [HttpGet]
        public IQueryable<ListResourceApiModel> Get()
        {
            var res = this.resourceService.GetAllResources().Select(r => new ListResourceApiModel()
                                                                             {
                                                                                 Id = r.Id,
                                                                                 Name = r.Name,
                                                                                 Location = r.Location,
                                                                                 PlacesCount = r.PlacesCount,
                                                                                 Rent = r.Rent
                                                                             });
            return res;
        }

        // GET: api/Resource/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = this.resourceService.GetAllResources().FirstOrDefault(r => r.Id == id);

            if (res == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(new DetailsResourceApiModel()
                       {
                           Id = res.Id,
                           Name = res.Name,
                           Location = res.Location,
                           PlacesCount = res.PlacesCount,
                           Rent = res.Rent
                       });
        }
        
        // POST: api/Resource
        [HttpPost]
        public async Task<Resource> Post([FromBody]AddResourceApiModel model)
        {
            // Validate input arguments
            if (model == null)
            {
                this.ModelState.AddModelError("model", "Missing update model in request body.");
            }

            if (!this.ModelState.IsValid)
            {
                throw new InvalidOperationException();
            }

            var postData = new Resource
                               {
                                   Name = model.Name,
                                   Location = model.Location,
                                   PlacesCount = model.PlacesCount,
                                   Rent = model.Rent
                               };

            await this.resourceService.AddResourceAsync(postData);
            return postData;
        }       
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var countEvents = this.eventService.GetAllEvents().Where(e => e.Resource.Id == id).Count();

            if (countEvents > 0)
            {
                return StatusCode(StatusCodes.Status422UnprocessableEntity, "There are events associated with the resource. Please, remove them first.");
            }

            await this.resourceService.DeleteResourceAsync(id);
            return Ok();
        }
    }
}
