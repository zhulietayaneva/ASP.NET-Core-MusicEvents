using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Genre
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(GenreNameMaxLenght)]
        public string GenreName { get; set; }

        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
        public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}
