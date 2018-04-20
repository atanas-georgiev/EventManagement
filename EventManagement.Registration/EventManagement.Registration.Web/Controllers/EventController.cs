namespace EventManagement.Registration.Web.Controllers
{
    using System;
    using System.Linq;

    using EventManagement.Registration.Services;
    using EventManagement.Registration.Web.ViewModels.Event;

    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("api/[Controller]")]
    public class EventController : Controller
    {
        private readonly IEventService eventService;

        public EventController(IEventService eventService)
        {
            this.eventService = eventService;
        }

        // GET: api/Event
        [HttpGet]
        public IQueryable<ListEventApiModel> Get()
        {
            var res = this.eventService.GetAllEvents().Select(
                e => new ListEventApiModel
                         {
                             Start = e.Start,
                             EventName = e.EventName,
                             ResourcePlacesCount = e.ResourcePlacesCount,
                             Price = e.Price,
                             EventId = e.EventId,
                             Location = e.Location,
                             End = e.End,
                             LecturerName = e.LecturerName,
                             ResourceName = e.ResourceName,
                             AdditionalInfo = e.AdditionalInfo
                         });

            res = res.Where(e => e.Start >= DateTime.Now).OrderBy(e => e.Start);

            return res;
        }

        // GET: api/Event/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = this.eventService.GetAllEvents().FirstOrDefault(e => e.EventId == id);

            if (result == null)
            {
                return new NotFoundResult();
            }

            return new JsonResult(result);
        }
    }
}