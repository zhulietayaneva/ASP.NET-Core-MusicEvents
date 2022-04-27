using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Songs
{
    public class AddSongFormModel
    {
        [Required]
        [MaxLength(SongNameMaxLenght)]
        [Display(Name = "Enter name")]
        public string SongName { get; set; }

        public int ArtistId { get; set; }
        public ICollection<Artist> Artists { get; set; } = new List<Artist>();

        [Display(Name = "Choose genre")]
        public int GenreId { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();

        [Required]
        [Url]
        [Display(Name = "Enter YouTube link")]
        public string SongURL { get; set; }


    }
}
