using User_Management_System_Implementation.Models;

namespace User_Management_System_Implementation.Repository.IRepository
{
    public interface IMenuRepository:IRepository<Menu>
    {
        public object GetMenuWithSubmenus(Menu Menu, List<Menu> AllMenus);
    }
}
