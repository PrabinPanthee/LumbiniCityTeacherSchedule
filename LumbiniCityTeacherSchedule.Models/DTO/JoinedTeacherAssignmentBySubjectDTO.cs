namespace LumbiniCityTeacherSchedule.Models.DTO
{
    public class JoinedTeacherAssignmentBySubjectDTO
    {
        public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public string? SubjectCode { get; set; }
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public int? NumberOfClasses {  get; set; }
    }
}
