using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Models.Artists
{
    public class AddArtistFormModel
    {


        [Required]
        [MaxLength(ArtistNameMaxLenght)]
        [Display(Name = "Enter artist name")]
        public string ArtistName { get; set; }
        [BirthDay]
        public DateTime BirthDate { get; set; }
        [Display(Name ="Choose genre")]
        public int GenreId { get; set; }
        public IEnumerable<Genre> Genres { get; set; } =  new List<Genre>();

        [MaxLength(ArtistBioMaxLenght)]
        public string? Biography { get; set; }

        [Required]
        [Url]
        [Display(Name = "Enter Image link")]
        public string ImageURL { get; set; }
        [Display(Name = "Choose country")]
        public int CountryId { get; set; }
        public IEnumerable<Country> Countries { get; set; } = new List<Country>();

       


    }
}
