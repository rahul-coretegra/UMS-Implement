using Microsoft.AspNetCore.Mvc;
using User_Management_System_Implementation.AwsSnsConfigurations;
using User_Management_System_Implementation.ElasticMailConfigurations;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.NexmoConfigurations;
using User_Management_System_Implementation.OutlookSmtpConfigurations;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.SD;
using User_Management_System_Implementation.TwilioModule;

namespace User_Management_System_Implementation.Controllers
{
    [ApiController]
    [Route(SDRoutes.Sms)]
    public class SendSmsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITwilioRepository _twilio;
        private readonly IAwsRepository _aws;
        private readonly IElasticMailRepository _elasticMail;
        private readonly INexmoRepository _nexmo;
        public SendSmsController(IUnitOfWork unitofwork,
            ITwilioRepository twilio, IAwsRepository aws,
            IElasticMailRepository elasticMail, INexmoRepository nexmo)
        {
            _unitOfWork = unitofwork;
            _twilio = twilio;
            _aws = aws;
            _elasticMail = elasticMail;
            _nexmo = nexmo;
        }

        [HttpPost(SDRoutes.Twilio)]
        public async Task<IActionResult> WithTwilio([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedPhoneNumber == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _twilio.SendSms(userindb.PhoneNumber);
                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.PhoneNumber);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.PhoneNumber,
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
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedPhoneNumber == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _elasticMail.SendSms(userindb.PhoneNumber);
                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.PhoneNumber);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.PhoneNumber,
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

        [HttpPost(SDRoutes.Nexmo)]
        public async Task<IActionResult> WithNexmo([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedPhoneNumber == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _nexmo.SendSms(userindb.PhoneNumber);
                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.PhoneNumber);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.PhoneNumber,
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

        [HttpPost(SDRoutes.AwsSimpleNotifiation)]
        public async Task<IActionResult> WithAwsSimpleNotifiation([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == verificationVM.Identity);
                if (userindb == null)
                    return NotFound(new { message = "NotFound" });

                if (userindb.IsVerifiedPhoneNumber == TrueFalse.True)
                    return Ok(new { message = "Already Verified" });
                else
                {
                    var otp = await _aws.SendNotification(userindb.PhoneNumber);
                    var indb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(x => x.Identity == userindb.PhoneNumber);
                    if (indb == null)
                    {
                        UserVerification verification = new UserVerification()
                        {
                            Identity = userindb.PhoneNumber,
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
