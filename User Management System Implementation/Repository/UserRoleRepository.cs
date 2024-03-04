using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRoleRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
