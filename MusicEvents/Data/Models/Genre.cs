using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        public string GenreName { get; set; }

        public ICollection<Song> Songs { get; set; }
        public ICollection<Artist> Artists { get; set; }
    }
}
