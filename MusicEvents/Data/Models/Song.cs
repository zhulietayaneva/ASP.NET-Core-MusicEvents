using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Song
    {

        public int Id { get; set; }
        [Required]
        public string SongName { get; set; }
        public Genre Genre { get; set; }
        public int GenreId { get; set; }
        [Url]
        public string SongURL { get; set; }

        public ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}
