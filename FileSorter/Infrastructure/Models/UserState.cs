namespace ImageFileSorter.Infrastructure.Models
{
    internal class UserState
    {
        public UserState(string message, bool isSuccess = true, bool isWarning = false)
        {
            Message = message;
            IsSuccess = isSuccess;
            CreatedOn = DateTime.Now;
            IsWarning = isWarning;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public bool IsWarning { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
