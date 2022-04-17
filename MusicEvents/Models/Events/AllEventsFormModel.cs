namespace MusicEvents.Models.Events
{
    public class AllEventsFormModel
    {
        public int Id { get; set; }
        public string EventName { get; set; }
       
        public string Venue { get; set; }

        public string Artists { get; set; }

        public string ImgURL { get; set; }

        public string Description { get; set; }

        public string CountryName { get; set; }

        public string CityName { get; set; }

        public DateTime Time { get; set; }

    }
}
