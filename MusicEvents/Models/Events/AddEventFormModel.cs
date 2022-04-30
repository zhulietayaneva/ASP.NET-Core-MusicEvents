using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Events
{
    public class AddEventFormModel
    {
        [Required(ErrorMessage = "Event name is required")]
        [MaxLength(EventNameMaxLength)]
        [MinLength(EventNameMinLength)]
        [Display(Name = "Enter name")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Venue name is required")]
        [MaxLength(VenueNameMaxLength)]
        [MinLength(VenueNameMinLength)]
        [Display(Name = "Enter venue")]
        public string Venue { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [Url]
        [Display(Name ="Image link")]
        public string ImgURL { get; set; }

        [MaxLength(EventDescriptionMaxLength)]
        [Display(Name="Description")]
        public string? Description { get; set; }
        [Display(Name = "Choose country")]
        public int CountryId { get; set; }
        [Display(Name = "Choose city")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "Event date and time are required")]
        [DateLessOrEqualToToday]
        public DateTime Time { get; set; }
        public  IEnumerable<Country> Countries { get; set; } = new List<Country>();     
        public  IEnumerable<City> Cities { get; set; } = new List<City>();
        public  IEnumerable<Artist> Artists { get; set; } = new List<Artist>();
        public int ArtistId { get; set; }
    }
}
