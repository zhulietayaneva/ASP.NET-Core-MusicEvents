using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Data.Models
{
    public class Organizer
    {

        public int Id { get; init; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [Required]
        public string UserId { get; set; }

        public ICollection<Event> Events { get; set; }


    }
}
