using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User_Management_System_Implementation.AwsSnsConfigurations;
using User_Management_System_Implementation.ElasticMailConfigurations;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.NexmoConfigurations;
using User_Management_System_Implementation.OutlookSmtpConfigurations;
using User_Management_System_Implementation.PostMarkConfigurations;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.SD;
using User_Management_System_Implementation.TwilioModule;

namespace User_Management_System_Implementation.Controllers
{
    [Route(SDRoutes.Emails)]
    [ApiController]
    public class SendEmailsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITwilioRepository _twilio;
        private readonly IOutlookSmtpRepository _outlook;
        private readonly IAwsRepository _aws;
        private readonly IElasticMailRepository _elasticMail;
        private readonly IPostMarkRepository _postMark;
        public SendEmailsController(IUnitOfWork unitofwork,
            ITwilioRepository twilio, IOutlookSmtpRepository outlook, IAwsRepository aws,
            IElasticMailRepository elasticMail, IPostMarkRepository postMark)
        {
            _unitOfWork = unitofwork;
            _twilio = twilio;
            _outlook = outlook;
            _aws = aws;
            _elasticMail = elasticMail;
            _postMark = postMark;
        }

        [HttpPost(SDRoutes.OutlookSmtp)]
        public async Task<IActionResult> WithOutlookSmtp([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedEmail == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _outlook.SendEmail(userindb.Email);

                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.Email);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.Email,
                            Otp = otp,
                            OtpTimeStamp = DateTime.UtcNow,
                        };
                        await _unitOfWork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _unitOfWork.UserVerifications.UpdateAsync(indb.Identity, async entity =>
                        {
                            entity.Otp = otp;
                            entity.OtpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }
        }

        [HttpPost(SDRoutes.ElasticMail)]
        public async Task<IActionResult> WithElasticMail([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedEmail == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _elasticMail.SendEmail(userindb.Email);

                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.Email);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.Email,
                            Otp = otp,
                            OtpTimeStamp = DateTime.UtcNow,
                        };
                        await _unitOfWork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _unitOfWork.UserVerifications.UpdateAsync(indb.Identity, async entity =>
                        {
                            entity.Otp = otp;
                            entity.OtpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost(SDRoutes.SendGrid)]
        public async Task<IActionResult> WithSendGrid([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedEmail == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _twilio.SendEmail(userindb.Email);

                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.Email);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.Email,
                            Otp = otp,
                            OtpTimeStamp = DateTime.UtcNow,
                        };
                        await _unitOfWork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _unitOfWork.UserVerifications.UpdateAsync(indb.Identity, async entity =>
                        {
                            entity.Otp = otp;
                            entity.OtpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost(SDRoutes.AwsSimpleEmail)]
        public async Task<IActionResult> WithAwsSimpleEmail([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedEmail == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _aws.SendEmail(verificationVM.Identity);

                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.Email);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.Email,
                            Otp = otp,
                            OtpTimeStamp = DateTime.UtcNow,
                        };
                        await _unitOfWork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _unitOfWork.UserVerifications.UpdateAsync(indb.Identity, async entity =>
                        {
                            entity.Otp = otp;
                            entity.OtpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost(SDRoutes.PostMark)]
        public async Task<IActionResult> WithMailPostMark([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedEmail == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _postMark.SendEmail(userindb.Email);

                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.Email);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.Email,
                            Otp = otp,
                            OtpTimeStamp = DateTime.UtcNow,
                        };
                        await _unitOfWork.UserVerifications.AddAsync(verification);
                    }
                    else
                    {
                        await _unitOfWork.UserVerifications.UpdateAsync(indb.Identity, async entity =>
                        {
                            entity.Otp = otp;
                            entity.OtpTimeStamp = DateTime.UtcNow;
                            await Task.CompletedTask;
                        });
                    }
                    return Ok(new { message = "Message Sent" });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }
        }

    }
}
