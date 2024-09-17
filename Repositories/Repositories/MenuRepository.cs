using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Repositories.Repositories.BaseRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class MenuRepository : BaseRepository, IMenuRepository
    {
        private readonly MenuDAL _MenuDAL;

        public MenuRepository(IHttpContextAccessor context, IOptions<DataBaseConfig> dataBaseConfig, IUserRepository userRepository, IConfiguration configuration) : base(context, dataBaseConfig, configuration, userRepository)
        {
            _MenuDAL = new MenuDAL(_SqlServerConnectString);
        }

        public async Task<List<MenuViewModel>> GetMenuParentAndChildAll()
        {
            var rslist = new List<MenuViewModel>();
            try
            {
                var ListMenu = await _MenuDAL.GetAllAsync();
                ListMenu = ListMenu.Where(n => n.Status == (int)Utilities.Contants.Status.HOAT_DONG).OrderBy(x => x.OrderNo).ToList();
                var ParentList = ListMenu.Where(s => s.ParentId <= 0);
                foreach (var item in ParentList)
                {
                    var modelrs = new MenuViewModel();

                    var ChildList = new List<Menu>();
                    GetListMenuChild(item.Id, ListMenu, ref ChildList);

                    modelrs.Parent = item;
                    modelrs.ChildList = ChildList;
                    rslist.Add(modelrs);
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetMenuParentAndChildAll - MenuRepository: " + ex);
            }
            return rslist;
        }

        public async Task<List<Menu>> GetMenuPermissionOfUser(IEnumerable<int> menu_ids)
        {
            var rslist = new List<Menu>();
            try
            {
                if (menu_ids != null && menu_ids.Any())
                {
                    var ListMenu =  _MenuDAL.GetAll();
                    var childList = ListMenu.Where(s => menu_ids.Contains(s.Id));
                    foreach (var item in childList)
                    {
                        rslist.Add(item);
                        GetListMenuParent(item, ListMenu, rslist);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetMenuPermissionOfUser - MenuRepository: " + ex);
            }
            return rslist.Distinct().ToList();
        }

        public void GetListMenuParent(Menu item, List<Menu> listdata, List<Menu> ListRs)
        {
            if (item.ParentId > 0)
            {
                var parent = listdata.FirstOrDefault(s => s.Id == item.ParentId);
                if (parent != null)
                {
                    ListRs.Add(parent);
                    GetListMenuParent(parent, listdata, ListRs);
                }
            }
        }

        public void GetListMenuChild(int ParentId, List<Menu> listdata, ref List<Menu> ListRs)
        {
            var childlist = listdata.Where(s => s.ParentId == ParentId).ToList();
            if (childlist != null && childlist.Count > 0)
            {
                ListRs.AddRange(childlist);
                foreach (var item in childlist)
                {
                    GetListMenuChild(item.Id, listdata, ref ListRs);
                }
            }
        }

        public async Task<List<Permission>> GetPermissionList()
        {
            var rslist = new List<Permission>();
            try
            {
                rslist = await _MenuDAL.GetPermissionList();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPermissionList - MenuRepository: " + ex);
            }
            return rslist;
        }

        public async Task<long> Create(Menu model)
        {
            try
            {
                Menu parent_model = null;
                if (model.ParentId != null && model.ParentId.Value > 0) parent_model = await GetById(model.ParentId.Value);

                model.FullParent = parent_model != null ? $"{(!String.IsNullOrEmpty(parent_model.FullParent) ? $"{parent_model.FullParent}," : String.Empty)}{parent_model.Id}" : String.Empty;
                model.Status = 0;
                model.CreatedOn = DateTime.Now;

                return await _MenuDAL.CreateAsync(model);
            }
            catch
            {
                throw;
            }
        }

        public async Task<long> Update(Menu model)
        {
            try
            {

                Menu parent_model = null;

                if (model.ParentId != null && model.ParentId.Value > 0) parent_model = await GetById(model.ParentId.Value);

                var data = await GetById(model.Id);
                data.Name = model.Name;
                data.FullParent = parent_model != null ? $"{(!String.IsNullOrEmpty(parent_model.FullParent) ? $"{parent_model.FullParent}," : String.Empty)}{parent_model.Id}" : String.Empty;
                data.MenuCode = model.MenuCode;
                data.Link = model.Link;
                data.Icon = model.Icon;
                data.Title = model.Title;
                data.Status = model.Status;
                data.ModifiedOn = DateTime.Now;

                await _MenuDAL.UpdateAsync(data);
                return model.Id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Menu>> GetAll(string name, string link)
        {
            try
            {
                var datas = await _MenuDAL.GetAllAsync();

                List<Menu> result = new List<Menu>();

                if (!String.IsNullOrEmpty(name))
                {
                    var data = datas.Where(s => s.Name.ToLower().Contains(name.ToLower())).ToList();
                    if (data != null && data.Any())
                        result.AddRange(data);
                }

                if (!String.IsNullOrEmpty(link))
                {
                    var data = datas.Where(s => (s.Link ?? string.Empty).ToLower().Contains(link.ToLower())).ToList();
                    if (data != null && data.Any())
                        result.AddRange(data);
                }

                if (result != null && result.Any())
                {
                    var full_parent_ids = result.Select(s => s.FullParent).Where(s => !string.IsNullOrEmpty(s))
                    .SelectMany(s => s.Split(',')).Select(s => int.Parse(s)).Distinct().ToList();

                    result.AddRange(datas.Where(s => full_parent_ids.Contains(s.Id)));

                    return result.Distinct();
                }
                else
                {
                    return datas;
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task<Menu> GetById(int id)
        {
            return await _MenuDAL.FindAsync(id);
        }

        public async Task<long> ChangeStatus(int id, int status)
        {
            try
            {
                if (status == 1)
                {
                    var child_datas = await _MenuDAL.GetByConditionAsync(s => s.ParentId == id && s.Status == 0);
                    if (child_datas != null && child_datas.Any())
                    {
                        foreach (var item in child_datas)
                        {
                            var child_data = await GetById(item.Id);
                            child_data.Status = status;
                            await _MenuDAL.UpdateAsync(child_data);
                        }
                    }
                }

                var data = await GetById(id);
                data.Status = status;
                await _MenuDAL.UpdateAsync(data);

                return id;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<Permission>> GetListPermission()
        {
            return await _MenuDAL.GetPermissionList();
        }

        public async Task<long> SavePermission(MenuPermissionModel model)
        {
            try
            {
                return await _MenuDAL.SaveMenuPermission(model);
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<int>> GetSelectedPermissionList(int id)
        {
            return await _MenuDAL.GetSelectedPermissionList(id);
        }

        public async Task<IEnumerable<MenuPermission>> GetAllMenuHasPermission()
        {
            return await _MenuDAL.GetAllMenuHasPermission();
        }
    }
}
