using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Artists
{
    public class AddArtistFormModel
    {
        [Required]
        [MaxLength(ArtistNameMaxLenght)]
        public string ArtistName { get; set; }
        [DateLessThanOrEqualToToday]
        public DateTime BirthDate { get; set; }
        public int GenreId { get; set; }
        public ICollection<Genre> Genres { get; set; }

        [MaxLength(ArtistBioMaxLenght)]
        public string? Biography { get; set; }

        [Required]
        [Url]
        public string ImageURL { get; set; }
        public int CountryId { get; set; }
        public ICollection<Country> Countries { get; set; } = new List<Country>();


    }
}
