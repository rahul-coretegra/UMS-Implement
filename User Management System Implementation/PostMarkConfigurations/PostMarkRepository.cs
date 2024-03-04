using Microsoft.Extensions.Options;
using PostmarkDotNet;
using User_Management_System_Implementation.SD;

namespace User_Management_System_Implementation.PostMarkConfigurations
{
    public class PostMarkRepository : IPostMarkRepository
    {
        private readonly PostMarkSettings _postMark;
        public PostMarkRepository(IOptions<PostMarkSettings> options)
        {
            _postMark = options.Value;
        }
        public async Task<string> SendEmail(string Email)
        {
            try
            {
                string randomValue = new Random().Next(100000, 999999).ToString();

                var client = new PostmarkClient(_postMark.ApiToken);

                var message = new PostmarkMessage
                {
                    From = _postMark.FromEmail,
                    To = Email,
                    Subject = SDValues.VerificationMail,
                    TextBody = "",
                    HtmlBody = $@"
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
                };
                var res = await client.SendMessageAsync(message);
                return randomValue;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
