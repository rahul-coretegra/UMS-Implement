using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class MenuAccessRepository:Repository<MenuAccess>, IMenuAccessRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuAccessRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
