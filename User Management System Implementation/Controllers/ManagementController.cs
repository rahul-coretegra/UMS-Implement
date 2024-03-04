using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Models.VMs;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.SD;

namespace User_Management_System_Implementation.Controllers
{
    [ApiController]
    [Route(SDRoutes.Management)]
    [Authorize(Policy = SDPolicies.IsAccess)]
    public class ManagementController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ManagementController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet(SDRoutes.UserRole+SDRoutes.GetAll )]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> GetUserRoles()
        {
            try
            {
                IEnumerable<UserRole> list;
                var userRoleInClaim = await _unitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == User.FindFirstValue(ClaimTypes.Role));

                switch (userRoleInClaim.RoleLevel)
                {
                    case RoleLevels.SupremeLevel:
                        list = await _unitOfWork.UserRoles.GetAllAsync();
                        break;

                    case RoleLevels.Authority:
                        list = await _unitOfWork.UserRoles.GetAllAsync(d => d.RoleLevel < RoleLevels.Authority);
                        break;

                    case RoleLevels.Intermediate:
                        list = await _unitOfWork.UserRoles.GetAllAsync(d => d.RoleLevel < RoleLevels.Intermediate);
                        break;

                    default:
                        list = await _unitOfWork.UserRoles.GetAllAsync(d => d.RoleLevel < RoleLevels.Secondary);
                        break;
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet(SDRoutes.RouteAccess + SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> GetRouteAccesses()
        {
            try
            {
                var roles = await _unitOfWork.RouteAccess.GetAllAsync(includeProperties: "UserRole,Route");
                return Ok(roles);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet(SDRoutes.MenuAccess + SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> GetMenuAccess()
        {
            try
            {
                var roles = await _unitOfWork.MenuAccess.GetAllAsync(includeProperties: "UserRole,Menu");
                return Ok(roles);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpGet(SDRoutes.User + SDRoutes.GetAll)]
        [Authorize(Policy = SDPolicies.SecondaryLevel)]
        public async Task<IActionResult> GetUsers(string RoleId)
        {
            try
            {
                var userRoleInClaim = await _unitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == User.FindFirst(ClaimTypes.Role).Value);
                var requiredRole = await _unitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == RoleId);

                var users = await _unitOfWork.Users.GetAllAsync();
                List<UserVM> list = new List<UserVM>();
                foreach (var user in users)
                {
                    UserVM User = new UserVM()
                    {
                        Id = user.Id,
                        UserId = user.UserId,
                        UserName = user.UserName,
                        Email = user.Email,
                        Password = user.Password,
                        PhoneNumber = user.PhoneNumber,
                        Address = user.Address,
                        IsActiveUser = user.IsActiveUser,
                        IsVerifiedEmail = user.IsVerifiedEmail,
                        IsVerifiedPhoneNumber = user.IsVerifiedPhoneNumber,
                        CreatedAt = user.CreatedAt,
                        UpdatedAt = user.UpdatedAt,
                    };
                    if (userRoleInClaim.RoleLevel == RoleLevels.SupremeLevel)
                    {
                        if (requiredRole == null)
                            User.UserAndRoles = (await _unitOfWork.RoleAccess.GetAllAsync(x => x.UserId == user.UserId, includeProperties: "UserRole")).ToList();
                        else
                            User.UserAndRoles = (await _unitOfWork.RoleAccess.GetAllAsync(x => x.UserId == user.UserId && x.RoleId == RoleId, includeProperties: "UserRole")).ToList();
                    }
                    if (userRoleInClaim.RoleLevel != RoleLevels.SupremeLevel)
                    {
                        if (requiredRole == null)
                            return BadRequest(new { Message = "Required Role Not Found" });
                        else if (requiredRole.RoleLevel >= userRoleInClaim.RoleLevel)
                            return BadRequest(new { Message = "No Access For This Role" });
                        else
                            User.UserAndRoles = (await _unitOfWork.RoleAccess.GetAllAsync(x => x.UserId == user.UserId && x.RoleId == RoleId, includeProperties: "UserRole")).ToList();
                    }
                    if (User.UserAndRoles.Any(x => x.UserRole.RoleLevel == RoleLevels.SupremeLevel))
                        continue;
                    else
                        list.Add(User);
                }
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

       
        [HttpPut(SDRoutes.User + SDRoutes.Update)]
        [Authorize(Policy = SDPolicies.SupremeLevel)]
        public async Task<IActionResult> UpdateUsers([FromBody] User User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var indb = await _unitOfWork.Users.GetAsync(User.UserId);

                    var inDbExists = await _unitOfWork.Users.FirstOrDefaultAsync(d => (d.PhoneNumber == User.PhoneNumber || d.Email == User.Email) && d.UserId != indb.UserId);

                    if (indb == null)
                        return NotFound(new { message = "NotFound" });

                    if (inDbExists != null)
                        return BadRequest(new { message = "Data Not Available" });

                    await _unitOfWork.Users.UpdateAsync(indb.UserId, async entity =>
                    {
                        entity.UserName = User.UserName;
                        entity.Email = User.Email;
                        entity.PhoneNumber = User.PhoneNumber;
                        entity.Address = User.Address;
                        entity.UpdatedAt = DateTime.UtcNow;
                        if (indb.IsActiveUser != User.IsActiveUser)
                            entity.IsActiveUser = User.IsActiveUser;
                        await Task.CompletedTask;
                    });
                    var updateduser = await _unitOfWork.Users.FirstOrDefaultAsync(d => d.UserId == indb.UserId);
                    var userRoles = await _unitOfWork.RoleAccess.GetAllAsync(d => d.UserId == updateduser.UserId);

                    if (updateduser.IsActiveUser == TrueFalse.False)
                    {
                        foreach (var UserRole in userRoles)
                        {
                            await _unitOfWork.RoleAccess.UpdateAsync(UserRole.UniqueId, async entity =>
                            {
                                entity.AccessToRole = TrueFalse.False;
                                await Task.CompletedTask;
                            });
                        }
                    }
                    else if (updateduser.IsActiveUser == TrueFalse.True)
                    {
                        foreach (var UserRole in userRoles)
                        {
                            if (UserRole.RoleId == SDValues.IndividualRoleId)
                            {
                                await _unitOfWork.RoleAccess.UpdateAsync(UserRole.UniqueId, async entity =>
                                {
                                    entity.AccessToRole = TrueFalse.True;
                                    await Task.CompletedTask;
                                });
                            }
                        }
                    }
                    return Ok(new { message = "Updated" });
                }
                else
                {
                    return BadRequest(new { message = "BadRequest" });
                }
            }

            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        [HttpPost(SDRoutes.UpSertRoleAccess)]
        [Authorize(Policy = SDPolicies.IntermediateLevel)]
        public async Task<IActionResult> UpsertUserAndRoles([FromBody] RoleAccess[] UserAndRoles)
        {
            try
            {
                foreach (var userAndRole in UserAndRoles)
                {
                    var userRoleInClaim = await _unitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == User.FindFirst(ClaimTypes.Role).Value);
                    var requiredRole = await _unitOfWork.UserRoles.FirstOrDefaultAsync(d => d.RoleId == userAndRole.RoleId);

                    if (requiredRole.RoleLevel >= userRoleInClaim.RoleLevel)
                        return BadRequest(new { Message = "No Access For This Role" });

                    var userAndRoleInDb = await _unitOfWork.RoleAccess.FirstOrDefaultAsync(x => x.RoleId == userAndRole.RoleId && x.UserId == userAndRole.UserId);
                    if (userAndRoleInDb == null)
                    {
                        var UniqueId = _unitOfWork.UniqueId();
                        RoleAccess addUserAndRole = new RoleAccess()
                        {
                            UniqueId = UniqueId,
                            UserId = userAndRole.UserId,
                            RoleId = userAndRole.RoleId,
                            AccessToRole = TrueFalse.True
                        };
                        await _unitOfWork.RoleAccess.AddAsync(addUserAndRole);
                    }
                    else if (userAndRoleInDb.AccessToRole != userAndRole.AccessToRole)
                    {
                        await _unitOfWork.RoleAccess.UpdateAsync(userAndRoleInDb.UniqueId, async entity =>
                        {
                            entity.AccessToRole = userAndRole.AccessToRole;
                            await Task.CompletedTask;
                        });
                    }
                }
                return Ok(new { message = "Ok" });
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

    }
}
