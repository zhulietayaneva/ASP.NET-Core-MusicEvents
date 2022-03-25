using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Event
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(50)]
        public string Venue { get; set; }
        [Required]
        public ICollection<Artist> Artists { get; set; } = new List<Artist>();
        [Required]
        [Url]
        public string ImgURL { get; set; }
        
        public string Description { get; set; }

        public Country Country { get; set; }
        public int CountryId { get; set; }

        public DateTime Time { get; set; }

    }
}
