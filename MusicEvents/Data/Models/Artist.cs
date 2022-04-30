using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(ArtistNameMaxLenght)]
        public string ArtistName { get; set; }
        
        public virtual Genre Genre { get; set; }
        public int GenreId { get; set; }    
        
        public DateTime BirthDate { get; set; }

        [MaxLength(ArtistBioMaxLenght)]
        public string? Biography { get; set; }
        
        [Required]
        [Url]
        public string ImageURL { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

     
    }
}