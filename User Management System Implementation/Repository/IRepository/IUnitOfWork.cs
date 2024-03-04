
namespace User_Management_System_Implementation.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRoleRepository UserRoles { get; }

        IUserRepository Users { get; }

        IRouteRepository Routes { get; }

        IRoleAccessRepository RoleAccess { get; }

        IRouteAccessRepository RouteAccess { get; }

        IMenuRepository Menus { get; }

        IMenuAccessRepository MenuAccess { get; }

        IServiceRepository Services { get; }

        IConfigureServiceRepository ConfigureServices { get; }

        IUserVerificationRepository UserVerifications { get; }

        string UniqueId();

    }
}
