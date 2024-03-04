using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;
using User_Management_System_Implementation.SD;

namespace User_Management_System_Implementation.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IOptions<AppSettings> _appsettings;

        public UnitOfWork(ApplicationDbContext context, IOptions<AppSettings> settings)
        {
            _context = context;
            _appsettings = settings;
            UserRoles = new UserRoleRepository(_context);
            Users = new UserRepository(_context, _appsettings);
            Routes = new RouteRepository(_context);
            RoleAccess = new RoleAccessRepository(_context);
            RouteAccess = new RouteAccessRepository(_context);
            Menus = new MenuRepository(_context);
            MenuAccess = new MenuAccessRepository(_context);
            Services = new ServiceRepository(_context);
            ConfigureServices = new ConfigureServiceRepository(_context);
            UserVerifications = new UserVerificationRepository(_context);


        }
        public IUserRoleRepository UserRoles { private set; get; }

        public IUserRepository Users { private set; get; }

        public IRouteRepository Routes { private set; get; }

        public IRoleAccessRepository RoleAccess { private set; get; }

        public IRouteAccessRepository RouteAccess { private set; get; }

        public IMenuRepository Menus { private set; get; }

        public IMenuAccessRepository MenuAccess { private set; get; }

        public IServiceRepository Services { private set; get; }

        public IConfigureServiceRepository ConfigureServices { private set; get; }

        public IUserVerificationRepository UserVerifications { private set; get; }

        public string UniqueId()
        {
            try
            {
                DateTime now = DateTime.Now;
                Random random = new Random();

                string additionalDigits = new string(Enumerable.Repeat(SDValues.ConstantStringKey, 6).Select(s => s[random.Next(s.Length)]).ToArray());

                string UniqueId = $"{additionalDigits}{now:yyyymmddHHssffff}";

                return UniqueId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
