using User_Management_System_Implementation.Models;

namespace User_Management_System_Implementation.Repository.IRepository
{
    public interface IDefaultConfigurationsRepository
    {
        void ConfigureService(IServiceCollection services);

        List<Service> GetConfiguredServices();

        List<ConfigureService> GetConfiguredItems();

    }
}























//services.AddDbContext > (options =>
//               options.UseNpgsql(_configuration.GetSection("PostgreSqlConfigurations")["PostgreSqlConnectionString"]));



//"Twilio": {
//    "AccountSID": "ACe65a8bc4fb2326dd39a5222bc11c1412",
//    "AuthToken": "d4947344e7ee7ba3c57bc5e550038ccd",
//    "PhoneNumber": "+12188455850"
//  },


//"SendGridSettings": {
//    "ApiKey": "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",
//    "FromEmail": "rahul@coretegra.com"
//  },


//"AwsCredentials": {
//    "AccessKey": "AKIASMZNPITNTGQ3FL67",
//    "SecretKey": "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",
//    "TopicArn": "arn:aws:sns:ap-south-1:164914742491:UserManagementSystem",
//    "FromEmail": "rahul@coretegra.com"
//  },



//"ElasticMailSettings": {
//    "UserId": "8708487287",
//    "ApiKey": "A1E205B4D9659BDFF8E7DC47B4BD684D8BBBC8ACF219C334E1C7BBE2565C841EF908DC986E1B98085A60B06459874DFC",
//    "FromEmail": "rahul@coretegra.com",
//    "Port": 2525,
//    "ServerName": "smtp.elasticemail.com",
//    "SmtpPassword": "BDA145C18467947CA6ADBC085D52ED047A63",
//    "SmsEndpoint": "https://api.elasticemail.com/v2/sms/send"
//  },
//  "PostMarkSettings": {
//    "ApiToken": "cc2a4a98-af00-4327-8689-7835f4319a80",
//    "FromEmail": "rahul@coretegra.com"
//  },
//  "EmailSettings": {
//    "PrimaryDomain": "smtp-mail.outlook.com",
//    "PrimaryPort": "587",
//    "UsernameEmail": "rahulsaggu667@outlook.com",
//    "UsernamePassword": "Mr091102",
//    "FromEmail": "rahulsaggu667@outlook.com"
//  },

//  "NexmoSettings": {
//    "ApiKey": "8d6bd23f",
//    "SecretKey": "A7feFshgGG6WXz9d",
//    "PhoneNumber":  "+918708487287"
//  }