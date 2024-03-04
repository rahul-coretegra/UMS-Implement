namespace User_Management_System_Implementation.NexmoConfigurations
{
    public interface INexmoRepository
    {
        Task<string> SendSms(string PhoneNumber);

    }
}
