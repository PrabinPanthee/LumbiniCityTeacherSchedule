using System.ComponentModel.DataAnnotations;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class UpdateSemesterScheduleConfigDto:IValidatableObject
    {
        [Required(ErrorMessage ="Required")]
        public int ProgramId { get; set; }
        public int ConfigId {  get; set; }//hidden value for update 
        [Required(ErrorMessage ="SemesterId is required")]
        public int SemesterId { get; set; }
        [Required(ErrorMessage = "TotalClasses is Required")]
        [Range(1, 6, ErrorMessage = "Must be Between 1 and 6")]
        public int TotalClasses { get; set; }

        public int? BreakAfterPeriod { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (BreakAfterPeriod.HasValue)
            {
                if (BreakAfterPeriod < 1 || BreakAfterPeriod >= TotalClasses)
                {
                    yield return new ValidationResult("BreakAfterPeriod must be greater than 1 and less than TotalClasses.",
                        [nameof(BreakAfterPeriod)]
                    );
                }
            }
        }
    }
}
