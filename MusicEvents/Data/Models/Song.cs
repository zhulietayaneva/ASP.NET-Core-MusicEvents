using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Song
    {

        public int Id { get; set; }
        [Required]
        [MaxLength(SongNameMaxLenght)]
        public string SongName { get; set; }

        public virtual Genre Genre { get; set; }
        public int GenreId { get; set; }
        [Required]
        [Url]
        public string SongURL { get; set; }

        public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
    }
}
