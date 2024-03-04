using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using User_Management_System_Implementation.SD;
using User_Management_System_Implementation.TwilioConfigurations;

namespace User_Management_System_Implementation.TwilioModule
{
    public class TwilioRepository : ITwilioRepository
    {
        private readonly TwilioSettings _twilio;
        private readonly SendGridSettings _sendGrid;
        public TwilioRepository(IOptions<TwilioSettings> twilio, IOptions<SendGridSettings> options)
        {
            _twilio = twilio.Value;
            _sendGrid = options.Value;
        }

        public async Task<string> SendEmail(string Email)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();
                var client = new SendGridClient(_sendGrid.ApiKey);
                var from = new EmailAddress(_sendGrid.FromEmail);
                var to = new EmailAddress(Email);

                string subject = SDValues.VerificationMail;
                string body = $@"
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
                var res = await client.SendEmailAsync(MailHelper.CreateSingleEmail(from, to, subject, "", body));
                return randomValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }

        }

        public async Task<string> SendSms(string PhoneNumber)
        {
            try
            {
                string authtoken = _twilio.AuthToken;
                string accountsid = _twilio.AccountSID;
                string phone = _twilio.PhoneNumber;

                string randomValue = new Random().Next(100000, 999999).ToString();

                TwilioClient.Init(accountsid, authtoken);
                var res = await MessageResource.CreateAsync(
                        body: $"{SDValues.Message} {SDValues.OTPTimeStamp} \n  {randomValue}",
                        from: new Twilio.Types.PhoneNumber(phone),
                        to: $"+91{PhoneNumber}"
                    );
                return randomValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
