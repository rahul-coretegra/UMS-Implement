using Microsoft.Extensions.Options;
using Nexmo.Api;
using Nexmo.Api.Messaging;
using Nexmo.Api.Request;
using User_Management_System_Implementation.SD;

namespace User_Management_System_Implementation.NexmoConfigurations
{
    public class NexmoRepository : INexmoRepository
    {
        private readonly NexmoSettings _nexmo;
        public NexmoRepository(IOptions<NexmoSettings> nexmoSettings)
        {
                _nexmo = nexmoSettings.Value;
        }

        public async Task<string> SendSms(string PhoneNumber)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();

                var client = new NexmoClient(Credentials.FromApiKeyAndSecret(_nexmo.ApiKey, _nexmo.SecretKey));

                await Task.Run(() => client.SmsClient.SendAnSms(new SendSmsRequest
                {
                    To = $"+91{PhoneNumber}",
                    From = _nexmo.PhoneNumber,
                    Text = $"{SDValues.Message} {SDValues.OTPTimeStamp} \n  {randomValue}\n\n\n"
                }));
                return randomValue;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
