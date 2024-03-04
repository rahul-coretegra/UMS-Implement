namespace User_Management_System_Implementation.TwilioModule
{
    public interface ITwilioRepository
    {
        Task<string> SendSms(string PhoneNumber);

        Task<string> SendEmail(string Email);

    }
}
