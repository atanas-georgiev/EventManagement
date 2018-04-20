namespace EventManagement.Resources.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using EventManagement.Resources.Data.Models;
    using EventManagement.Resources.Services;
    using EventManagement.Resources.Web.ViewModels.Events;

    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/Event")]
    public class EventController : Controller
    {
        private readonly IEventService eventService;

        private readonly IResourceService resourceService;

        public EventController(IEventService eventService, IResourceService resourceService)
        {
            this.eventService = eventService;
            this.resourceService = resourceService;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await this.eventService.DeleteEventAsync(id);
        }

        // GET: api/Event
        [HttpGet]
        public IQueryable<ListEventApiModel> Get()
        {
            var res = this.eventService.GetAllEvents().Select(
                e => new ListEventApiModel()
                         {
                             Id = e.Id,
                             Name = e.Name,
                             AdditionalInfo = e.AdditionalInfo,
                             End = e.End,
                             Start = e.Start,
                             LecturerName = e.LecturerName,
                             Price = e.Price,
                             ResourceId = e.Resource.Id
                         });
            return res;
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var res = this.eventService.GetAllEvents().FirstOrDefault(r => r.Id == id);

            if (res == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(new DetailsEventApiModel()
                       {
                           Id = res.Id,
                           Name = res.Name,
                           AdditionalInfo = res.AdditionalInfo,
                           End = res.End,
                           Start = res.Start,
                           LecturerName = res.LecturerName,
                           Price = res.Price,
                           ResourceId = res.Resource.Id
                       });
        }

        // GET: api/Event/?resourceId=10
        [HttpGet("byResource/{resourceId}")]
        public IQueryable<ListEventApiModel> ByResourceGet(int resourceId)
        {
            var res = this.eventService.GetAllEvents().Where(r => r.Resource.Id == resourceId).Select(
                e => new ListEventApiModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    AdditionalInfo = e.AdditionalInfo,
                    End = e.End,
                    Start = e.Start,
                    LecturerName = e.LecturerName,
                    Price = e.Price,
                    ResourceId = e.Resource.Id
                });
            return res;
        }

        // POST: api/Event
        [HttpPost]
        public async Task<Event> Post([FromBody] AddEventApiModel model)
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

            var resource = this.resourceService.GetAllResources().FirstOrDefault(r => r.Id == model.ResourceId);

            var postData = new Event()
                               {
                                   Name = model.Name,
                                   AdditionalInfo = model.AdditionalInfo,
                                   End = model.End,
                                   Start = model.Start,
                                   LecturerName = model.LecturerName,
                                   Price = model.Price,
                                   Resource = resource
                               };

            await this.eventService.AddEventAsync(postData);
            return postData;
        }
    }
}