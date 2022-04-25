namespace MusicEvents.Services.Artists
{
    public class ArtistsQueryServiceModel
    {
        public int CurrentPage { get; set; }
        public int TotalArtists{ get; set; }
        public int ArtistsPerPage { get; set; }

 

        public ICollection<ArtistServiceModel> Artists { get; set; }
    }
}
