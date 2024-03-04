using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class RoleAccessRepository: Repository<RoleAccess>, IRoleAccessRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleAccessRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
