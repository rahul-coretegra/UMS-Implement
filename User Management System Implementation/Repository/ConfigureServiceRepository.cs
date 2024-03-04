using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class ConfigureServiceRepository : Repository<ConfigureService>, IConfigureServiceRepository
    {
        private readonly ApplicationDbContext _context;
        public ConfigureServiceRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
