using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Models.VMs;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.SD;


namespace User_Management_System_Implementation.Controllers
{
    [Route(SDRoutes.Individual)]
    [ApiController]
    public class IndividualController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IndividualController(IUnitOfWork unitofwork)
        {
            _unitOfWork = unitofwork;
        }
        [HttpPost(SDRoutes.User + SDRoutes.Register)]
        public async Task<IActionResult> RegisterUser([FromBody] UserVM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var isuniqueuser = await _unitOfWork.Users.IsUniqueUser(user.PhoneNumber, user.Email);
                    if (!isuniqueuser)
                        return NotFound(new { message = "Exists" });
                    var userId = _unitOfWork.UniqueId();
                    User registerUser = new User
                    {
                        UserId = userId,
                        UserName = user.UserName,
                        PhoneNumber = user.PhoneNumber,
                        IsVerifiedPhoneNumber = TrueFalse.False,
                        Email = user.Email,
                        IsVerifiedEmail = TrueFalse.False,
                        Address = user.Address,
                        Password = user.Password,
                        CreatedAt = DateTime.UtcNow            
                    };
                    RoleAccess userrole = new RoleAccess
                    {
                        UniqueId = _unitOfWork.UniqueId(),
                        UserId = userId,
                        RoleId = SDValues.IndividualRoleId,
                        AccessToRole = TrueFalse.True
                    };
                    await _unitOfWork.Users.RegisterUser(registerUser);
                    await _unitOfWork.RoleAccess.AddAsync(userrole);
                    return Ok(new { message = "Created" });
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPut(SDRoutes.VerifyEmailsAndMessages)]
        public async Task<IActionResult> VerifyEmailsAndMessages([FromBody] UserVerification verificationVM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Email == verificationVM.Identity || u.PhoneNumber == verificationVM.Identity);
                if (userindb == null)
                    return NotFound();
                var verificationindb = await _unitOfWork.UserVerifications.FirstOrDefaultAsync(v => (v.Identity == userindb.PhoneNumber || v.Identity == userindb.Email) && v.Otp == verificationVM.Otp);
                if (verificationindb == null)
                    return NotFound();

                if (_unitOfWork.UserVerifications.IsOtpExpired(verificationindb))
                    return BadRequest(new { message = "otp expired" });
                else
                {
                    var isverified = await _unitOfWork.UserVerifications.IsVerified(verificationVM.Identity, verificationVM.Otp);
                    if (isverified)
                    {
                        if (verificationVM.Identity == userindb.PhoneNumber)
                        {
                            await _unitOfWork.Users.UpdateAsync(userindb.UserId, async entity =>
                            {
                                entity.IsVerifiedPhoneNumber = TrueFalse.True;
                                if (entity.IsVerifiedEmail == TrueFalse.True)
                                    entity.IsActiveUser = TrueFalse.True;
                                await Task.CompletedTask;
                            });
                            await _unitOfWork.UserVerifications.RemoveAsync(verificationindb.Identity);

                        }
                        else if (verificationVM.Identity == userindb.Email)
                        {
                            await _unitOfWork.Users.UpdateAsync(userindb.UserId, async entity =>
                            {
                                entity.IsVerifiedEmail = TrueFalse.True;
                                if (entity.IsVerifiedPhoneNumber == TrueFalse.True)
                                    entity.IsActiveUser = TrueFalse.True;
                                await Task.CompletedTask;
                            });
                            await _unitOfWork.UserVerifications.RemoveAsync(verificationindb.Identity);
                        }
                        return Ok(new { status = isverified });

                    }
                    else
                        return BadRequest(new { status = isverified });

                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }
        }

        [HttpPost(SDRoutes.Authenticate)]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateVM VM)
        {
            try
            {
                var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == VM.Identity

                                || u.UserId == VM.Identity || u.Email == VM.Identity);


                if (userindb == null)
                    return NotFound(new { message = "Not Found" });

                else if (userindb.Password != VM.Password)
                    return BadRequest(new { message = "Wrong Password" });

                else if (userindb.IsActiveUser != TrueFalse.True)
                    return BadRequest(new { message = "User Not Active." });
                else
                {
                    var userroles = (await _unitOfWork.RoleAccess.GetAllAsync(x => x.UserId == userindb.UserId && x.AccessToRole == TrueFalse.True , includeProperties:"UserRole")).ToList();

                    if (VM.RoleId == null)
                    {
                        if (userroles== null)
                            return BadRequest(new { message = "Sorry !! You don't have access to Any Role." });
                        else if (userroles.Count == 1)
                        {
                            var tokenindb = await _unitOfWork.Users.Authenticate(VM.Identity, userroles.First().RoleId);
                            return Ok(new { token = tokenindb });
                        }
                        else
                        {
                            return Ok(new { UserRoles = userroles });
                        }
                    }
                    else
                    {
                        var userrole = userroles.FirstOrDefault(x => x.RoleId == VM.RoleId && x.AccessToRole == TrueFalse.True);
                        if (userrole == null)
                            return BadRequest(new { message = "Sorry !! You don't have access to this Role." });

                        var tokenindb = await _unitOfWork.Users.Authenticate(VM.Identity, userrole.RoleId);

                        return Ok(new { token = tokenindb });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
       
        [HttpPut(SDRoutes.User+ SDRoutes.Update)]
        [Authorize(Policy = SDPolicies.IsAccess)]
        public async Task<IActionResult> UpdateUser([FromBody] UserVM user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var inRoleUserId = User.FindFirstValue(ClaimTypes.SerialNumber);
                    var indb = await _unitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == inRoleUserId);

                    var inDbExists = await _unitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == user.PhoneNumber || d.Email == user.Email) && d.UserId != indb.UserId);

                    if (indb == null)
                        return NotFound(new { message = "NotFound" });

                    if (inRoleUserId != user.UserId)
                        return NotFound(new { message = "No access" });

                    if (inDbExists != null)
                        return BadRequest(new { message = "Data Not Available" });

                    await _unitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
                    {
                        entity.UserName = user.UserName;
                        if (indb.Email != user.Email)
                        {
                            entity.IsVerifiedEmail = TrueFalse.False;
                            entity.IsActiveUser = TrueFalse.False;
                        }
                        else
                            entity.Email = user.Email;
                        if (indb.PhoneNumber != user.PhoneNumber)
                        {
                            entity.IsVerifiedPhoneNumber = TrueFalse.False;
                            entity.IsActiveUser = TrueFalse.False;
                        }
                        else
                            entity.PhoneNumber = user.PhoneNumber;
                        entity.Address = user.Address;
                        entity.Password = user.Password;
                        entity.UpdatedAt = DateTime.UtcNow;
                        await Task.CompletedTask;
                    });

                    return Ok(new { message = "Updated" });
                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);

            }
        }

        [HttpPut(SDRoutes.User + SDRoutes.ResetPassword)]
        public async Task<IActionResult> ResetPassword(string Identity, [FromBody] PasswordVM VM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var userindb = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.PhoneNumber == Identity

                                 || u.UserId == Identity || u.Email == Identity);

                    if (userindb == null)
                        return NotFound(new { message = "NotFound" });


                    await _unitOfWork.Users.UpdateAsync(userindb.UserId, async entity =>
                    {
                        entity.Password = VM.Password;

                        entity.UpdatedAt = DateTime.UtcNow;
                        await Task.CompletedTask;
                    });

                    return Ok(new { message = "Updated" });

                }
                else
                    return BadRequest(new { message = "BadRequest" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
