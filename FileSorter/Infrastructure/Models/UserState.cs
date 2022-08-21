namespace ImageFileSorter.Infrastructure.Models
{
    internal class UserState
    {
        public UserState(string message, bool isSucess = true)
        {
            Message = message;
            IsSucess = isSucess;
            CreatedOn = DateTime.Now;
        }

        public string Message { get; set; }
        public bool IsSucess { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
