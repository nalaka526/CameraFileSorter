namespace ImageFileSorter.Infrastructure.Models
{
    internal class UserState
    {
        public UserState(string message, bool isSucess = true, bool isWarning = false)
        {
            Message = message;
            IsSucess = isSucess;
            CreatedOn = DateTime.Now;
            IsWarning = isWarning;
        }

        public string Message { get; set; }
        public bool IsSucess { get; set; }
        public bool IsWarning { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
