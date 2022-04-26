using MusicEvents.Data.Models;
using MusicEvents.Services.Artists;
using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Models.Artists
{
    public class AllArtistsQueryModel
    {
        public int CurrentPage { get; init; } =1;

        public const int ArtistsPerPage  = 3;
        public int TotalArtists { get; set; }

        [Display(Name = "Search...")]
        public string SearchTerm { get; set; }

        public int GenreId { get; set; }
        public ICollection<Genre> Genres { get; set; } = new List<Genre>();
        public ICollection<Country> Countries { get; set; } = new List<Country>();
        public int CountryId { get; set; }

        public ArtistSorting SortingType { get; set; }

        public IEnumerable<ArtistServiceModel> Artists { get; set; }
    }
}
