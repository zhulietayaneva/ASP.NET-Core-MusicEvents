using MusicEvents.Data.Models;

namespace MusicEvents.Models.Api
{
    public class AllEventsRequestApiModel
    {

        public int TotalEvents { get; set; } 
        public int CurrentPage { get; init; } = 1;

        public int EventsPerPage = 4;
        public string? SearchTerm{ get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public EventSorting SortingType { get; set; }
        public ICollection<EventsResponseModel>? Events { get; set; }


    }
}
