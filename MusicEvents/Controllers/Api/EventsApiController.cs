using Microsoft.AspNetCore.Mvc;
using MusicEvents.Infrastructure;
using MusicEvents.Models.Api;
using MusicEvents.Services.Events;

namespace MusicEvents.Controllers.Api
{
    [ApiController]
    [Route("api/events")]
    public class EventsApiController:ControllerBase
    {
        private readonly IEventService events;


        public EventsApiController(IEventService events)
        {
           this.events = events;
        }

        [HttpGet]
        public EventsQueryServiceModel All([FromQuery] AllEventsRequestApiModel query)
        {
            return this.events.All(query.SearchTerm = null, query.CountryId, query.CityId, query.SortingType, query.CurrentPage, query.EventsPerPage,User.GetId());
        }
    }
}
