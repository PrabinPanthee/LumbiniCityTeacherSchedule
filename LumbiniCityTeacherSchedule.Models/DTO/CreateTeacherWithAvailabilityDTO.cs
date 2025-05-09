using System.ComponentModel.DataAnnotations;
namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class CreateTeacherWithAvailabilityDTO:IValidatableObject
    {
        [Required(ErrorMessage ="FirstName is Required")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage ="Last name is required")]
        public string? LastName { get; set; }
        [Required(ErrorMessage ="Number of classes is required")]
        [Range(1,6,ErrorMessage ="Must be between 1 and 6")]
        public int NumberOfClasses { get; set; }
        [Required(ErrorMessage ="StartTime is required")]
        public TimeOnly StartTime { get; set; }
        [Required(ErrorMessage ="EndTime is required")]
        public TimeOnly EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime <= StartTime)
            {
                yield return new ValidationResult("End time must be later than start time", [nameof(EndTime)]);
            }
            var minStart = new TimeOnly(6, 30);
            var maxEnd = new TimeOnly(12, 0);

            if (StartTime < minStart)
            {
                yield return new ValidationResult("Start time cannot be earlier than 6:30 AM", [nameof(StartTime)]);
            }

            if (EndTime > maxEnd) 
            {
              yield return new ValidationResult($"End time cannot be later than 12:00 AM", [nameof(EndTime)]);
            }

        }
    }
}
