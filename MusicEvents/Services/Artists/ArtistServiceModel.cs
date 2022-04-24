namespace MusicEvents.Services.Artists
{
    public class ArtistServiceModel
    {
        public int Id { get; set; }
        public string ArtistName { get; set; }
        public string? Biography { get; set; }
        public int NumberOfEvents { get; set; }
        public string GenreName { get; set; }
        public string ImageUrl { get; set; }
        public string CountryName{ get; set; }

    }
}
