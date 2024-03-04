using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using User_Management_System_Implementation.Repository.IRepository;

namespace User_Management_System_Implementation.Repository 
{ 

    public class MenuRepository : Repository<Menu>, IMenuRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuRepository(ApplicationDbContext options) : base(options)
        {
            _context = options;
        }

        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus)
        {
            try
            {
                return new
                {
                    id = Menu.Id,
                    UniqueId = Menu.MenuId,
                    MenuName = Menu.MenuName,
                    MenuPath = Menu.MenuPath,
                    MenuIcon = Menu.MenuIcon,
                    Status = Menu.Status,
                    ParentId = Menu.ParentId,
                    SubMenus = AllMenus
                                   .Where(subMenu => subMenu.ParentId == Menu.MenuId)
                                   .Select(subMenu => GetMenuWithSubmenus(subMenu, AllMenus))
                                   .ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
    }
}
