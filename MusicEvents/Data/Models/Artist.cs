using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        public string ArtistName { get; set; }
        [Required]
        public Genre Genre { get; set; }

        public int GenreId { get; set; }
        public int Age { get; set; }
        public string Biography { get; set; }
        [Url]
        [Required]
        public string ImageURL { get; set; }

        public Country Country { get; set; }
        public int CountryId { get; set; }

        public ICollection<Event> Events { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}