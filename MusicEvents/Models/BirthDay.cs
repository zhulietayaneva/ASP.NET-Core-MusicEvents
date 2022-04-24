using System.ComponentModel.DataAnnotations;

namespace MusicEvents.Models
{
    public class BirthDay : ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "You cannot choose a date in the future!";
        }

        protected override ValidationResult IsValid(object objValue,
                                                       ValidationContext validationContext)
        {
            var dateValue = objValue as DateTime? ?? new DateTime();


            if (dateValue.Date >
                DateTime.Now.Date)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
