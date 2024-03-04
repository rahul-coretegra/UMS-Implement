using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;
using Route = User_Management_System_Implementation.Models.Route;

namespace User_Management_System_Implementation.Repository
{
    public class RouteRepository : Repository<Route>, IRouteRepository
    {
        private readonly ApplicationDbContext _context;

        public RouteRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }
    }
}
