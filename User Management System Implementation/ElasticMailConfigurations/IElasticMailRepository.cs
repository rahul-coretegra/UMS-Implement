namespace User_Management_System_Implementation.ElasticMailConfigurations
{
    public interface IElasticMailRepository
    {
        Task<string> SendEmail(string Email);

        Task<string> SendSms(string Phonenumber);

    }
}
