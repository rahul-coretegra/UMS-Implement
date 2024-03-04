using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.AwsConfigurations;
using User_Management_System_Implementation.AwsSnsConfigurations;
using User_Management_System_Implementation.ElasticMailConfigurations;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.NexmoConfigurations;
using User_Management_System_Implementation.OutlookSmtpConfigurations;
using User_Management_System_Implementation.PostMarkConfigurations;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.TwilioConfigurations;
using User_Management_System_Implementation.TwilioModule;

namespace User_Management_System_Implementation.Repository
{
    public class DefaultConfigurationsRepository : IDefaultConfigurationsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public DefaultConfigurationsRepository(ApplicationDbContext applicationDb, IConfiguration configuration)
        {
            _context = applicationDb;
            _configuration = configuration;

        }
        public void ConfigureService(IServiceCollection services)
        {
            var configuredServices = GetConfiguredServices();

            var configureditems = GetConfiguredItems();

            foreach (var service in configuredServices)
            {
                if (service.ServiceType == TypeOfService.AwsSimpleEmail)
                {
                    services.Configure<AwsSimpleEmailSettings>(options =>
                    {
                        options.AccessKey = configureditems.FirstOrDefault(x => x.Service== service && x.ItemName == "AccessKey").ItemValue;
                        options.SecretKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "SecretKey").ItemValue;
                        options.FromEmail = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "FromEmail").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.AwsSimpleNotification)
                {
                    services.Configure<AwsSimpleNotificationSettings>(options =>
                    {
                        options.AccessKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "AccessKey").ItemValue;
                        options.SecretKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "SecretKey").ItemValue;
                        options.TopicArn = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "TopicArn").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.ElasticMailEmail)
                {
                    services.Configure<ElasticMailEmailSettings>(options =>
                    {
                        options.ApiKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ApiKey").ItemValue;
                        options.FromEmail = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "FromEmail").ItemValue;
                        options.Port = int.Parse(configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "Port").ItemValue);
                        options.ServerName = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ServerName").ItemValue;
                        options.SmtpPassword = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "SmtpPassword").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.ElasticMailSms)
                {
                    services.Configure<ElasticMailSmsSettings>(options =>
                    {
                        options.ApiKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ApiKey").ItemValue;
                        options.SmsEndpoint = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "SmsEndpoint").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.NexmoSms)
                {
                    services.Configure<NexmoSettings>(options =>
                    {
                        options.ApiKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ApiKey").ItemValue;
                        options.SecretKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "SecretKey").ItemValue;
                        options.PhoneNumber = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "PhoneNumber").ItemValue;
                    });
                }

                else if(service.ServiceType == TypeOfService.OutLookEmail)
                {
                    services.Configure<OutlookSettings>(options =>
                    {
                        options.Domain = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "Domain").ItemValue;
                        options.Port = int.Parse(configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "Port").ItemValue);
                        options.Email = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "Email").ItemValue;
                        options.Password = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "Password").ItemValue;
                        options.FromEmail = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "FromEmail").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.PostMarkEmail)
                {
                    services.Configure<PostMarkSettings>(options =>
                    {
                        options.ApiToken = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ApiToken").ItemValue;
                        options.FromEmail = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "FromEmail").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.TwilioSendGridEmail)
                {
                    services.Configure<SendGridSettings>(options =>
                    {
                        options.ApiKey = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "ApiKey").ItemValue;
                        options.FromEmail = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "FromEmail").ItemValue;
                    });
                }

                else if (service.ServiceType == TypeOfService.TwilioSms)
                {
                    services.Configure<TwilioSettings>(options =>
                    {
                        options.AccountSID = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "AccountSID").ItemValue;
                        options.AuthToken = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "AuthToken").ItemValue;
                        options.PhoneNumber = configureditems.FirstOrDefault(x => x.Service == service && x.ItemName == "PhoneNumber").ItemValue;
                    });
                }

                else
                    continue;
            }

        }

        public List<Service> GetConfiguredServices()
        {
            _context.Database.EnsureCreated();
            return
                _context.Services.Where(x=>x.Status == TrueFalse.True).ToList();
        }

        public List<ConfigureService> GetConfiguredItems()
        {
            _context.Database.EnsureCreated();
            return
                _context.ConfigureServices.Where(x => x.IsConfigured == TrueFalse.True).Include("Service").ToList();
        }
    }
}



