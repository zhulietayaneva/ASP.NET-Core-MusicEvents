using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(CountryNameMaxLenght)]
        public string CountryName { get; set; }

        public ICollection<City> Cities { get; set; } = new List<City>();

        public ICollection<Event> Events { get; set; } = new List<Event>();

        public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}