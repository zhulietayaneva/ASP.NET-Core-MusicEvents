using MusicEvents.Data.Models;
using MusicEvents.Services.Events;
using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Models.Events
{
    public class AllEventsQueryModel
    {

        public int TotalEvents { get; set; }

        public const int EventsPerPage = 3;
        public int CurrentPage { get; init; } = 1;

        [Display(Name = "Search...")]
        public string SearchTerm { get; set; }


        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public ICollection<City> Cities { get; set; } = new List<City>();


        public EventSorting SortingType { get; set; }

        public IEnumerable<EventServiceModel> Events { get; set; }
    }
}
