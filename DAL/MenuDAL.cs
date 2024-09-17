using DAL.Generic;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace DAL
{
    public class MenuDAL : GenericService<Menu>
    {
        private PermissionDAL _PermissionDAL;
        public MenuDAL(string connection) : base(connection)
        {
            _PermissionDAL = new PermissionDAL(connection);
        }

        public async Task<List<Permission>> GetPermissionList()
        {
            try
            {
                var rslist = await _PermissionDAL.GetAllAsync();
                return rslist.Where(s => s.Status == 0).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPermissionList : MenuDAL: " + ex);
                return new List<Permission>();
            }
        }

        public async Task<int> SaveMenuPermission(MenuPermissionModel model)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    _DbContext.MenuPermissions.RemoveRange(_DbContext.MenuPermissions.Where(x => x.MenuId == model.menu_id));

                    if (model.permission_ids != null && model.permission_ids.Any())
                    {
                        var datas = model.permission_ids.Select(x => new MenuPermission
                        {
                            Id = 0,
                            MenuId = model.menu_id,
                            PermissionId = x
                        });

                        await _DbContext.MenuPermissions.AddRangeAsync(datas);
                    }

                    await _DbContext.SaveChangesAsync();
                    return model.menu_id;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<MenuPermission>> GetAllMenuHasPermission()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.MenuPermissions.ToListAsync();
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<int>> GetSelectedPermissionList(int menuId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.MenuPermissions.Where(x => x.MenuId == menuId).Select(s => s.PermissionId).ToListAsync();
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<Menu>> GetActiveMenu()
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    return await _DbContext.Menus.AsNoTracking().Where(x=>x.Status==0).ToListAsync();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
