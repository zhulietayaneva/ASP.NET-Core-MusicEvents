using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Models.Artists
{
    public class AddArtistsInEventsFormModel
    {
        [Required]
        public string ArtistName { get; set; }

        

    }
}
