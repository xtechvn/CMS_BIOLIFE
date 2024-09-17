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
    public class RoleDAL : GenericService<Role>
    {
        public RoleDAL(string connection) : base(connection)
        {
        }

        public List<int> GetListRoleIdByUser(string strUserId)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var arrUserId = strUserId.Split(',').Select(s => int.Parse(s)).ToArray();
                    return _DbContext.UserRoles.Where(s => arrUserId.Contains(s.UserId)).Select(s => s.RoleId).ToList();
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetListRoleIdByUser - RoleDAL: " + ex);
                return new List<int>();
            }
        }

        public List<RoleDataModel> GetRolePagingList(string roleName, string strUserId, int currentPage, int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var datalist = _DbContext.Roles.AsQueryable();

                    if (!string.IsNullOrEmpty(roleName))
                    {
                        datalist = datalist.Where(s => s.Name.Contains(roleName));
                    }

                    if (!string.IsNullOrEmpty(strUserId))
                    {
                        var ListRoleId = GetListRoleIdByUser(strUserId);
                        datalist = datalist.Where(s => ListRoleId.Contains(s.Id));
                    }

                    totalRecord = datalist.Count();
                    var data = datalist.Select(a => new RoleDataModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        Status = a.Status,
                        CountUser = _DbContext.UserRoles.Where(s => s.RoleId == a.Id).Count()
                    }).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRolePagingList - RoleDAL: " + ex);
            }
            return null;
        }

        public async Task<bool> AddOrDeleteRolePermission(int roleid, int menuid, int permissionid, int type)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    // Add Permission
                    if (type == 1)
                    {
                        var entity = new RolePermission()
                        {
                            RoleId = roleid,
                            MenuId = menuid,
                            PermissionId = permissionid
                        };
                        _DbContext.RolePermissions.Add(entity);
                        await _DbContext.SaveChangesAsync();
                    }
                    else // Remove Permission
                    {
                        var entity = await _DbContext.RolePermissions.Where(s => s.RoleId == roleid && s.MenuId == menuid && s.PermissionId == permissionid).FirstOrDefaultAsync();
                        if (entity != null)
                        {
                            _DbContext.RolePermissions.Remove(entity);
                            await _DbContext.SaveChangesAsync();
                        }
                    }
                    return true;
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("AddOrDeleteRolePermission - RoleDAL: " + ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="userid"></param>
        /// <param name="type">
        /// 1 : Add
        /// 0 : Remove
        /// </param>
        /// <returns></returns>
        public async Task<int> UpdateUserRole(int roleid, int userid, int type)
        {
            int rs = 0;
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    if (type == 0)
                    {
                        var entity = await _DbContext.UserRoles.Where(s => s.UserId == userid && s.RoleId == roleid).FirstOrDefaultAsync();
                        _DbContext.UserRoles.Remove(entity);
                        await _DbContext.SaveChangesAsync();
                        rs = roleid;
                    }
                    else
                    {
                        var entity = new UserRole
                        {
                            RoleId = roleid,
                            UserId = userid
                        };
                        _DbContext.UserRoles.Add(entity);
                        await _DbContext.SaveChangesAsync();
                        rs = entity.Id;
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("UpdateUserRole - RoleDAL: " + ex);
            }
            return rs;
        }

        public async Task<List<RolePermission>> GetRolePermissionById(int roleid)
        {
            var rslist = new List<RolePermission>();
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    rslist = await _DbContext.RolePermissions.Where(s => s.RoleId == roleid).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRolePermissionById - RoleDAL: " + ex);
            }
            return rslist;
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>
        ///  1 : succeed
        ///  0 : failed
        /// -1 : is used
        /// </returns>
        public async Task<int> DeleteRole(int Id)
        {
            try
            {
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    var IsUsed = false;
                    var IsUseForUser = _DbContext.UserRoles.Any(s => s.RoleId == Id);
                    var IsUseForPermission = _DbContext.RolePermissions.Any(s => s.RoleId == Id);

                    if (IsUseForUser || IsUseForPermission)
                    {
                        IsUsed = true;
                    }

                    if (IsUsed)
                    {
                        return -1;
                    }

                    var entity = await FindAsync(Id);
                    _DbContext.Roles.Remove(entity);
                    await _DbContext.SaveChangesAsync();
                    return Id;
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteRole - RoleDAL: " + ex);
            }
            return 0;
        }

        public async Task<List<Role>> GetRoleListByUserId(int userId)
        {
            var rsList = new List<Role>();
            try
            {
                var _RoleIdList = GetListRoleIdByUser(userId.ToString());
                using (var _DbContext = new EntityDataContext(_connection))
                {
                    rsList = await _DbContext.Roles.Where(s => _RoleIdList.Contains(s.Id)).ToListAsync();
                }
            }
            catch(Exception ex)
            {
                LogHelper.InsertLogTelegram("GetRoleListByUserId - RoleDAL: " + ex);
            }
            return rsList;
        }
    }
}
