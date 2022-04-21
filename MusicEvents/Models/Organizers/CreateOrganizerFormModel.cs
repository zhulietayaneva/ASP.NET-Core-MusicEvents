using System.ComponentModel.DataAnnotations;
using static MusicEvents.Data.DataConstants;


namespace MusicEvents.Models.Organizers
{
    public class CreateOrganizerFormModel
    {
        [Required]
        [MaxLength(OrganizerNameMaxLenght)]

        public string Name { get; set; }
        [Required]
        [MaxLength(PhoneNumebrMaxLength)]

        public string PhoneNumber { get; set; }

    }
}
