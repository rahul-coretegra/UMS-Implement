namespace User_Management_System_Implementation.PostMarkConfigurations
{
    public interface IPostMarkRepository
    {
        Task<string> SendEmail(string Email);
    }
}
