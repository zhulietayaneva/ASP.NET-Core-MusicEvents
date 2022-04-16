using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class City
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(CityNameMaxLenght)]
        public string CityName { get; set; }
        public Country Country { get; set; }
        public int CountryId { get; set; }
        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}