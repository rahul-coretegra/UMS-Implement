using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Options;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon;
using User_Management_System_Implementation.SD;
using User_Management_System_Implementation.AwsConfigurations;

namespace User_Management_System_Implementation.AwsSnsConfigurations
{
    public class AwsRepository : IAwsRepository
    {
        private readonly AwsSimpleNotificationSettings _awsNotification;
        private readonly AwsSimpleEmailSettings _awsEmail;


        public AwsRepository(IOptions<AwsSimpleNotificationSettings> notification, IOptions<AwsSimpleEmailSettings> email)
        {
            _awsNotification = notification.Value;
            _awsEmail = email.Value;
        }

        public async Task<string> SendEmail(string Email)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();

                var credentials = new BasicAWSCredentials(_awsEmail.AccessKey, _awsEmail.SecretKey);

                var client = new AmazonSimpleEmailServiceClient(credentials, RegionEndpoint.APSouth1);

                var sendRequest = new SendEmailRequest
                {
                    Source = _awsEmail.FromEmail,
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { Email }
                    },
                    Message = new Message
                    {
                        Subject = new Content(SDValues.VerificationMail),
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = $@"
                                    <html>                  
                                        <body>
                                            <div style=""font-family: Helvetica,Arial,sans-serif;min-width:1000px;overflow:auto;line-height:2"">
                                                <div style=""margin:50px auto;width:70%;padding:20px 0"">
                                                <div style=""border-bottom:1px solid #eee"">
                                                    <h3 style=""font-size:1.4em;color: #00466a;text-decoration:none;font-weight:600"">{SDValues.UserManagementSystem}</h3>
                                                </div>
                                                <p style=""font-size:1.1em"">Hi,</p>
                                                <p>{SDValues.Message + SDValues.OTPTimeStamp}</p>
                                                <h2 style=""background: #00466a;margin: 0 auto;width: max-content;padding: 0 10px;color: #fff;border-radius: 4px;""> {randomValue}</h2>
                                                <p style=""font-size:0.9em;"">Regards,<br />{SDValues.UserManagementSystem}</p>
                                                <hr style=""border:none;border-top:1px solid #eee"" />
                                                <div style=""float:right;padding:8px 0;color:#aaa;font-size:0.8em;line-height:1;font-weight:300"">
                                                    <p>{SDValues.UserManagementSystem}</p>
                                                </div>
                                                </div>
                                            </div>
                                        </body>
                                    </html>"
                            }
                        }
                    }
                };
                var res = await client.SendEmailAsync(sendRequest);
                if(res.HttpStatusCode == System.Net.HttpStatusCode.OK)

                    return randomValue;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        public async Task<string> SendNotification(string PhoneNumber)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();

                var credentials = new BasicAWSCredentials(_awsNotification.AccessKey, _awsNotification.SecretKey);

                var client = new AmazonSimpleNotificationServiceClient(credentials, Amazon.RegionEndpoint.APSouth1);

                var subscribeRequest = new SubscribeRequest
                {
                    TopicArn = _awsNotification.TopicArn,
                    Protocol = "SMS",
                    Endpoint = $"+91{PhoneNumber}"
                };

                await client.SubscribeAsync(subscribeRequest);

                var request = new PublishRequest
                {
                    PhoneNumber = $"+91{PhoneNumber}",
                    Subject = SDValues.VerificationPhoneNumber,
                    Message = $"{SDValues.Message} {SDValues.OTPTimeStamp} \n  {randomValue}"
                };
                var res = await client.PublishAsync(request);
                if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)

                    return randomValue;
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }

        }
    }
}
