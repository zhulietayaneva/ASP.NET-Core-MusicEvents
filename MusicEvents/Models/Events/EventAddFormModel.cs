using MusicEvents.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Models.Events
{
    public class EventAddFormModel
    {
        [Required]
        [MaxLength(60)]
        public string EventName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Venue { get; set; }

        [Required]
        [Url]
        public string ImgURL { get; set; }
        public string Description { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public DateTime Time { get; set; }

    }
}
