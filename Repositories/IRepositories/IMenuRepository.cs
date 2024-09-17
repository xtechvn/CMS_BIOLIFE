using Entities.Models;
using Entities.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IMenuRepository
    {
        Task<List<MenuViewModel>> GetMenuParentAndChildAll();
        Task<List<Permission>> GetPermissionList();
        Task<IEnumerable<MenuPermission>> GetAllMenuHasPermission();
        Task<List<Menu>> GetMenuPermissionOfUser(IEnumerable<int> menu_ids);

        Task<long> Create(Menu model);
        Task<long> Update(Menu model);
        Task<Menu> GetById(int id);
        Task<IEnumerable<Menu>> GetAll(string name, string link);
        Task<long> ChangeStatus(int id, int status);
        Task<IEnumerable<Permission>> GetListPermission();
        Task<IEnumerable<int>> GetSelectedPermissionList(int id);
        Task<long> SavePermission(MenuPermissionModel model);
    }
}
