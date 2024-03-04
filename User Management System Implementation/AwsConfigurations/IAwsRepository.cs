namespace User_Management_System_Implementation.AwsSnsConfigurations
{
    public interface IAwsRepository
    {
        Task<string> SendNotification(string PhoneNumber);

        Task<string> SendEmail(string Email);
    }
}
