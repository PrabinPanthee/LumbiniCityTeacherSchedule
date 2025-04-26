
namespace LumbiniCityTeacherSchedule.Models.Results
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public List<string>? Errors { get; set; }

        // Factory methods for easy creation
        public static ServiceResult Ok(string message) => new ServiceResult { Success = true, Message = message };
        public static ServiceResult Fail(string message, List<string> errors = null)
            => new ServiceResult { Success = false, Message = message, Errors = errors ?? new List<string>() };
    }

    public class ServiceResult<T> : ServiceResult
    {
        public T? Data { get; set; }
        public static ServiceResult<T> Ok(T data, string message = "") =>
            new ServiceResult<T> { Success = true, Message = message, Data = data };
        public static ServiceResult<T> Fail(string message, List<string>? errors = null) =>
            new ServiceResult<T> { Success = false, Message = message, Errors = errors ?? new List<string>(), Data = default };
        
    }

}
