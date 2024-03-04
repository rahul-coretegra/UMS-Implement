
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using User_Management_System_Implementation.Models;
using static User_Management_System_Implementation.AuthorizationResources.AuthorizationRequirements;

namespace User_Management_System_Implementation.AuthorizationResources
{
    public class SupremeLevelAuthorizationHandler : AuthorizationHandler<SupremeLevelRequirement>
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SupremeLevelAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SupremeLevelRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirstValue(ClaimTypes.Role);

            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.RoleLevel == RoleLevels.SupremeLevel);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.RoleId == userRoleInClaim))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;

        }
    }

    public class AuthorityLevelAuthorizationHandler : AuthorizationHandler<AuthorityLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AuthorityLevelAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorityLevelRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirstValue(ClaimTypes.Role);

            var roles = _dbContext.UserRoles.ToList().Where(d => d.RoleLevel >= RoleLevels.Authority);

            if (userRoleInClaim != null && roles.Any(role => role.RoleId == userRoleInClaim))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;

        }

    }

    public class IntermediateLevelAuthorizationHandler : AuthorizationHandler<IntermediateLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public IntermediateLevelAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IntermediateLevelRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirstValue(ClaimTypes.Role);

            var roles = _dbContext.UserRoles.ToList().Where(d => d.RoleLevel >= RoleLevels.Intermediate);

            if (userRoleInClaim != null && roles.Any(role => role.RoleId == userRoleInClaim))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }

    public class SecondaryLevelAuthorizationHandler : AuthorizationHandler<SecondaryLevelRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public SecondaryLevelAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecondaryLevelRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirstValue(ClaimTypes.Role);


            var supremeLevelRoles = _dbContext.UserRoles.ToList().Where(d => d.RoleLevel >= RoleLevels.Secondary);

            if (userRoleInClaim != null && supremeLevelRoles.Any(role => role.RoleId == userRoleInClaim))
            {
                context.Succeed(requirement); // Authorization requirement is met
            }
            return Task.CompletedTask;
        }
    }

    public class IsAccssAuthorizationHandler : AuthorizationHandler<IsAccssRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public IsAccssAuthorizationHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAccssRequirement requirement)
        {

            var userRoleInClaim = context.User.FindFirstValue(ClaimTypes.Role);

            var CurrentRoute = _httpContextAccessor.HttpContext.Request.Path;

            var RolesWithAccessToRead = _dbContext.RouteAccess.Include(d => d.Route).ToList().Where(d => d.Route.RoutePath == CurrentRoute && d.IsAccess == TrueFalse.True);

            if (userRoleInClaim != null && RolesWithAccessToRead.Any(role => role.RoleId == userRoleInClaim))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}