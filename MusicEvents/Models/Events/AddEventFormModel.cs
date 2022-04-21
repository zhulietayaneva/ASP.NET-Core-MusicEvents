using MusicEvents.Data.Models;
using MusicEvents.Models.Artists;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Events
{
    public class AddEventFormModel
    {
        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(EventNameMaxLength)]
        [MinLength(EventNameMinLength)]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [MaxLength(VenueNameMaxLength)]
        [MinLength(VenueNameMinLength)]
        public string Venue { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url]
        [Display(Name ="Image URL")]
        public string ImgURL { get; set; }

        [MaxLength(EventDescriptionMaxLength)]
        public string? Description { get; set; }

        public int CountryId { get; set; }
        public int CityId { get; set; }

        [Required(ErrorMessage = "Event date and time are required")]
        [DateLessThanOrEqualToToday]
        public DateTime Time { get; set; }
        public ICollection<Country> Countries { get; set; } = new List<Country>();     
        public ICollection<City> Cities { get; set; } = new List<City>();

        public ICollection<string> Artists { get; set; } = new List<string>();

    }
}
