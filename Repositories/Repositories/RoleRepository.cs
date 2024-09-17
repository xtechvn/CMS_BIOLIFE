using DAL;
using Entities.ConfigModels;
using Entities.Models;
using Entities.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Repositories.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleDAL _RoleDAL;
        private readonly UserDAL _UserDAL;
        public RoleRepository(IOptions<DataBaseConfig> dataBaseConfig)
        {
            _RoleDAL = new RoleDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
            _UserDAL = new UserDAL(dataBaseConfig.Value.SqlServer.ConnectionString);
        }

        public async Task<int> UpdateUserRole(int roleId, int userId, int type)
        {
            int rs = 0;
            try
            {
                rs = await _RoleDAL.UpdateUserRole(roleId, userId, type);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateUserRole - RoleRepository: " + ex);
            }
            return rs;
        }

        public async Task<List<Role>> GetAll()
        {
            var _RoleList = await _RoleDAL.GetAllAsync();
            return _RoleList.Where(s => s.Status == 0).ToList();
        }

        public async Task<Role> GetById(int Id)
        {
            return await _RoleDAL.FindAsync(Id);
        }

        public GenericViewModel<RoleDataModel> GetPagingList(string roleName, string strUserId, int currentPage, int pageSize)
        {
            var model = new GenericViewModel<RoleDataModel>();
            try
            {
                model.ListData = _RoleDAL.GetRolePagingList(roleName, strUserId, currentPage, pageSize, out int totalRecord);
                model.PageSize = pageSize;
                model.CurrentPage = currentPage;
                model.TotalRecord = totalRecord;
                model.TotalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetPagingList - RoleRepository: " + ex);
            }
            return model;
        }

        public async Task<int> Upsert(RoleViewModel model)
        {
            int rs = 0;
            try
            {
                if (model.Id == 0)
                {
                    var entity = new Role
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        Status = model.Status,
                        CreatedOn = DateTime.Now
                    };
                    rs = (int)await _RoleDAL.CreateAsync(entity);
                }
                else
                {
                    var entity = await _RoleDAL.FindAsync(model.Id);
                    entity.Name = model.Name;
                    entity.Description = model.Description;
                    entity.Status = model.Status;
                    entity.ModifiedOn = DateTime.Now;
                    await _RoleDAL.UpdateAsync(entity);
                    rs = entity.Id;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Upsert - RoleRepository: " + ex);
            }
            return rs;
        }

        public async Task<List<User>> GetListUserOfRole(int Id)
        {
            var model = new List<User>();
            try
            {
                var ListUserId = _UserDAL.GetListUserIdByRole(Id.ToString());
                model = await _UserDAL.GetAllAsync();
                model = model.Where(s => ListUserId.Contains(s.Id)).ToList();
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListUserOfRole - RoleRepository: " + ex);
            }
            return model;
        }

        public async Task<bool> AddOrDeleteRolePermission(string data, int type)
        {
            try
            {
                var ListPermission = data.Split(',');
                foreach (var item in ListPermission)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var arrParam = item.Split('-').Select(s => int.Parse(s)).ToArray();
                        await _RoleDAL.AddOrDeleteRolePermission(arrParam[0], arrParam[1], arrParam[2], type);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("AddOrDeleteRolePermission - RoleRepository: " + ex);
                return false;
            }
        }

        public async Task<List<RolePermission>> GetRolePermissionById(int roleId)
        {
            return await _RoleDAL.GetRolePermissionById(roleId);
        }

        public async Task<int> DeleteRole(int roleId)
        {
            try
            {
                return await _RoleDAL.DeleteRole(roleId);
            }
            catch
            {

            }
            return 0;
        }

        public async Task<List<Role>> GetRoleListByUserId(int userId)
        {
            return await _RoleDAL.GetRoleListByUserId(userId);
        }
    }
}
