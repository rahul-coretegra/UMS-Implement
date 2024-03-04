using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using User_Management_System_Implementation.SD;
using System.Text.Json;

namespace User_Management_System_Implementation.ElasticMailConfigurations
{
    public class ElasticMailRepository : IElasticMailRepository
    {
        private readonly ElasticMailEmailSettings _elasticEmail;
        private readonly ElasticMailSmsSettings _elasticSms;

        public ElasticMailRepository(IOptions<ElasticMailEmailSettings> elasticEmail, IOptions<ElasticMailSmsSettings> elasticSms)
        {
            _elasticEmail = elasticEmail.Value;
            _elasticSms = elasticSms.Value;
        }

        public async Task<string> SendEmail(string Email)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();

                using (SmtpClient client = new SmtpClient(_elasticEmail.ServerName, _elasticEmail.Port))
                {
                    client.Credentials = new NetworkCredential(_elasticEmail.FromEmail, _elasticEmail.SmtpPassword);
                    client.EnableSsl = true;

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(_elasticEmail.FromEmail);
                        message.To.Add(Email);
                        message.Subject = SDValues.VerificationMail;
                        message.IsBodyHtml = true;
                        message.Body = $@"
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
                    </html>";

                        await client.SendMailAsync(message);
                    }
                }

                return randomValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        public async Task<string> SendSms(string Phonenumber)
        {

            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();
                var bodyObject = new
                {
                    apikey = _elasticSms.ApiKey,
                    to = "+91" + Phonenumber,
                    body = $"{SDValues.Message} {SDValues.OTPTimeStamp} \r\n {randomValue}"
                };

                string jsonBody = JsonSerializer.Serialize(bodyObject);

                HttpClient client = new HttpClient();
                var content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");
                var res = await client.PostAsync(_elasticSms.SmsEndpoint, content);
                if (res.IsSuccessStatusCode)
                {
                    return randomValue;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}


