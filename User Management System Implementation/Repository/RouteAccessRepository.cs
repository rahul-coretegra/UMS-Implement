using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository
{
    public class RouteAccessRepository : Repository<RouteAccess>, IRouteAccessRepository
    {
        private readonly ApplicationDbContext _context;

        public RouteAccessRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
