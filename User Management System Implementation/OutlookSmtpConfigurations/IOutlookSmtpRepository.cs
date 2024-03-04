namespace User_Management_System_Implementation.OutlookSmtpConfigurations
{
    public interface IOutlookSmtpRepository
    {
        Task<string> SendEmail(string Email);
    }
}
