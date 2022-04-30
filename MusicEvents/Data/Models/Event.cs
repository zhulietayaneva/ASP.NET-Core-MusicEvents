using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Event
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(EventNameMaxLength)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(VenueNameMaxLength)]
        public string Venue { get; set; }
        [Required]
        public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
        [Required]
        [Url]
        public string ImgURL { get; set; }
        
        public string? Description { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public virtual City City { get; set; }
        public int CityId { get; set; }

        public DateTime Time { get; set; }

        public int OrganizerId { get; set; }
        public virtual Organizer Organizer { get; set; }

    }
}
