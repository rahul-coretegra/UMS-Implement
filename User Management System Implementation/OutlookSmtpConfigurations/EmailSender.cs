using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;


namespace User_Management_System_Implementation.OutlookSmtpConfigurations
{
    public class EmailSender : IEmailSender
    {
        private OutlookSettings _emailSettings { get; set; }

        public EmailSender(IOptions<OutlookSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Execute(email, subject, htmlMessage).Wait();
            return Task.FromResult(0);
        }
        public async Task Execute(string toEMail, string subject, string htmlMessage)
        {
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.Email, "User Management System")
                };
                mail.To.Add(toEMail);
                mail.Subject = subject;
                mail.Body = htmlMessage;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                using (SmtpClient smtp = new SmtpClient(_emailSettings.Domain, _emailSettings.Port))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Password);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

    }
}
