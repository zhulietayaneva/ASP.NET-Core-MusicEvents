using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;

namespace MusicEvents.Data.Models
{
    public class Organizer
    {

        public int Id { get; init; }
        [Required]
        [MaxLength(OrganizerNameMaxLenght)]
        public string Name { get; set; }
        [Required]
        [MaxLength(PhoneNumebrMaxLength)]
        public string PhoneNumber { get; set; }
        [Required]
        public string UserId { get; set; }

        public virtual ICollection<Event> Events { get; set; }


    }
}
