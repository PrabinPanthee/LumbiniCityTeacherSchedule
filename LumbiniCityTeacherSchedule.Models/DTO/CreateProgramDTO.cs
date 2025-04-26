using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class CreateProgramDTO
    {
        [Required(ErrorMessage = "Program Name is required")]
        [StringLength(10,MinimumLength = 3,ErrorMessage = "Program Name must be Between 3 and 10")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Program Name must only contain letters and no spaces.")]
        public string? ProgramName { get; set; }
    }
}
