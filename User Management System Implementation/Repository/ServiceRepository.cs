using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class ServiceRepository:Repository<Service>, IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
