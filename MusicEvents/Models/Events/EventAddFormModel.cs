using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Events
{
    public class EventAddFormModel
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
        public ICollection<string> Countries { get; set; } = new List<string>();     
        public ICollection<string> Cities { get; set; } = new List<string>();

        public ICollection<Artist> Artists { get; set; } = new List<Artist>();

    }
}
