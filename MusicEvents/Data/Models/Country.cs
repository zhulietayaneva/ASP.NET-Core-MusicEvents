using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        public string CountryName { get; set; }

        public ICollection<Event> Events { get; set; } = new List<Event>();

        public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}